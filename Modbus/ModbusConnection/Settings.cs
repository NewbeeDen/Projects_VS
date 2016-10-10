using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace ModbusConnection
{

    public partial class Settings : Form
    {
        TextBox[,] tb = new TextBox[300, 7];
        ComboBox[] cb = new ComboBox[300];
        int[] arCoord = {13, 271, 395, 643, 767, 891, 1015};
        int[] arSize = {256, 122, 122, 122, 122, 122, 122};
        int NumberOfStrings;
        Button[] btDelete = new Button[300];
              
        public Settings()
        {
            InitializeComponent();
            ReadSettings();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //При открытии формы настроек, считываем уже имеющиеся настройки с файла
        private void ReadSettings()
        {
            if (File.Exists("Settings.xml"))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("Settings.xml");
                BindingList<SettingsData> sd = new BindingList<SettingsData>();

                foreach (XmlElement element in xml.GetElementsByTagName("Row"))
                {
                    if (element.Attributes["Name"] == null)
                        continue;
                    SettingsData setting_line = new SettingsData();
                    setting_line.Name = element.Attributes["Name"].Value;
                    setting_line.IP = element.Attributes["IP"].Value;
                    setting_line.Address = element.Attributes["Address"].Value;
                    setting_line.Type = element.Attributes["Type"].Value;
                    setting_line.Check_delay = Convert.ToInt16(element.Attributes["TimerDelay"].Value);
                    setting_line.ID = element.Attributes["FileName"].Value;
                    sd.Add(setting_line);
                }
                //dataGridView1.DataSource = sd;
                dataGridView1.Columns.Add("Name", "Найменування");
                dataGridView1.Columns.Add("IP", "ІР сервера");
                dataGridView1.Columns.Add("Address", "Адреса");
                DataGridViewComboBoxCell cbc = new DataGridViewComboBoxCell();
                cbc.Items.AddRange(new string[] { "BOOL", "INT" });
                DataGridViewColumn combo = new DataGridViewColumn(cbc);
                combo.DefaultCellStyle.NullValue = "BOOL";
                dataGridView1.Columns.Add(combo);
                dataGridView1.Columns[3].HeaderText = "Тип";
                dataGridView1.Columns.Add("TimerDelay", "Інтервал часу");
                dataGridView1.Columns.Add("FileName", "Ідентифікатор");
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView1.Columns[0].Width = 600;
                for (int i = 0; i < sd.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = sd[i].Name;
                    dataGridView1[1, i].Value = sd[i].IP;
                    dataGridView1[2, i].Value = sd[i].Address;
                    if (sd[i].Type == "BOOL")
                    {
                        dataGridView1[3, i].Value = "BOOL";
                    }
                    else
                    {
                        dataGridView1[3, i].Value = "INT";
                    }
                    dataGridView1[4, i].Value = sd[i].Check_delay;
                    dataGridView1[5, i].Value = sd[i].ID;
                    //if (i < sd.Count)
                    //{
                    //    dataGridView1.Rows.Add();
                    //}
                }
                //dataGridView1.Rows.RemoveAt(sd.Count);
                //dataGridView1.Refresh();
                //dataGridView1.Columns[3].
                NumberOfStrings = sd.Count;
            }
            
            //int x = 0;
            //string str;
            //if (System.IO.File.Exists(Application.StartupPath + "\\Settings.txt"))
            //{
            //    StreamReader sr = new StreamReader("Settings.txt", Encoding.Default);
            //    do { 
            //        str = sr.ReadLine();
            //        if (str != null && str != "")
            //        {
            //            string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //            for (int i = 0; i < 7; i++)
            //            {
            //                tb[x, i] = new System.Windows.Forms.TextBox();
            //                tb[x, i].Location = new System.Drawing.Point(arCoord[i], 70 + x * 22);
            //                tb[x, i].Size = new System.Drawing.Size(arSize[i], 23);
            //                tb[x, i].BorderStyle = BorderStyle.FixedSingle;
            //                Controls.Add(tb[x, i]);
            //                if (i >= 3) tb[x, i].Text = words[i + 1];
            //                else tb[x, i].Text = words[i];
            //            }

            //            cb[x] = new System.Windows.Forms.ComboBox();
            //            cb[x].Location = new System.Drawing.Point(519, 70 + x * 22);
            //            cb[x].Size = new System.Drawing.Size(122, 23);
            //            cb[x].Items.AddRange(new string[] { "BOOL", "INT", "REAL" });
            //            cb[x].SelectedIndex = 0;
            //            Controls.Add(cb[x]);
            //            if (words[3] == "BOOL")
            //            {
            //                cb[x].SelectedIndex = 0;
            //            }
            //            else if (words[3] == "INT")
            //            {
            //                cb[x].SelectedIndex = 1;
            //            }
            //            else if (words[3] == "REAL")
            //            {
            //                cb[x].SelectedIndex = 2;
            //            }

            //            btDelete[x] = new System.Windows.Forms.Button();
            //            btDelete[x].Location = new System.Drawing.Point(1139, 69 + x * 22);
            //            btDelete[x].Size = new System.Drawing.Size(23, 23);
            //            btDelete[x].BackgroundImage = ModbusConnection.Properties.Resources.Buttons_accept_and_delete;
            //            btDelete[x].BackgroundImageLayout = ImageLayout.Stretch;
            //            btDelete[x].Tag = x.ToString();
            //            Controls.Add(btDelete[x]);
            //            btDelete[x].Click += new EventHandler(DelPosition);

            //            x++;
            //        } 
            //    } while (str != null);
            //    sr.Close();
            //}
            //return x;
        }

        private void DelPosition(object sender, EventArgs e)
        {
            
            ////Получаем номер удаляемой строки и удаляем элементы
            //int x = Convert.ToInt32((sender as Button).Tag);

            ////Смещение расположения оставшихся элементов
            //for (int i = x; i < NumberOfStrings - 1; i++)
            //{
            //    for (int j = 0; j < 7; j++)
            //    {
            //        tb[i, j].Text = tb[i + 1, j].Text;
            //    }
            //    cb[i].SelectedIndex = cb[i + 1].SelectedIndex; 
            //}
            //for (int i = 0; i < 7; i++)
            //{
            //    Controls.Remove(tb[NumberOfStrings, i]);
            //    tb[NumberOfStrings, i] = null;
            //}
            //Controls.Remove(cb[NumberOfStrings]);
            //cb[NumberOfStrings] = null;
            //Controls.Remove(btDelete[NumberOfStrings]);
            //btDelete[NumberOfStrings] = null;

            //NumberOfStrings--;
        }

        //Добавляем новую строку
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Add();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Перед закрытием формы запрос - сохранить ли изменения?
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dg = MessageBox.Show("Save changes?", "Attention", MessageBoxButtons.YesNo);
            if (dg == DialogResult.Yes)
            {
                XmlTextWriter writer = new XmlTextWriter("Settings.xml", System.Text.Encoding.Unicode);
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
                        writer.WriteAttributeString("Name", dgvr.Cells[0].Value.ToString());
                        writer.WriteAttributeString("IP", dgvr.Cells[1].Value.ToString());
                        writer.WriteAttributeString("Address", dgvr.Cells[2].Value.ToString());
                        writer.WriteAttributeString("Type", dgvr.Cells[3].Value.ToString());
                        writer.WriteAttributeString("TimerDelay", dgvr.Cells[4].Value.ToString());
                        writer.WriteAttributeString("FileName", dgvr.Cells[5].Value.ToString());
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
                //    StreamWriter sw = new StreamWriter("Settings.txt", false, Encoding.Default);
                //    for (int i = 0; i < NumberOfStrings; i++)
                //    {
                //        if (tb[i, 0] != null)
                //        {
                //            settingsstring = tb[i, 0].Text.ToString().Replace(' ', '_');
                //            //if (cb[i].Text == "REAL")
                //            //{
                //                sw.WriteLine(settingsstring + " " + tb[i, 1].Text.ToString().Trim() + " " + tb[i, 2].Text.ToString().Trim() + " " + cb[i].Text.ToString().Trim() + " " + tb[i, 3].Text.ToString().Trim() + " " + tb[i, 4].Text.ToString().Trim() + " " + tb[i, 5].Text.ToString().Trim() + " " + tb[i, 6].Text.ToString().Trim());
                //            //}
                //            //else
                //            //{
                //            //    sw.WriteLine(settingsstring + " " + tb[i, 1].Text.ToString().Trim() + " " + tb[i, 2].Text.ToString().Trim() + " " + cb[i].Text.ToString().Trim() + " " + tb[i, 4].Text.ToString().Trim() + " " + tb[i, 5].Text.ToString().Trim());
                //            //}
                //        }
                //    }
                //    sw.Close();
            }               
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void buttonStatusTime_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedCells[0].RowIndex;
            dataGridView1.Rows.RemoveAt(n);
        }
    }

    public class SettingsData
    {
        string _Name;
        string _IP;
        string _Address;
        string _Type;
        int _Check_delay;
        string _ID;

        public string Name { get { return _Name; } set { _Name = value; } }
        public string IP { get { return _IP; } set { _IP = value; } }
        public string Address { get { return _Address; } set { _Address = value; } }
        public string Type { get { return _Type; } set { _Type = value; } }
        public int Check_delay { get { return _Check_delay; } set { _Check_delay = value; } }
        public string ID { get { return _ID; } set { _ID = value; } }

    }
}
