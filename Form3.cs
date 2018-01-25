using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dive;

namespace ListBoxExSample
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            _model = new Model();
            _actions = new Actions(_model, MyUpdate);
        }

        Model _model;
        Actions _actions;

        private void Form1_Load(object sender, EventArgs e)
        {
            _actions.Initialize();
        }

        void MyUpdate(Model model)
        {
            if (model.Tree == null)
            {
                this.panel1.Controls.Clear();
            }
            else
            {
                if (this.panel1.Controls.Count != 1 || this.panel1.Controls[0] != model.Tree.Instance)
                {
                    this.panel1.Controls.Clear();
                    this.panel1.Controls.Add((Control)model.Tree.Instance);
                }
            }

            if (model.LastError == null)
            {
                // nop
            }
            else
            {
                var msg = "Error: \r\n" + model.LastError.Message + "\r\n";
#if !PocketPC
                msg += "source: " + model.LastError.Source + "\r\n";
#endif
                if (model.LastError.InnerException != null)
                {
                    msg += "Inner Exception: \r\n" + model.LastError.InnerException.Message + "\r\n";
#if !PocketPC
                    msg += "Inner source: " + model.LastError.InnerException.Source + "\r\n";
#endif
                    msg += "Inner stack trace: " + model.LastError.InnerException.StackTrace + "\r\n";
                }
                System.Windows.Forms.MessageBox.Show(msg);
            }
        }

        private void menuItem1_Click_1(object sender, EventArgs e)
        {
            string chosenFileName = null;
            using (var dlg = new System.Windows.Forms.OpenFileDialog())
            {
                var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                dlg.InitialDirectory = path;

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
}