using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppleBasic_IDE
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {


            //default reserved line number
            //100 - 990     Main Program
            //1000 - 1990   Screen Drawing
            //2000 - 2990   Input
            //3000 - 3990   File / Data
            //9000 - 9990   Utility Subs


        }
    }
}
