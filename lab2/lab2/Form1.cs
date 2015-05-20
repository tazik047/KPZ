using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        private Lexeme[] lexemes;

        private Terminal[] terminals;

        private bool isGood = true;

        public Form1()
        {
            InitializeComponent();
            fillTheLexemes();
            terminals = new[]
            {
                new Terminal("A"), //0
                new Terminal("D"), //1
                new Terminal("B"), //2
                new Terminal("M"), //3
                new Terminal("P")  //4
            };

            terminals[0].AddTerminal(terminals[1], Terminal.End);
            terminals[1].AddTerminal(terminals[3], terminals[2]);
            terminals[2].AddTerminal(lexemes[2], terminals[3], terminals[2]);
            terminals[2].AddTerminal(Terminal.Empty);
            terminals[3].AddTerminal(lexemes[6], terminals[4]);
            terminals[3].AddTerminal(terminals[4]);
            terminals[4].AddTerminal(lexemes[3], terminals[1], lexemes[4]);
            terminals[4].AddTerminal(lexemes[5]);
            terminals[4].AddTerminal(lexemes[1]);
            terminals[4].AddTerminal(lexemes[0]);

            terminals[0].SetForAll(0,lexemes[5],lexemes[3],lexemes[6],lexemes[1],lexemes[0]);
            terminals[1].SetForAll(0, lexemes[5], lexemes[3], lexemes[6], lexemes[1], lexemes[0]);
            terminals[2].SetForAll(0, lexemes[2]);
            terminals[2].SetForAll(1, lexemes[4]);
            terminals[3].SetForAll(1, lexemes[5], lexemes[3], lexemes[1], lexemes[0]);
            terminals[3].SetForAll(0, lexemes[6]);
            terminals[4].SetForAll(0, lexemes[3]);
            terminals[4].SetForAll(1, lexemes[5]);
            terminals[4].SetForAll(2, lexemes[1]);
            terminals[4].SetForAll(3, lexemes[0]);
        }
        private void fillTheLexemes()
        {
            lexemes = new[]
            {
                new Lexeme(1, "Идентификатор", "[a-zA-Z]+"),
                new Lexeme(2, "Константа", "[01]"),
                new Lexeme(3, "Операция"),
                new Lexeme(4, "Скобка"),
                new Lexeme(5, "Скобка"),
                new Lexeme(6, "Предикат", @"[a-zA-Z]+\([^,]+(,[^,]+)*\)"), 
                new Lexeme(7, "Отрицание"),
            };

            lexemes[2].Lexemes.AddRange(new[] { "&", "|", "^", "->" });
            lexemes[3].Lexemes.AddRange(new[] { "(" });
            lexemes[4].Lexemes.AddRange(new[] { ")" });
            lexemes[6].Lexemes.AddRange(new[] { "!" });
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var t = textBox1.Text;
            var f = lexemes.FirstOrDefault(l => l.isCorrect(t));
            label1.Text = f == null ? "Неизвестная лексема." : f.Type;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            dataGridView5.Rows.Clear();
            fillTheLexemes();
            isGood = true;
            var t = new string(textBox2.Text.Where(c => c != ' ').ToArray());
            List<string> resLexemes = new List<string>();
            for (int index = 0; index < t.Length; )
            {

                try
                {
                    resLexemes.Add(FindLexem(t, ref index));
                }
                catch (ArgumentException ex)
                {
                    label2.Text = ex.Message;
                    isGood = false;
                    return;
                }
                catch (System.IndexOutOfRangeException)
                {
                    label2.Text = "Скобка в предикате не закрыта.";
                    isGood = false;
                    return;
                }
            }
            int oldIndex = 0, prevLength = -1;
            var predLexemes = new List<string>();
            while (lexemes[5].Lexemes.Count != prevLength)
            {
                prevLength = lexemes[5].Lexemes.Count;
                for (; oldIndex < prevLength; oldIndex++)
                {
                    var pred = lexemes[5].Lexemes[oldIndex];
                    predLexemes.Clear();
                    var param = pred.SkipWhile(c => c != '(').ToList();
                    predLexemes.Add(pred.Substring(0, pred.Length - param.Count + 1));
                    param.RemoveAt(param.Count - 1);
                    param.RemoveAt(0);
                    var temp = new string(param.ToArray());
                    for (int i = 0; i < temp.Length; )
                    {
                        try
                        {
                            predLexemes.Add(FindLexem(temp, ref i));

                            if (temp.Length != i && temp[i] == ',')
                            {
                                i++;
                                predLexemes.Add(",");
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            label2.Text = ex.Message;
                            isGood = false;
                            return;
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            label2.Text = "Скобка в предикате не закрыта.";
                            isGood = false;
                            return;
                        }
                    }
                    predLexemes.Add(")");
                    lexemes[5].Lexemes[oldIndex] = predLexemes.Aggregate("", (seed, str) => seed + str);
                }

            }
            label2.Text = "";
            resLexemes.ForEach(s => label2.Text += s);
            for (int i = 0; i < lexemes.Length; i++)
            {
                for (int j = 0; j < lexemes[i].Lexemes.Count; j++)
                {
                    fillTheTable(i, j, lexemes[i].Lexemes[j]);
                }
            }
        }

        private string FindLexem(string s, ref int index)
        {
            var str = s.Substring(index);
            for (int i = lexemes.Length - 1; i >= 0; i--)
            {
                var t = lexemes[i].StartWith(str);
                if (t == null)
                    continue;
                if (!lexemes[i].Lexemes.Contains(t))
                    lexemes[i].Lexemes.Add(t);
                index += t.Length;
                return String.Format("({0},{1})", lexemes[i].Id, lexemes[i].Lexemes.IndexOf(t));

            }
            throw new ArgumentException("Неизвестная лексема - " + s[index] + ".");
        }
        private void fillTheTable(int index, int indexOfElement, string t)
        {
            switch (index)
            {
                case 0:
                    dataGridView1.Rows.Add(indexOfElement, t);
                    break;
                case 1:
                    dataGridView2.Rows.Add(indexOfElement, t);
                    break;
                case 2:
                case 6:
                    dataGridView3.Rows.Add(indexOfElement, t);
                    break;
                case 3:
                case 4:
                    dataGridView5.Rows.Add(indexOfElement, t);
                    break;
                case 5:
                    dataGridView4.Rows.Add(indexOfElement, t);
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox3.Text;
            button2_Click(sender, e);
            if (!isGood)
            {
                MessageBox.Show("Неизвестная лексима");
                richTextBox1.Text = "";
                richTextBox1.BackColor = Color.White;
                textBox3.BackColor = Color.White;
                return;
            }
            var tuple = analyze(label2.Text + "#");
            bool res = tuple.Item1;
            richTextBox1.Text = "Проверка " + textBox3.Text + Environment.NewLine + tuple.Item2;
            foreach (var j in lexemes[5].Lexemes)
            {
                var param = j.SkipWhile(c => c != '(').ToList();
                param.RemoveAt(param.Count - 1);
                param.RemoveAt(0);
                //var temp = new string(param.ToArray());
                var p = new List<string>();
                int ind = 0;
                int size = sizeLexeme(new string(param.ToArray()), ind);
                while ((ind+size)<=param.Count)
                {
                    if(param.Count==(ind+size) || param[ind + size]==','){
                        p.Add(new string(param.Take(ind + size).ToArray()));
                        param = param.Skip(ind + size + 1).ToList();
                        ind = 0;
                    }
                    ind += size;
                    size = sizeLexeme(new string(param.ToArray()), ind);
                }
                if(param.Count!=0) p.Add(new string(param.ToArray()));
                foreach (var temp1 in p)
                {
                    tuple = analyze(temp1 + "#");
                    res &= tuple.Item1; 
                    richTextBox1.Text += Environment.NewLine + Environment.NewLine+"Проверка " + temp1 + Environment.NewLine + tuple.Item2;
                }
            }
            richTextBox1.BackColor = res ? Color.White : Color.Red;
            textBox3.BackColor = res ? Color.Green : Color.White;
        }

        private Tuple<bool, string> analyze(string text)
        {            
            Stack<object> stack = new Stack<object>();
            stack.Push(terminals[0]);
            int index = 0;
            StringBuilder log = new StringBuilder(), temp;
            StringBuilder left = new StringBuilder(Environment.NewLine);
            List<string> right = new List<string>{terminals[0].Name};
            while (stack.Count != 0)
            {
                Lexeme l = getLexeme(text, index);
                object lex = stack.Pop();
                right.RemoveAt(0);
                if(l==null){
                    if(lex.Equals(Terminal.End))
                        return new Tuple<bool,string>(true, log.ToString());
                    if (!((lex is Terminal) && ((Terminal)lex).haveEmpty()))
                        return new Tuple<bool, string>(false, log.ToString());
                }
                else if(l.Equals(lex)){
                    left.Append(l.Type + " ");
                    index += sizeLexeme(text, index);
                }
                else{
                    Terminal t = lex as Terminal;
                    var next = t[l];
                    if(next == null){
                        return new Tuple<bool, string>(false, log.ToString());
                    }
                    else if(!(next.Count==1 && (next[0] is Terminal) && (next[0].Equals(Terminal.Empty)))){
                        for(int i = next.Count - 1; i>=0; i--)
                            stack.Push(next[i]);
                        right.InsertRange(0, next.Select(o=>o.ToString()));
                    }
                }
                temp = new StringBuilder();
                log.Append(left + " => " + right.Aggregate(temp, (t,i)=>t.Append(i)));
            }
            return new Tuple<bool,string>(false, log.ToString());
        }

        private Lexeme getLexeme(string text, int index)
        {
            if(text[index]=='#') return null;
            //int ind = text[index+1] - '0';
            string temp = new string(text.Skip(index + 1).TakeWhile(c => c != ',').ToArray());
            int ind = Convert.ToInt32(temp);
            if (ind > 9)
                ind = ind;
            return lexemes[ind-1];
        }

        private int sizeLexeme(string text, int index)
        {
            return  text.Skip(index).TakeWhile(c => c != ')').Count() + 1;
        }
    }
}