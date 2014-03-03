using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace xk3yScanner
{
    public partial class CoverForm : Form
    {
        private Image m;
        public CoverForm(Image im)
        {
            m = im;
            InitializeComponent();
            this.picBox.Image = im;
        }

        private void CoverForm_Load(object sender, EventArgs e)
        {
            if ((m.Width!=picBox.Width) || (m.Height!=picBox.Height))
            {
                int w = picBox.Width - m.Width;
                int h = picBox.Height - m.Height;
                this.Width += w;
                this.Height += h;
            }
        }
    }
}
