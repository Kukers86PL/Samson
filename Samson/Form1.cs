using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
                }
            }
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(5000);
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
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Delete("settings.xml");
            refreshSettings();
        }
    }
}
