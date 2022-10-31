using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Samson
{
    public partial class Form1 : Form
    {
        private Form2 form2;
        private System.Timers.Timer aTimer;
        private delegate void SafeCallDelegate(Object source, ElapsedEventArgs e);

        public Form1()
        {
            InitializeComponent();
            SetTimer();
            Hide();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form2 = new Form2();
            form2.Show();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (listView1.InvokeRequired)
            {
                var d = new SafeCallDelegate(OnTimedEvent);
                listView1.Invoke(d, new object[] { source, e });
            }
            else
            {
                refreshSettings();

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    string name = listView1.Items[i].SubItems[0].Text;
                    string priority = listView1.Items[i].SubItems[1].Text;
                    string affinity = listView1.Items[i].SubItems[2].Text;

                    listView1.Items[i].SubItems[3].Text = "Error";

                    try
                    {
                        Process[] localByName = Process.GetProcessesByName(name);
                        foreach (Process p in localByName)
                        {
                            switch (priority)
                            {
                                case "Realtime":
                                    p.PriorityClass = ProcessPriorityClass.RealTime;
                                    break;
                                case "High":
                                    p.PriorityClass = ProcessPriorityClass.High;
                                    break;
                                case "Above Normal":
                                    p.PriorityClass = ProcessPriorityClass.AboveNormal;
                                    break;
                                case "Below normal":
                                    p.PriorityClass = ProcessPriorityClass.BelowNormal;
                                    break;
                                case "Low":
                                    p.PriorityClass = ProcessPriorityClass.Idle;
                                    break;
                                default:
                                    p.PriorityClass = ProcessPriorityClass.Normal;
                                    break;
                            }

                            p.ProcessorAffinity = (IntPtr)(Convert.ToInt32(affinity, 2));
                            
                            listView1.Items[i].SubItems[3].Text = "Set";
                        }
                    }
                    catch
                    {
                        // nothing to do
                    }
                }
            }
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void refreshSettings()
        {
            try
            {
                listView1.Items.Clear();
                XDocument doc = XDocument.Load("settings.xml");
                foreach (XElement xe in doc.Element("settings").Descendants("process"))
                {
                    string[] row = { xe.Element("name").Value, xe.Element("priority").Value, xe.Element("affinity").Value, "Initial" };
                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                }
            }
            catch
            {
                // nothing to do
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            refreshSettings();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            notifyIcon1.Visible = true;
            ShowInTaskbar = false;
            Hide();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Delete("settings.xml");
            refreshSettings();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                ShowInTaskbar = false;
                Hide();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            Show();
        }
    }
}
