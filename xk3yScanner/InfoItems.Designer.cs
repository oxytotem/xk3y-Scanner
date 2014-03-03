namespace xk3yScanner
{
    partial class InfoItems
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butOk = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butCancel = new System.Windows.Forms.Button();
            this.chkRegionCode = new System.Windows.Forms.CheckBox();
            this.chkAbgxInfo = new System.Windows.Forms.CheckBox();
            this.chkGenre = new System.Windows.Forms.CheckBox();
            this.chkGameDate = new System.Windows.Forms.CheckBox();
            this.chkTrailer = new System.Windows.Forms.CheckBox();
            this.chkGameType = new System.Windows.Forms.CheckBox();
            this.chkPublisher = new System.Windows.Forms.CheckBox();
            this.chkDiscs = new System.Windows.Forms.CheckBox();
            this.chkDeveloper = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.chkRegionCode, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkAbgxInfo, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkGenre, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkGameDate, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkTrailer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkGameType, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkPublisher, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkDiscs, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkDeveloper, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20, 20, 0, 0);
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(434, 217);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butOk);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.butCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(5, 184);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(434, 38);
            this.panel1.TabIndex = 8;
            // 
            // butOk
            // 
            this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.butOk.Location = new System.Drawing.Point(145, 5);
            this.butOk.Margin = new System.Windows.Forms.Padding(10, 5, 0, 5);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(137, 28);
            this.butOk.TabIndex = 11;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(282, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 28);
            this.panel2.TabIndex = 13;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.butCancel.Location = new System.Drawing.Point(292, 5);
            this.butCancel.Margin = new System.Windows.Forms.Padding(5, 5, 10, 5);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(137, 28);
            this.butCancel.TabIndex = 12;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // chkRegionCode
            // 
            this.chkRegionCode.AutoSize = true;
            this.chkRegionCode.Checked = global::xk3yScanner.Properties.Settings.Default.chkRegionCode;
            this.chkRegionCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRegionCode.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkRegionCode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkRegionCode.Location = new System.Drawing.Point(230, 113);
            this.chkRegionCode.Name = "chkRegionCode";
            this.chkRegionCode.Size = new System.Drawing.Size(88, 17);
            this.chkRegionCode.TabIndex = 8;
            this.chkRegionCode.Text = "Region Code";
            this.chkRegionCode.UseVisualStyleBackColor = true;
            // 
            // chkAbgxInfo
            // 
            this.chkAbgxInfo.AutoSize = true;
            this.chkAbgxInfo.Checked = global::xk3yScanner.Properties.Settings.Default.chkAbgxInfo;
            this.chkAbgxInfo.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkAbgxInfo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkAbgxInfo.Location = new System.Drawing.Point(23, 143);
            this.chkAbgxInfo.Name = "chkAbgxInfo";
            this.chkAbgxInfo.Size = new System.Drawing.Size(71, 17);
            this.chkAbgxInfo.TabIndex = 7;
            this.chkAbgxInfo.Text = "Abgx Info";
            this.chkAbgxInfo.UseVisualStyleBackColor = true;
            // 
            // chkGenre
            // 
            this.chkGenre.AutoSize = true;
            this.chkGenre.Checked = global::xk3yScanner.Properties.Settings.Default.chkGenre;
            this.chkGenre.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenre.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkGenre", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkGenre.Location = new System.Drawing.Point(23, 23);
            this.chkGenre.Name = "chkGenre";
            this.chkGenre.Size = new System.Drawing.Size(55, 17);
            this.chkGenre.TabIndex = 4;
            this.chkGenre.Text = "Genre";
            this.chkGenre.UseVisualStyleBackColor = true;
            // 
            // chkGameDate
            // 
            this.chkGameDate.AutoSize = true;
            this.chkGameDate.Checked = global::xk3yScanner.Properties.Settings.Default.chkGameDate;
            this.chkGameDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGameDate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkGameDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkGameDate.Location = new System.Drawing.Point(23, 113);
            this.chkGameDate.Name = "chkGameDate";
            this.chkGameDate.Size = new System.Drawing.Size(80, 17);
            this.chkGameDate.TabIndex = 6;
            this.chkGameDate.Text = "Game Date";
            this.chkGameDate.UseVisualStyleBackColor = true;
            // 
            // chkTrailer
            // 
            this.chkTrailer.AutoSize = true;
            this.chkTrailer.Checked = global::xk3yScanner.Properties.Settings.Default.chkTrailer;
            this.chkTrailer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrailer.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkTrailer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkTrailer.Location = new System.Drawing.Point(230, 23);
            this.chkTrailer.Name = "chkTrailer";
            this.chkTrailer.Size = new System.Drawing.Size(55, 17);
            this.chkTrailer.TabIndex = 1;
            this.chkTrailer.Text = "Trailer";
            this.chkTrailer.UseVisualStyleBackColor = true;
            // 
            // chkGameType
            // 
            this.chkGameType.AutoSize = true;
            this.chkGameType.Checked = global::xk3yScanner.Properties.Settings.Default.chkGameType;
            this.chkGameType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGameType.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkGameType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkGameType.Location = new System.Drawing.Point(230, 83);
            this.chkGameType.Name = "chkGameType";
            this.chkGameType.Size = new System.Drawing.Size(81, 17);
            this.chkGameType.TabIndex = 5;
            this.chkGameType.Text = "Game Type";
            this.chkGameType.UseVisualStyleBackColor = true;
            // 
            // chkPublisher
            // 
            this.chkPublisher.AutoSize = true;
            this.chkPublisher.Checked = global::xk3yScanner.Properties.Settings.Default.chkPublisher;
            this.chkPublisher.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPublisher.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkPublisher", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkPublisher.Location = new System.Drawing.Point(23, 53);
            this.chkPublisher.Name = "chkPublisher";
            this.chkPublisher.Size = new System.Drawing.Size(69, 17);
            this.chkPublisher.TabIndex = 2;
            this.chkPublisher.Text = "Publisher";
            this.chkPublisher.UseVisualStyleBackColor = true;
            // 
            // chkDiscs
            // 
            this.chkDiscs.AutoSize = true;
            this.chkDiscs.Checked = global::xk3yScanner.Properties.Settings.Default.chkDiscs;
            this.chkDiscs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDiscs.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkDiscs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkDiscs.Location = new System.Drawing.Point(23, 83);
            this.chkDiscs.Name = "chkDiscs";
            this.chkDiscs.Size = new System.Drawing.Size(52, 17);
            this.chkDiscs.TabIndex = 0;
            this.chkDiscs.Text = "Discs";
            this.chkDiscs.UseVisualStyleBackColor = true;
            // 
            // chkDeveloper
            // 
            this.chkDeveloper.AutoSize = true;
            this.chkDeveloper.Checked = global::xk3yScanner.Properties.Settings.Default.chkDeveloper;
            this.chkDeveloper.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeveloper.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::xk3yScanner.Properties.Settings.Default, "chkDeveloper", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkDeveloper.Location = new System.Drawing.Point(230, 53);
            this.chkDeveloper.Name = "chkDeveloper";
            this.chkDeveloper.Size = new System.Drawing.Size(75, 17);
            this.chkDeveloper.TabIndex = 3;
            this.chkDeveloper.Text = "Developer";
            this.chkDeveloper.UseVisualStyleBackColor = true;
            // 
            // InfoItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 227);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoItems";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Select Items to Show in xKey interfaces";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDiscs;
        private System.Windows.Forms.CheckBox chkTrailer;
        private System.Windows.Forms.CheckBox chkPublisher;
        private System.Windows.Forms.CheckBox chkDeveloper;
        private System.Windows.Forms.CheckBox chkGenre;
        private System.Windows.Forms.CheckBox chkGameType;
        private System.Windows.Forms.CheckBox chkGameDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.CheckBox chkAbgxInfo;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkRegionCode;
    }
}