using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIM
{
    public partial class BlocksDetail : Form
    {
        public BlocksDetail(PIM.Blocks.Theme theme)
        {
            _theme = theme;
            InitializeComponent();
        }

        PIM.Blocks.Theme _theme;
        public string action;
        public string note;

        public DialogResult ShowDialog(string note)
        {
            textBox1.Text = note;

            return ShowDialog();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            action = "save";
            note = textBox1.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            action = "none";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            action = "delete";

        }

        private void BlocksDetail_Load(object sender, EventArgs e)
        {
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            Blocks b = new Blocks();
            b.ApplySkin(_theme, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Cipherer.Cipher(textBox1.Text, int.Parse(textBox2.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = Cipherer.DeCipher(textBox1.Text, int.Parse(textBox2.Text));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}