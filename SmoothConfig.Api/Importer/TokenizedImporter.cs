using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SmoothConfig.Api.Importer
{
    public class TokenizedImporter
    {

        private XmlDocument _document;
        private XmlDocument _documentTokenized;
        
        //character used to separate names in tokens
        private readonly string _separator = "|";

        /// <summary>
        /// Create object with XML loaded from file stream and close file stream
        /// </summary>
        /// <param name="documentStream">File stream of XML</param>
        public TokenizedImporter(Stream documentStream)
        {
            _document = new XmlDocument();
            _document.Load(documentStream);
            documentStream.Close();
        }

        /// <summary>
        /// Create a new XmlDocument with values replaced for tokens
        /// </summary>
        /// <returns>XmlDocument with tokens</returns>
        public XmlDocument GetXMLTokenized()
        {
            _documentTokenized = (XmlDocument)_document.Clone();
            ReadAllElements(_documentTokenized);

            return _documentTokenized;
        }

        /// <summary>
        /// Replace all child nodes and attributes for tokens
        /// </summary>
        /// <param name="document">XmlDocument wich values will be replaced for tokens</param>
        private void ReadAllElements(XmlDocument document)
        {
            XmlNodeList nodes = document.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                //initial prefix
                var prefix = node.Name;

                ReadAllAttributes(node, prefix);
                ReadChildNodes(node, prefix);
            }
        }

        /// <summary>
        /// Replacing non-name and non-key attributes with tokens
        /// </summary>
        /// <param name="node">Node with attributes</param>
        /// <param name="prefix">Prefix of token name</param>
        private void ReadAllAttributes(XmlNode node, string prefix)
        {
            //skip function if has no attributes
            if (node.Attributes == null)
                return;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                //skip attributes named 'name' or 'key' because they are not considered tokens
                if (attribute.Name == "name" || attribute.Name == "key")
                    continue;

                //replace attribute value for token
                attribute.Value = $"{prefix}{_separator}{attribute.Name}";
            }
        }

        /// <summary>
        /// Replace all childrens nodes for tokens
        /// </summary>
        /// <param name="node">Node with childrens</param>
        /// <param name="prefix">Prefix of token name</param>
        private void ReadChildNodes(XmlNode node, string prefix)
        {
            var nodes = node.ChildNodes;

            //skip function if has no child nodes
            if (nodes == null) return;

            foreach (XmlNode n in nodes)
            {
                var newPrefix = IncrementPrefix(prefix, n);

                ReadAllAttributes(n, newPrefix);
                ReadChildNodes(n, newPrefix);
            }
        }

        /// <summary>
        /// Increment the prefix adding name of node and attribute if needed
        /// </summary>
        /// <param name="prefix">Prefix to be incremented</param>
        /// <param name="node">Node used for build the increment</param>
        /// <returns>New prefix incremented</returns>
        private string IncrementPrefix(string prefix, XmlNode node)
        {
            string newPrefix = $"{prefix}{_separator}{node.Name}";

            var name = node.Attributes["name"];
            var key = node.Attributes["key"];

            //if the node has attribute named 'name' or 'key' its used to build new prefix
            newPrefix =
                name != null ? $"{newPrefix}{_separator}{name.Value}"
                : key != null ? $"{newPrefix}{_separator}{key.Value}"
                : newPrefix;

            return newPrefix;
        }
    }
}
