
namespace Modbus_2._0
{

    public static class ResponceData
    {
        public static ushort Id { get; set; }

        public static byte[] Responcevalues { get; set; }
    }

    public class Values
    {
        private string _ip;
        private int _word;
        private int _bit;
        private int _value;

        public string IProgress { get { return _ip; } set { _ip = value; } }
        public int Word { get { return _word; } set { _word = value; } }
        public int Bit { get { return _bit; } set { _bit = value; } }
        public int Value { get { return _value; } set { _value = value; } }
    }

    public class AskingControllers
    {
        private string _ip;
        private int _startaddress;
        private int _numberofadresses;
        private int _numberofaddressesmusttake;
        private ushort _id;
        private int _currentValue;
        private int _errors;

        public string IP { get { return _ip; } set { _ip = value; } }
        public int StartAddress { get { return _startaddress; } set { _startaddress = value; } }
        public int NumberOfAddresses { get { return _numberofadresses; } set { _numberofadresses = value; } }
        public int NumberOfAddressessMustTake { get { return _numberofaddressesmusttake; } set
        {
            _numberofaddressesmusttake = value;
        } }
        public ushort ID { get { return _id; } set { _id = value; } }
        public int CurrentValue { get { return _currentValue; } set { _currentValue = value; } }
        public int Errors { get { return _errors; } set { _errors = value; } }
    }

    public class Netresource
    {
        public ResourceScope DwScope = 0;
        public ResourceType DwType = 0;
        public ResourceDisplayType DwDisplayType = 0;
        public ResourceUsage DwUsage = 0;
        public string LpLocalName = null;
        public string LpRemoteName = null;
        public string LpComment = null;
        public string LpProvider = null;
    }

    public enum ResourceScope
    {
        ResourceConnected = 1,
        ResourceGlobalnet,
        ResourceRemembered,
        ResourceRecent,
        ResourceContext
    }

    public enum ResourceType
    {
        ResourcetypeAny,
        ResourcetypeDisk,
        ResourcetypePrint,
        ResourcetypeReserved
    }

    public enum ResourceUsage
    {
        ResourceusageConnectable = 0x00000001,
        ResourceusageContainer = 0x00000002,
        ResourceusageNolocaldevice = 0x00000004,
        ResourceusageSibling = 0x00000008,
        ResourceusageAttached = 0x00000010,
        ResourceusageAll = (ResourceusageConnectable | ResourceusageContainer | ResourceusageAttached),
    }

    public enum ResourceDisplayType
    {
        ResourcedisplaytypeGeneric,
        ResourcedisplaytypeDomain,
        ResourcedisplaytypeServer,
        ResourcedisplaytypeShare,
        ResourcedisplaytypeFile,
        ResourcedisplaytypeGroup,
        ResourcedisplaytypeNetwork,
        ResourcedisplaytypeRoot,
        ResourcedisplaytypeShareadmin,
        ResourcedisplaytypeDirectory,
        ResourcedisplaytypeTree,
        ResourcedisplaytypeNdscontainer
    }

    class DataClasses
    {
        public string AddressToSettingFile { get; set; }
    }
    
    class UpdateVar
    {
        public string Ipaddress;
        public int Addr;
        public int Readedvalue;
        public int Numberofbits;
        public int Bitsreaded;
    }

    public class SettingsData
    {
        private string _ip;

        public SettingsData(string ip)
        {
            _ip = ip;
        }

        public SettingsData()
        {
            
        }

        public string Name { get; set; }

        public string Ip { get; set; }
        public string Address { get; set; }

        public string Type { get; set; }

        public string TimerDelay { get; set; }

        public string FileName { get; set; }
    }

    public class DgvList
    {
        public string Ip { get; set; }

        public int Address { get; set; }

        public int CurrentValue { get; set; }

        public int NewValue { get; set; }
    }
}
