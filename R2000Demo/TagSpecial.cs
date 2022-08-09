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
    public partial class TagSpecial : Form
    {
        public TagSpecial()
        {
            InitializeComponent();

            if (0 == ReaderParams.LanguageFlag)
            {
                Text                    = "特殊功能";
                label1.Text             = "距离控制";
                bt_QTParaSet.Text       = "设置";
                bt_QTParaGet.Text       = "获取";
            }
            else
            {
                Text                    = "Special Function";
                label1.Text             = "Range Control";
                bt_QTParaSet.Text       = "Set";
                bt_QTParaGet.Text       = "Get";
                cB_RangeCt.Location     = new System.Drawing.Point(90, 12); 
            }

            cB_RangeCt.SelectedIndex    = 1;
            cB_MemoryMap.SelectedIndex  = 1;
            lb_QTResult.Text            = "";
        }

        private void bt_QTParaSet_Click(object sender, EventArgs e)
        {

        }
    }
}
