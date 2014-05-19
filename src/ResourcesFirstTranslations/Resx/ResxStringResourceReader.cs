using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace ResourcesFirstTranslations.Resx
{
    public class ResxStringResourceReader
    {
        public List<ResxStringResource> ResxToResourceStringDictionary(TextReader resxFileContent)
        {
            try
            {
                var result = new List<ResxStringResource>();

                // Reading comments: http://msdn.microsoft.com/en-us/library/system.resources.resxdatanode.aspx
                using (var reader = new ResXResourceReader(resxFileContent))
                {
                    reader.UseResXDataNodes = true;
                    var dict = reader.GetEnumerator();

                    while (dict.MoveNext())
                    {
                        var node = (ResXDataNode)dict.Value;

                        result.Add(new ResxStringResource()
                        {
                            Name = node.Name,
                            Value = (string)node.GetValue((ITypeResolutionService)null),
                            Comment = node.Comment
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return null;
        }
    }
}
