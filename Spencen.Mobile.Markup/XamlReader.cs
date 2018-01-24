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

                        if ( parentElement != null )
                            parentElement.Children.Add( currentElement );

                        if ( !xmlReader.IsEmptyElement )
                        {
                            parentElement = currentElement;
                            stack.Push( currentElement );
                        }

                        if ( xmlReader.HasAttributes )
                            xmlReader.MoveToFirstAttribute();
                        else
                            xmlReader.Read();

                        break;

                    case XmlNodeType.Attribute:
                        ProcessAttribute( xmlReader, currentElement );
                        break;

                    case XmlNodeType.Text:
                        currentElement.Value = xmlReader.Value;
                        xmlReader.Read();
                        break;

                    case XmlNodeType.EndElement:
                        currentElement = stack.Pop();
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

        private void ProcessAttribute( XmlReader xmlReader, XamlElement currentElement )
        {
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
                    currentElement.Attributes.Add( attribute );
                }
            }
            if ( !xmlReader.MoveToNextAttribute() )
                xmlReader.Read();
        }
    }
}
