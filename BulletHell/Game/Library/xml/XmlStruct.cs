using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletHell.Game.Library.xml
{
    public abstract class XmlStruct
    {
        public XmlTag Root { get; set; }

        /* Tags that are required. If the required tags are not specified, XmlValidator will throw an exception */
        public List<TagID> EssentialTags { get; set; } 

        public XmlStruct(XmlTag root)
        {
            Root = root;
        }

        /* Recursively prints the whole xml */
        public void printXmlStruct(XmlTag tag, String append, String frontAppend)
        {
            tag.printTag(frontAppend);
            Console.WriteLine(); // Line break

            for (int i = 0; tag.Children != null && i < tag.Children.Count; ++i)
                printXmlStruct(tag.Children[i], append, frontAppend += append);
        }

        public abstract XmlTag getTagFromString(String element);
    }
}
