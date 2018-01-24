using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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
