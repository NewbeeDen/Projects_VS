using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus_2._01
{
    class ClassData
    {
    }

    public class SettingsData
    {
        private string _name;
        private string _ip;
        private string _address;
        private string _type;
        private string _timerDelay;
        private string _fileName;

        public string Name { get { return _name; } set { _name = value; } }
        public string IP { get { return _ip; } set { _ip = value; } }
        public string Address { get { return _address; } set { _address = value; } }
        public string Type { get { return _type; } set { _type = value; } }
        public string TimerDelay { get { return _timerDelay; } set { _timerDelay = value; } }
        public string FileName { get { return _fileName; } set { _fileName = value; } }
    }

    public class VerstatSettings
    {
        string _id;
        string _ip;
        string _address;
        string _name;

        public string ID { get { return _id; } set { _id = value; } }
        public string IP { get { return _ip; } set { _ip = value; } }
        public string Address { get { return _address; } set { _address = value; } }
        public string Name { get { return _name; } set { _name = value; } }
    }
}
