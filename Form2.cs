using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using ListBoxExSample.Xaml;
using System.Xml;

namespace ListBoxExSample
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            _model = new Model();
            _actions = new Actions(_model, MyUpdate);
        }

        Spencen.Mobile.Markup.XamlElement _root;

        void MyUpdate(Model model)
        {
            if (_root != model.Tree)
            {
                _root = model.Tree;
                // construct the new tree...
                this.xamlNodes.Nodes.Clear();
                this.xamlNodes.Nodes.Add(PopulateNodes(_root));
            }
            if (this.textBox1.Text != model.Source)
            {
                this.textBox1.Text = model.Source;
            }

            if (model.Tree == null)
            {
                this.panelXamlForm.Controls.Clear();
            }
            else
            {
                if (this.panelXamlForm.Controls.Count != 1 || this.panelXamlForm.Controls[0] != model.Tree.Instance)
                {
                    this.panelXamlForm.Controls.Clear();
                    this.panelXamlForm.Controls.Add((Control)model.Tree.Instance);
                }
            }

            this.textBoxFileName.Text = model.FileName;

            var nd = model.Selected;
            if (nd == null)
            {
                textBox1.DeselectAll();
                xamlNodes.SelectedNode = null;
                this.xamlPropertyGrid.SelectedObject = null;
            }
            else
            {
                var startOfs = this.textBox1.GetFirstCharIndexFromLine(nd.StartPos.Line - 1) + nd.StartPos.Column;
                var endOfs = this.textBox1.GetFirstCharIndexFromLine(nd.EndPos.Line - 1) + nd.EndPos.Column;
                textBox1.Select(startOfs, endOfs - startOfs);
                textBox1.ScrollToCaret();

                var myNode = FindCorrespondingNode((XamlTreeNode)xamlNodes.Nodes[0], nd);
                xamlNodes.SelectedNode = myNode;
                xamlPropertyGrid.SelectedObject = myNode != null? myNode.Element.Instance : null;
            }

            if (model.LastError == null)
                this.textBoxError.Text = "";
            else
            {
                var msg = "Error: \r\n" + model.LastError.Message + "\r\n";
                msg += "source: " + model.LastError.Source + "\r\n";
                if (model.LastError.InnerException != null) {
                    msg += "Inner Exception: \r\n" + model.LastError.InnerException.Message + "\r\n";
                    msg += "Inner source: " + model.LastError.InnerException.Source + "\r\n";
                    msg += "Inner stack trace: " + model.LastError.InnerException.StackTrace + "\r\n";
                }
                this.textBoxError.Text = msg;
            }
        }

        XamlTreeNode FindCorrespondingNode(XamlTreeNode node, Spencen.Mobile.Markup.XamlElement elem)
        {
            if (node.Element == elem)
                return node;
            foreach (var c in node.Nodes)
            {
                var n1 = (XamlTreeNode)c;
                var r = FindCorrespondingNode(n1, elem);
                if (r != null)
                    return r;
            }
            return null;
        }

        Model _model;
        Actions _actions;

        private void Form2_Load(object sender, EventArgs e)
        {
            _actions.Initialize();
            /*
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ListBoxExSample.TestView.xml";

            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            // put into treeview & link back to source, somehow
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var rd = new System.IO.StreamReader(stream);
                var text = rd.ReadToEnd();
                this.textBox1.Text = text;
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var render = new XamlRenderer(null);
                var tree = render.Render(stream);
                var obj = tree.Instance as Control;

                this.Controls.Add(obj);
                if (obj is System.Windows.Forms.ListBoxEx)
                {
                    var f = ((System.Windows.Forms.ListBoxEx)obj);
                }
                this.xamlNodes.Nodes.Clear();
                this.xamlNodes.Nodes.Add(PopulateNodes(tree));
            }*/
        }

        XamlTreeNode PopulateNodes(Spencen.Mobile.Markup.XamlElement element)
        {
            if (element == null)
                return null;

            var res = new XamlTreeNode(element.Name, element);
            foreach (var c in element.Children)
            {
                var r1 = PopulateNodes(c);
                res.Nodes.Add(r1);
            }

            return res;
        }

        private void xamlNodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var nd = (XamlTreeNode)this.xamlNodes.SelectedNode;
            if (nd == null)
            {
                _actions.SelectElement(null);
            }
            else
            {
                _actions.SelectElement(nd.Element);
            }
        }

        private void xamlNodes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == xamlNodes.SelectedNode)
                xamlNodes_AfterSelect(this, null);
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            string chosenFileName = null;
            using (var dlg = new System.Windows.Forms.OpenFileDialog())
            {
                dlg.CheckFileExists = true;
                dlg.DefaultExt = "*.xaml";
                dlg.Multiselect = false;
                dlg.Title = "Please select a file to view";
                var res = dlg.ShowDialog();
                if (res == DialogResult.Yes || res == DialogResult.OK)
                {
                    chosenFileName = dlg.FileName;
                }
            }
            if (string.IsNullOrEmpty(chosenFileName))
                return;

            _actions.SetFile(chosenFileName);
        }
    }

    public class XamlTreeNode : System.Windows.Forms.TreeNode
    {
        public XamlTreeNode(string id, Spencen.Mobile.Markup.XamlElement element)
            : base(id)
        {
            _element = element;
        }
        readonly Spencen.Mobile.Markup.XamlElement _element;

        public Spencen.Mobile.Markup.XamlElement Element { get { return _element; } }
    }
}
