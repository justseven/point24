using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentCheckOn
{
    public partial class Form1 : Form
    {
        private Calc24Points calc;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] inputStrs = this.textBox1.Text.Split(',');
            int[] inputInt = new int[inputStrs.Length];
            for (int i = 0; i < 4; i++)
            {
                inputInt[i] = int.Parse(inputStrs[i]);
            }
            calc = new Calc24Points(inputInt[0], inputInt[1], inputInt[2], inputInt[3]);
            this.richTextBox1.Text = calc.ToString();
            Clipboard.SetText(calc.ToString());
        }
    }
}
