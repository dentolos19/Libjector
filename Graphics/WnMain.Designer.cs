namespace WxInjector.Graphics
{

    partial class WnMain
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (components != null))
                components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WnMain));
            this.GbProcesses = new System.Windows.Forms.GroupBox();
            this.TbProcess = new System.Windows.Forms.TextBox();
            this.BnRefresh = new System.Windows.Forms.Button();
            this.LbProcesses = new System.Windows.Forms.ListBox();
            this.LaTitle = new System.Windows.Forms.Label();
            this.GbDLLs = new System.Windows.Forms.GroupBox();
            this.TbDLL = new System.Windows.Forms.TextBox();
            this.LbClear = new System.Windows.Forms.Button();
            this.BnRemove = new System.Windows.Forms.Button();
            this.BnAdd = new System.Windows.Forms.Button();
            this.LbDLLs = new System.Windows.Forms.ListBox();
            this.BnInject = new System.Windows.Forms.Button();
            this.LaStatus = new System.Windows.Forms.Label();
            this.BnExit = new System.Windows.Forms.Button();
            this.Checker = new System.ComponentModel.BackgroundWorker();
            this.GbProcesses.SuspendLayout();
            this.GbDLLs.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbProcesses
            // 
            this.GbProcesses.Controls.Add(this.TbProcess);
            this.GbProcesses.Controls.Add(this.BnRefresh);
            this.GbProcesses.Controls.Add(this.LbProcesses);
            this.GbProcesses.Location = new System.Drawing.Point(12, 12);
            this.GbProcesses.Name = "GbProcesses";
            this.GbProcesses.Size = new System.Drawing.Size(250, 329);
            this.GbProcesses.TabIndex = 0;
            this.GbProcesses.TabStop = false;
            this.GbProcesses.Text = "Processes";
            // 
            // TbProcess
            // 
            this.TbProcess.Location = new System.Drawing.Point(6, 265);
            this.TbProcess.Name = "TbProcess";
            this.TbProcess.ReadOnly = true;
            this.TbProcess.Size = new System.Drawing.Size(238, 22);
            this.TbProcess.TabIndex = 2;
            // 
            // BnRefresh
            // 
            this.BnRefresh.Location = new System.Drawing.Point(6, 293);
            this.BnRefresh.Name = "BnRefresh";
            this.BnRefresh.Size = new System.Drawing.Size(238, 30);
            this.BnRefresh.TabIndex = 1;
            this.BnRefresh.Text = "Refresh";
            this.BnRefresh.UseVisualStyleBackColor = true;
            this.BnRefresh.Click += new System.EventHandler(this.Refresh);
            // 
            // LbProcesses
            // 
            this.LbProcesses.FormattingEnabled = true;
            this.LbProcesses.ItemHeight = 16;
            this.LbProcesses.Location = new System.Drawing.Point(6, 31);
            this.LbProcesses.Name = "LbProcesses";
            this.LbProcesses.Size = new System.Drawing.Size(238, 228);
            this.LbProcesses.TabIndex = 0;
            this.LbProcesses.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProcessesClick);
            // 
            // LaTitle
            // 
            this.LaTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LaTitle.Location = new System.Drawing.Point(268, 12);
            this.LaTitle.Name = "LaTitle";
            this.LaTitle.Size = new System.Drawing.Size(502, 30);
            this.LaTitle.TabIndex = 1;
            this.LaTitle.Text = "WxInjector v1.1.2";
            this.LaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GbDLLs
            // 
            this.GbDLLs.Controls.Add(this.TbDLL);
            this.GbDLLs.Controls.Add(this.LbClear);
            this.GbDLLs.Controls.Add(this.BnRemove);
            this.GbDLLs.Controls.Add(this.BnAdd);
            this.GbDLLs.Controls.Add(this.LbDLLs);
            this.GbDLLs.Location = new System.Drawing.Point(268, 45);
            this.GbDLLs.Name = "GbDLLs";
            this.GbDLLs.Size = new System.Drawing.Size(502, 263);
            this.GbDLLs.TabIndex = 2;
            this.GbDLLs.TabStop = false;
            this.GbDLLs.Text = "DLLs";
            // 
            // TbDLL
            // 
            this.TbDLL.Location = new System.Drawing.Point(6, 235);
            this.TbDLL.Name = "TbDLL";
            this.TbDLL.ReadOnly = true;
            this.TbDLL.Size = new System.Drawing.Size(384, 22);
            this.TbDLL.TabIndex = 4;
            // 
            // LbClear
            // 
            this.LbClear.Location = new System.Drawing.Point(396, 93);
            this.LbClear.Name = "LbClear";
            this.LbClear.Size = new System.Drawing.Size(100, 30);
            this.LbClear.TabIndex = 3;
            this.LbClear.Text = "Clear";
            this.LbClear.UseVisualStyleBackColor = true;
            this.LbClear.Click += new System.EventHandler(this.Clear);
            // 
            // BnRemove
            // 
            this.BnRemove.Location = new System.Drawing.Point(396, 57);
            this.BnRemove.Name = "BnRemove";
            this.BnRemove.Size = new System.Drawing.Size(100, 30);
            this.BnRemove.TabIndex = 2;
            this.BnRemove.Text = "Remove";
            this.BnRemove.UseVisualStyleBackColor = true;
            this.BnRemove.Click += new System.EventHandler(this.Remove);
            // 
            // BnAdd
            // 
            this.BnAdd.Location = new System.Drawing.Point(396, 21);
            this.BnAdd.Name = "BnAdd";
            this.BnAdd.Size = new System.Drawing.Size(100, 30);
            this.BnAdd.TabIndex = 1;
            this.BnAdd.Text = "Add";
            this.BnAdd.UseVisualStyleBackColor = true;
            this.BnAdd.Click += new System.EventHandler(this.Add);
            // 
            // LbDLLs
            // 
            this.LbDLLs.AllowDrop = true;
            this.LbDLLs.FormattingEnabled = true;
            this.LbDLLs.ItemHeight = 16;
            this.LbDLLs.Location = new System.Drawing.Point(6, 21);
            this.LbDLLs.Name = "LbDLLs";
            this.LbDLLs.Size = new System.Drawing.Size(384, 212);
            this.LbDLLs.TabIndex = 0;
            this.LbDLLs.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DLLsClick);
            this.LbDLLs.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDrop);
            this.LbDLLs.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileEnter);
            // 
            // BnInject
            // 
            this.BnInject.Location = new System.Drawing.Point(268, 314);
            this.BnInject.Name = "BnInject";
            this.BnInject.Size = new System.Drawing.Size(100, 30);
            this.BnInject.TabIndex = 3;
            this.BnInject.Text = "Inject";
            this.BnInject.UseVisualStyleBackColor = true;
            this.BnInject.Click += new System.EventHandler(this.Inject);
            // 
            // LaStatus
            // 
            this.LaStatus.Location = new System.Drawing.Point(480, 314);
            this.LaStatus.Name = "LaStatus";
            this.LaStatus.Size = new System.Drawing.Size(290, 30);
            this.LaStatus.TabIndex = 4;
            this.LaStatus.Text = "Created By Dennise";
            this.LaStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BnExit
            // 
            this.BnExit.Location = new System.Drawing.Point(374, 314);
            this.BnExit.Name = "BnExit";
            this.BnExit.Size = new System.Drawing.Size(100, 30);
            this.BnExit.TabIndex = 5;
            this.BnExit.Text = "Exit";
            this.BnExit.UseVisualStyleBackColor = true;
            this.BnExit.Click += new System.EventHandler(this.Exit);
            // 
            // Checker
            // 
            this.Checker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CheckForUpdates);
            // 
            // WnMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 353);
            this.Controls.Add(this.BnExit);
            this.Controls.Add(this.LaStatus);
            this.Controls.Add(this.BnInject);
            this.Controls.Add(this.GbDLLs);
            this.Controls.Add(this.LaTitle);
            this.Controls.Add(this.GbProcesses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WnMain";
            this.Text = "WxInjector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Release);
            this.Load += new System.EventHandler(this.Start);
            this.GbProcesses.ResumeLayout(false);
            this.GbProcesses.PerformLayout();
            this.GbDLLs.ResumeLayout(false);
            this.GbDLLs.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox GbProcesses;
        private System.Windows.Forms.ListBox LbProcesses;
        private System.Windows.Forms.Label LaTitle;
        private System.Windows.Forms.GroupBox GbDLLs;
        private System.Windows.Forms.Button BnInject;
        private System.Windows.Forms.Button BnAdd;
        private System.Windows.Forms.ListBox LbDLLs;
        private System.Windows.Forms.Button BnRemove;
        private System.Windows.Forms.Button LbClear;
        private System.Windows.Forms.Label LaStatus;
        private System.Windows.Forms.Button BnExit;
        private System.Windows.Forms.Button BnRefresh;
        private System.Windows.Forms.TextBox TbProcess;
        private System.Windows.Forms.TextBox TbDLL;
        private System.ComponentModel.BackgroundWorker Checker;
    }

}