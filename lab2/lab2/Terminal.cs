using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    class Terminal
    {
        public string Name { get; set; }
        public List<List<object>> Terminals { get; private set; }
        private Dictionary<Lexeme, int> table;

        public List<object> this[Lexeme index]
        {
            get
            {
                bool t = table.ContainsKey(index); 
                return t ? Terminals[table[index]] : null;
            }
        }

        public void SetForAll(int i, params Lexeme[] mas)
        {
            foreach (var m in mas)
                table[m] = i;

        }

        public bool haveEmpty()
        {
            foreach (var i in Terminals)
            {
                if (i.Count == 1 && i[0].Equals(Terminal.Empty))
                    return true;
            }
            return false;
        }

        public void AddTerminal(params object[] t)
        {
            Terminals.Add(t.ToList());
        }

        public Terminal(string name, params List<object>[] terminals)
        {
            Name = name;
            Terminals = terminals.Select(t => t.ToList()).ToList();
            table = new Dictionary<Lexeme, int>();
        }

        public static bool operator ==(Terminal t1, Terminal t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(Terminal t1, Terminal t2)
        {
            return !(t1 == t2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Name == ((Terminal)obj).Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        
        public static Terminal End
        {
            get { return new Terminal("#"); }
        }

        public static Terminal Empty
        {
            get { return new Terminal("e"); }
        }
    }
}
