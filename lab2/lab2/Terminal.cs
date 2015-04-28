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
                return table.ContainsKey(index) ? Terminals[table[index]] : null;
            }
        }

        public void SetForAll(int i, params Lexeme[] mas)
        {
            foreach (var m in mas)
                table[m] = i;

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
            return t1.Name == t2.Name;
        }

        public static bool operator !=(Terminal t1, Terminal t2)
        {
            return !(t1 == t2);
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
