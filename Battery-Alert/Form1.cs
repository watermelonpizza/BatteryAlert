using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Threading;

namespace Battery_Alert
{
    public partial class Form1 : Form
    {
        System.Timers.Timer t = new System.Timers.Timer(60000);
        bool showpopup = true;

        public Form1()
        {
            InitializeComponent();

            t.Elapsed += t_Elapsed;
            t.Start();

            notifyIcon1.Visible = true;
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ObjectQuery query = new ObjectQuery("Select EstimatedChargeRemaining FROM Win32_Battery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                int charge = Convert.ToInt32(mo["EstimatedChargeRemaining"]);

                if (charge > 99 && showpopup)
                {
                    MessageBox.Show("Battery level: 100%", "Battery Charged");
                    showpopup = false;
                }
                else if (charge < 99)
                    showpopup = true;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
        }
    }
}
