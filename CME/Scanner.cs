using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CME
{
    enum TokenType { Matrix, OpeningMatrixBracket, ClosingMatrixBracket, OpeningMathBracket, ClosingMathBracket, Integer, Float, MathematicalOperator, Comparator, Function, Unknown, Comma, Semicolon, Assign, Help};
class Scanner
    {
        Char[] Terminals = { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm', 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '(', ')', '[', ']', '+', '-', '*', '<', '>', '=', '!','.',',','?' };
        Char[] Separators = { '[', ']', '(', ')', ',', ';', '+', '-', '*', '=', '!', '>', '<' };
        String[] Comparators = { "==", "!=" };
        public KeyValuePair<TokenType,String> tokenize(ref String input)
        {
            if(input.Equals(""))
            {
                input = ";";
            }
            // Skips whitespaces 
            while(char.IsWhiteSpace (input[0]))
            {
                input = input.Remove(0, 1);
            }
            StringBuilder buff = new StringBuilder();
            Char s;
            TokenType type = TokenType.Unknown;
            KeyValuePair<TokenType, String> token;
            if (Comparators.Contains(input.Take(2)))
            {
                token = new KeyValuePair<TokenType, string>(TokenType.Comparator, (string)input.Take(2));
                input = input.Remove(0, 2);
                return token;
            }
            else if (Separators.Contains(input[0]))
            {
                Char tmp = input[0];
                // Check type of token

                switch(tmp)
                {
                    case '[':
                        type = TokenType.OpeningMatrixBracket;
                        break;
                    case ']':
                        type = TokenType.ClosingMatrixBracket;
                        break;
                    case '(':
                        type = TokenType.OpeningMathBracket;
                        break;
                    case ')':
                        type = TokenType.ClosingMathBracket;
                        break;
                    case ',':
                        type = TokenType.Comma;
                        break;
                    case ';':
                        type = TokenType.Semicolon;
                        break;
                    case '=':
                        type = TokenType.Assign;
                        break;
                    case '+':
                    case '-':
                    case '*':
                        type = TokenType.MathematicalOperator;
                        break;
                }
                token = new KeyValuePair<TokenType, string>(type, tmp.ToString());
                input = input.Remove(0, 1);
                return token;
            }
            else if (input[0].Equals('?'))
            {
                type = TokenType.Help;
                token = new KeyValuePair<TokenType, string>(type, input[0].ToString());
                input = input.Remove(0, 1);
                input = ";";
                return token;
            }
            while (true)
            {
                if (input == "")
                {
                    String message = "Unexpected line ending: " + buff + "";
                    throw new System.Exception(message);
                }
                s = input[0];
                if( char.IsWhiteSpace(s) || Separators.Contains(s))
                {
                    if (type == TokenType.Unknown)
                    {
                        String message = "Unrecognized Token: " + buff + "";
                        throw new System.Exception(message);
                    }
                    else
                    {
                        token = new KeyValuePair<TokenType, String>(type, buff.ToString());
                        return token;
                    }
                }
                else if (!Terminals.Contains(s))
                {
                    String message = "Unrecognized character: " + buff + ""; 
                    throw new System.Exception(message);
                }
                else
                {

                    buff.Append(input[0]);
                    input = input.Remove(0, 1);
                }
                // Find out the type of token
                if (s == '.')
                {
                    if (char.IsDigit(input[0]))
                    {
                        type = TokenType.Float;
                    }
                    else
                    {
                        String message = "Syntax Error: " + buff + "";
                        throw new System.Exception(message);
                    }
                }
                else if (type == TokenType.Unknown)
                {
                    if(char.IsDigit(s))
                    {
                        type = TokenType.Integer;
                    }
                    else if(char.IsUpper(s))
                    {
                        type = TokenType.Function;
                    }
                    else if(char.IsLower(s))
                    {
                        type = TokenType.Matrix;
                    }
                }
            }
        }
    }
}