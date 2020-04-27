using SmoothConfig.Api.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SmoothConfig.Api.Importer
{
    public class ConfigImporter
    {
        private XmlDocument _document;
        private List<Setting> _settings;

        /// <summary>
        /// Create object with XML loaded from file stream and close file stream
        /// </summary>
        /// <param name="documentStream">File stream of XML</param>
        public ConfigImporter(Stream documentStream, string name)
        {
            _document = new XmlDocument();
            _document.Load(documentStream);
            _settings = new List<Setting>();
            documentStream.Close();
        }

        /// <summary>
        /// Get list of model setting using Xml
        /// </summary>
        public List<Setting> GetSettings()
        {
            ReadAllElements(_document);
            return _settings;
        }

        /// <summary>
        /// Create all setting and attributes with document
        /// </summary>
        /// <param name="document">XmlDocument wich values will be used for setting and attributes</param>
        private void ReadAllElements(XmlDocument document)
        {
            XmlNodeList nodes = document.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                _settings.Add(new Setting { Name = node.Name });
                var father = _settings.Where(x => x.Name == node.Name).FirstOrDefault();

                ReadAllAttributes(node, father);
                ReadChildNodes(node, father);
            }
        }

        /// <summary>
        /// Create attributes with for setting using node attributes
        /// </summary>
        /// <param name="node">Node with attributes</param>
        /// <param name="setting">Setting where will be created attributes</param>
        private void ReadAllAttributes(XmlNode node, Setting setting)
        {
            //skip function if has no attributes
            if (node.Attributes == null)
                return;

            if (setting.Attributes == null)
                setting.Attributes = new List<Model.Attribute>();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                
                setting.Attributes.Add(
                    new Model.Attribute 
                    { 
                        Name = attribute.Name, 
                        Value = attribute.Value 
                    });
            }
        }

        /// <summary>
        /// Create a setting childrens for all childrens nodes
        /// </summary>
        /// <param name="node">Node with childrens</param>
        /// <param name="settingFather">Setting father where will be created childrens</param>
        private void ReadChildNodes(XmlNode node, Setting settingFather)
        {
            var nodes = node.ChildNodes;

            //skip function if has no child nodes
            if (nodes == null) return;

            if (settingFather.Childrens == null)
                settingFather.Childrens = new List<Setting>();

            foreach (XmlNode n in nodes)
            {
                settingFather.Childrens.Add(new Setting { Name = n.Name });
                var newFather = settingFather.Childrens.Where(x => x.Name == n.Name).FirstOrDefault();

                ReadAllAttributes(n, newFather);
                ReadChildNodes(n, newFather);
            }
        }
    }
}
