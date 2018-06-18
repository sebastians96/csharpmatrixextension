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
            variables.Add("m1", new Matrix<int>(new Matrix<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3, 3)));
            variables.Add("m2", new Matrix<double>(new Matrix<double>(new double[] { 1.0, 2.0, 3.0, 4.0}, 2, 2)));
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
                List<dynamic> arguments = new List<dynamic>();
                dynamic m;
                switch (tokens[pointer].Key)
                {
                    case TokenType.Matrix:
                        String name = tokens[pointer].Value;
                        pointer++;
                        switch (tokens[pointer].Key)
                        {
                            case TokenType.Comparator:
                                Boolean tmp = false;
                                KeyValuePair<String, dynamic> tmp1 = variables.FirstOrDefault(t => t.Key == tokens[pointer - 1].Value);
                                KeyValuePair<String, dynamic> tmp2 = variables.FirstOrDefault(t => t.Key == tokens[pointer + 1].Value);
                                switch (tokens[pointer].Value)
                                {
                                    case "==":
                                        pointer++;
                                        tmp = tmp1.Value == tmp2.Value;
                                        break;
                                    case "!=":
                                        pointer++;
                                        tmp = tmp1.Value != tmp2.Value;
                                        break;

                                }
                                Console.WriteLine("{0}", tmp);
                                break;
                            case TokenType.Assign:
                                if(!variables.ContainsKey(name))
                                {
                                    pointer++;
                                    Matrix<int> data = ParseData(ref pointer, ref tokens);
                                    result.Append(data.Write());
                                    variables.Add(name, data);
                                    pointer++;
                                    break;
                                }
                                else
                                {
                                    return "Variable already exist!";
                                }
                            case TokenType.OpeningMatrixBracket:    
                                if (variables.ContainsKey(name))
                                {
                                    pointer = pointer + 6;
                                    KeyValuePair<String, dynamic> matrix = new KeyValuePair<String, dynamic>(name, variables[name]);
                                    int first = Convert.ToInt32(tokens[pointer - 5].Value);
                                    int second = Convert.ToInt32(tokens[pointer - 2].Value);
                                    if(first < matrix.Value.GetRows() && second < matrix.Value.GetCols())
                                        result.Append(matrix.Value.PickValue(first, second));
                                    else
                                    {
                                        result.Append("Out of bound exception!");
                                    }
                                    break;
                                }
                                else
                                {
                                    return "Variable doesnt exist!";
                                }
                        }
                        break;
                    case TokenType.Function:
                        current = pointer;
                        pointer += 2;
                        while(tokens[pointer].Key != TokenType.ClosingMathBracket)
                        {
                            if(arguments.Count > 0) pointer++;
                            switch (tokens[pointer].Key)
                            {
                                case TokenType.Float:
                                    arguments.Add(float.Parse(tokens[pointer].Value));
                                    pointer++;
                                    break;
                                case TokenType.Integer:
                                    arguments.Add(Int32.Parse(tokens[pointer].Value));
                                    pointer++;
                                    break;
                                case TokenType.Matrix:
                                    if (variables.ContainsKey(tokens[pointer].Value))
                                    {
                                        arguments.Add(variables[tokens[pointer].Value]);
                                        pointer++;
                                    }
                                    else return "Matrix does not exist!";
                                    //variables.TryGetValue(tokens[pointer].Value, out arguments);

                                    break;
                                case TokenType.OpeningMatrixBracket:
                                    arguments.Add(this.ParseData(ref pointer, ref tokens));
                                    pointer++;
                                    break;
                                case TokenType.Function:
                                    // funkcja zagnieżdżona
                                    Parser p = new Parser();
                                    List<KeyValuePair<TokenType, String>> tmp = new List<KeyValuePair<TokenType, string>>();
                                    int check = 0;
                                    while (!(tokens[pointer].Key == TokenType.ClosingMathBracket && check == 0))
                                    {
                                        switch (tokens[pointer].Key)
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
                                    arguments.Add(p.parse_req(tmp, level + 1));
                                    break;
                                default:
                                    throw new System.Exception("Error: Bad function argument!");
                            }
                        }
                        switch (tokens[current].Value)
                        {
                            case "Zeros":
                                if(arguments.Count > 1)
                                {
                                    if (level == 0)
                                    {
                                        result.Append(Matrix<int>.Zeros(arguments[0],arguments[1]).Write());
                                        break;
                                    }
                                    else return Matrix<int>.Zeros(arguments[0],arguments[1]);
                                }
                                else
                                {
                                    if (level == 0)
                                    {
                                        result.Append(Matrix<int>.Zeros(arguments[0]).Write());
                                        break;
                                    }
                                    else return Matrix<int>.Zeros(arguments[0]);
                                }
                            case "Diagonal":
                                if (level == 0)
                                {
                                    result.Append(Matrix<int>.Identity(arguments[0]).Write());
                                    break;
                                }
                                else return Matrix<int>.Identity(arguments[0]);
                            case "Row":
                                m = arguments[0];
                                if (level == 0)
                                {
                                    result.Append(m.Row(arguments[1]).Write());
                                    break;
                                }
                                else return m.Row(arguments[1]);
                                
                            case "Col":
                                m = arguments[0];
                                if (level == 0)
                                {
                                    result.Append(m.Column(arguments[1]).Write());
                                    break;
                                }
                                else return m.Column(arguments[1]);
                            case "Pow":
                                m = arguments[0];
                                if (level == 0)
                                {
                                    result.Append(m.Power(arguments[1]).Write());
                                    break;
                                }
                                else return m.Power(arguments[1]);
                            case "Det":
                                if (level == 0)
                                {
                                    result.Append(arguments[0].Determinant().ToString());
                                    break;
                                }
                                else return arguments[0].Determinant();
                            case "Transpose":
                                if (level == 0)
                                {
                                    result.Append(arguments[0].Transpose().Write());
                                    break;
                                }
                                else return arguments[0].Transpose();
                            case "ComplementsMatrix":
                            case "Complementsmatrix":
                                if (level == 0)
                                {
                                    result.Append(arguments[0].Complements().Write());
                                    break;
                                }
                                else return arguments[0].Complements();
                            case "Inverse":
                                if (level == 0)
                                {
                                    result.Append(arguments[0].Inverse().Write());
                                    break;
                                }
                                else return arguments[0].Inverse();
                            case "Write":
                                if (level == 0)
                                {
                                    result.Append(arguments[0].Write());
                                    break;
                                }
                                else return arguments[0].Write();
                                
                        }
                        pointer += 1;
                        break;
                    case TokenType.Help:
                        // Write list of functions - help page.
                        result.Append("###################### Help ######################");
                        //ToDo - write help section
                        break;
                    //default:
                    //    foreach (KeyValuePair<String, dynamic> entry in variables)
                    //        result.Append(entry.Key);
                    //    return result;
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
