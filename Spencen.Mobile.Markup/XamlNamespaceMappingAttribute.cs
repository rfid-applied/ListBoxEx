using System;

namespace Spencen.Mobile.Markup
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class XamlNamespaceMappingAttribute : Attribute
    {
        public string XmlNamespace { get; set; }
        public string ClrNamespace { get; set; }
        public string AssemblyName { get; set; }
    }
}
