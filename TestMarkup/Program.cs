using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using Spencen.Mobile.Markup;

namespace TestMarkup
{
    public class MyOwnClass
    {
        public MyOwnClass()
        {
            _children = new List<MyOwnChild>();
        }
        public string Description { get; set; }
        public int Height { get; set; }
        List<MyOwnChild> _children;
        public List<MyOwnChild> Children { get { return _children; } }
    }
    public struct Rect2F
    {
        public Rect2F(float x, float y) { _x = x; _y = y; }
        float _x, _y;
        public float X { get { return _x; } }
        public float Y { get { return _y; } }
    }

    public class MyOwnChild
    {
        public MyOwnChild() { }
        public string ID { get; set; }
        public Rect2F Rect { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TestMarkup.TestView.xml";

            using (var stream = assembly.GetManifestResourceStream("TestMarkup.Test0.xml"))
            {
                var xmlReader = new System.Xml.XmlTextReader(stream);
                var xamlReader = new Spencen.Mobile.Markup.XamlReader();
                var rootElement = xamlReader.Read(xmlReader);
                Debug.Assert(rootElement != null, "woops");
                Debug.Assert(rootElement.StartPos == new Position(3, 1));
                Debug.Assert(rootElement.EndPos == new Position(4, 44));
            }

            using (var stream = assembly.GetManifestResourceStream("TestMarkup.Test1.xml"))
            {
                var xmlReader = new System.Xml.XmlTextReader(stream);
                var xamlReader = new Spencen.Mobile.Markup.XamlReader();
                var rootElement = xamlReader.Read(xmlReader);
                Debug.Assert(rootElement != null, "woops");
                Debug.Assert(rootElement.StartPos == new Position(2, 1));
                Debug.Assert(rootElement.EndPos == new Position(5, 7));
            }

            using (var stream = assembly.GetManifestResourceStream("TestMarkup.Test2.xml"))
            {
                var xmlReader = new System.Xml.XmlTextReader(stream);
                var xamlReader = new Spencen.Mobile.Markup.XamlReader();
                var rootElement = xamlReader.Read(xmlReader);
                Debug.Assert(rootElement != null, "woops");
                Debug.Assert(rootElement.StartPos == new Position(2, 1));
                Debug.Assert(rootElement.EndPos == new Position(7, 7));

                Debug.Assert(rootElement.Children.Count == 2);
                
                Debug.Assert(rootElement.Children[0].StartPos == new Position(5, 3));
                Debug.Assert(rootElement.Children[0].EndPos == new Position(5, 21));
                Debug.Assert(rootElement.Children[0].Attributes.Count == 2);
                Debug.Assert(rootElement.Children[0].Attributes[0].StartPos == new Position(5, 8));
                Debug.Assert(rootElement.Children[0].Attributes[0].EndPos == new Position(5, 14)); // not very accurate...
                Debug.Assert(rootElement.Children[0].Attributes[1].StartPos == new Position(5, 14));
                Debug.Assert(rootElement.Children[0].Attributes[1].EndPos == new Position(5, 21)); // not very accurate...

                Debug.Assert(rootElement.Children[1].StartPos == new Position(6, 3));
                Debug.Assert(rootElement.Children[1].EndPos == new Position(6, 9));
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var render = new XamlRenderer(null);
                var obj = render.Render(stream);

                System.Diagnostics.Debug.Assert(obj != null && obj is MyOwnClass);
                var o1 = (obj as MyOwnClass);
                System.Diagnostics.Debug.Assert(o1.Description == "THE MOST DESCRIPTIVE THING");
                System.Diagnostics.Debug.Assert(o1.Height == 25);

                System.Diagnostics.Debug.Assert(o1.Children != null);
                System.Diagnostics.Debug.Assert(o1.Children[0].ID == "child1");
                System.Diagnostics.Debug.Assert(o1.Children[1].ID == "child2");

            }
        }
    }
}
