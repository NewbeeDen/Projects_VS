using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeOfWork_manual
{
    public partial class NewStatus : Form
    {
        public NewStatus()
        {
            InitializeComponent();
        }

        private TimeOfWork ToW;

        public NewStatus(TimeOfWork _ToW)
        {
            InitializeComponent();
            ToW = _ToW;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            
            if (tbNewStatus.Text != "" || tbStatusTime.Text != "")
            {
                //ToW = new TimeOfWork();
                ToW.NewStatus = Convert.ToInt32(tbNewStatus.Text);
                ToW.StatusTime = Convert.ToInt32(tbStatusTime.Text);
                ToW.Enabled = true;
                ToW.StatusAdd();
            }
            this.Close();
        }

        private void NewStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void NewStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ToW.Enabled == false) ToW.Enabled = true;
        }

        private void tbNewStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbStatusTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
