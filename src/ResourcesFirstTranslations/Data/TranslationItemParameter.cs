using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ResourcesFirstTranslations.Data
{
    public class TranslationItemParameter
    {
        public int FileId { get; set; }
        public string ResourceId { get; set; }
        public string Culture { get; set; }

        // Format of Xml:
        //
        // <Translations>
        //    <Item FileId="1" ResourceId="X.Y" Culture="de" />
        //    <Item FileId="1" ResourceId="A.B" Culture="de" />
        // </Translations>
        public static string ListToXml(List<TranslationItemParameter> items)
        {
            var xdoc = new XmlDocument();
            XmlElement root = xdoc.CreateElement("Translations");

            foreach (var item in items)
            {
                XmlElement da = xdoc.CreateElement("Item");
                da.SetAttribute("FileId", item.FileId.ToString());
                da.SetAttribute("ResourceId", item.ResourceId);
                da.SetAttribute("Culture", item.Culture);

                root.AppendChild(da);
            }

            xdoc.AppendChild(root);

            var stb = new StringBuilder();
            var tw = new StringWriter(stb);
            xdoc.Save(tw);
            tw.Flush();

            return stb.ToString();
        }
    }
}
