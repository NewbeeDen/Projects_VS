using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Data;
//using ModbusTCP;

namespace Modbus_2._0
{
    public partial class MainForm : Form
    {
        private Timer[] Timers { get; set; }
        int _connected, _queuenow = 0;
/*
        List<Values> _values = new List<Values>();
*/
        List<DataTable> _dtList = new List<DataTable>();
        private readonly BindingList<DgvList> _dgvlist = new BindingList<DgvList>();
        private AddressValuesDataSet _addressValuesDataSet = new AddressValuesDataSet();
        byte[] _data;
        static Master[] _master;
        bool _someServersConnected;
        private readonly List<string> _addresses = new List<string>();
        private readonly List<SettingsData> _settingsData = new List<SettingsData>();
        private static readonly List<UpdateVar> UpdVar = new List<UpdateVar>();
        private static readonly List<AskingControllers> RequestsQueue = new List<AskingControllers>();
/*
        enum Type {BOOL = 1, INT = 4 };
*/

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(Netresource lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        public MainForm()
        {
            InitializeComponent();
        }

        private void btSettings_Click(object sender, EventArgs e)
        {
            var settings = new Settings();
            settings.Show();
        }

        private void ReadValuesFromSettingsFile(XmlDocument xml)
        {
            foreach (XmlElement element in xml.GetElementsByTagName("Row"))
            {
                //Вносимо значення адрес з файлу в таблицю
                if (element.Attributes["IP"] == null)
                    continue;
                var settingsString = new SettingsData
                {
                    Name = element.Attributes["Name"].Value,
                    Ip = element.Attributes["IP"].Value,
                    Address = element.Attributes["Address"].Value,
                    Type = element.Attributes["Type"].Value,
                    TimerDelay = element.Attributes["TimerDelay"].Value,
                    FileName = element.Attributes["FileName"].Value
                };
                _settingsData.Add(settingsString);

                //Визначаємо кількість необхідних бітів в кожному слові
                var adr = settingsString.Address.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (adr.Length > 1)
                {
                    var index = UpdVar.FindIndex(
                        uv => uv.Ipaddress == settingsString.Ip && uv.Addr == Convert.ToInt16(adr[0])
                    );
                    if (index == -1)
                    {
                        var uv = new UpdateVar
                        {
                            Ipaddress = settingsString.Ip,
                            Addr = Convert.ToInt16(adr[0]),
                            Readedvalue = -1,
                            Numberofbits = 1
                        };
                        UpdVar.Add(uv);
                    }
                    else {
                        UpdVar[index].Numberofbits++;
                    }
                }

                //Робимо вибірку унікальних ІР адрес
                var result = _addresses.FindIndex(z => z == settingsString.Ip);
                if (result == -1) _addresses.Add(settingsString.Ip);
            }
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            if (!_someServersConnected)
            {
                if (File.Exists("Settings.xml"))
                {
                    var xml = new XmlDocument();
                    xml.Load("Settings.xml");
                    ReadValuesFromSettingsFile(xml);
                    _master = new Master[_addresses.Count];
                    Timers = new Timer[1];
                    
                    _settingsData.Sort((a,b) => int.Parse(a.Address).CompareTo(int.Parse((b.Address))));

                    //Заполняем DataGridView
                    foreach (var t in _settingsData)
                    {
                        var item = new DgvList
                        {
                            Ip = t.Ip,
                            Address = int.Parse(t.Address),
                            CurrentValue = 0,
                            NewValue = 0
                        };
                        _dgvlist.Add(item);
                    }

                    dataGridView1.DataSource = _dgvlist;

                    var startaddress = 0;
                    for (var i = 1; i < _settingsData.Count; i++)
                    {
                        if ((int.Parse(_settingsData[i].Address) - int.Parse(_settingsData[startaddress].Address)) <= 100 &&
                            i != _settingsData.Count - 1) continue;
                        RequestsQueue.Add( new AskingControllers
                        {
                            StartAddress = int.Parse(_settingsData[startaddress].Address),
                            IP = "",
                            CurrentValue = 0,
                            Errors = 0,
                            ID = 0,
                            NumberOfAddresses = int.Parse(_settingsData[i-1].Address) - int.Parse(_settingsData[startaddress].Address) + 1,
                            NumberOfAddressessMustTake = i
                        });
                        startaddress = i;
                    }

                    RequestsQueue.Add(new AskingControllers
                    {
                        StartAddress = int.Parse(_settingsData[startaddress].Address),
                        IP = "",
                        CurrentValue = 0,
                        Errors = 0,
                        ID = 1,
                        NumberOfAddresses = (int.Parse(_settingsData[_settingsData.Count - 1].Address) - int.Parse(_settingsData[startaddress].Address)) + 1
                    });


                    //Создание таблиц опроса контроллеров
                    foreach (var controller in _addresses)
                    {
                        _addressValuesDataSet.Tables.Add(new DataTables().CreateAddressValueDataTable(controller));
                    }

                    foreach (var settingsString in _settingsData)
                    {
                        var dataTableIndex = _addressValuesDataSet.Tables.IndexOf(settingsString.Ip);
                        _addressValuesDataSet.Tables[dataTableIndex].Rows.Add();
                    }

                    for (var i = 0; i < _addresses.Count; i++)
                    {
                        if (_addresses[i] != null)
                        {
                            _master[i] = new Master(_addresses[i], 502);
                            _master[i].OnResponseData += MBmaster_OnResponseData;
                            if (!_master[i].connected)
                            {
                                MessageBox.Show(@"Помилка при підключенні до адреси " + _addresses[i]);
                            }
                            else
                            {
                                _connected++;
                            }
                        }
                        else break;
                    }
                    
                    if (_connected == _addresses.Count)
                    {
                        lbStatus.Text = @"Connected";
                        btConnect.Text = @"Disconnect";
                        _someServersConnected = true;
                        //foreach (System.Windows.Forms.Timer timer in timers)
                        //{
                        //    timer.Start();
                        //}
                        AskController(_queuenow);
                        var rc = new Netresource
                        {
                            DwType = 0x00000000,
                            LpRemoteName = @"\\10.0.4.242\ASUTP\",
                            LpLocalName = null,
                            LpProvider = null
                        };
                        //rc.lpRemoteName = @"D:\testdata\";
                        WNetAddConnection2(rc, "asu-tp", "ASUTP", 0);
                    }
                    else
                    {
                        foreach(var singleMaster in _master)
                        {
                            if (singleMaster.connected)
                            {
                                singleMaster.disconnect();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(@"Відсутній файл з налаштуваннями!");
                }
            }
            else
            {
                //foreach (System.Windows.Forms.Timer timer in timers)
                //{
                //    timer.Stop();
                //}
                foreach (var singleMaster in _master)
                {
                    singleMaster.disconnect();
                }
                _someServersConnected = false;
                lbStatus.Text = @"Disconnected";
                btConnect.Text = @"Connect";
            }
        }

        private void AskController(int queuenumber)
        {
            if (_someServersConnected)
            {
                _master[0].ReadInputRegister(0, 0, (ushort)RequestsQueue[queuenumber].StartAddress,
                    (ushort)RequestsQueue[queuenumber].NumberOfAddresses);
                //master[0].ReadInputRegister(0, 0, 209, 127);
            }
        }

        private void MBmaster_OnResponseData(ushort id, byte unit, byte function, byte[] responcevalues)
        {
            try {
                // ------------------------------------------------------------------
                // Seperate calling threads
                if (InvokeRequired)
                {
                    BeginInvoke(new Master.ResponseData(MBmaster_OnResponseData), id, unit, function, responcevalues);
                    return;
                }
                WriteDgv((ushort)_queuenow, responcevalues);
                _queuenow = (_queuenow < RequestsQueue.Count - 1) ? ++_queuenow : 0;
                AskController(_queuenow);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void WriteDgv(ushort id, byte[] responcevalues)
        {
            try {
                //var index = RequestsQueue.FindIndex(x => x.ID == id);
                var word = new int[_data.Length / 2];
                for (var x = 0; x < _data.Length; x = x + 2)
                {
                    word[x / 2] = _data[x] * 256 + _data[x + 1];
                }
                for (var i = 0; i < RequestsQueue[_queuenow].NumberOfAddressessMustTake; i++)
                {
                    UpdateDgv(_queuenow, word[i]);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void UpdateDgv (int dgvIndex, int responceValue)
        {
            _dgvlist[dgvIndex].CurrentValue = _dgvlist[dgvIndex].NewValue;
            _dgvlist[dgvIndex].NewValue = responceValue;
            dataGridView1.Refresh();
        }
    }
}
