using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    class Lexeme
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Pattern { get; set; }
        public string Information { get; set; }
        public List<string> Lexemes { get; set; }


        public Lexeme(int id, string type, string pattern = null, string information = "")
        {
            Id = id;
            Type = type;
            Pattern = pattern;
            Information = information;
            Lexemes = new List<string>();
        }

        public bool isCorrect(string s)
        {
            if (Pattern == null)
            {
                return Lexemes.Contains(s);
            }
            else
            {
                Regex r = new Regex(Pattern);
                return s == r.Match(s).Value;
            }
        }

        public string StartWith(string s)
        {
            if (Pattern == null)
                return Lexemes.FirstOrDefault(s.StartsWith);
            Regex r = new Regex("^"+Pattern);
            if (r.IsMatch(s))
                return r.Match(s).Value;
            return null;
        }
    }
}
