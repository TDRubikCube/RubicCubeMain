using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace RubikCube
{
    class SaveGame
    {
        private XmlDocument doc;
        private string fileName;
        private string grandFather;
        public SaveGame(string FileName,string GrandFather)
        {
            doc = new XmlDocument();
            fileName = FileName;
            Path.GetFullPath(fileName);
            doc.Load(fileName);
            grandFather = GrandFather;
            EndingProt();
        }

        public void AddBool(string boolName, string boolValue)
        {
            doc.Load(fileName);
            XmlElement boolElement = doc.CreateElement("bool");
            XmlElement name = doc.CreateElement("name");
            XmlElement value = doc.CreateElement("value");
            name.InnerText = boolName;
            value.InnerText = boolValue;
            boolElement.AppendChild(name);
            boolElement.AppendChild(value);
            doc.GetElementsByTagName(grandFather)[0].AppendChild(boolElement);
            EndingProt();
        }

        public List<Tuple<string, string>> LoadBools()
        {
            doc.Load(fileName);
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            XmlNodeList nodeList = doc.GetElementsByTagName("bool");
            for (int i = 0; i < nodeList.Count; i++)
            {
                result.Add(new Tuple<string, string>(nodeList[i].ChildNodes[0].InnerText, nodeList[i].ChildNodes[1].InnerText));
            }
            return result;
        }

        private void EndingProt()
        {
            FileStream stream = new FileStream(fileName,FileMode.Truncate,FileAccess.Write,FileShare.ReadWrite);
            doc.Save(stream);
            stream.Close();
        }
    }
}
