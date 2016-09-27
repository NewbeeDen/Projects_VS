﻿using System;
using System.Text;
using System.Windows.Forms;
using EasyModbus;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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
        bool SomeServerConnected, BIT;
        bool[] dataBool = new bool[3];
        int[,] values, states, statesTime;
        int[] dataInt, state, stateTime;
        private static System.Timers.Timer myTimer = new System.Timers.Timer();
        private ModbusClient[] modbusClient = new ModbusClient[20];
        string[,] param;
        int[] queue;
        string[] addresses = new string[20];
        string[] AddressSplit = new string[2];
        int count, z;
        System.Windows.Forms.Timer[] timers; 
        DateTime time;
        ushort ID;
        string str, Address;

        private static List<UpdateVar> UpdVar = new List<UpdateVar>();

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(Data.NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string lpName, int dwFlags, int fForce);

        public Trend()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            int index = Convert.ToInt32((sender as System.Windows.Forms.Timer).Tag);
            if (param[index, 3] == "BOOL") ID = 1;
            else ID = 4;
            byte unit = Convert.ToByte(0);
            if (param[index, 2] != null && modbusClient != null)
            {
                Address = param[index, 2];
                byte Length = Convert.ToByte(1);
                for (int i = 0; i < 20; i++)
                {
                    if (param[index, 1] == addresses[i])
                    {
                        if (modbusClient[i] != null)
                        {
                            if (ID == 1)
                            {
                                AddressSplit = Address.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (AddressSplit.Length > 1)
                                {
                                    if (values[index, 0] == 0)
                                    {
                                        dataInt = modbusClient[i].ReadInputRegisters(int.Parse(AddressSplit[0]), Length);
                                        int j = UpdVar.FindIndex(
                                                delegate (UpdateVar uv)
                                                {
                                                    if (uv.ipaddress == modbusClient[i].IPAddress && uv.addr == Convert.ToInt16(AddressSplit[0]))
                                                    {
                                                        return true;
                                                    }
                                                    return false;
                                                }
                                                );
                                        UpdVar[j].readedvalue = dataInt[0];
                                        dataBool[0] = Convert.ToBoolean((dataInt[0] >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                    }
                                    else
                                    {
                                        int j = UpdVar.FindIndex(
                                                delegate (UpdateVar uv)
                                                {
                                                    if (uv.ipaddress == param[index, 1] && uv.addr == Convert.ToInt16(AddressSplit[0]))
                                                    {
                                                        return true;
                                                    }
                                                    return false;
                                                }
                                                );
                                        if (j != -1)
                                        {
                                            if (UpdVar[j].readedvalue == -1)
                                            {
                                                dataInt = modbusClient[i].ReadInputRegisters(int.Parse(AddressSplit[0]), Length);
                                                UpdVar[j].readedvalue = dataInt[0];
                                                dataBool[0] = Convert.ToBoolean((UpdVar[j].readedvalue >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                                UpdVar[j].bitsreaded++;
                                            }
                                            else
                                            {
                                                if (UpdVar[j].bitsreaded != UpdVar[j].numberofbits)
                                                {
                                                    dataBool[0] = Convert.ToBoolean((UpdVar[j].readedvalue >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                                    UpdVar[j].bitsreaded++;
                                                }
                                                else
                                                {
                                                    dataInt = modbusClient[i].ReadInputRegisters(int.Parse(AddressSplit[0]), Length);
                                                    UpdVar[j].readedvalue = dataInt[0];
                                                    dataBool[0] = Convert.ToBoolean((UpdVar[j].readedvalue >> Convert.ToInt16(AddressSplit[1])) & 0x01);
                                                    UpdVar[j].bitsreaded = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                                else {
                                    dataBool = modbusClient[i].ReadCoils(int.Parse(Address), Length);
                                }
                                //if (Convert.ToUInt16(param[index, 4]) != 0)
                                //{
                                //    state = modbusClient[i].ReadInputRegisters(Convert.ToUInt16(param[index, 4]), Convert.ToByte(1));
                                //}
                                //if (Convert.ToUInt16(param[index, 5]) != 0)
                                //{
                                //    stateTime = modbusClient[i].ReadInputRegisters(Convert.ToUInt16(param[index, 5]), Convert.ToByte(1));
                                //}
                                if (z <= 0)
                                {
                                    textBox.Text += "\r\n";
                                    z = count;
                                }
                                values[index, 3] = Convert.ToInt16(dataBool[0]);
                                if (values[index, 0] == 0)
                                {
                                    if (AddressSplit.Length > 1)
                                    {
                                        values[index, 0] = Convert.ToInt16(AddressSplit[0]);
                                        values[index, 1] = Convert.ToInt16(AddressSplit[1]);
                                    }
                                    else
                                    {
                                        values[index, 0] = Convert.ToInt16(Address);
                                    }
                                    //states[index, 0] = Convert.ToUInt16(param[index, 4]);
                                    //statesTime[index, 0] = Convert.ToUInt16(param[index, 5]);
                                    values[index, 2] = Convert.ToInt16(dataBool[0]);
                                    //states[index, 1] = state[0];
                                    //statesTime[index, 1] = stateTime[0];
                                    ////values[index, 3] = Convert.ToInt16(dataBool[0]);
                                    //states[index, 2] = state[0];
                                    //statesTime[index, 2] = stateTime[0];
                                    string filename = @"\\10.0.4.242\ASUTP\" + param[index, 7] + ".txt";
                                    //string filename = @"D:\testdata\" + param[index, 7] + ".txt";
                                    StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                                    string Time = DateTime.Now.ToString();
                                    string[] words = Time.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    sw.Write("\r\n");
                                    if (words[1].Length == 7)
                                    {
                                        sw.Write(words[0].ToString() + "\t" + "0" + words[1].ToString() + "\t" + values[index, 3].ToString());
                                    }
                                    else
                                    {
                                        sw.Write(words[0].ToString() + "\t" + words[1].ToString() + "\t" + values[index, 3]);
                                    }
                                    sw.Close();
                                }
                                if (values[index, 2] != values[index, 3])
                                {
                                    string filename = @"\\10.0.4.242\ASUTP\" + param[index, 7] + ".txt";
                                    //string filename = @"D:\testdata\" + param[index, 7] + ".txt";
                                    StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                                    string Time = DateTime.Now.ToString();
                                    string[] words = Time.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    sw.Write("\r\n");
                                    if (words[1].Length == 7)
                                    {
                                        sw.Write(words[0].ToString() + "\t" + "0" + words[1].ToString() + "\t" + values[index, 3].ToString());
                                    }
                                    else
                                    {
                                        sw.Write(words[0].ToString() + "\t" + words[1].ToString() + "\t" + values[index, 3].ToString());
                                    }
                                    sw.Close();
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
                    else dataInt = modbusClient[i].ReadInputRegisters(int.Parse(Address), Length);
                    if (z <= 0)
                    {
                        textBox.Text += DateTime.Now + " ";
                        textBox.Text += "\r\n";
                        z = count;
                    }
                    AddressSplit = Address.ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values[index, 0] == 0)
                        {
                        if (AddressSplit.Length > 1)
                            {
                                values[index, 0] = Convert.ToInt16(AddressSplit[0]);
                                values[index, 1] = Convert.ToInt16(AddressSplit[1]);
                            }
                            else
                            {
                                values[index, 0] = Convert.ToInt16(Address);
                            }
                            values[index, 2] = dataInt[0];
                            values[index, 3] = dataInt[0];
                        }
                        values[index, 3] = dataInt[0];
                        if (values[index, 2] != values[index, 3])
                        {
                            values[index, 2] = values[index, 3];
                            string filename = @"\\10.0.4.242\ASUTP\" + param[index, 7] + ".txt";
                            //string filename = @"D:\testdata\" + param[index, 7] + ".txt";
                            StreamWriter sw = new StreamWriter(filename, true, Encoding.Default);
                            string Time = DateTime.Now.ToString();
                            string[] words = Time.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            sw.WriteLine(words[0].ToString() + "\t" + words[1].ToString() + "\t" + values[index, 3]);
                            sw.Close();
                        }
                        //textBox.Text += Address.ToString() + ":" + dataInt[0].ToString() + " ";
                        //z--;
                    }
                }
                time = DateTime.Now;
                int[] word = new int[1];
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!SomeServerConnected)
                {
                    //Read settings file
                    StreamReader sr = new StreamReader("Settings.txt", Encoding.Default);
                    count = System.IO.File.ReadAllLines("Settings.txt").Length;
                    z = count;
                    values = new int[count, 4];
                    states = new int[count, 4];
                    param = new string[count, 8];
                    timers = new System.Windows.Forms.Timer[count];
                    queue = new int[count];
                    for (int x = 0; x < count; x++)
                    {
                        str = sr.ReadLine();
                        if (str != null)
                        {
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
                            param[x, 6] = words[6];
                            //else param[x, 6] = words[5];
                            param[x, 7] = words[7];
                            string[] Adr = param[x, 2].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (Adr.Length > 1)
                            {
                                int index = UpdVar.FindIndex(
                                    delegate (UpdateVar uv)
                                    {
                                        if (uv.ipaddress == param[x, 1] && uv.addr == Convert.ToInt16(Adr[0]))
                                        {
                                            return true;
                                        }
                                        return false;
                                    }
                                );
                                if (index == -1)
                                {
                                    UpdateVar uv = new UpdateVar();
                                    uv.ipaddress = param[x, 1];
                                    uv.addr = Convert.ToInt16(Adr[0]);
                                    uv.readedvalue = -1;
                                    uv.numberofbits++;
                                    UpdVar.Add(uv);
                                }
                                else
                                {
                                    UpdVar[index].numberofbits++;
                                }
                            }
                        }
                    } 
                    sr.Close();

                    int j = 0;
                    addresses[0] = param[0, 1]; 
                    for (int x = 1; x < count; x++)
                    {
                        j = 0;
                        for (;;)
                            if (param[x, 1] == addresses[j]) break;
                            else
                            {
                                j++;
                                if (addresses[j] == null)
                                {
                                    addresses[j] = param[x, 1];
                                    break;
                                }
                            }
                    }

                    for (int i = 0; i < count; i++)
                    {
                        timers[i] = new System.Windows.Forms.Timer();
                        timers[i].Tag = i;
                        timers[i].Interval = Convert.ToInt32(param[i, 6]) * 1000;
                        timers[i].Tick += timer1_Tick;
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        if (addresses[i] != null)
                        {
                            modbusClient[i] = new ModbusClient(addresses[i], 502);
                        }
                        else break;
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        if (modbusClient[i] != null)
                        {
                            do
                            {
                                modbusClient[i].Connect();
                            } while (!modbusClient[i].Connected);
                        }
                        else { break; }
                    }
                    
                    labelStatus.Text = "Connected";
                    buttonConnect.Text = "Disconnect";
                    SomeServerConnected = true;
                    for (int i = 0; i < count; i++)
                    {
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
                else
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (modbusClient[i] != null)
                        {
                            modbusClient[i].Disconnect();
                            modbusClient[i] = null;
                        }
                    }
                    SomeServerConnected = false;
                    labelStatus.Text = "Disconnected";
                    buttonConnect.Text = "Connect";
                    for (int i = 0; i < count; i++)
                    {
                        timers[i].Stop();
                    }
                }
            }
            catch (SystemException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    if (modbusClient[i] != null)
                    {
                        modbusClient[i].Disconnect();
                        modbusClient[i] = null;
                    }
                }
            }
            catch (SystemException er)
            {
                MessageBox.Show(er.Message);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
    }
}