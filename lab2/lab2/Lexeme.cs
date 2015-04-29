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
            Regex r = new Regex(Pattern);
            return s.Length!=0 && s == r.Match(s).Value;
        }

        public string StartWith(string s)
        {
            if (Pattern == null)
                return Lexemes.FirstOrDefault(s.StartsWith);
            Regex r = new Regex("^" + Pattern);
            if (r.IsMatch(s))
            {
                if (Id != 5)
                    return r.Match(s).Value;
                var str = r.Match(s).Value;
                int firstIndex = str.IndexOf('(') + 1;
                int count = 1;
                while (count != 0)
                {
                    if (str[firstIndex] == '(')
                        count++;
                    if (str[firstIndex] == ')')
                        count--;
                    firstIndex++;
                }
                return str.Substring(0, firstIndex);
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Id == ((Lexeme)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
