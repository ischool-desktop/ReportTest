namespace ReportTest
{
    partial class MergeNameExprotForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.col01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkGroup = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.iptAdd = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.txtDisplayName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btn01 = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iptAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // dgData
            // 
            this.dgData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgData.BackgroundColor = System.Drawing.Color.White;
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col01,
            this.col02,
            this.col03,
            this.col04});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgData.Location = new System.Drawing.Point(13, 44);
            this.dgData.Name = "dgData";
            this.dgData.RowTemplate.Height = 24;
            this.dgData.Size = new System.Drawing.Size(513, 307);
            this.dgData.TabIndex = 0;
            // 
            // col01
            // 
            this.col01.HeaderText = "顯示名稱";
            this.col01.Name = "col01";
            // 
            // col02
            // 
            this.col02.HeaderText = "合併名稱";
            this.col02.Name = "col02";
            // 
            // col03
            // 
            this.col03.HeaderText = "前置文字";
            this.col03.Name = "col03";
            // 
            // col04
            // 
            this.col04.HeaderText = "後置文字";
            this.col04.Name = "col04";
            // 
            // chkGroup
            // 
            this.chkGroup.AutoSize = true;
            this.chkGroup.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkGroup.BackgroundStyle.Class = "";
            this.chkGroup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkGroup.Location = new System.Drawing.Point(13, 13);
            this.chkGroup.Name = "chkGroup";
            this.chkGroup.Size = new System.Drawing.Size(54, 21);
            this.chkGroup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkGroup.TabIndex = 1;
            this.chkGroup.Text = "群組";
            this.chkGroup.CheckedChanged += new System.EventHandler(this.chkGroup_CheckedChanged);
            // 
            // iptAdd
            // 
            this.iptAdd.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.iptAdd.BackgroundStyle.Class = "DateTimeInputBackground";
            this.iptAdd.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.iptAdd.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.iptAdd.Enabled = false;
            this.iptAdd.Location = new System.Drawing.Point(160, 11);
            this.iptAdd.MinValue = 1;
            this.iptAdd.Name = "iptAdd";
            this.iptAdd.ShowUpDown = true;
            this.iptAdd.Size = new System.Drawing.Size(80, 25);
            this.iptAdd.TabIndex = 2;
            this.iptAdd.Value = 1;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(120, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(34, 21);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "數量";
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.AutoSize = true;
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(370, 361);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 25);
            this.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "產生選取";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(451, 361);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            // 
            // 
            // 
            this.txtDisplayName.Border.Class = "TextBoxBorder";
            this.txtDisplayName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtDisplayName.Location = new System.Drawing.Point(110, 361);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(100, 25);
            this.txtDisplayName.TabIndex = 6;
            this.txtDisplayName.WatermarkText = "輸入修改內容";
            // 
            // btn01
            // 
            this.btn01.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn01.AutoSize = true;
            this.btn01.BackColor = System.Drawing.Color.Transparent;
            this.btn01.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn01.Location = new System.Drawing.Point(13, 361);
            this.btn01.Name = "btn01";
            this.btn01.Size = new System.Drawing.Size(91, 25);
            this.btn01.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn01.TabIndex = 7;
            this.btn01.Text = "修改顯示名稱";
            this.btn01.Click += new System.EventHandler(this.btn01_Click);
            // 
            // MergeNameExprotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 393);
            this.Controls.Add(this.btn01);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.iptAdd);
            this.Controls.Add(this.chkGroup);
            this.Controls.Add(this.dgData);
            this.DoubleBuffered = true;
            this.Name = "MergeNameExprotForm";
            this.Text = "產生Word合併欄位";
            this.Load += new System.EventHandler(this.MergeNameExprotForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iptAdd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgData;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkGroup;
        private DevComponents.Editors.IntegerInput iptAdd;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn col01;
        private System.Windows.Forms.DataGridViewTextBoxColumn col02;
        private System.Windows.Forms.DataGridViewTextBoxColumn col03;
        private System.Windows.Forms.DataGridViewTextBoxColumn col04;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDisplayName;
        private DevComponents.DotNetBar.ButtonX btn01;
    }
}