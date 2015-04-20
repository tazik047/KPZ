using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    class CycleAnalizer
    {
        public static void main()
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("S");
            GMgmt[] gmt = new GMgmt[50];
            gmt[0] = new GMgmt("A", "for", "BC");
            gmt[1] = new GMgmt("A", "id", "BC");
            gmt[2] = new GMgmt("A", "cnst", "BC");
            gmt[3] = new GMgmt("A", "(", "BC");

            gmt[4] = new GMgmt("C", ")", "");
            gmt[5] = new GMgmt("C", "r", "");
            gmt[6] = new GMgmt("C", ";", "");
            gmt[7] = new GMgmt("C", "#", "");
            gmt[8] = new GMgmt("C", "+", "+BC");
            gmt[9] = new GMgmt("C", "-", "-BC");

            gmt[10] = new GMgmt("B", "for", "DE");
            gmt[11] = new GMgmt("B", "id", "DE");
            gmt[12] = new GMgmt("B", "cnst", "DE");
            gmt[13] = new GMgmt("B", "(", "DE");

            gmt[14] = new GMgmt("E", ")", "");
            gmt[15] = new GMgmt("E", "r", "");
            gmt[16] = new GMgmt("E", ";", "");
            gmt[17] = new GMgmt("E", "#", "");
            gmt[18] = new GMgmt("E", "+", "");
            gmt[19] = new GMgmt("E", "-", "");
            gmt[20] = new GMgmt("E", "*", "*DE");
            gmt[21] = new GMgmt("E", "/", "/DE");

            gmt[22] = new GMgmt("D", "for", "(A)");
            gmt[23] = new GMgmt("D", "id", "id");
            gmt[24] = new GMgmt("D", "cnst", "cnst");
            gmt[25] = new GMgmt("D", "(", "(A)");

            gmt[26] = new GMgmt("K", "for", "ArA");
            gmt[27] = new GMgmt("K", "id", "ArA");
            gmt[28] = new GMgmt("K", "cnst", "ArA");
            gmt[29] = new GMgmt("K", "(", "ArA");

            gmt[30] = new GMgmt("L", "for", "id=A");
            gmt[31] = new GMgmt("L", "id", "id=A");
            gmt[32] = new GMgmt("L", "cnst", "id=A");
            gmt[33] = new GMgmt("L", "(", "id=A");

            gmt[34] = new GMgmt("S", "for", "FM#");
            gmt[35] = new GMgmt("S", "id", "M#");
            gmt[36] = new GMgmt("S", "cnst", "FM#");
            gmt[37] = new GMgmt("S", "(", "FM#");

            gmt[38] = new GMgmt("F", "for", "for(L;K;A)");
            gmt[39] = new GMgmt("F", "id", "for(L;K;A)");
            gmt[40] = new GMgmt("F", "cnst", "for(L;K;A)");
            gmt[41] = new GMgmt("F", "(", "for(L;K;A)");

            gmt[42] = new GMgmt("M", "for", "G");
            gmt[43] = new GMgmt("M", "id", "G");
            gmt[44] = new GMgmt("M", "cnst", "G");
            gmt[45] = new GMgmt("M", "(", "G");

            gmt[46] = new GMgmt("G", "for", "A");
            gmt[47] = new GMgmt("G", "id", "A");
            gmt[48] = new GMgmt("G", "cnst", "A");
            gmt[49] = new GMgmt("G", "(", "A");

            string line = "for(id=cnst;idrcnst;id+cnst)id+id#";
            string line1 = "id+id#";
            int current = 0;
            string temp = stack.Peek();
            bool res = false;
            string qw = "";
            string q = "";
            while (temp != "#")
            {
                temp = stack.Peek();
                bool b = false;
                for (int i = 0; i < gmt.Length; i++)
                {
                    if (temp == gmt[i].Row)
                    {
                        b = true;
                        break;
                    }
                }
                if (line[current].ToString() != "#")
                {
                    if (line[current].ToString() + line[current + 1].ToString() == "id")
                    {
                        qw = "id";
                    }
                    if (line[current].ToString() != "#" && current < line.Length - 2)
                    {
                        if (line[current].ToString() + line[current + 1].ToString() + line[current + 2].ToString() == "for")
                        {
                            qw = "for";
                        }
                    }
                    if (line[current].ToString() != "#" && current < line.Length - 3)
                    {
                        if (line[current].ToString() + line[current + 1].ToString() + line[current + 2].ToString() + line[current + 3].ToString() == "cnst")
                        {
                            qw = "cnst";
                        }
                    }
                    if (line[current].ToString() + line[current + 1].ToString() != "id")
                    {
                        if (current < line.Length - 2)
                        {
                            if (line[current].ToString() + line[current + 1].ToString() + line[current + 2].ToString() != "for")
                            {
                                if (current < line.Length - 3)
                                {
                                    if (line[current].ToString() + line[current + 1].ToString() + line[current + 2].ToString() + line[current + 3].ToString() != "cnst")
                                        qw = line[current].ToString();
                                }
                                else
                                    qw = line[current].ToString();
                            }
                        }
                        else
                            qw = line[current].ToString();
                    }
                }
                if (line[current].ToString() == "#")
                {
                    qw = "#";
                    b = false;
                    temp = "#";
                }
                if (b)
                {
                    bool b1 = false;

                    for (int i = 0; i < gmt.Length; i++)
                    {

                        if (gmt[i].Row == temp && gmt[i].Column == qw)
                        {
                            b1 = true;
                            stack.Pop();
                            q = gmt[i].Value;
                            if (q == "id")
                            {
                                stack.Push(q);
                                break;
                            }
                            if (q == "id=A")
                            {
                                stack.Push("A");
                                stack.Push("=");
                                stack.Push("id");
                                break;
                            }
                            if (q == "for(L;K;A)")
                            {
                                stack.Push(")");
                                stack.Push("A");
                                stack.Push(";");
                                stack.Push("K");
                                stack.Push(";");
                                stack.Push("L");
                                stack.Push("(");
                                stack.Push("for");
                                break;
                            }
                            if (q == "cnst")
                            {
                                stack.Push(q);
                                break;
                            }
                            else
                            {
                                for (int j = q.Length - 1; j >= 0; j--)
                                    stack.Push(q[j].ToString());
                                break;
                            }
                        }
                    }
                    if (!b1)
                    {
                        Console.Write("Error 22!!");
                        break;
                        break;
                    }

                }
                if (!b || temp == "#")
                {
                    if (temp == qw)
                    {
                        stack.Pop();
                        if (qw == "id")
                            current += 2;
                        if (qw == "for")
                            current += 3;
                        if (qw == "cnst")
                            current += 4;
                        if (qw != "id" && qw != "for" && qw != "cnst")
                            current++;
                        if (current == line.Length - 1)
                            res = true;
                    }
                    else
                    {
                        Console.Write("Error 33!!");
                        break;
                    }
                }
            }
            if (res)
                Console.WriteLine("True!!!");
            else
                Console.WriteLine("False!!!");
        }
    }
    class GMgmt
    {
        private string row;
        private string column;
        private string val;

        public string Row
        {
            get { return row; }
            set { row = value; }
        }

        public string Column
        {
            get { return column; }
            set { column = value; }
        }

        public string Value
        {
            get { return val; }
            set { val = value; }
        }

        public GMgmt(string row, string col, string val)
        {
            Row = row;
            Column = col;
            Value = val;
        }

        public GMgmt()
        {
            Row = "";
            Column = "";
            Value = "";
        }
    }
}
