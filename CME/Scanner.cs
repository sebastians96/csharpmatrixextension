using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace csharpmatrixextension
{
    class Scanner
    {
        public String[] tokenize(String input)
        {
            List<String> tokens = new List<String>();
            String tmp = Regex.Replace(input, @"[=]{2}|[-=\+\*<>(),;]|!=|\[|\]", m => string.Format(@"|{0}|", m.Value));
            tokens.AddRange(tmp.Split(new Char[] {'|',' '}).ToList());
            tokens = tokens.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            return tokens.ToArray();
        }
    }
}
