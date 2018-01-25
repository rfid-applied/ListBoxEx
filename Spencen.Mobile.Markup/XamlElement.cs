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

        // link to the widget that was instantiated from this XamlElement
        public object Instance { get; set; }

        public bool IsMember { get { return Name.Contains( "." ); } }

        public string MemberName
        {
            get { return Name.Substring( Name.IndexOf( '.' ) + 1 ); }
        }

        public bool IsNamed { get { return !string.IsNullOrEmpty( ResourceName ); } }
    }

    public struct Position
    {
        public Position(int line, int column)
        {
            _line = line;
            _column = column;
        }
        int _line, _column;
        public int Line { get { return _line; } }
        public int Column { get { return _column; } }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1._column == p2._column && p1._line == p2._line;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1._column == p2._column && p1._line == p2._line);
        }
    }
}
