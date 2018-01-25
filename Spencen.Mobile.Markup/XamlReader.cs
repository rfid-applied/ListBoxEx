using System;

using System.Xml;
using System.Collections.Generic;

namespace Spencen.Mobile.Markup
{
    public class XamlReader
    {
        public const string MobileXamlUri = @"http://mobileui.codeplex.com/xaml";

        public XamlElement Read( XmlReader xmlReader )
        {
            var xli = (IXmlLineInfo)xmlReader;
            Stack<XamlElement> stack = new Stack<XamlElement>();
            XamlElement currentElement = null;
            XamlElement parentElement = null;
            xmlReader.Read();
            while ( !xmlReader.EOF )
            {
                switch ( xmlReader.NodeType )
                {
                    case XmlNodeType.Element:
                        currentElement = new XamlElement( xmlReader.LocalName, xmlReader.NamespaceURI );
                        currentElement.StartPos = new Position(xli.LineNumber, xli.LinePosition-1);

                        if ( parentElement != null )
                            parentElement.Children.Add( currentElement );

                        if ( !xmlReader.IsEmptyElement )
                        {
                            parentElement = currentElement;
                            stack.Push( currentElement );
                        }

                        if (xmlReader.HasAttributes)
                            xmlReader.MoveToFirstAttribute();
                        else
                        {
                            xmlReader.Read();
                            currentElement.EndPos = new Position(xli.LineNumber, xli.LinePosition);
                        }

                        break;

                    case XmlNodeType.Attribute:
                        ProcessAttribute( xmlReader, currentElement, xli );
                        break;

                    case XmlNodeType.Text:
                        currentElement.Value = xmlReader.Value;
                        xmlReader.Read();
                        break;

                    case XmlNodeType.EndElement:
                        currentElement = stack.Pop();
                        currentElement.EndPos = new Position(xli.LineNumber, xli.LinePosition + xmlReader.LocalName.Length + (string.IsNullOrEmpty(xmlReader.Prefix)? 0 : xmlReader.Prefix.Length + 1) + 1);
                        parentElement = stack.Count == 0 ? null : stack.Peek();
                        xmlReader.Read();
                        break;

                    default:
                        xmlReader.Read();
                        break;
                }
            }
            return currentElement;
        }

        private void ProcessAttribute( XmlReader xmlReader, XamlElement currentElement, IXmlLineInfo xli )
        {
            XamlAttribute lastAttribute = null;

            if ( xmlReader.Prefix == "xmlns" || xmlReader.Name == "xmlns" )
            { /* Ignore */ }
            else
            {
                if ( xmlReader.NamespaceURI == MobileXamlUri )
                {
                    if ( xmlReader.LocalName == "Key" )
                        currentElement.ResourceKey = xmlReader.Value;
                    else if ( xmlReader.LocalName == "Name" )
                        currentElement.ResourceName = xmlReader.Value;
                }
                else
                {
                    var attribute = new XamlAttribute( xmlReader.LocalName, xmlReader.NamespaceURI );
                    attribute.Value = xmlReader.Value;
                    attribute.StartPos = new Position(xli.LineNumber, xli.LinePosition);
                    currentElement.Attributes.Add(attribute);
                    lastAttribute = attribute;
                }
            }
            if (!xmlReader.MoveToNextAttribute())
            {
                xmlReader.Read();
                currentElement.EndPos = new Position(xli.LineNumber, xli.LinePosition);
            }

            if (lastAttribute != null)
            {
                lastAttribute.EndPos = new Position(xli.LineNumber, xli.LinePosition);
            }
        }
    }
}
