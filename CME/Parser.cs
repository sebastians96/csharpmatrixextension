using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CME
{
    class Parser
    {
        Scanner scanner;
        List<KeyValuePair<TokenType, String>> tokens;
        Dictionary<String, dynamic> variables = new Dictionary<string, dynamic>();
        StringBuilder result;
        public Parser()
        {
            scanner = new Scanner();
            tokens = new List<KeyValuePair<TokenType, string>>();
            result = new StringBuilder();
        }
        public dynamic parse(String input)
        {
            tokens.Clear();
            do
            {
                tokens.Add(scanner.tokenize(ref input));
            } while (tokens.Last().Key != TokenType.Semicolon);
            return this.parse_req(tokens,0);
        }
        public dynamic parse_req(List<KeyValuePair<TokenType, String>>  tokens, int level)
        {
            result.Clear();
            int pointer = 0,current = 0;
            while(tokens[pointer].Key != TokenType.Semicolon)
            {
                switch (tokens[pointer].Key)
                {
                    case TokenType.Matrix:
                        // Assign, compare etc.
                        break;
                    case TokenType.Function:
                        dynamic arg;
                        current = pointer;
                        pointer += 2;
                        switch (tokens[pointer].Key)
                        {
                            case TokenType.Matrix:
                                variables.TryGetValue(tokens[pointer].Value, out arg);
                                break;
                            case TokenType.OpeningMatrixBracket:
                                arg = this.ParseData(ref pointer, ref tokens);
                                pointer++;
                                break;
                            case TokenType.Function:
                                // funkcja zagnieżdżona
                                Parser p = new Parser();
                                List<KeyValuePair<TokenType, String>> tmp = new List<KeyValuePair<TokenType, string>>();
                                int check = 0;
                                while(!(tokens[pointer].Key == TokenType.ClosingMathBracket && check == 0))
                                {
                                    switch(tokens[pointer].Key)
                                        {
                                            case TokenType.Function:
                                                check++;
                                                break;
                                            case TokenType.ClosingMathBracket:
                                                check--;
                                                break;
                                        }
                                    tmp.Add(tokens[pointer++]);
                                }
                                arg = p.parse_req(tmp, level + 1);
                                break;
                            default:
                                throw new System.Exception("Error: Bad function argument!");
                        }
                        switch (tokens[current].Value)
                        {
                            case "Zeros":
                                // Zeros function

                                break;
                            case "Identity":
                                // Identity function
                                break;
                            case "PickValue":
                            case "Pickvalue":
                                // Pick Value function
                                break;
                            case "Row":
                                // Row function
                                break;
                            case "Column":
                                // Column function
                                break;
                            case "Power":
                                // Power function
                                break;
                            case "Determinant":
                                if (level == 0)
                                {
                                    result.Append(arg.Determinant().ToString());
                                    break;
                                }
                                else return arg.Determinant();
                            case "Transpose":
                                if (level == 0)
                                {
                                    result.Append(arg.Transpose().Write());
                                    break;
                                }
                                else return arg.Transpose();
                            case "ComplementsMatrix":
                            case "Complementsmatrix":
                                if (level == 0)
                                {
                                    result.Append(arg.Complements().Write());
                                    break;
                                }
                                else return arg.Complements();
                            case "Inverse":
                                if (level == 0)
                                {
                                    result.Append(arg.Inverse().Write());
                                    break;
                                }
                                else return arg.Inverse();
                            case "Write":
                                if (level == 0)
                                {
                                    result.Append(arg.Write());
                                    break;
                                }
                                else return arg.Write();
                                
                        }
                        pointer += 1;
                        break;
                    case TokenType.Help:
                        // Write list of functions - help page.
                        result.Append("###################### Help ######################");
                        //ToDo - write help section
                        break;
                    default:
                        return "Error, please write correct query!";
                }
            }

            return result.ToString().Trim('\r');
        }

        private dynamic ParseData(ref int ptr, ref List<KeyValuePair<TokenType, String>> tokens)
        {
            ptr++;
            int columns = 0, rows = 1;
            int tmp = ptr;
            dynamic result;
            while (tokens[tmp].Key != TokenType.Comma && tokens[tmp].Key != TokenType.ClosingMatrixBracket)
            {
                columns++;
                tmp++;
            }
            if (tokens[ptr].Key == TokenType.Integer)
            {
                List<int> nums = new List<int>();
                while(tokens[ptr].Key != TokenType.ClosingMatrixBracket)
                {
                    if(tokens[ptr].Key == TokenType.Integer)
                    {
                        nums.Add(Int32.Parse(tokens[ptr].Value));
                    }
                    else if (tokens[ptr].Key == TokenType.Comma)
                    {
                        rows++;
                    }
                    ptr++;
                }
                result = new Matrix<int>(nums.ToArray(), rows, columns);
            }
            else if (tokens[ptr].Key == TokenType.Float)
            {
                List<double> nums = new List<double>();
                while (tokens[ptr].Key != TokenType.ClosingMatrixBracket)
                {
                    if (tokens[ptr].Key == TokenType.Float)
                    {
                        nums.Add(Double.Parse(tokens[ptr].Value));
                    }
                    else if (tokens[ptr].Key == TokenType.Comma)
                    {
                        rows++;
                    }
                    ptr++;
                }
                result = new Matrix<double>(nums.ToArray(), rows, columns);
            }
            else
            {
                throw new System.Exception("Incorrect data after matrix bracket!");
            }
            
            return result;
        }
    }
}
