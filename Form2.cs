using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using ListBoxExSample.Xaml;

namespace ListBoxExSample
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ListBoxExSample.TestView.xml";

            string[] resourceNames =
    Assembly.GetExecutingAssembly().GetManifestResourceNames();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var render = new XamlRenderer(null);
                var obj = render.Render(stream);

                this.Controls.Add(obj as Control);
                if (obj is System.Windows.Forms.ListBoxEx)
                {
                    var f = ((System.Windows.Forms.ListBoxEx)obj);
                    /*
                    f.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right)));
                    f.ScrollTop = 0;
                    f.Border.Top = true;*/
                }

                /*
                System.Diagnostics.Debug.Assert(obj != null && obj is MyOwnClass);
                var o1 = (obj as MyOwnClass);
                System.Diagnostics.Debug.Assert(o1.Description == "THE MOST DESCRIPTIVE THING");
                System.Diagnostics.Debug.Assert(o1.Height == 25);

                System.Diagnostics.Debug.Assert(o1.Children != null);
                System.Diagnostics.Debug.Assert(o1.Children[0].ID == "child1");
                System.Diagnostics.Debug.Assert(o1.Children[1].ID == "child2");
                */
            }
        }
    }
}
