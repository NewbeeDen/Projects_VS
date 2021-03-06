﻿using System;
using System.Text;
using System.Windows.Forms;
using EasyModbus;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Xml;
using System.ComponentModel;

namespace ModbusConnection
{
    class UpdateVar
    {
        public string ipaddress;
        public int addr;
        public int readedvalue;
        public int numberofbits;
        public int bitsreaded;
    }

    public partial class Trend : Form
    {
        private byte[] data1;
        bool SomeServerConnected;
        bool dataBool;
        int[,] values, states, statesTime;
        int[] state, stateTime, queue;
        private static System.Timers.Timer myTimer = new System.Timers.Timer();
        private ModbusClient[] modbusClient;
        string[,] param;
        List<string> addresses = new List<string>();
        string[] AddressSplit = new string[2];
        int count, z, UpdateHours, UpdateMinutes, ByryakClientIndex, OldBeet=0, NewBeet, dataInt;
        Timer[] timers; 
        DateTime time;
        ushort ID;
        string str, Address;
        BindingList<VerstSettings> vs = new BindingList<VerstSettings>();

        private static List<UpdateVar> UpdVar = new List<UpdateVar>();

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(Data.NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string lpName, int dwFlags, int fForce);

        public Trend() {
            InitializeComponent();

            Button VerstatOptions = new Button();
            VerstatOptions.Location = new System.Drawing.Point(Options.Size.Width + Options.Location.X + 2, Options.Location.Y);
            VerstatOptions.Size = new System.Drawing.Size(193, Options.Size.Height);
            VerstatOptions.Text = "Налаштування верстату";
            this.Controls.Add(VerstatOptions);
            VerstatOptions.Click += VerstatOptions_Click;
        }

        private void VerstatOptions_Click(object sender, EventArgs e) {
            VerstatSettings vs = new VerstatSettings();
            vs.Show();
        }

        private void label1_Click(object sender, EventArgs e) {
        }
        
