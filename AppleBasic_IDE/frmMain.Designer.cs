
namespace AppleBasic_IDE
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mstMainForm = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closefileStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectallStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renumberToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tlsfNumberOfLines = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsfCurrentLineNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsfCurrentLinePosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsfIssueCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsfAnalyzeProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.rtbFile = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstIssues = new System.Windows.Forms.ListBox();
            this.mstMainForm.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mstMainForm
            // 
            this.mstMainForm.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.mstMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem1,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mstMainForm.Location = new System.Drawing.Point(0, 0);
            this.mstMainForm.Name = "mstMainForm";
            this.mstMainForm.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.mstMainForm.Size = new System.Drawing.Size(1143, 29);
            this.mstMainForm.TabIndex = 1;
            this.mstMainForm.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closefileStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(57, 23);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 6);
            // 
            // closefileStripMenuItem
            // 
            this.closefileStripMenuItem.Name = "closefileStripMenuItem";
            this.closefileStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.closefileStripMenuItem.Text = "Close File";
            this.closefileStripMenuItem.Click += new System.EventHandler(this.closefileStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(165, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selectallStripMenuItem,
            this.toolStripSeparator1,
            this.findToolStripMenuItem});
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(57, 23);
            this.editToolStripMenuItem1.Text = "&Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(228, 6);
            // 
            // selectallStripMenuItem
            // 
            this.selectallStripMenuItem.Name = "selectallStripMenuItem";
            this.selectallStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectallStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.selectallStripMenuItem.Text = "Select All";
            this.selectallStripMenuItem.Click += new System.EventHandler(this.selectallStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(231, 24);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyzeToolStripMenuItem,
            this.renumberToolStripMenuItem1,
            this.toolStripMenuItem3,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(66, 23);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // analyzeToolStripMenuItem
            // 
            this.analyzeToolStripMenuItem.Name = "analyzeToolStripMenuItem";
            this.analyzeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.analyzeToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.analyzeToolStripMenuItem.Text = "Analyze";
            this.analyzeToolStripMenuItem.Click += new System.EventHandler(this.analyzeToolStripMenuItem_Click);
            // 
            // renumberToolStripMenuItem1
            // 
            this.renumberToolStripMenuItem1.Name = "renumberToolStripMenuItem1";
            this.renumberToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.renumberToolStripMenuItem1.Size = new System.Drawing.Size(213, 24);
            this.renumberToolStripMenuItem1.Text = "Renumber";
            this.renumberToolStripMenuItem1.Click += new System.EventHandler(this.renumberToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(210, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(57, 23);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(123, 24);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsfNumberOfLines,
            this.tsfCurrentLineNumber,
            this.tsfCurrentLinePosition,
            this.tsfIssueCount,
            this.tsfAnalyzeProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 598);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1143, 28);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tlsfNumberOfLines
            // 
            this.tlsfNumberOfLines.ActiveLinkColor = System.Drawing.Color.Red;
            this.tlsfNumberOfLines.AutoSize = false;
            this.tlsfNumberOfLines.BackColor = System.Drawing.Color.White;
            this.tlsfNumberOfLines.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tlsfNumberOfLines.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tlsfNumberOfLines.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tlsfNumberOfLines.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tlsfNumberOfLines.Name = "tlsfNumberOfLines";
            this.tlsfNumberOfLines.Size = new System.Drawing.Size(200, 23);
            this.tlsfNumberOfLines.Text = "Total Lines:";
            this.tlsfNumberOfLines.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsfCurrentLineNumber
            // 
            this.tsfCurrentLineNumber.AutoSize = false;
            this.tsfCurrentLineNumber.BackColor = System.Drawing.Color.White;
            this.tsfCurrentLineNumber.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsfCurrentLineNumber.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsfCurrentLineNumber.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsfCurrentLineNumber.Name = "tsfCurrentLineNumber";
            this.tsfCurrentLineNumber.Size = new System.Drawing.Size(200, 23);
            this.tsfCurrentLineNumber.Text = "Current Line:";
            this.tsfCurrentLineNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsfCurrentLinePosition
            // 
            this.tsfCurrentLinePosition.AutoSize = false;
            this.tsfCurrentLinePosition.BackColor = System.Drawing.Color.White;
            this.tsfCurrentLinePosition.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsfCurrentLinePosition.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsfCurrentLinePosition.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsfCurrentLinePosition.Name = "tsfCurrentLinePosition";
            this.tsfCurrentLinePosition.Size = new System.Drawing.Size(200, 23);
            this.tsfCurrentLinePosition.Text = "Current Position:";
            this.tsfCurrentLinePosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsfIssueCount
            // 
            this.tsfIssueCount.BackColor = System.Drawing.Color.White;
            this.tsfIssueCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsfIssueCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsfIssueCount.Name = "tsfIssueCount";
            this.tsfIssueCount.Size = new System.Drawing.Size(139, 23);
            this.tsfIssueCount.Text = "Issue Count:  ";
            this.tsfIssueCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsfAnalyzeProgress
            // 
            this.tsfAnalyzeProgress.AutoSize = false;
            this.tsfAnalyzeProgress.Name = "tsfAnalyzeProgress";
            this.tsfAnalyzeProgress.Size = new System.Drawing.Size(270, 22);
            // 
            // openFile
            // 
            this.openFile.Filter = "\"Text Files|*.txt|Apple Basic 2 Files|*.AB2|All Files|*.*";
            // 
            // rtbFile
            // 
            this.rtbFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbFile.Location = new System.Drawing.Point(0, 0);
            this.rtbFile.Margin = new System.Windows.Forms.Padding(4);
            this.rtbFile.Name = "rtbFile";
            this.rtbFile.Size = new System.Drawing.Size(1143, 506);
            this.rtbFile.TabIndex = 0;
            this.rtbFile.Text = "";
            this.rtbFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtbFile_KeyPress);
            this.rtbFile.SelectionChanged += new System.EventHandler(this.rtbFile_SelectionChanged);
            this.rtbFile.TextChanged += new System.EventHandler(this.rtbFile_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 29);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbFile);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstIssues);
            this.splitContainer1.Size = new System.Drawing.Size(1143, 569);
            this.splitContainer1.SplitterDistance = 506;
            this.splitContainer1.TabIndex = 4;
            // 
            // lstIssues
            // 
            this.lstIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstIssues.FormattingEnabled = true;
            this.lstIssues.ItemHeight = 22;
            this.lstIssues.Location = new System.Drawing.Point(0, 0);
            this.lstIssues.Name = "lstIssues";
            this.lstIssues.Size = new System.Drawing.Size(1143, 59);
            this.lstIssues.TabIndex = 0;
            this.lstIssues.DoubleClick += new System.EventHandler(this.lstIssues_DoubleClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 626);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mstMainForm);
            this.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip = this.mstMainForm;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "AppleBasic IDE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.mstMainForm.ResumeLayout(false);
            this.mstMainForm.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mstMainForm;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analyzeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renumberToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tlsfNumberOfLines;
        private System.Windows.Forms.ToolStripStatusLabel tsfCurrentLineNumber;
        private System.Windows.Forms.ToolStripStatusLabel tsfCurrentLinePosition;
        private System.Windows.Forms.ToolStripProgressBar tsfAnalyzeProgress;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.RichTextBox rtbFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstIssues;
        private System.Windows.Forms.ToolStripStatusLabel tsfIssueCount;
        private System.Windows.Forms.ToolStripMenuItem selectallStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closefileStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

