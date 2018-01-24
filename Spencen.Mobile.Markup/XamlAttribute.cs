using System;

namespace Spencen.Mobile.Markup
{
    public class XamlAttribute : XamlNode
    {
        public XamlAttribute( string name, string @namespace ) : base( name, @namespace )
        {
        }

        public object Value { get; set; }
    }
}
