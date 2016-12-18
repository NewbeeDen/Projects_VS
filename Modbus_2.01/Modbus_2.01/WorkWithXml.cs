using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace Modbus_2._01
{
    class WorkWithXml
    {
        private XmlDocument xml;

        internal XmlDocument load(string XmlFileName)
        {
            xml = new XmlDocument();
            xml.Load(XmlFileName);
            return xml;
        }

        internal BindingList<SettingsData> readSettingsData(XmlDocument xml)
        {
            BindingList<SettingsData> settingsData = new BindingList<SettingsData>();
            foreach (XmlElement element in xml.GetElementsByTagName("Row"))
            {
                var settingsString = new SettingsData
                {
                    Name = element.Attributes["Name"].Value.ToString(),
                    IP = element.Attributes["IP"].Value.ToString(),
                    Address = element.Attributes["Address"].Value.ToString(),
                    Type = element.Attributes["Type"].Value.ToString(),
                    TimerDelay = element.Attributes["TimerDelay"].Value.ToString(),
                    FileName = element.Attributes["FileName"].Value.ToString()
                };
                settingsData.Add(settingsString);
            }
            return settingsData;
        }

        internal BindingList<VerstatSettings> readVerstatSettings(XmlDocument xml)
        {
            BindingList<VerstatSettings> settingsData = new BindingList<VerstatSettings>();
            foreach (XmlElement element in xml.GetElementsByTagName("Row"))
            {
                var vsSingle = new VerstatSettings
                {
                    ID = element.Attributes["ID"].Value.ToString(),
                    IP = element.Attributes["IP"].Value.ToString(),
                    Address = element.Attributes["Address"].Value.ToString(),
                    Name = element.Attributes["Name"].Value.ToString()
                };
                settingsData.Add(vsSingle);
            }
            return settingsData;
        }
    }
}
