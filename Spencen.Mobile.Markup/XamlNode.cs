using System;

namespace Spencen.Mobile.Markup
{
    public abstract class XamlNode
    {
        public XamlNode( string name, string @namespace )
        {
            Name = name;
            Namespace = @namespace;
        }

        public string Name { get; private set; }
        public string Namespace { get; private set; }
    }
}
