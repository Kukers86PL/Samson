using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Samson
{
    public partial class Form1 : Form
    {
        private Form2 form2;

        public Form1()
        {
            InitializeComponent();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form2 = new Form2();
            form2.Show();
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