        private void timer1_Tick(object sender, EventArgs e) {
            int index = Convert.ToInt32((sender as System.Windows.Forms.Timer).Tag);
            if (param[index, 3] == "BOOL") ID = 1;
            else ID = 4;
            byte unit = Convert.ToByte(0);
            if (param[index, 2] != null && modbusClient != null) {
                Address = param[index, 2];
                byte Length = Convert.ToByte(1);
                try {
                    int ClientIndex;
                    ClientIndex = addresses.FindIndex(x => x == param[index, 1]);
                    if (modbusClient[ClientIndex] != null) {
                        if (ID == 1) {
                            AddressSplit = Address.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (AddressSplit.Length > 1) {
                                if (values[index, 0] == 0) {
                                    dataInt = AskControllerRegister(modbusClient[ClientIndex], int.Parse(AddressSplit[0]), Length);
                                    // modbusClient[ClientIndex].ReadInputRegisters(int.Parse(AddressSplit[0]), Length);
                                    int j = UpdVar.FindIndex(
                                            delegate (UpdateVar uv) {
                                                if (uv.ipaddress == modbusClient[ClientIndex].IPAddress && uv.addr == Convert.ToInt16(AddressSplit[0])) {
                                                    return true;
                                                }
                                                return false;
                                            }
                                            );
                                    UpdVar[j].readedvalue = dataInt;
                                    dataBool = Convert.ToBoolean((dataInt >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                }
                                else {
                                    int j = UpdVar.FindIndex(
                                            delegate (UpdateVar uv) {
                                                if (uv.ipaddress == param[index, 1] && uv.addr == Convert.ToInt16(AddressSplit[0])) {
                                                    return true;
                                                }
                                                return false;
                                            }
                                            );
                                    if (j != -1) {
                                        if (UpdVar[j].readedvalue == -1) {
                                            dataInt = AskControllerRegister(modbusClient[ClientIndex], int.Parse(AddressSplit[0]), Length);
                                            UpdVar[j].readedvalue = dataInt;
                                            dataBool = Convert.ToBoolean((UpdVar[j].readedvalue >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                            UpdVar[j].bitsreaded++;
                                        }
                                        else {
                                            if (UpdVar[j].bitsreaded != UpdVar[j].numberofbits) {
                                                dataBool = Convert.ToBoolean((UpdVar[j].readedvalue >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                                UpdVar[j].bitsreaded++;
                                            }
                                            else {
                                                dataInt = AskControllerRegister(modbusClient[ClientIndex], int.Parse(AddressSplit[0]), Length);
                                                UpdVar[j].readedvalue = dataInt;
                                                dataBool = Convert.ToBoolean((UpdVar[j].readedvalue >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                                UpdVar[j].bitsreaded = 1;
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                dataBool = AskControllerCoil(modbusClient[ClientIndex], Int32.Parse(param[index, 2]), 1);
                            }
                            if (z <= 0) {
                                textBox.Text += "\r\n";
                                z = count;
                            }
                            values[index, 3] = Convert.ToInt16(dataBool);
                            if (values[index, 0] == 0) {
                                if (AddressSplit.Length > 1) {
                                    values[index, 0] = Convert.ToInt16(AddressSplit[0]);
                                    values[index, 1] = Convert.ToInt16(AddressSplit[1]);
                                }
                                else {
                                    values[index, 0] = Convert.ToInt16(Address);
                                }
                                //states[index, 0] = Convert.ToUInt16(param[index, 4]);
                                //statesTime[index, 0] = Convert.ToUInt16(param[index, 5]);
                                values[index, 2] = Convert.ToInt16(dataBool);
                                //states[index, 1] = state[0];
                                //statesTime[index, 1] = stateTime[0];
                                ////values[index, 3] = Convert.ToInt16(dataBool[0]);
                                //states[index, 2] = state[0];
                                //statesTime[index, 2] = stateTime[0];
                                string filename = @"\\10.0.4.242\ASUTP\" + param[index, 5] + ".txt";
                                //string filename = @"D:\testdata\" + param[index, 5] + ".txt";
                                //if (File.Exists(filename)) {
                                    StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                                    string Time = DateTime.Now.ToString();
                                    string[] words = Time.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    sw.Write("\r\n");
                                    if (words[1].Length == 7) {
                                        sw.Write(words[0].ToString() + "\t" + "0" + words[1].ToString() + "\t" + values[index, 3].ToString());
                                    }
                                    else {
                                        sw.Write(words[0].ToString() + "\t" + words[1].ToString() + "\t" + values[index, 3]);
                                    }
                                    sw.Close();
                                //}
                            }
                            if (values[index, 2] != values[index, 3]) {
                                string filename = @"\\10.0.4.242\ASUTP\" + param[index, 5] + ".txt";
                                //string filename = @"D:\testdata\" + param[index, 5] + ".txt";
                                //if (File.Exists(filename)) {
                                    StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                                    string Time = DateTime.Now.ToString();
                                    string[] words = Time.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    sw.Write("\r\n");
                                    if (words[1].Length == 7) {
                                        sw.Write(words[0].ToString() + "\t" + "0" + words[1].ToString() + "\t" + values[index, 3].ToString());
                                    }
                                    else {
                                        sw.Write(words[0].ToString() + "\t" + words[1].ToString() + "\t" + values[index, 3].ToString());
                                    }
                                    sw.Close();
                                //}
                                values[index, 2] = values[index, 3];
                            }
                            //states[index, 2] = state[0];
                            //if (states[index, 1] != states[index, 2])
                            //{
                            //    states[index, 1] = states[index, 2];
                            //    string filename = @"\\10.0.4.243\exchange$\ASUTP\" + param[index, 6] + ".txt";
                            //    StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                            //    sw.Write("\t" + states[index, 2]);
                            //    sw.Close();
                            //}
                            //statesTime[index, 2] = stateTime[0];
                            //if (statesTime[index, 1] != statesTime[index, 2])
                            //{
                            //    statesTime[index, 1] = statesTime[index, 2];
                            //    string filename = @"\\10.0.4.243\exchange$\ASUTP\" + param[index, 6] + ".txt";
                            //    StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                            //    sw.Write("\t" + statesTime[index, 2]);
                            //    sw.Close();
                            //}

                            //textBox.Text += Address + ":" + values[index, 3].ToString() + " "/* + " " + statesTime[index, 2].ToString()*/;
                            //z--;
                        }
                        return;
                    }
                    else dataInt = AskControllerRegister(modbusClient[ClientIndex], int.Parse(Address), Length);
                    if (z <= 0) {
                        textBox.Text += DateTime.Now + " ";
                        textBox.Text += "\r\n";
                        z = count;
                    }
                    AddressSplit = Address.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values[index, 0] == 0) {
                        if (AddressSplit.Length > 1) {
                            values[index, 0] = Convert.ToInt16(AddressSplit[0]);
                            values[index, 1] = Convert.ToInt16(AddressSplit[1]);
                        }
                        else {
                            values[index, 0] = Convert.ToInt16(Address);
                        }
                        values[index, 2] = dataInt;
                        values[index, 3] = dataInt;
                    }
                    values[index, 3] = dataInt;
                    if (values[index, 2] != values[index, 3]) {
                        values[index, 2] = values[index, 3];
                        string filename = @"\\10.0.4.242\ASUTP\" + param[index, 5] + ".txt";
                        //string filename = @"D:\testdata\" + param[index, 5] + ".txt";
                        //if (File.Exists(filename)) {
                            StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                            string Time = DateTime.Now.ToString();
                            string[] words = Time.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            sw.WriteLine(words[0].ToString() + "\t" + words[1].ToString() + "\t" + values[index, 3]);
                            sw.Close();
                        //}
                    }
                }
                catch (Exception) { WriteLog(param[index, 1] + " Exception in MainTimer function"); }
                time = DateTime.Now;
                int[] word = new int[1];
            }
        }

        private void WriteLog(string Adr) {
            string filename = "Log.txt";
            StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
            sw.WriteLine(DateTime.Now + " " + Adr);
            sw.Close();
        }

        private void buttonConnect_Click(object sender, EventArgs e) {
            try {
                if (File.Exists("VerstatSettings.xml")) {
                    XmlDocument xml = new XmlDocument();
                    xml.Load("VerstatSettings.xml");
                    
                    foreach (XmlElement element in xml.GetElementsByTagName("Row"))
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

                    foreach (XmlElement elem in xml.GetElementsByTagName("Time")) {
                        UpdateHours = Convert.ToInt16(elem.Attributes["Hours"].Value);
                        UpdateMinutes = Convert.ToInt16(elem.Attributes["Minutes"].Value);
                    }
                }
                if (!SomeServerConnected) {
                    //Read settings file
                    StreamReader sr = new StreamReader("Settings.txt", Encoding.Default);
                    count = System.IO.File.ReadAllLines("Settings.txt").Length;
                    values = new int[count, 4];
                    states = new int[count, 4];
                    param = new string[count, 8];
                    timers = new System.Windows.Forms.Timer[count + 2];
                    queue = new int[count];
                    for (int x = 0; x < count; x++) {
                        str = sr.ReadLine();
                        if (str != null) {
                            string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            param[x, 0] = words[0];
                            param[x, 1] = words[1];
                            param[x, 2] = words[2];
                            param[x, 3] = words[3];
                            //if (param[x, 3] == "REAL")
                            param[x, 4] = words[4];
                            //if (param[x, 3] == "REAL")
                            param[x, 5] = words[5];
                            //else param[x, 5] = words[4];
                            //if (param[x, 3] == "REAL")
                            //param[x, 6] = words[6];
                            //else param[x, 6] = words[5];
                            //param[x, 7] = words[7];

                            //Проводим подсчет переменных на каждое слово данных
                            string[] Adr = param[x, 2].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (Adr.Length > 1) {
                                int index = UpdVar.FindIndex(
                                    delegate (UpdateVar uv)
                                    {
                                        if (uv.ipaddress == param[x, 1] && uv.addr == Convert.ToInt16(Adr[0])) {
                                            return true;
                                        }
                                        return false;
                                    }
                                );
                                if (index == -1) {
                                    UpdateVar uv = new UpdateVar();
                                    uv.ipaddress = param[x, 1];
                                    uv.addr = Convert.ToInt16(Adr[0]);
                                    uv.readedvalue = -1;
                                    uv.numberofbits++;
                                    UpdVar.Add(uv);
                                }
                                else {
                                    UpdVar[index].numberofbits++;
                                }
                            }

                            //Делаем выборку уникальных ІР адрессов
                            int result = addresses.FindIndex(z => z == param[x, 1]);
                            if (result == -1) addresses.Add(param[x, 1]);
                        }
                    } 
                    sr.Close();

                    //int j = 0;
                    //addresses[0] = param[0, 1]; 
                    //for (int x = 1; x < count; x++) {
                    //    j = 0;
                    //    for (;;)
                    //        if (param[x, 1] == addresses[j]) break;
                    //        else {
                    //            j++;
                    //            if (addresses[j] == null) {
                    //                addresses[j] = param[x, 1];
                    //                break;
                    //            }
                    //        }
                    //}

                    modbusClient = new ModbusClient[addresses.Count];

                    for (int i = 0; i < count; i++) {
                        timers[i] = new System.Windows.Forms.Timer();
                        timers[i].Tag = i;
                        timers[i].Interval = Convert.ToInt32(param[i, 4]) * 1000;
                        timers[i].Tick += timer1_Tick;
                    }

                    //Таймер для верстата
                    timers[count] = new Timer();
                    timers[count].Interval = 45000;
                    timers[count].Tick += VerstatTimer_Tick;
                    
                    //Таймер для счетчика буряка
                    timers[count + 1] = new Timer();
                    timers[count + 1].Interval = 10000;
                    timers[count + 1].Tick += ByryakTimer_Tick;

                    for (int i = 0; i < addresses.Count; i++) {
                        if (addresses[i] != null) {
                            modbusClient[i] = new ModbusClient(addresses[i], 502);
                        }
                        else break;
                    }

                    foreach (ModbusClient mc in modbusClient) {
                        if (mc != null)
                        {
                            do
                            {
                                mc.Connect();
                            } while (!mc.Connected);
                            textBox.Text += mc.IPAddress + " " + "connected" + "\r\n";
                        }
                    }

                    labelStatus.Text = "Connected";
                    buttonConnect.Text = "Disconnect";
                    SomeServerConnected = true;
                    ByryakClientIndex = addresses.FindIndex(z => z == "192.168.1.60");
                    for (int i = 0; i < count + 2; i++) {
                        timers[i].Start();
                    }
                    Data.NETRESOURCE rc = new Data.NETRESOURCE();
                    rc.dwType = 0x00000000;
                    rc.lpRemoteName = @"\\10.0.4.242\ASUTP\";
                    //rc.lpRemoteName = @"D:\testdata\";
                    rc.lpLocalName = null;
                    rc.lpProvider = null;
                    int ret = WNetAddConnection2(rc, "asu-tp", "ASUTP", 0);
                    time = DateTime.Now;
                    textBox.Text += time.ToString() + "\r\n";
                }
                else {
                    foreach (ModbusClient mc in modbusClient) {
                        if (mc != null) {
                            mc.Disconnect();
                        }
                    }
                    SomeServerConnected = false;
                    labelStatus.Text = "Disconnected";
                    buttonConnect.Text = "Connect";
                    for (int i = 0; i < count + 2; i++) {
                        timers[i].Stop();
                    }
                }
            }
            catch (SystemException error) {
                MessageBox.Show(error.Message);
            }
        }

        private void ByryakTimer_Tick(object sender, EventArgs e) {
            string filename = @"\\10.0.4.242\ASUTP\Byryak.txt";
            //string filename = @"D:\testdata\Byryak.txt";
            if (ByryakClientIndex != -1) {
                if (modbusClient[ByryakClientIndex] != null) {
                    try {
                        NewBeet = AskControllerRegister(modbusClient[ByryakClientIndex], 500, 1);
                        if (NewBeet < OldBeet) {
                            StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                            string Time = DateTime.Now.ToString();
                            string[] words = Time.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (words[1].Length == 7)
                            {
                                sw.WriteLine(words[0].ToString() + " " + "0" + words[1].ToString() + " " + OldBeet.ToString() + " " + "0");
                            }
                            else {
                                sw.WriteLine(words[0].ToString() + " " + words[1].ToString() + " " + OldBeet.ToString() + " " + "0");
                            }
                            //sw.WriteLine(Time.ToString() + " " + OldBeet.ToString() + " " + "0");
                            sw.Close();
                        }
                        if (NewBeet < 700)
                        {
                            OldBeet = NewBeet;
                        }
                    }
                    catch (Exception) { WriteLog("192.168.1.60 Exception in ByryakTimer"); }
                }
            }
        }

        private void VerstatTimer_Tick(object sender, EventArgs e) {
            if ((DateTime.Now.Hour == UpdateHours && DateTime.Now.Minute == UpdateMinutes) || (DateTime.Now.Hour == (UpdateHours + 12) && DateTime.Now.Minute == UpdateMinutes)) {
                List<int> result = new List<int>();
                for (int j = 0; j < vs.Count; j++) {
                    for (int i = 0; i < modbusClient.Length; i++) {
                        if (vs[j].IP == addresses[i]) {
                            try {
                                if (modbusClient[i] != null) {
                                    int[] dataIntTemp = modbusClient[i].ReadInputRegisters(int.Parse(vs[j].Address), 1);
                                    if (dataIntTemp[0] < 0) {
                                        dataIntTemp[0] = 0;
                                    }
                                    result.Add(dataIntTemp[0]);
                                    System.Threading.Thread.Sleep(200);
                                }
                            }
                            catch (Exception) { WriteLog("Exception in VerstatTimer");}
                        }
                    }
                }
                string filename = @"\\10.0.4.242\ASUTP\Verstat.txt";
                //string filename = @"D:\testdata\Verstat.txt";
                if (File.Exists(filename)) { File.Delete(filename); }
                StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                string Time = DateTime.Now.ToString();
                sw.WriteLine(Time.ToString());
                for (int i = 0; i < result.Count; i++) {
                    if (i < 40) {
                        sw.WriteLine(vs[i].ID + "\t" + result[i] + "\t" + vs[i].Name);
                    }
                    else {
                        int res_real = result[i] / 100;
                        sw.WriteLine(vs[i].ID + "\t" + res_real + "\t" + vs[i].Name);
                    }
                }
                sw.Close();
                result.Clear();
                result = null;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                foreach (ModbusClient mc in modbusClient) {
                        mc.Disconnect();
                }
            }
            catch (SystemException er) {
                MessageBox.Show(er.Message);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e) {
            data1 = new byte[0];
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBox.Text = "";
        }

        private void Options_Click(object sender, EventArgs e)
        {
            Form Form2 = new Settings();
            Form2.Show();
        }

        private void Trend_Activated(object sender, EventArgs e)
        {
        }

        private int AskControllerRegister(ModbusClient mc, int Address, int NumberOfWords)
        {
            try {
                    int[] resultArray = mc.ReadInputRegisters(Address, NumberOfWords);
                    return resultArray[0];
                }
            catch (Exception) {
                WriteLog("Exception in AskControllerRegister at address: " + mc.IPAddress + " " + Address.ToString());
                return 0;
            }
        }

        private bool AskControllerCoil(ModbusClient mc, int Address, int NumberOfWords)
        {
            try {
                bool[] resultArray = mc.ReadCoils(Address, NumberOfWords);
                return resultArray[0];
            }
            catch (Exception) {
                WriteLog("Exception in AskControllerCoil at address: " + mc.IPAddress + " " + Address.ToString());
                return false;
            }
        }
    }
}
