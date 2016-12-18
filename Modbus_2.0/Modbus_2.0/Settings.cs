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

namespace Modbus_2._0
{
    public partial class Settings : Form
    {

        int NumberOfAddressesMustRead;
        int[] queue;
        int[,] values;
        string[,] param;

        public Settings()
        {
            InitializeComponent();
            tbPathToSettingFile.ReadOnly = true;
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdSettingsFile = new OpenFileDialog();
            ofdSettingsFile.InitialDirectory = Application.StartupPath;
            ofdSettingsFile.Filter = "settings file (*.xml)|*.xml";
            ofdSettingsFile.RestoreDirectory = true;

            if (ofdSettingsFile.ShowDialog() == DialogResult.OK)
            {
                tbPathToSettingFile.Text = ofdSettingsFile.FileName;
                
                ToolTip toolTipPathToSettingsFile = new ToolTip();
                toolTipPathToSettingsFile.SetToolTip(tbPathToSettingFile, ofdSettingsFile.FileName);

                ReadSettingsFile(tbPathToSettingFile.Text);
            }
        }

        private void ReadSettingsFile(string PathToSettingsFile)
        {
            try {
                XmlDocument xml = new XmlDocument();
                xml.Load(PathToSettingsFile);
                if (xml.GetElementsByTagName("SettingsTable").Count > 0)
                {
                    NumberOfAddressesMustRead = xml.GetElementsByTagName("Row").Count;
                    InitializeTables(NumberOfAddressesMustRead);
                    ReadValuesFromSettingsFile(xml);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Невірний файл!");
            }
        }

        private void ReadValuesFromSettingsFile(XmlDocument xml)
        {
            throw new NotImplementedException();
        }

        private void InitializeTables(int numberOfAddressesMustRead)
        {
            values = new int[NumberOfAddressesMustRead, 4];
            param = new string[NumberOfAddressesMustRead, 8];
            queue = new int[NumberOfAddressesMustRead];
        }

       
    }
}
