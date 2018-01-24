using System;
using System.Collections.Generic;

namespace Spencen.Mobile.Markup
{
    public class XamlElement : XamlNode
    {
        public XamlElement( string name, string @namespace ) : base( name, @namespace )
        {
            Attributes = new List<XamlAttribute>();
            Children = new List<XamlElement>();
        }

        public string Value { get; set; }
        public IList<XamlAttribute> Attributes { get; private set; }
        public IList<XamlElement> Children { get; private set; }
        public string ResourceKey { get; set; }
        public string ResourceName { get; set; }

        public bool IsMember { get { return Name.Contains( "." ); } }

        public string MemberName
        {
            get { return Name.Substring( Name.IndexOf( '.' ) + 1 ); }
        }

        public bool IsNamed { get { return !string.IsNullOrEmpty( ResourceName ); } }
    }
}
