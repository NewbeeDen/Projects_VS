using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TimeOfWork_manual
{
    public partial class TimeOfWork : Form
    {
        int NumberOfStrings, LinesInFiles, NewStatusAdd, DelLastStatus, ElementIndex;
        string[,] param = new string[300, 3];
        string[] Plants = new string[300];
        int[] arCoord = new int[] {0, 62, 124}; 
        int[] arSize = new int[] {60, 60, 416};
        string[] FileData;
        StreamWriter sw;
        StreamReader sr;
        TextBox[,] tb;
        public int NewStatus, StatusTime;
        Button[] btNewLine, btNewStatus, btDelStatus, btDelete;
        ToolTip ttRefresh, ttSettings, ttNewLine, ttNewStatus, ttDelStatus, ttDelete;

        private class NETRESOURCE
        {
            public ResourceScope dwScope = 0;
            public ResourceType dwType = 0;
            public ResourceDisplayType dwDisplayType = 0;
            public ResourceUsage dwUsage = 0;
            public string lpLocalName = null;
            public string lpRemoteName = null;
            public string lpComment = null;
            public string lpProvider = null;
        }

        public enum ResourceScope
        {
            RESOURCE_CONNECTED = 1,
            RESOURCE_GLOBALNET,
            RESOURCE_REMEMBERED,
            RESOURCE_RECENT,
            RESOURCE_CONTEXT
        }

        public enum ResourceType
        {
            RESOURCETYPE_ANY,
            RESOURCETYPE_DISK,
            RESOURCETYPE_PRINT,
            RESOURCETYPE_RESERVED
        }

        public enum ResourceUsage
        {
            RESOURCEUSAGE_CONNECTABLE = 0x00000001,
            RESOURCEUSAGE_CONTAINER = 0x00000002,
            RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
            RESOURCEUSAGE_SIBLING = 0x00000008,
            RESOURCEUSAGE_ATTACHED = 0x00000010,
            RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        }

        public enum ResourceDisplayType
        {
            RESOURCEDISPLAYTYPE_GENERIC,
            RESOURCEDISPLAYTYPE_DOMAIN,
            RESOURCEDISPLAYTYPE_SERVER,
            RESOURCEDISPLAYTYPE_SHARE,
            RESOURCEDISPLAYTYPE_FILE,
            RESOURCEDISPLAYTYPE_GROUP,
            RESOURCEDISPLAYTYPE_NETWORK,
            RESOURCEDISPLAYTYPE_ROOT,
            RESOURCEDISPLAYTYPE_SHAREADMIN,
            RESOURCEDISPLAYTYPE_DIRECTORY,
            RESOURCEDISPLAYTYPE_TREE,
            RESOURCEDISPLAYTYPE_NDSCONTAINER
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string lpName, int dwFlags, int fForce);

        public TimeOfWork()
        {
            InitializeComponent();
            NETRESOURCE rc = new NETRESOURCE();
            rc.dwType = 0x00000000;
            rc.lpRemoteName = @"\\10.0.4.243\exchange$\ASUTP";
            rc.lpLocalName = null;
            rc.lpProvider = null;
            int ret = WNetAddConnection2(rc, "kfp314", "LazarenkoD", 0);
            ttRefresh = new ToolTip();
            ttRefresh.SetToolTip(this.buttonRefresh, "Оновити базу");
            ttSettings = new ToolTip();
            ttSettings.SetToolTip(this.Settings, "Налаштування");
            UpdateForm();
        }

        public void UpdateForm()
        {
            ElementIndex = 0;
            NumberOfStrings = ReadSettings();
            InitializeFilter();
        }

        public int ReadSettings()
        {
            int x = 0;
            string str;
            if (System.IO.File.Exists(Application.StartupPath + "\\Settings.txt"))
            {
                StreamReader sr = new StreamReader("Settings.txt", Encoding.Default);
                do
                {
                    str = sr.ReadLine();
                    if (str != null)
                    {
                        string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (words.Length > 1)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                param[x, i] = words[i];
                            }
                            x++;
                        }
                        
                    }
                } while (str != null);
                sr.Close();
            }
            return x;
        }

        public void InitializeFilter()
        {
            if (!File.Exists("Settings.txt")) File.Create("Settings.txt");
            else
            {
                int count = System.IO.File.ReadAllLines("Settings.txt").Length;
                Plants[0] = "Все";
                if (param[0, 0] != "") Plants[1] = param[0, 0];
                for (int i = 1; i < NumberOfStrings; i++)
                {
                    int j = 1;
                    for (;;)
                        if (param[i, 0] == Plants[j]) break;
                        else
                        {
                            j++;
                            if (Plants[j] == null)
                            {
                                Plants[j] = param[i, 0];
                                break;
                            }
                        }
                }
                for (int i = 0; ; i++)
                    if (Plants[i] != null) cbPlants.Items.Add(Plants[i]);
                    else break;
                cbPlants.SelectedIndex = 0;
            }
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        public void cbPlants_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbElement.Items.Clear();
            if (cbPlants.SelectedIndex == 0)
            {
                for (int i = 0; i < NumberOfStrings; i++)
                {
                    cbElement.Items.Add(param[i, 1]);
                }
                if (cbElement.Items.Count > 0) cbElement.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < NumberOfStrings; i++)
                {
                    if (param[i, 0] == cbPlants.Text)
                    {
                        cbElement.Items.Add(param[i, 1]);
                    }
                }
                if (cbElement.Items.Count > 0) cbElement.SelectedIndex = 0;
            }
        }

        //Проводим очистку данных и повторное заполнение
        public void buttonRefresh_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NumberOfStrings; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    param[i, j] = null;
                }
            }
            cbElement.Items.Clear();
            do
            {
                cbPlants.Items.RemoveAt(0);
            } while (cbPlants.Items.Count > 0);
            for (int i = 0; ; i++)
            {
                if (Plants[i] != null)
                {
                    Plants[i] = null;
                }
                else break;
            }

            UpdateForm();
        }

        private void cbElement_Enter(object sender, EventArgs e)
        {

        }

        private void TimeOfWork_Enter(object sender, EventArgs e)
        {
        }

        private void TimeOfWork_Activated(object sender, EventArgs e)
        {

        }

        private void cbElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WorkPanel.Controls.Count != 0)
            {
                DialogResult dg = MessageBox.Show("Save changes?", "Warning!", MessageBoxButtons.YesNo);
                if (dg == DialogResult.Yes)
                {
                    string filename = @"\\10.0.4.243\exchange$\ASUTP\" + param[ElementIndex, 2] + ".txt";
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    sw = new StreamWriter(filename, false, Encoding.Default);
                    for (int i = 0; i < LinesInFiles; i++)
                    {
                        if (tb[i, 2].Text.Trim() != "")
                        {
                            string[] words = tb[i, 2].Text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (words.Length > 1)
                            {
                                sw.Write(words[0]);
                                for (int j = 1; j < words.Length; j++)
                                {
                                    sw.Write("\t" + words[j]);
                                }
                            }
                            else
                            {
                                sw.WriteLine(tb[i, 0].Text + "\t" + tb[i,1].Text + "\t" + tb[i, 2].Text);
                            }
                            //sw.WriteLine(tb[i].Text.Trim());
                        }
                    }
                    sw.Close();
                    ElementIndex = cbElement.SelectedIndex;
                }
            }
            ReadFile(cbElement.SelectedIndex);
        }

        private void ReadFile(int index)
        {
            string filename = @"\\10.0.4.243\exchange$\ASUTP\" + param[index, 2] + ".txt";
            if (!File.Exists(filename))
            {
                File.AppendAllText(filename, " ");
            }
            LinesInFiles = System.IO.File.ReadAllLines(filename).Length;
            sr = new StreamReader(filename, Encoding.Default);
            if (FileData != null)
            {
                FileData = null;
            }
            FileData = new string[LinesInFiles + 30];
            for (int i = 0; i < LinesInFiles; i++)
            {
                FileData[i] = sr.ReadLine();
            }
            if (tb != null || btNewLine != null)
            {
                WorkPanel.Controls.Clear();
            }
            tb = new TextBox[LinesInFiles + 30, 3];
            btNewLine = new Button[LinesInFiles + 30];
            btNewStatus = new Button[LinesInFiles + 30];
            btDelStatus = new Button[LinesInFiles + 30];
            btDelete = new Button[LinesInFiles + 30];
            ttNewLine = new ToolTip();
            ttNewStatus = new ToolTip();
            ttDelStatus = new ToolTip();
            ttDelete = new ToolTip();

            for (int i = 0; i < LinesInFiles; i++)
            {
                string[] words = FileData[i].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                DateTime dt = DateTime.Now;
                for (int j = 0; j < 3; j++)
                {
                    tb[i, j] = new System.Windows.Forms.TextBox();
                    tb[i, j].Location = new System.Drawing.Point(arCoord[j], 5 + i * 22);
                    tb[i, j].Size = new System.Drawing.Size(arSize[j], 20);
                    tb[i, j].BorderStyle = BorderStyle.FixedSingle;
                    //tb[i, j].ReadOnly = true;
                    WorkPanel.Controls.Add(tb[i, j]);
                    string[] datetime = dt.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 2) { if (j < 2) tb[i, j].Text = datetime[j]; }
                    else {
                        if (j < 2) tb[i, j].Text = words[j];
                        else {
                            tb[i, j].Text = words[j];
                            if (words.Length > 3)
                                for (int x = 3; x < words.Length; x++)
                                {
                                    tb[i, j].Text = tb[i, j].Text + "\t" + words[x];
                                }
                        }
                    }
                }
                
                btNewLine[i] = new System.Windows.Forms.Button();
                btNewLine[i].Location = new System.Drawing.Point(542, 4 + i * 22);
                btNewLine[i].Size = new System.Drawing.Size(23, 23);
                btNewLine[i].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Add;
                btNewLine[i].BackgroundImageLayout = ImageLayout.Stretch;
                btNewLine[i].Tag = i.ToString();
                ttNewLine.SetToolTip(this.btNewLine[i], "Вставити строку після");
                WorkPanel.Controls.Add(btNewLine[i]);
                btNewLine[i].Click += new EventHandler(InsertNewLine);

                btNewStatus[i] = new System.Windows.Forms.Button();
                btNewStatus[i].Location = new System.Drawing.Point(567, 4 + i * 22);
                btNewStatus[i].Size = new System.Drawing.Size(23, 23);
                btNewStatus[i].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Next;
                btNewStatus[i].BackgroundImageLayout = ImageLayout.Stretch;
                btNewStatus[i].Tag = i.ToString();
                ttNewStatus.SetToolTip(this.btNewStatus[i], "Добавити статус");
                WorkPanel.Controls.Add(btNewStatus[i]);
                btNewStatus[i].Click += new EventHandler(NewStatusField);

                btDelStatus[i] = new System.Windows.Forms.Button();
                btDelStatus[i].Location = new System.Drawing.Point(592, 4 + i * 22);
                btDelStatus[i].Size = new System.Drawing.Size(23, 23);
                btDelStatus[i].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Cancel;
                btDelStatus[i].BackgroundImageLayout = ImageLayout.Stretch;
                btDelStatus[i].Tag = i.ToString();
                ttDelStatus.SetToolTip(this.btDelStatus[i], "Видалити останній статус");
                WorkPanel.Controls.Add(btDelStatus[i]);
                btDelStatus[i].Click += new EventHandler(DelStatus);

                btDelete[i] = new System.Windows.Forms.Button();
                btDelete[i].Location = new System.Drawing.Point(617, 4 + i * 22);
                btDelete[i].Size = new System.Drawing.Size(23, 23);
                btDelete[i].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Delete;
                btDelete[i].BackgroundImageLayout = ImageLayout.Stretch;
                btDelete[i].Tag = i.ToString();
                ttDelete.SetToolTip(this.btDelete[i], "Видалити строку");
                WorkPanel.Controls.Add(btDelete[i]);
                btDelete[i].Click += new EventHandler(DeleteLine);
            }
            sr.Close();
        }

        private void InsertNewLine(object sender, EventArgs e)
        {
            int x = Convert.ToInt32((sender as Button).Tag);
            DateTime dt = DateTime.Now;
            string[] datetime = dt.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 3; i++)
            {
                tb[LinesInFiles, i] = new TextBox();
                tb[LinesInFiles, i].Location = new System.Drawing.Point(arCoord[i], 5 + LinesInFiles * 22);
                tb[LinesInFiles, i].Size = new System.Drawing.Size(arSize[i], 20);
                tb[LinesInFiles, i].BorderStyle = BorderStyle.FixedSingle;
                //tb[LinesInFiles].ReadOnly = true;
                WorkPanel.Controls.Add(tb[LinesInFiles, i]);
            }
            
            btNewLine[LinesInFiles] = new Button();
            btNewLine[LinesInFiles].Location = new System.Drawing.Point(542, 4 + LinesInFiles * 22);
            btNewLine[LinesInFiles].Size = new System.Drawing.Size(23, 23);
            btNewLine[LinesInFiles].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Add;
            btNewLine[LinesInFiles].BackgroundImageLayout = ImageLayout.Stretch;
            btNewLine[LinesInFiles].Tag = LinesInFiles.ToString();
            ttNewLine.SetToolTip(this.btNewLine[LinesInFiles], "Вставити строку після");
            WorkPanel.Controls.Add(btNewLine[LinesInFiles]);
            btNewLine[LinesInFiles].Click += new EventHandler(InsertNewLine);
            btNewStatus[LinesInFiles] = new System.Windows.Forms.Button();
            btNewStatus[LinesInFiles].Location = new System.Drawing.Point(567, 4 + LinesInFiles * 22);
            btNewStatus[LinesInFiles].Size = new System.Drawing.Size(23, 23);
            btNewStatus[LinesInFiles].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Next;
            btNewStatus[LinesInFiles].BackgroundImageLayout = ImageLayout.Stretch;
            btNewStatus[LinesInFiles].Tag = LinesInFiles.ToString();
            ttNewStatus.SetToolTip(this.btNewStatus[LinesInFiles], "Добавити статус");
            WorkPanel.Controls.Add(btNewStatus[LinesInFiles]);
            btNewStatus[LinesInFiles].Click += new EventHandler(NewStatusField);
            btDelStatus[LinesInFiles] = new System.Windows.Forms.Button();
            btDelStatus[LinesInFiles].Location = new System.Drawing.Point(592, 4 + LinesInFiles * 22);
            btDelStatus[LinesInFiles].Size = new System.Drawing.Size(23, 23);
            btDelStatus[LinesInFiles].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Cancel;
            btDelStatus[LinesInFiles].BackgroundImageLayout = ImageLayout.Stretch;
            btDelStatus[LinesInFiles].Tag = LinesInFiles.ToString();
            ttDelStatus.SetToolTip(this.btDelStatus[LinesInFiles], "Видалити останній статус");
            WorkPanel.Controls.Add(btDelStatus[LinesInFiles]);
            btDelStatus[LinesInFiles].Click += new EventHandler(DelStatus);
            btDelete[LinesInFiles] = new System.Windows.Forms.Button();
            btDelete[LinesInFiles].Location = new System.Drawing.Point(617, 4 + LinesInFiles * 22);
            btDelete[LinesInFiles].Size = new System.Drawing.Size(23, 23);
            btDelete[LinesInFiles].BackgroundImage = TimeOfWork_manual.Properties.Resources.Button_Delete;
            btDelete[LinesInFiles].BackgroundImageLayout = ImageLayout.Stretch;
            btDelete[LinesInFiles].Tag = LinesInFiles.ToString();
            ttDelete.SetToolTip(this.btDelete[LinesInFiles], "Видалити строку");
            WorkPanel.Controls.Add(btDelete[LinesInFiles]);
            btDelete[LinesInFiles].Click += new EventHandler(DeleteLine);

            for (int i = LinesInFiles; i > x + 1; i--)
            {
                for (int j = 0; j < 3; j++)
                {
                    tb[i, j].Text = tb[i - 1, j].Text;
                    tb[i, j].Location = new System.Drawing.Point(arCoord[j], 5 + i * 22);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                tb[x + 1, i].Text = datetime[i];
            }
            LinesInFiles++;
        }

        private void NewStatusField(object sender, EventArgs e)
        {
            NewStatusAdd = Convert.ToInt32((sender as Button).Tag);
            NewStatus ns = new NewStatus(this);
            this.Enabled = false;
            ns.Location = new System.Drawing.Point(this.Left, this.Top);
            ns.Show();
        }

        public void StatusAdd()
        {
            if (NewStatusAdd != -1)
            {
                tb[NewStatusAdd, 2].Text = tb[NewStatusAdd, 2].Text + "\t" + NewStatus.ToString() + "\t" + StatusTime.ToString();
            }
        }

        private void DelStatus(object sender, EventArgs e)
        {
            DelLastStatus = Convert.ToInt32((sender as Button).Tag);
            string[] words = tb[DelLastStatus, 2].Text.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            tb[DelLastStatus, 2].Text = "";
            for (int i = 0; i < words.Length - 2; i++)
            {
                if (i == 0)
                {
                    tb[DelLastStatus, 2].Text = tb[DelLastStatus, 2].Text + words[i];
                }
                else tb[DelLastStatus, 2].Text = tb[DelLastStatus, 2].Text + "\t" + words[i];
            } 
        }

        private void DeleteLine(object sender, EventArgs e)
        {
            int DelPosition = Convert.ToInt32((sender as Button).Tag);
            for (int i = 0; i < 3; i++)
            {
                tb[DelPosition, i].Text = "";
            }
            for (int i = DelPosition; i < LinesInFiles - 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tb[i, j].Text = tb[i + 1, j].Text;
                }
            }
            for(int i = 0; i < 3; i++)
            {
                WorkPanel.Controls.Remove(tb[LinesInFiles - 1, i]);
                tb[LinesInFiles - 1, i] = null;
            }
            WorkPanel.Controls.Remove(btNewLine[LinesInFiles - 1]);
            btNewLine[LinesInFiles - 1] = null;
            WorkPanel.Controls.Remove(btNewStatus[LinesInFiles - 1]);
            btNewStatus[LinesInFiles - 1] = null;
            WorkPanel.Controls.Remove(btDelStatus[LinesInFiles - 1]);
            btDelStatus[LinesInFiles - 1] = null;
            WorkPanel.Controls.Remove(btDelete[LinesInFiles - 1]);
            btDelete[LinesInFiles - 1] = null;
            LinesInFiles--;
        }
    }
}
