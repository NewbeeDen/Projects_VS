using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ModbusConnection
{

    public partial class VerstatSettings : Form
    {
        public BindingSource bindingSource1 = new BindingSource();
        public VerstatSettings()
        {
            InitializeComponent();
            ReadSettings();
        }

        private void ReadSettings()
        {
            if (File.Exists("VerstatSettings.xml"))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("VerstatSettings.xml");
                BindingList<VerstSettings> vs = new BindingList<VerstSettings>();
                
                foreach (XmlElement element in xml.GetElementsByTagName("Row")) // + j.ToString()))
                {
                    if (element.Attributes["ID"] == null)
                        continue;
                    VerstSettings vsOne = new VerstSettings();
                    vsOne.ID = element.Attributes["ID"].Value.ToString();
                    vsOne.IP = element.Attributes["IP"].Value.ToString();
                    vsOne.Address = element.Attributes["Address"].Value.ToString();
                    vsOne.Name = element.Attributes["Name"].Value.ToString();
                    vs.Add(vsOne);
                }
                dataGridView1.DataSource = vs;

                foreach (XmlElement elem in xml.GetElementsByTagName("Time"))
                {
                    listBoxHour.Text = elem.Attributes["Hour"].Value;
                    listBoxMinutes.Text = elem.Attributes["Minutes"].Value;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedCells[0].RowIndex;
            dataGridView1.Rows.RemoveAt(n);
        }

        private void VerstatSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dg = MessageBox.Show("Save changes?", "Attention", MessageBoxButtons.YesNo);
            if (dg == DialogResult.Yes)
            {
                XmlTextWriter writer = new XmlTextWriter("VerstatSettings.xml", System.Text.Encoding.Unicode);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();

                writer.WriteStartElement("SettingsTable");
                writer.WriteAttributeString("Type", "XML0.1");
                writer.WriteAttributeString("Version", "1.0.0.0");

                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    if (dgvr.Cells[0].Value != null)
                    {
                        writer.WriteStartElement("Row");
                        writer.WriteAttributeString("ID", dgvr.Cells[0].Value.ToString());
                        writer.WriteAttributeString("IP", dgvr.Cells[1].Value.ToString());
                        writer.WriteAttributeString("Address", dgvr.Cells[2].Value.ToString());
                        writer.WriteAttributeString("Name", dgvr.Cells[3].Value.ToString());
                        writer.WriteEndElement();
                    }
                 }
                writer.WriteStartElement("Time");
                writer.WriteAttributeString("Hour", listBoxHour.Text);
                writer.WriteAttributeString("Minutes", listBoxMinutes.Text);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Close();
            }
        }

        private void listBoxHour_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBoxMinutes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class VerstSettings
    {
        string _ID;
        string _IP;
        string _Address;
        string _Name;

        public string ID { get { return _ID; } set { _ID = value; }}
        public string IP { get { return _IP; } set { _IP = value; } }
        public string Address { get { return _Address; } set { _Address = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
    }
}
