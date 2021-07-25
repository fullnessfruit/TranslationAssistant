namespace TranslationAssistant
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.labelLineNumber = new System.Windows.Forms.Label();
			this.checkBoxClipboard = new System.Windows.Forms.CheckBox();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonPrevious = new System.Windows.Forms.Button();
			this.numericUpDownLineNumber = new System.Windows.Forms.NumericUpDown();
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.contextMenuStripRichTextBox = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.jumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dictionaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.translateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dictionaryLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.translateLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.buttonWebBrowserVersion = new System.Windows.Forms.Button();
			this.textBoxAddress = new System.Windows.Forms.TextBox();
			this.buttonTranslate = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonRefresh = new System.Windows.Forms.Button();
			this.buttonForward = new System.Windows.Forms.Button();
			this.buttonBack = new System.Windows.Forms.Button();
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			this.timerClipboardObserver = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLineNumber)).BeginInit();
			this.contextMenuStripRichTextBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer1.Size = new System.Drawing.Size(1280, 720);
			this.splitContainer1.SplitterDistance = 569;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.labelLineNumber);
			this.splitContainer2.Panel1.Controls.Add(this.checkBoxClipboard);
			this.splitContainer2.Panel1.Controls.Add(this.buttonNext);
			this.splitContainer2.Panel1.Controls.Add(this.buttonPrevious);
			this.splitContainer2.Panel1.Controls.Add(this.numericUpDownLineNumber);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.richTextBox);
			this.splitContainer2.Size = new System.Drawing.Size(569, 720);
			this.splitContainer2.SplitterDistance = 26;
			this.splitContainer2.TabIndex = 0;
			// 
			// labelLineNumber
			// 
			this.labelLineNumber.AutoSize = true;
			this.labelLineNumber.Location = new System.Drawing.Point(4, 9);
			this.labelLineNumber.Name = "labelLineNumber";
			this.labelLineNumber.Size = new System.Drawing.Size(43, 13);
			this.labelLineNumber.TabIndex = 4;
			this.labelLineNumber.Text = "줄 번호";
			// 
			// checkBoxClipboard
			// 
			this.checkBoxClipboard.AutoSize = true;
			this.checkBoxClipboard.Location = new System.Drawing.Point(339, 8);
			this.checkBoxClipboard.Name = "checkBoxClipboard";
			this.checkBoxClipboard.Size = new System.Drawing.Size(95, 17);
			this.checkBoxClipboard.TabIndex = 3;
			this.checkBoxClipboard.Text = "클립보드 감시";
			this.checkBoxClipboard.UseVisualStyleBackColor = true;
			this.checkBoxClipboard.CheckedChanged += new System.EventHandler(this.checkBoxClipboard_CheckedChanged);
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(257, 4);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(75, 23);
			this.buttonNext.TabIndex = 2;
			this.buttonNext.Text = "↓";
			this.buttonNext.UseVisualStyleBackColor = true;
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.Location = new System.Drawing.Point(175, 4);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(75, 23);
			this.buttonPrevious.TabIndex = 1;
			this.buttonPrevious.Text = "↑";
			this.buttonPrevious.UseVisualStyleBackColor = true;
			this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
			// 
			// numericUpDownLineNumber
			// 
			this.numericUpDownLineNumber.Location = new System.Drawing.Point(48, 6);
			this.numericUpDownLineNumber.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownLineNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownLineNumber.Name = "numericUpDownLineNumber";
			this.numericUpDownLineNumber.Size = new System.Drawing.Size(120, 20);
			this.numericUpDownLineNumber.TabIndex = 0;
			this.numericUpDownLineNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDownLineNumber.ValueChanged += new System.EventHandler(this.numericUpDownLineNumber_ValueChanged);
			// 
			// richTextBox
			// 
			this.richTextBox.ContextMenuStrip = this.contextMenuStripRichTextBox;
			this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox.Font = new System.Drawing.Font("Yu Gothic", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.richTextBox.HideSelection = false;
			this.richTextBox.Location = new System.Drawing.Point(0, 0);
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.Size = new System.Drawing.Size(569, 690);
			this.richTextBox.TabIndex = 0;
			this.richTextBox.Text = "";
			this.richTextBox.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
			// 
			// contextMenuStripRichTextBox
			// 
			this.contextMenuStripRichTextBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToolStripMenuItem,
            this.dictionaryToolStripMenuItem,
            this.translateToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.dictionaryLineToolStripMenuItem,
            this.translateLineToolStripMenuItem});
			this.contextMenuStripRichTextBox.Name = "contextMenuStripRichTextBox";
			this.contextMenuStripRichTextBox.Size = new System.Drawing.Size(192, 136);
			this.contextMenuStripRichTextBox.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripRichTextBox_Opening);
			// 
			// jumpToolStripMenuItem
			// 
			this.jumpToolStripMenuItem.Name = "jumpToolStripMenuItem";
			this.jumpToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
			this.jumpToolStripMenuItem.Text = "이 줄로 이동";
			this.jumpToolStripMenuItem.Click += new System.EventHandler(this.jumpToolStripMenuItem_Click);
			// 
			// dictionaryToolStripMenuItem
			// 
			this.dictionaryToolStripMenuItem.Name = "dictionaryToolStripMenuItem";
			this.dictionaryToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
			this.dictionaryToolStripMenuItem.Text = "사전에서 찾기";
			this.dictionaryToolStripMenuItem.Click += new System.EventHandler(this.dictionaryToolStripMenuItem_Click);
			// 
			// translateToolStripMenuItem
			// 
			this.translateToolStripMenuItem.Name = "translateToolStripMenuItem";
			this.translateToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
			this.translateToolStripMenuItem.Text = "번역";
			this.translateToolStripMenuItem.Click += new System.EventHandler(this.translateToolStripMenuItem_Click);
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
			this.searchToolStripMenuItem.Text = "검색";
			this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
			// 
			// dictionaryLineToolStripMenuItem
			// 
			this.dictionaryLineToolStripMenuItem.Name = "dictionaryLineToolStripMenuItem";
			this.dictionaryLineToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
			this.dictionaryLineToolStripMenuItem.Text = "이 줄을 사전에서 찾기";
			this.dictionaryLineToolStripMenuItem.Click += new System.EventHandler(this.dictionaryLineToolStripMenuItem_Click);
			// 
			// translateLineToolStripMenuItem
			// 
			this.translateLineToolStripMenuItem.Name = "translateLineToolStripMenuItem";
			this.translateLineToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
			this.translateLineToolStripMenuItem.Text = "이 줄을 번역";
			this.translateLineToolStripMenuItem.Click += new System.EventHandler(this.translateLineToolStripMenuItem_Click);
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer3.IsSplitterFixed = true;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.buttonWebBrowserVersion);
			this.splitContainer3.Panel1.Controls.Add(this.textBoxAddress);
			this.splitContainer3.Panel1.Controls.Add(this.buttonTranslate);
			this.splitContainer3.Panel1.Controls.Add(this.buttonStop);
			this.splitContainer3.Panel1.Controls.Add(this.buttonRefresh);
			this.splitContainer3.Panel1.Controls.Add(this.buttonForward);
			this.splitContainer3.Panel1.Controls.Add(this.buttonBack);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.webBrowser);
			this.splitContainer3.Size = new System.Drawing.Size(707, 720);
			this.splitContainer3.TabIndex = 0;
			// 
			// buttonWebBrowserVersion
			// 
			this.buttonWebBrowserVersion.Location = new System.Drawing.Point(414, 4);
			this.buttonWebBrowserVersion.Name = "buttonWebBrowserVersion";
			this.buttonWebBrowserVersion.Size = new System.Drawing.Size(75, 23);
			this.buttonWebBrowserVersion.TabIndex = 5;
			this.buttonWebBrowserVersion.Text = "버전 설정";
			this.buttonWebBrowserVersion.UseVisualStyleBackColor = true;
			this.buttonWebBrowserVersion.Click += new System.EventHandler(this.buttonWebBrowserVersion_Click);
			// 
			// textBoxAddress
			// 
			this.textBoxAddress.Location = new System.Drawing.Point(5, 30);
			this.textBoxAddress.Name = "textBoxAddress";
			this.textBoxAddress.Size = new System.Drawing.Size(691, 20);
			this.textBoxAddress.TabIndex = 6;
			this.textBoxAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxAddress_KeyDown);
			// 
			// buttonTranslate
			// 
			this.buttonTranslate.Location = new System.Drawing.Point(332, 4);
			this.buttonTranslate.Name = "buttonTranslate";
			this.buttonTranslate.Size = new System.Drawing.Size(75, 23);
			this.buttonTranslate.TabIndex = 4;
			this.buttonTranslate.Text = "페이지 번역";
			this.buttonTranslate.UseVisualStyleBackColor = true;
			this.buttonTranslate.Click += new System.EventHandler(this.buttonTranslate_Click);
			// 
			// buttonStop
			// 
			this.buttonStop.Location = new System.Drawing.Point(250, 4);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(75, 23);
			this.buttonStop.TabIndex = 3;
			this.buttonStop.Text = "중지";
			this.buttonStop.UseVisualStyleBackColor = true;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// buttonRefresh
			// 
			this.buttonRefresh.Location = new System.Drawing.Point(168, 4);
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
			this.buttonRefresh.TabIndex = 2;
			this.buttonRefresh.Text = "새로 고침";
			this.buttonRefresh.UseVisualStyleBackColor = true;
			this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
			// 
			// buttonForward
			// 
			this.buttonForward.Location = new System.Drawing.Point(86, 4);
			this.buttonForward.Name = "buttonForward";
			this.buttonForward.Size = new System.Drawing.Size(75, 23);
			this.buttonForward.TabIndex = 1;
			this.buttonForward.Text = "→";
			this.buttonForward.UseVisualStyleBackColor = true;
			this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
			// 
			// buttonBack
			// 
			this.buttonBack.Location = new System.Drawing.Point(4, 4);
			this.buttonBack.Name = "buttonBack";
			this.buttonBack.Size = new System.Drawing.Size(75, 23);
			this.buttonBack.TabIndex = 0;
			this.buttonBack.Text = "←";
			this.buttonBack.UseVisualStyleBackColor = true;
			this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
			// 
			// webBrowser
			// 
			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser.Location = new System.Drawing.Point(0, 0);
			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new System.Drawing.Size(707, 666);
			this.webBrowser.TabIndex = 0;
			this.webBrowser.Url = new System.Uri("about:blank", System.UriKind.Absolute);
			this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
			this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
			// 
			// timerClipboardObserver
			// 
			this.timerClipboardObserver.Interval = 1000;
			this.timerClipboardObserver.Tick += new System.EventHandler(this.timerClipboardObserver_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1280, 720);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.ShowIcon = false;
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLineNumber)).EndInit();
			this.contextMenuStripRichTextBox.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel1.PerformLayout();
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonPrevious;
		private System.Windows.Forms.NumericUpDown numericUpDownLineNumber;
		private System.Windows.Forms.RichTextBox richTextBox;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.TextBox textBoxAddress;
		private System.Windows.Forms.Button buttonTranslate;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Button buttonRefresh;
		private System.Windows.Forms.Button buttonForward;
		private System.Windows.Forms.Button buttonBack;
		private System.Windows.Forms.WebBrowser webBrowser;
		private System.Windows.Forms.CheckBox checkBoxClipboard;
		private System.Windows.Forms.Timer timerClipboardObserver;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripRichTextBox;
		private System.Windows.Forms.ToolStripMenuItem jumpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dictionaryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem translateToolStripMenuItem;
		private System.Windows.Forms.Label labelLineNumber;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dictionaryLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem translateLineToolStripMenuItem;
		private System.Windows.Forms.Button buttonWebBrowserVersion;
	}
}

