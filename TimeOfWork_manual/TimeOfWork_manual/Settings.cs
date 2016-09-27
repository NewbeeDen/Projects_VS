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

namespace TimeOfWork_manual
{
    public partial class Settings : Form
    {

        TextBox[,] tb = new TextBox[300, 3];
        Button[] btDelete = new Button[300];
        ComboBox[] cbPlant = new ComboBox[300];
        int[] arCoord = {15, 167, 469};
        int[] arSize = {150, 300, 100};
        int NumberOfStrings;
        enum Plants {
            Бурякопереробний_цех,
            Газовапняковий_цех,
            Компресорне_господарство,
            Очисні_споруди,
            Продуктовий_цех,
            Сокоочисний_цех,
            ТЕЦ,
            Цех_готової_продукції
        }

        public Settings()
        {
            InitializeComponent();
            NumberOfStrings = ReadSettings();
        }

        private int ReadSettings()
        {
            int x = 0;
            string str;
            if (System.IO.File.Exists(Application.StartupPath + "\\Settings.txt"))
            {
                StreamReader sr = new StreamReader("Settings.txt", Encoding.Default);
                do
                {
                    str = sr.ReadLine();
                    if (str != null && str != "")
                    {
                        string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        cbPlant[x] = new ComboBox();
                        cbPlant[x].Location = new Point(arCoord[0], 5 + x * 22);
                        cbPlant[x].Size = new Size(arSize[0], 23);
                        foreach (var item in Enum.GetValues(typeof(Plants)))
                        {
                            cbPlant[x].Items.Add(item);
                        }
                        WorkedPlace.Controls.Add(cbPlant[x]);

                        switch (words[0])
                        {
                            case "Бурякопереробний_цех":
                                cbPlant[x].SelectedIndex = 0;
                                break;
                            case "Газовапняковий_цех":
                                cbPlant[x].SelectedIndex = 1;
                                break;
                            case "Компресорне_господарство":
                                cbPlant[x].SelectedIndex = 2;
                                break;
                            case "Очисні_споруди":
                                cbPlant[x].SelectedIndex = 3;
                                break;
                            case "Продуктовий_цех":
                                cbPlant[x].SelectedIndex = 4;
                                break;
                            case "Сокоочисний_цех":
                                cbPlant[x].SelectedIndex = 5;
                                break;
                            case "ТЕЦ":
                                cbPlant[x].SelectedIndex = 6;
                                break;
                            case "Цех_готової_продукції":
                                cbPlant[x].SelectedIndex = 7;
                                break;
                            default:
                                cbPlant[x].SelectedIndex = 0;
                                break;
                        }
                        
                        for (int i = 1; i < 3; i++)
                        {
                            tb[x, i] = new System.Windows.Forms.TextBox();
                            tb[x, i].Location = new System.Drawing.Point(arCoord[i], 5 + x * 22);
                            tb[x, i].Size = new System.Drawing.Size(arSize[i], 23);
                            tb[x, i].BorderStyle = BorderStyle.FixedSingle;
                            WorkedPlace.Controls.Add(tb[x, i]);
                            tb[x, i].Text = words[i];
                        }

                        btDelete[x] = new System.Windows.Forms.Button();
                        btDelete[x].Location = new System.Drawing.Point(569, 4 + x * 22);
                        btDelete[x].Size = new System.Drawing.Size(23, 23);
                        btDelete[x].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Delete;
                        btDelete[x].BackgroundImageLayout = ImageLayout.Stretch;
                        btDelete[x].Tag = x.ToString();
                        WorkedPlace.Controls.Add(btDelete[x]);
                        btDelete[x].Click += new EventHandler(DelPosition);

                        x++;
                    }
                } while (str != null);
                sr.Close();
            }
            return x;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            for (int x = 0; ; x++)
            {
                if (tb[x,1] == null)
                {
                    cbPlant[x] = new ComboBox();
                    cbPlant[x].Location = new Point(arCoord[0], 5 + x * 22);
                    cbPlant[x].Size = new Size(arSize[0], 23);
                    foreach (var item in Enum.GetValues(typeof(Plants)))
                    {
                        cbPlant[x].Items.Add(item);
                    }
                    WorkedPlace.Controls.Add(cbPlant[x]);
                    cbPlant[x].SelectedIndex = 0;

                    for (int j = 1; j < 3; j++)
                    {
                        tb[x, j] = new System.Windows.Forms.TextBox();
                        tb[x, j].Location = new System.Drawing.Point(arCoord[j], 5 + NumberOfStrings * 22);
                        tb[x, j].Size = new System.Drawing.Size(arSize[j], 23);
                        tb[x, j].BorderStyle = BorderStyle.FixedSingle;
                        WorkedPlace.Controls.Add(tb[x, j]);
                    }

                    btDelete[x] = new System.Windows.Forms.Button();
                    btDelete[x].Location = new System.Drawing.Point(569, 4 + NumberOfStrings * 22);
                    btDelete[x].Size = new System.Drawing.Size(23, 23);
                    btDelete[x].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Delete;
                    btDelete[x].BackgroundImageLayout = ImageLayout.Stretch;
                    btDelete[x].Tag = x.ToString();
                    WorkedPlace.Controls.Add(btDelete[x]);
                    btDelete[x].Click += new EventHandler(DelPosition);

                    NumberOfStrings++;
                    break;
                }
            }
        }

        private void DelPosition(object sender, EventArgs e)
        {
            //Получаем номер удаляемой строки и удаляем элементы
            int x = Convert.ToInt32((sender as Button).Tag);

            //Смещение расположения оставшихся элементов
            for (int i = x; i < NumberOfStrings - 1; i++)
            {
                cbPlant[i].SelectedIndex = cbPlant[i + 1].SelectedIndex;
                for (int j = 1; j < 3; j++)
                {
                    tb[i, j].Text = tb[i + 1, j].Text;
                }
            }
            WorkedPlace.Controls.Remove(cbPlant[NumberOfStrings - 1]);
            cbPlant[NumberOfStrings - 1] = null;
            for (int i = 0; i < 3; i++)
            {
                WorkedPlace.Controls.Remove(tb[NumberOfStrings - 1, i]);
                tb[NumberOfStrings - 1, i] = null;
            }
            WorkedPlace.Controls.Remove(btDelete[NumberOfStrings - 1]);
            btDelete[NumberOfStrings - 1] = null;
            NumberOfStrings--;
        }

        private void MainGroup_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dg = MessageBox.Show("Save changes?", "Attention", MessageBoxButtons.YesNo);
            if (dg == DialogResult.Yes)
            {
                StreamWriter sw = new StreamWriter("Settings.txt", false, Encoding.Default);
                for (int i = 0; i < NumberOfStrings; i++)
                {
                    if (tb[i, 1] != null)
                    {
                        sw.WriteLine(cbPlant[i].Text + " " + tb[i, 1].Text.ToString().Replace(' ', '_') + " " + tb[i, 2].Text.ToString().Replace(' ', '_'));
                    }
                }
                sw.Close();
            }
            TimeOfWork ToW = new TimeOfWork();
            ToW.Refresh();
        }
    }
}
