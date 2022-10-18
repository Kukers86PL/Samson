using Samson.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Samson
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            for (int i = 0; i < Environment.ProcessorCount; ++i)
            {
                checkedListBox1.Items.Add("Processor " + i.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Load("settings.xml");
            }
            catch
            {
                doc = new XDocument();
                doc.Add(new XElement("settings"));
            }
            XElement settings = doc.Element("settings");
            XElement process = new XElement("process");
            process.Add(new XElement("name", textBox1.Text),
                       new XElement("priority", comboBox1.Text),
                       new XElement("affinity", get_affinity_string()));
            settings.Add(process);
            doc.Save("settings.xml");

            Close();
        }

        private string get_affinity_string()
        {
            string ret = "";
            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    ret += "1";
                }
                else
                {
                    ret += "0";
                }
            }
            return ret;
        }
    }
}
