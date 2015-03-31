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
            lexemes = new[]
            {
                new Lexeme(1, "Идентификатор", "[a-zA-Z]+"),
                new Lexeme(2, "Константа", "[01]"),
                new Lexeme(3, "Операция"),
                new Lexeme(4, "Скобка"),
                //new Lexeme(5, "Предикат", @"[a-zA-Z]+\((\([0-9]+,[0-9]+\))+(,(\([0-9]+,[0-9]+\))+)*\)")
                new Lexeme(5, "Предикат", @"[a-zA-Z]+\([^,]+(,[^,]+)*\)"), 
            };

            lexemes[2].Lexemes.AddRange(new[] { "&", "|", "^", "->", "!" });
            lexemes[3].Lexemes.AddRange(new[] {"(", ")"});
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var t = textBox1.Text;
            var f = lexemes.FirstOrDefault(l => l.isCorrect(t));
            label1.Text = f == null ? "Неизвестная лексема." : f.Type;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = textBox2.Text;
            List<string> resLexemes = new List<string>();
            for (int index = 0; index < t.Length;)
            {
                try
                {
                    resLexemes.Add(FindLexem(t, ref index));
                }
                catch (ArgumentException ex)
                {
                    label2.Text = ex.Message;
                }
            }
        }

        private string FindLexem(string s, ref int index)
        {
            var str = s.Substring(index);
            bool undefind = true;
            for (int i = lexemes.Length - 1; i >= 0; i--)
            {
                var t = lexemes[i].StartWith(str);
                if(t==null)
                    continue;
                if (!lexemes[i].Lexemes.Contains(t))
                    lexemes[i].Lexemes.Add(t);
                index += t.Length;
                return String.Format("({0},{1})", i, lexemes[i].Lexemes.IndexOf(t));

            }
            if(undefind)
                throw new ArgumentException("Неизвестная лексема.");
        }
    }
}
