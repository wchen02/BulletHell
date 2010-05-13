using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletHell.Game.Library.xml
{
    public class XmlTag
    {
        public String Tag { get; private set; } // name of the tag
        public Dictionary<String, String> Attributes { get; private set; } // Key = Value pair. If Value is not specified, the default will be used

        /* Attributes that are required. If the required attribs are not specified, XmlValidator will throw an exception */
        public List<String> EssentialAttributes { get; private set; }
        public List<XmlTag> Children { get; private set; }
        public bool Initialized { get; private set; }
        public int tagId { get; set; } // Unique tag ID for this type of tags

        public XmlTag(String tag, Dictionary<String, String> attributes, List<XmlTag> children)
        {
            Tag = tag;
            Attributes = attributes;
            Children = children;
            Initialized = true;
            Attributes = new Dictionary<string, string>();
            EssentialAttributes = new List<string>();
            Children = new List<XmlTag>();
        }

        public XmlTag(String tag, Dictionary<String, String> attributes) : this(tag, attributes, null) { }

        public XmlTag(String tag) : this(tag, null, null) { }

        public XmlTag(XmlTag tag)
        { // C# doesn't have the idea of copy constructor!
            Tag = tag.Tag;
            tagId = tag.tagId;

            if (tag.Attributes != null)
            {
                Attributes = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> attrib in tag.Attributes)
                    Attributes.Add(attrib.Key, attrib.Value);
            }

            if (tag.Children != null)
            {
                Children = new List<XmlTag>();
                foreach (XmlTag child in tag.Children)
                    Children.Add(child);
            }

            if (tag.EssentialAttributes != null)
            {
                EssentialAttributes = new List<string>();
                foreach (string attrib in tag.EssentialAttributes)
                    EssentialAttributes.Add(attrib);
            }

            Initialized = true;
        }

        public XmlTag() { Initialized = false; }

        /* Prints the tag, its attribs. Doesn't print its children */
        public void printTag(String frontAppend)
        {
            Console.WriteLine(frontAppend + "Tag: " + Tag);
            Console.Write(frontAppend + "Attributes: ");
            if (Attributes != null && Attributes.Count > 0)
            {
                Console.WriteLine("");

                foreach (KeyValuePair<String, String> attrib in Attributes)
                    Console.WriteLine(frontAppend + "\t" + attrib.Key + " = " + attrib.Value);
            }
            else
                Console.WriteLine("none");
        }

        public void printTag()
        {
            printTag("");
        }

        /* Prints the tag, its attribs and its children's tag name */
        public void printTagVerbose(String frontAppend)
        {
            printTag();
            Console.Write(frontAppend + "Children Tag: ");
            if (Children != null && Children.Count > 0)
            {
                Console.WriteLine("");
                for (int i = 0; i < Children.Count; ++i)
                    Console.WriteLine(frontAppend + "\t" + Children[i]);
            }
            else
                Console.WriteLine("none");
        }
    }
}
