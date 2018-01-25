namespace ListBoxExSample
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xamlNodes = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.xamlPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.panelXamlForm = new System.Windows.Forms.Panel();
            this.textBoxError = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // xamlNodes
            // 
            this.xamlNodes.Location = new System.Drawing.Point(504, 3);
            this.xamlNodes.Name = "xamlNodes";
            this.xamlNodes.Size = new System.Drawing.Size(180, 283);
            this.xamlNodes.TabIndex = 0;
            this.xamlNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.xamlNodes_AfterSelect);
            this.xamlNodes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.xamlNodes_NodeMouseClick);
            // 
            // textBox1
            // 
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(3, 317);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(681, 372);
            this.textBox1.TabIndex = 1;
            // 
            // xamlPropertyGrid
            // 
            this.xamlPropertyGrid.Location = new System.Drawing.Point(690, 3);
            this.xamlPropertyGrid.Name = "xamlPropertyGrid";
            this.xamlPropertyGrid.Size = new System.Drawing.Size(357, 395);
            this.xamlPropertyGrid.TabIndex = 2;
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(3, 291);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.Size = new System.Drawing.Size(496, 20);
            this.textBoxFileName.TabIndex = 3;
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(504, 288);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectFile.TabIndex = 4;
            this.buttonSelectFile.Text = "Select file...";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // panelXamlForm
            // 
            this.panelXamlForm.Location = new System.Drawing.Point(3, 3);
            this.panelXamlForm.Name = "panelXamlForm";
            this.panelXamlForm.Size = new System.Drawing.Size(496, 282);
            this.panelXamlForm.TabIndex = 5;
            // 
            // textBoxError
            // 
            this.textBoxError.Location = new System.Drawing.Point(691, 405);
            this.textBoxError.Multiline = true;
            this.textBoxError.Name = "textBoxError";
            this.textBoxError.Size = new System.Drawing.Size(356, 284);
            this.textBoxError.TabIndex = 6;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1049, 701);
            this.Controls.Add(this.textBoxError);
            this.Controls.Add(this.panelXamlForm);
            this.Controls.Add(this.buttonSelectFile);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.xamlPropertyGrid);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.xamlNodes);
            this.Location = new System.Drawing.Point(0, 52);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView xamlNodes;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PropertyGrid xamlPropertyGrid;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.Panel panelXamlForm;
        private System.Windows.Forms.TextBox textBoxError;

    }
}