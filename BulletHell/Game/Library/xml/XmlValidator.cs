using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BulletHell.System;
using BulletHell.Game;
using BulletHell.Game.Object;
using BulletHell.Game.Interface;

namespace BulletHell.Game.Library.xml
{
    /* After I wrote this class, I realized that a validator is already present in the C# System.Xml.
     * Writing a whole new validator seems to be time-wasting, however, 
     * I do appreciate my own class as I know it's capability and how the underlying validation works. */

    public class XmlValidator
    {
        private Stack<XmlTag> stack = new Stack<XmlTag>();
        private XmlStruct xmlStruct = null;
        private int indent = 0; // To be used with DEBUG, keeps track of indentation

        public XmlValidator(XmlStruct xmlStruct)
        {
            this.xmlStruct = xmlStruct;
        }

        /* Validates the file given, with the xmlStruct */
        public bool validateXml(String filename)
        {
            stack.Clear();
            XmlTextReader reader = new XmlTextReader(filename);
            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // Open tag
                            if (_GLOBAL.Debug)
                            {
                                for (int i = 0; i < indent; ++i)
                                    Console.Write("\t");
                                Console.Write("<" + reader.Name.ToLower());
                                indent++;
                            }

                            /* Reads the tag and its attribs */
                            readElement(xmlStruct, reader, xmlStruct.getTagFromString(reader.Name.ToLower()));

                            /* If the tag is an EssentialTag, removes it from the struct. 
                             * At the end if all EssentialTags are removed, then it means all of the required tags are satisfied */
                            if (xmlStruct.EssentialTags.Contains((TagID)stack.Peek().tagId))
                                xmlStruct.EssentialTags.Remove((TagID)stack.Peek().tagId);

                            if (_GLOBAL.Debug) Console.WriteLine(">");
                            break;
                        case XmlNodeType.Text: // Value of tag
                            if (_GLOBAL.Debug)
                            {
                                for (int i = 0; i < indent; ++i)
                                    Console.Write("\t");
                                Console.WriteLine(reader.Value);
                            }
                            break;
                        case XmlNodeType.EndElement: // Close tag
                            if (_GLOBAL.Debug)
                            {
                                indent--;
                                for (int i = 0; i < indent; ++i)
                                    Console.Write("\t");
                                Console.WriteLine("</" + reader.Name.ToLower() + ">");
                            }

                            if (stack.Peek().Tag != reader.Name.ToLower())
                                throw new Exception("Error 2: " + stack.Peek().Tag + " should be closed before preceeding. " + "Line " + reader.LineNumber + ", position " + reader.LinePosition);

                            stack.Pop();
                            break;
                        case XmlNodeType.Comment:
                            break; // ignore comments
                    }
                }

                if (xmlStruct.EssentialTags.Count > 0)
                    throw new Exception("Error 7: Not all required tags for the xmlStruct are satisfied.");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        private void readAttribute(XmlTag tag, XmlTextReader reader, XmlTag element)
        {
            List<string> essentialAttributes = null; //C# doesn't have copy constructor
            if (tag.EssentialAttributes != null)
            {
                essentialAttributes = new List<string>();
                foreach (string attrib in tag.EssentialAttributes)
                    essentialAttributes.Add(attrib);
            }

            while (reader.MoveToNextAttribute())
            {
                if (tag.Attributes != null && tag.Attributes.Count > 0 && tag.Attributes.ContainsKey(reader.Name.ToLower()))
                {
                    if (_GLOBAL.Debug) Console.Write(" " + reader.Name.ToLower() + "=\"" + reader.Value.ToLower() + "\"");
                    if (essentialAttributes != null)
                        essentialAttributes.Remove(reader.Name.ToLower()); // Doesn't check for dupe attribs, C# checks it for us
                    continue;
                }
                throw new Exception("Error 3: " + tag.Tag + " does not have an Attribute called '" + reader.Name.ToLower() + "'. " + "Line " + reader.LineNumber + ", position " + reader.LinePosition);
            }
            if (essentialAttributes != null && essentialAttributes.Count > 0)
                throw new Exception("Error 6: Not all required attributes for '" + tag.Tag + "' are satisified. " + "Line " + reader.LineNumber + ", position " + reader.LinePosition);
        }

        private bool readChildren(XmlTag tag, XmlTextReader reader, XmlTag element)
        {
            int i;
            for (i = 0; tag.Children != null && i < tag.Children.Count; ++i)
                if (element.Tag == tag.Children[i].Tag)
                    return true;

            if (tag.Children == null || i >= tag.Children.Count)
                throw new Exception("Error 4: " + tag.Tag + " does not have a child tag called '" + reader.Name.ToLower() + "'. " + "Line " + reader.LineNumber + ", position " + reader.LinePosition);

            return false;
        }

        private void readElement(XmlStruct xmlStruct, XmlTextReader reader, XmlTag element)
        {
            /* If the stack is emepty, push only the root tag */
            if (stack.Count == 0 && element.Tag == xmlStruct.Root.Tag)
            {
                stack.Push(element);
                readAttribute(xmlStruct.Root, reader, element);
            }
            else if (stack.Count > 0 && readChildren(stack.Peek(), reader, element)) // else push, if the tag is a child of the stack.Peek().
            {
                stack.Push(element);
                readAttribute(stack.Peek(), reader, element);
            }
            else
                throw new Exception("Error 5: Invalid Tag '" + element.Tag + "'. " + "Line " + reader.LineNumber + ", position " + reader.LinePosition);
        }
    }
}
