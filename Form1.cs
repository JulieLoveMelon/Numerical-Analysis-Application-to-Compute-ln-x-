using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ln
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text;
            BigNumber numinput = new BigNumber();
            numinput = numinput.StringToBn(input);

            BigNumber answer1 = numinput.Taylor(numinput);
            string mystr1 = answer1.output();
            textBox2.Text = mystr1;

            BigNumber answer2 = numinput.Taylor_Former(numinput);
            string mystr2 = answer2.output();
            textBox3.Text = mystr2;

            BigNumber answer3 = numinput.RK(numinput);
            string mystr3 = answer3.output();
            textBox4.Text = mystr3;

            BigNumber answer4 = numinput.Newton(numinput);
            string mystr4 = answer4.output();
            textBox5.Text = mystr4;

            BigNumber answer5 = numinput.Trapezoid(numinput);
            string mystr5 = answer5.output();
            textBox6.Text = mystr5;
        }
    }
}
