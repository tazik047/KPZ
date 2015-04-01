using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        private Lexeme[] lexemes;
        public Form1()
        {
            InitializeComponent();
            fillTheLexemes();
        }
        private void fillTheLexemes()
        {
            lexemes = new[]
            {
                new Lexeme(1, "Идентификатор", "[a-zA-Z]+"),
                new Lexeme(2, "Константа", "[01]"),
                new Lexeme(3, "Операция"),
                new Lexeme(4, "Скобка"),
                new Lexeme(5, "Предикат", @"[a-zA-Z]+\([^,]+(,[^,]+)*\)"), 
            };

            lexemes[2].Lexemes.AddRange(new[] { "&", "|", "^", "->", "!" });
            lexemes[3].Lexemes.AddRange(new[] { "(", ")" });
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
            var t = textBox2.Text;
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
                    return;
                }
            }
            int oldIndex = 0,prevLength = -1;
            var predLexemes = new List<string>();
            while (lexemes[4].Lexemes.Count != prevLength)
            {
                prevLength = lexemes[4].Lexemes.Count;
                for (; oldIndex<prevLength; oldIndex++)
                {
                    var pred = lexemes[4].Lexemes[oldIndex];
                    predLexemes.Clear();
                    var param = pred.SkipWhile(c => c != '(').ToList();
                    param.RemoveAt(param.Count - 1);
                    param.RemoveAt(0);
                    var temp = new string(param.ToArray());
                    for (int i = 0; i < temp.Length;)
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
                            return;
                        }
                    }
                    lexemes[4].Lexemes[oldIndex] = predLexemes.Aggregate("", (seed, str) => seed + str);
                }
                
            }
            label2.Text = "";
            resLexemes.ForEach(s=>label2.Text+=s);
            for (int i = 0; i < lexemes.Length; i++)
            {
                for (int j = 0; j< lexemes[i].Lexemes.Count; j++)
                {
                    fillTheTable(i, j, lexemes[i].Lexemes[j]);
                }
            }
            fillTheLexemes();
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
                return String.Format("({0},{1})", i, lexemes[i].Lexemes.IndexOf(t));

            }
            throw new ArgumentException("Неизвестная лексема - "+s[index]+".");
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
                    dataGridView3.Rows.Add(indexOfElement, t);
                    break;
                case 3:
                    dataGridView5.Rows.Add(indexOfElement, t);
                    break;
                case 4:
                    dataGridView4.Rows.Add(indexOfElement, t);
                    break;
            }
        }
    }
}
