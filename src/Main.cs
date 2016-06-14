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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void blocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Blocks _blocks = new Blocks();
            _blocks.MdiParent = this;
            _blocks.Show();
        }
    }
}
