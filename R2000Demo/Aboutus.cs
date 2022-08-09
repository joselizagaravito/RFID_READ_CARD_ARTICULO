using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace R2000Demo
{
    public partial class Aboutus : Form
    {
        public Aboutus()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            if (0 == ReaderParams.LanguageFlag)
            {
                Text = "关于我们";
            }
            else
            {
                Text = "About us";
            }
        }        
    }
}
