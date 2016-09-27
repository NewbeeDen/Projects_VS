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

namespace ModbusConnection
{
    public partial class Settings : Form
    {
        TextBox[,] tb = new TextBox[300, 7];
        ComboBox[] cb = new ComboBox[300];
        int[] arCoord = {13, 271, 395, 643, 767, 891, 1015};
        int[] arSize = {256, 122, 122, 122, 122, 122, 122};
        string settingsstring;
        int NumberOfStrings;
        Button[] btDelete = new Button[300];
              
        public Settings()
        {
            InitializeComponent();
            NumberOfStrings = ReadSettings();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //При открытии формы настроек, считываем уже имеющиеся настройки с файла
        private int ReadSettings()
        {
            int x = 0;
            string str;
            if (System.IO.File.Exists(Application.StartupPath + "\\Settings.txt"))
            {
                StreamReader sr = new StreamReader("Settings.txt", Encoding.Default);
                do { 
                    str = sr.ReadLine();
                    if (str != null && str != "")
                    {
                        string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < 7; i++)
                        {
                            tb[x, i] = new System.Windows.Forms.TextBox();
                            tb[x, i].Location = new System.Drawing.Point(arCoord[i], 70 + x * 22);
                            tb[x, i].Size = new System.Drawing.Size(arSize[i], 23);
                            tb[x, i].BorderStyle = BorderStyle.FixedSingle;
                            Controls.Add(tb[x, i]);
                            if (i >= 3) tb[x, i].Text = words[i + 1];
                            else tb[x, i].Text = words[i];
                        }

                        cb[x] = new System.Windows.Forms.ComboBox();
                        cb[x].Location = new System.Drawing.Point(519, 70 + x * 22);
                        cb[x].Size = new System.Drawing.Size(122, 23);
                        cb[x].Items.AddRange(new string[] { "BOOL", "INT", "REAL" });
                        cb[x].SelectedIndex = 0;
                        Controls.Add(cb[x]);
                        if (words[3] == "BOOL")
                        {
                            cb[x].SelectedIndex = 0;
                        }
                        else if (words[3] == "INT")
                        {
                            cb[x].SelectedIndex = 1;
                        }
                        else if (words[3] == "REAL")
                        {
                            cb[x].SelectedIndex = 2;
                        }
                        
                        btDelete[x] = new System.Windows.Forms.Button();
                        btDelete[x].Location = new System.Drawing.Point(1139, 69 + x * 22);
                        btDelete[x].Size = new System.Drawing.Size(23, 23);
                        btDelete[x].BackgroundImage = ModbusConnection.Properties.Resources.Buttons_accept_and_delete;
                        btDelete[x].BackgroundImageLayout = ImageLayout.Stretch;
                        btDelete[x].Tag = x.ToString();
                        Controls.Add(btDelete[x]);
                        btDelete[x].Click += new EventHandler(DelPosition);

                        x++;
                    } 
                } while (str != null);
                sr.Close();
            }
            return x;
        }

        private void DelPosition(object sender, EventArgs e)
        {
            //Получаем номер удаляемой строки и удаляем элементы
            int x = Convert.ToInt32((sender as Button).Tag);

            //Смещение расположения оставшихся элементов
            for (int i = x; i < NumberOfStrings - 1; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    tb[i, j].Text = tb[i + 1, j].Text;
                }
                cb[i].SelectedIndex = cb[i + 1].SelectedIndex; 
            }
            for (int i = 0; i < 7; i++)
            {
                Controls.Remove(tb[NumberOfStrings, i]);
                tb[NumberOfStrings, i] = null;
            }
            Controls.Remove(cb[NumberOfStrings]);
            cb[NumberOfStrings] = null;
            Controls.Remove(btDelete[NumberOfStrings]);
            btDelete[NumberOfStrings] = null;

            NumberOfStrings--;
        }

        //Добавляем новую строку
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int x = 0; ; x++)
                {
                    if (tb[x,0] == null)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            tb[x, j] = new System.Windows.Forms.TextBox();
                            tb[x, j].Location = new System.Drawing.Point(arCoord[j], 70 + x * 22);
                            tb[x, j].Size = new System.Drawing.Size(arSize[j], 23);
                            tb[x, j].BorderStyle = BorderStyle.FixedSingle;
                            Controls.Add(tb[x, j]);
                        }

                        cb[x] = new System.Windows.Forms.ComboBox();
                        cb[x].Location = new System.Drawing.Point(519, 70 + x * 22);
                        cb[x].Size = new System.Drawing.Size(122, 23);
                        cb[x].Items.AddRange(new string[] { "BOOL", "INT", "REAL" });
                        cb[x].SelectedIndex = 0;
                        Controls.Add(cb[x]);

                        btDelete[x] = new System.Windows.Forms.Button();
                        btDelete[x].Location = new System.Drawing.Point(1139, 69 + x * 22);
                        btDelete[x].Size = new System.Drawing.Size(23, 23);
                        btDelete[x].BackgroundImage = ModbusConnection.Properties.Resources.Buttons_accept_and_delete;
                        btDelete[x].BackgroundImageLayout = ImageLayout.Stretch;
                        btDelete[x].Tag = x.ToString();
                        Controls.Add(btDelete[x]);
                        btDelete[x].Click += new EventHandler(DelPosition);

                        NumberOfStrings++;
                        break;
                    }
                }
                

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
                StreamWriter sw = new StreamWriter("Settings.txt", false, Encoding.Default);
                for (int i = 0; i < NumberOfStrings; i++)
                {
                    if (tb[i, 0] != null)
                    {
                        settingsstring = tb[i, 0].Text.ToString().Replace(' ', '_');
                        //if (cb[i].Text == "REAL")
                        //{
                            sw.WriteLine(settingsstring + " " + tb[i, 1].Text.ToString().Trim() + " " + tb[i, 2].Text.ToString().Trim() + " " + cb[i].Text.ToString().Trim() + " " + tb[i, 3].Text.ToString().Trim() + " " + tb[i, 4].Text.ToString().Trim() + " " + tb[i, 5].Text.ToString().Trim() + " " + tb[i, 6].Text.ToString().Trim());
                        //}
                        //else
                        //{
                        //    sw.WriteLine(settingsstring + " " + tb[i, 1].Text.ToString().Trim() + " " + tb[i, 2].Text.ToString().Trim() + " " + cb[i].Text.ToString().Trim() + " " + tb[i, 4].Text.ToString().Trim() + " " + tb[i, 5].Text.ToString().Trim());
                        //}
                    }
                }
                sw.Close();
            }               
        }

        private void buttonDellete_Click(object sender, EventArgs e)
        {

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
    }
}
