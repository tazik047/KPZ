using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lab1
{
    class Operation
    {
        public string Character { get; private set; }
        public int Prioryty { get; private set; }
        public int Size { get; set; }

        public Operation(string character, int prioryty, int size)
        {
            Character = character;
            Prioryty = prioryty;
            Size = size;
        }

        public bool isOperation(string s, int start = 0)
        {
            var r = new Regex("^.{" + start + "," + start + "}" + Regex.Escape(Character));
            return r.IsMatch(s);
        }

        public Operation(string s)
        {
            Character = s;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var op = obj as Operation;
            return op.Prioryty==Prioryty && op.Character==Character;

        }

        public override int GetHashCode()
        {
            return Character.GetHashCode() + Size + Prioryty;
        }

        public static bool operator <(Operation op1, Operation op2)
        {
            return op1.Prioryty < op2.Prioryty;
        }

        public static bool operator >(Operation op1, Operation op2)
        {
            return op1.Prioryty > op2.Prioryty;
        }

        public static bool operator >=(Operation op1, Operation op2)
        {
            return !(op1 < op2);
        }

        public static bool operator <=(Operation op1, Operation op2)
        {
            return !(op1 > op2);
        }

        public static bool operator ==(Operation op1, Operation op2)
        {
            try
            {
                return op1.Equals(op2);
            }
            catch
            {
                return false;
            }
        }

        public static bool operator !=(Operation op1, Operation op2)
        {
            return !(op1 == op2);
        }

        public override string ToString()
        {
            return Size + Character;
        }


        /// <summary>
        /// Operation - (
        /// </summary>
        public static readonly Operation OpenScob = new Operation("(");

        /// <summary>
        /// Operation - )
        /// </summary>
        public static readonly Operation CloseScob = new Operation(")");
        
        /// <summary>
        /// Operation - [
        /// </summary>
        public static readonly Operation OpenSqScob = new Operation("[");

        /// <summary>
        /// Operation - ]
        /// </summary>
        public static readonly Operation CloseSqScob = new Operation("]");

        /// <summary>
        /// Operation - @
        /// </summary>
        public static readonly Operation GetOp = new Operation("@");

        /// <summary>
        /// Operation - ,
        /// </summary>
        public static readonly Operation Separator = new Operation(",");
    }
}
