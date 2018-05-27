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
        StringBuilder result;
        public Parser()
        {
            scanner = new Scanner();
            tokens = new List<KeyValuePair<TokenType, string>>();
        }
        public String parse(String input)
        {
            result.Clear();
            tokens.RemoveRange(0, tokens.Count - 1);
            do
            {
                tokens.Add(scanner.tokenize(ref input));
            } while (tokens.Last().Key == TokenType.Semicolon);

            // Interpretate
            //m1 = [1, 2, 3];
            //m2 = [1 2 3, 4 5 6, 7 8 9];

            return result.ToString();
        }
    }
}
