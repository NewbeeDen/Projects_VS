using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xml
{

    public class Param
    {
        public string Name;
        public string IP;
        public string Address;
        public string Type;
        public int TimerDelay;
        public string FileName;
    }

    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("Settings.txt", Encoding.Default);
            int count = System.IO.File.ReadAllLines("Settings.txt").Length;
            List<Param> param = new List<Param>();

            for (int i = 0; i < count; i++)
            {
                string str = sr.ReadLine();
                if (str != null)
                {
                    string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Param paramstring = new Param();
                    paramstring.Name = words[0];
                    paramstring.IP = words[1];
                    paramstring.Address = words[2];
                    paramstring.Type = words[3];
                    paramstring.TimerDelay = Convert.ToInt32(words[4]);
                    paramstring.FileName = words[5];
                    param.Add(paramstring);
                }
            }
            sr.Close();

            XmlTextWriter writer = new XmlTextWriter("Settings.xml", System.Text.Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();

            writer.WriteStartElement("SettingsTable");
            writer.WriteAttributeString("Type", "XML0.1");
            writer.WriteAttributeString("Version", "1.0.0.0");
            //int j = 1;
            foreach (Param paramdata in param)
            {
                writer.WriteStartElement("Row"); //+ (j).ToString());
                writer.WriteAttributeString("Name", paramdata.Name);
                writer.WriteAttributeString("IP", paramdata.IP);
                writer.WriteAttributeString("Address", paramdata.Address);
                writer.WriteAttributeString("Type", paramdata.Type);
                writer.WriteAttributeString("TimerDelay", paramdata.TimerDelay.ToString());
                writer.WriteAttributeString("FileName", paramdata.FileName);
                writer.WriteEndElement();
                //j++;
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            XmlDocument xml = new XmlDocument();
            xml.Load("Settings.xml");

            foreach (XmlElement element in xml.GetElementsByTagName("Row")) // + j.ToString()))
            {
                if (element.Attributes["Name"] == null)
                    continue;
                
                Console.Write("{0} {1} {2} {3}", element.Attributes["Name"].Value, element.Attributes["IP"].Value, element.Attributes["Address"].Value, element.Attributes["Type"].Value);
                Console.Write("\r\n");
            }
            Console.ReadKey();
        }
    }
}

