using System;
using System.ComponentModel;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Modbus_2._01
{
    [TestClass]
    public class UnitTests
    {
        private string SettingsFileName = "Settings.xml";

        [TestMethod]
        public void testCheckSettingsFileExist()
        {
            Assert.IsTrue(File.Exists(SettingsFileName));
        }

        [TestMethod]
        public void testCheckSettingsFileData()
        {
            WorkWithXml WWX = new WorkWithXml();
            Assert.IsTrue(WWX.load(SettingsFileName).GetElementsByTagName("Row").Count > 0);
        }

        [TestMethod]
        public void testReadSettingsDataXmlFile()
        {
            var wwx = new WorkWithXml();
            var xml = wwx.load(SettingsFileName);
            //var type1 = xml.DocumentElement;
            Assert.IsTrue(wwx.readSettingsData(xml) != null);
        }

        [TestMethod]
        public void testReadVerstatSettingsXmlFile()
        {
            var wwx = new WorkWithXml();
            var xml = wwx.load("VerstatSettings.xml");
            Assert.IsTrue(wwx.readVerstatSettings(xml) != null);
        }
    }
}
