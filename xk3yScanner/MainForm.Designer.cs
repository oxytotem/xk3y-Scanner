namespace xk3yScanner
{
    partial class MainForm
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
            this.picBanner = new System.Windows.Forms.PictureBox();
            this.picCover = new System.Windows.Forms.PictureBox();
            this.richDescription = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.isosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuPrefer3DCovers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.fastListView = new BrightIdeasSoftware.ObjectListView();
            this.tableButtons = new System.Windows.Forms.TableLayoutPanel();
            this.groupBut3 = new System.Windows.Forms.GroupBox();
            this.butXml = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butGamesInactive = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.butGameActives = new System.Windows.Forms.Button();
            this.groupBut1 = new System.Windows.Forms.GroupBox();
            this.tableBut1 = new System.Windows.Forms.TableLayoutPanel();
            this.butProcessSelected = new System.Windows.Forms.Button();
            this.butProcessUnprocessed = new System.Windows.Forms.Button();
            this.butProcessAll = new System.Windows.Forms.Button();
            this.groupBut2 = new System.Windows.Forms.GroupBox();
            this.tabBut2 = new System.Windows.Forms.TableLayoutPanel();
            this.butAbgxSelected = new System.Windows.Forms.Button();
            this.butAbgxUnprocessed = new System.Windows.Forms.Button();
            this.butAbgxAll = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.progressMain = new xk3yScanner.Control.TextProgressBar();
            this.progressSub = new xk3yScanner.Control.TextProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listYoutube = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.butClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.tableMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastListView)).BeginInit();
            this.tableButtons.SuspendLayout();
            this.groupBut3.SuspendLayout();
            this.groupBut1.SuspendLayout();
            this.tableBut1.SuspendLayout();
            this.groupBut2.SuspendLayout();
            this.tabBut2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBanner
            // 
            this.picBanner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picBanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBanner.Location = new System.Drawing.Point(5, 305);
            this.picBanner.Margin = new System.Windows.Forms.Padding(0, 7, 5, 5);
            this.picBanner.Name = "picBanner";
            this.picBanner.Size = new System.Drawing.Size(145, 35);
            this.picBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBanner.TabIndex = 2;
            this.picBanner.TabStop = false;
            this.picBanner.DoubleClick += new System.EventHandler(this.picBanner_DoubleClick);
            // 
            // picCover
            // 
            this.picCover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCover.Location = new System.Drawing.Point(5, 96);
            this.picCover.Margin = new System.Windows.Forms.Padding(0, 5, 5, 7);
            this.picCover.Name = "picCover";
            this.picCover.Size = new System.Drawing.Size(145, 195);
            this.picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCover.TabIndex = 3;
            this.picCover.TabStop = false;
            this.picCover.DoubleClick += new System.EventHandler(this.picCover_DoubleClick);
            // 
            // richDescription
            // 
            this.tableMain.SetColumnSpan(this.richDescription, 3);
            this.richDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richDescription.Location = new System.Drawing.Point(5, 350);
            this.richDescription.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.richDescription.Name = "richDescription";
            this.richDescription.ReadOnly = true;
            this.richDescription.Size = new System.Drawing.Size(994, 18);
            this.richDescription.TabIndex = 4;
            this.richDescription.Text = "";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(160, 96);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.tableMain.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(324, 244);
            this.panel1.TabIndex = 7;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.isosToolStripMenuItem,
            this.toolStripInfo});
            this.menuStrip.Location = new System.Drawing.Point(5, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1014, 24);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "Select \'games\' folder...";
            // 
            // isosToolStripMenuItem
            // 
            this.isosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSelect,
            this.toolStripSeparator2,
            this.menuExport,
            this.toolStripSeparator1,
            this.menuSettings,
            this.toolStripMenuItem1,
            this.menuPrefer3DCovers});
            this.isosToolStripMenuItem.Name = "isosToolStripMenuItem";
            this.isosToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.isosToolStripMenuItem.Text = "Menu";
            // 
            // menuSelect
            // 
            this.menuSelect.Name = "menuSelect";
            this.menuSelect.Size = new System.Drawing.Size(254, 22);
            this.menuSelect.Text = "Scan games";
            this.menuSelect.Click += new System.EventHandler(this.menuSelect_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(251, 6);
            // 
            // menuExport
            // 
            this.menuExport.Name = "menuExport";
            this.menuExport.Size = new System.Drawing.Size(254, 22);
            this.menuExport.Text = "Export CSV...";
            this.menuExport.Click += new System.EventHandler(this.menuExport_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(251, 6);
            // 
            // menuSettings
            // 
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(254, 22);
            this.menuSettings.Text = "Settings...";
            this.menuSettings.Click += new System.EventHandler(this.menuSettings_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(251, 6);
            // 
            // menuPrefer3DCovers
            // 
            this.menuPrefer3DCovers.Checked = global::xk3yScanner.Properties.Settings.Default.menuPrefer3DCovers;
            this.menuPrefer3DCovers.CheckOnClick = true;
            this.menuPrefer3DCovers.Name = "menuPrefer3DCovers";
            this.menuPrefer3DCovers.Size = new System.Drawing.Size(254, 22);
            this.menuPrefer3DCovers.Text = "Prefer spiffycovers.com 3D Covers";
            // 
            // toolStripInfo
            // 
            this.toolStripInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripInfo.Enabled = false;
            this.toolStripInfo.Name = "toolStripInfo";
            this.toolStripInfo.Size = new System.Drawing.Size(12, 20);
            this.toolStripInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 3;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 334F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.fastListView, 0, 0);
            this.tableMain.Controls.Add(this.tableButtons, 0, 4);
            this.tableMain.Controls.Add(this.picCover, 0, 1);
            this.tableMain.Controls.Add(this.panel1, 1, 1);
            this.tableMain.Controls.Add(this.picBanner, 0, 2);
            this.tableMain.Controls.Add(this.richDescription, 0, 3);
            this.tableMain.Controls.Add(this.butCancel, 0, 5);
            this.tableMain.Controls.Add(this.tableLayoutPanel2, 0, 6);
            this.tableMain.Controls.Add(this.panel4, 2, 1);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(5, 18);
            this.tableMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableMain.Name = "tableMain";
            this.tableMain.Padding = new System.Windows.Forms.Padding(5);
            this.tableMain.RowCount = 7;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 207F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableMain.Size = new System.Drawing.Size(1004, 529);
            this.tableMain.TabIndex = 17;
            // 
            // fastListView
            // 
            this.tableMain.SetColumnSpan(this.fastListView, 3);
            this.fastListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastListView.FullRowSelect = true;
            this.fastListView.GridLines = true;
            this.fastListView.HideSelection = false;
            this.fastListView.Location = new System.Drawing.Point(8, 8);
            this.fastListView.Name = "fastListView";
            this.fastListView.OwnerDraw = true;
            this.fastListView.ShowGroups = false;
            this.fastListView.Size = new System.Drawing.Size(988, 80);
            this.fastListView.TabIndex = 20;
            this.fastListView.UseAlternatingBackColors = true;
            this.fastListView.UseCellFormatEvents = true;
            this.fastListView.UseCompatibleStateImageBehavior = false;
            this.fastListView.View = System.Windows.Forms.View.Details;
            this.fastListView.VirtualMode = true;
            this.fastListView.SelectedIndexChanged += new System.EventHandler(this.listView_GameList_SelectedIndexChanged);
            this.fastListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_GameList_MouseDoubleClick);
            // 
            // tableButtons
            // 
            this.tableButtons.ColumnCount = 2;
            this.tableMain.SetColumnSpan(this.tableButtons, 3);
            this.tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableButtons.Controls.Add(this.groupBut3, 1, 0);
            this.tableButtons.Controls.Add(this.groupBut1, 0, 0);
            this.tableButtons.Controls.Add(this.groupBut2, 0, 1);
            this.tableButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableButtons.Location = new System.Drawing.Point(8, 376);
            this.tableButtons.Name = "tableButtons";
            this.tableButtons.RowCount = 2;
            this.tableButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableButtons.Size = new System.Drawing.Size(988, 114);
            this.tableButtons.TabIndex = 19;
            // 
            // groupBut3
            // 
            this.groupBut3.Controls.Add(this.butXml);
            this.groupBut3.Controls.Add(this.panel2);
            this.groupBut3.Controls.Add(this.butGamesInactive);
            this.groupBut3.Controls.Add(this.panel3);
            this.groupBut3.Controls.Add(this.butGameActives);
            this.groupBut3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBut3.Location = new System.Drawing.Point(744, 3);
            this.groupBut3.Name = "groupBut3";
            this.groupBut3.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.tableButtons.SetRowSpan(this.groupBut3, 2);
            this.groupBut3.Size = new System.Drawing.Size(241, 110);
            this.groupBut3.TabIndex = 19;
            this.groupBut3.TabStop = false;
            this.groupBut3.Text = "Other Operations";
            // 
            // butXml
            // 
            this.butXml.Dock = System.Windows.Forms.DockStyle.Top;
            this.butXml.Location = new System.Drawing.Point(8, 74);
            this.butXml.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.butXml.Name = "butXml";
            this.butXml.Size = new System.Drawing.Size(225, 28);
            this.butXml.TabIndex = 19;
            this.butXml.Text = "Change Visibility Selected...";
            this.butXml.UseVisualStyleBackColor = true;
            this.butXml.Click += new System.EventHandler(this.butXml_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(8, 73);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(225, 1);
            this.panel2.TabIndex = 22;
            // 
            // butGamesInactive
            // 
            this.butGamesInactive.Dock = System.Windows.Forms.DockStyle.Top;
            this.butGamesInactive.Location = new System.Drawing.Point(8, 45);
            this.butGamesInactive.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.butGamesInactive.Name = "butGamesInactive";
            this.butGamesInactive.Size = new System.Drawing.Size(225, 28);
            this.butGamesInactive.TabIndex = 21;
            this.butGamesInactive.Text = "Make Selected Games Inactive";
            this.butGamesInactive.UseVisualStyleBackColor = true;
            this.butGamesInactive.Click += new System.EventHandler(this.butGamesInactive_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(8, 44);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(225, 1);
            this.panel3.TabIndex = 23;
            // 
            // butGameActives
            // 
            this.butGameActives.Dock = System.Windows.Forms.DockStyle.Top;
            this.butGameActives.Location = new System.Drawing.Point(8, 16);
            this.butGameActives.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.butGameActives.Name = "butGameActives";
            this.butGameActives.Size = new System.Drawing.Size(225, 28);
            this.butGameActives.TabIndex = 20;
            this.butGameActives.Text = "Make Selected Games Active";
            this.butGameActives.UseVisualStyleBackColor = true;
            this.butGameActives.Click += new System.EventHandler(this.butGamesActive_Click);
            // 
            // groupBut1
            // 
            this.groupBut1.Controls.Add(this.tableBut1);
            this.groupBut1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBut1.Location = new System.Drawing.Point(3, 3);
            this.groupBut1.Name = "groupBut1";
            this.groupBut1.Size = new System.Drawing.Size(735, 52);
            this.groupBut1.TabIndex = 17;
            this.groupBut1.TabStop = false;
            this.groupBut1.Text = "Web Scan Operations";
            // 
            // tableBut1
            // 
            this.tableBut1.ColumnCount = 3;
            this.tableBut1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableBut1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableBut1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableBut1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableBut1.Controls.Add(this.butProcessSelected, 0, 0);
            this.tableBut1.Controls.Add(this.butProcessUnprocessed, 1, 0);
            this.tableBut1.Controls.Add(this.butProcessAll, 2, 0);
            this.tableBut1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBut1.Location = new System.Drawing.Point(3, 16);
            this.tableBut1.Margin = new System.Windows.Forms.Padding(0);
            this.tableBut1.Name = "tableBut1";
            this.tableBut1.RowCount = 1;
            this.tableBut1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBut1.Size = new System.Drawing.Size(729, 33);
            this.tableBut1.TabIndex = 17;
            // 
            // butProcessSelected
            // 
            this.butProcessSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butProcessSelected.Location = new System.Drawing.Point(5, 0);
            this.butProcessSelected.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.butProcessSelected.Name = "butProcessSelected";
            this.butProcessSelected.Size = new System.Drawing.Size(233, 28);
            this.butProcessSelected.TabIndex = 8;
            this.butProcessSelected.Text = "Web Scan Selected";
            this.butProcessSelected.UseVisualStyleBackColor = true;
            this.butProcessSelected.Click += new System.EventHandler(this.butProcessSelected_Click);
            // 
            // butProcessUnprocessed
            // 
            this.butProcessUnprocessed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butProcessUnprocessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butProcessUnprocessed.Location = new System.Drawing.Point(248, 0);
            this.butProcessUnprocessed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.butProcessUnprocessed.Name = "butProcessUnprocessed";
            this.butProcessUnprocessed.Size = new System.Drawing.Size(233, 28);
            this.butProcessUnprocessed.TabIndex = 9;
            this.butProcessUnprocessed.Text = "Web Scan Unprocessed";
            this.butProcessUnprocessed.UseVisualStyleBackColor = true;
            this.butProcessUnprocessed.Click += new System.EventHandler(this.butProcessUnprocessed_Click);
            // 
            // butProcessAll
            // 
            this.butProcessAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butProcessAll.Location = new System.Drawing.Point(491, 0);
            this.butProcessAll.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.butProcessAll.Name = "butProcessAll";
            this.butProcessAll.Size = new System.Drawing.Size(233, 28);
            this.butProcessAll.TabIndex = 10;
            this.butProcessAll.Text = "Web Scan All";
            this.butProcessAll.UseVisualStyleBackColor = true;
            this.butProcessAll.Click += new System.EventHandler(this.butProcessAll_Click);
            // 
            // groupBut2
            // 
            this.groupBut2.Controls.Add(this.tabBut2);
            this.groupBut2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBut2.Location = new System.Drawing.Point(3, 61);
            this.groupBut2.Name = "groupBut2";
            this.groupBut2.Size = new System.Drawing.Size(735, 52);
            this.groupBut2.TabIndex = 18;
            this.groupBut2.TabStop = false;
            this.groupBut2.Text = "Abgx Operations";
            // 
            // tabBut2
            // 
            this.tabBut2.ColumnCount = 3;
            this.tabBut2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabBut2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabBut2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabBut2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabBut2.Controls.Add(this.butAbgxSelected, 0, 0);
            this.tabBut2.Controls.Add(this.butAbgxUnprocessed, 1, 0);
            this.tabBut2.Controls.Add(this.butAbgxAll, 2, 0);
            this.tabBut2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabBut2.Location = new System.Drawing.Point(3, 16);
            this.tabBut2.Margin = new System.Windows.Forms.Padding(0);
            this.tabBut2.Name = "tabBut2";
            this.tabBut2.RowCount = 1;
            this.tabBut2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tabBut2.Size = new System.Drawing.Size(729, 33);
            this.tabBut2.TabIndex = 18;
            // 
            // butAbgxSelected
            // 
            this.butAbgxSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butAbgxSelected.Location = new System.Drawing.Point(5, 0);
            this.butAbgxSelected.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.butAbgxSelected.Name = "butAbgxSelected";
            this.butAbgxSelected.Size = new System.Drawing.Size(233, 28);
            this.butAbgxSelected.TabIndex = 8;
            this.butAbgxSelected.Text = "Verify Selected";
            this.butAbgxSelected.UseVisualStyleBackColor = true;
            this.butAbgxSelected.Click += new System.EventHandler(this.butAbgxSelected_Click);
            // 
            // butAbgxUnprocessed
            // 
            this.butAbgxUnprocessed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butAbgxUnprocessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butAbgxUnprocessed.Location = new System.Drawing.Point(248, 0);
            this.butAbgxUnprocessed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.butAbgxUnprocessed.Name = "butAbgxUnprocessed";
            this.butAbgxUnprocessed.Size = new System.Drawing.Size(233, 28);
            this.butAbgxUnprocessed.TabIndex = 9;
            this.butAbgxUnprocessed.Text = "Verify Unprocessed";
            this.butAbgxUnprocessed.UseVisualStyleBackColor = true;
            this.butAbgxUnprocessed.Click += new System.EventHandler(this.butAbgxUnprocessed_Click);
            // 
            // butAbgxAll
            // 
            this.butAbgxAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butAbgxAll.Location = new System.Drawing.Point(491, 0);
            this.butAbgxAll.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.butAbgxAll.Name = "butAbgxAll";
            this.butAbgxAll.Size = new System.Drawing.Size(233, 28);
            this.butAbgxAll.TabIndex = 10;
            this.butAbgxAll.Text = "Verify All";
            this.butAbgxAll.UseVisualStyleBackColor = true;
            this.butAbgxAll.Click += new System.EventHandler(this.butAbgxAll_Click);
            // 
            // butCancel
            // 
            this.tableMain.SetColumnSpan(this.butCancel, 3);
            this.butCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.butCancel.Enabled = false;
            this.butCancel.Location = new System.Drawing.Point(5, 498);
            this.butCancel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(994, 1);
            this.butCancel.TabIndex = 17;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableMain.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.progressMain, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.progressSub, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 493);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(994, 31);
            this.tableLayoutPanel2.TabIndex = 16;
            // 
            // progressMain
            // 
            this.progressMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressMain.Location = new System.Drawing.Point(3, 3);
            this.progressMain.Name = "progressMain";
            this.progressMain.ProgressAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.progressMain.ProgressColor = System.Drawing.SystemColors.WindowText;
            this.progressMain.ProgressFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.progressMain.ProgressText = "";
            this.progressMain.Size = new System.Drawing.Size(491, 25);
            this.progressMain.TabIndex = 1;
            // 
            // progressSub
            // 
            this.progressSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressSub.Location = new System.Drawing.Point(500, 3);
            this.progressSub.Name = "progressSub";
            this.progressSub.ProgressAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.progressSub.ProgressColor = System.Drawing.SystemColors.WindowText;
            this.progressSub.ProgressFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.progressSub.ProgressText = "";
            this.progressSub.Size = new System.Drawing.Size(491, 25);
            this.progressSub.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.listYoutube);
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(492, 94);
            this.panel4.Name = "panel4";
            this.tableMain.SetRowSpan(this.panel4, 2);
            this.panel4.Size = new System.Drawing.Size(504, 248);
            this.panel4.TabIndex = 21;
            // 
            // listYoutube
            // 
            this.listYoutube.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader11});
            this.listYoutube.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listYoutube.FullRowSelect = true;
            this.listYoutube.GridLines = true;
            this.listYoutube.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listYoutube.HideSelection = false;
            this.listYoutube.Location = new System.Drawing.Point(0, 30);
            this.listYoutube.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.listYoutube.MultiSelect = false;
            this.listYoutube.Name = "listYoutube";
            this.listYoutube.Size = new System.Drawing.Size(504, 218);
            this.listYoutube.TabIndex = 7;
            this.listYoutube.UseCompatibleStateImageBehavior = false;
            this.listYoutube.View = System.Windows.Forms.View.Details;
            this.listYoutube.SelectedIndexChanged += new System.EventHandler(this.listYoutube_SelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Title";
            this.columnHeader6.Width = 280;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Author";
            this.columnHeader11.Width = 90;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Controls.Add(this.butClear, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSearch, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(504, 30);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // butClear
            // 
            this.butClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.butClear.Location = new System.Drawing.Point(438, 4);
            this.butClear.Margin = new System.Windows.Forms.Padding(4);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(62, 22);
            this.butClear.TabIndex = 11;
            this.butClear.Text = "Clear";
            this.butClear.UseVisualStyleBackColor = true;
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(73, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(358, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableMain);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 24);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(1014, 552);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 581);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::xk3yScanner.Properties.Settings.Default, "formLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Location = global::xk3yScanner.Properties.Settings.Default.formLocation;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.Text = "xk3y Game Scanner & ISO Verifier v2.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tableMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastListView)).EndInit();
            this.tableButtons.ResumeLayout(false);
            this.groupBut3.ResumeLayout(false);
            this.groupBut1.ResumeLayout(false);
            this.tableBut1.ResumeLayout(false);
            this.groupBut2.ResumeLayout(false);
            this.tabBut2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBanner;
        private System.Windows.Forms.PictureBox picCover;
        private System.Windows.Forms.RichTextBox richDescription;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem isosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuPrefer3DCovers;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.TableLayoutPanel tableButtons;
        private System.Windows.Forms.GroupBox groupBut3;
        private System.Windows.Forms.Button butXml;
        private System.Windows.Forms.GroupBox groupBut1;
        private System.Windows.Forms.TableLayoutPanel tableBut1;
        private System.Windows.Forms.Button butProcessSelected;
        private System.Windows.Forms.Button butProcessUnprocessed;
        private System.Windows.Forms.Button butProcessAll;
        private System.Windows.Forms.GroupBox groupBut2;
        private System.Windows.Forms.TableLayoutPanel tabBut2;
        private System.Windows.Forms.Button butAbgxSelected;
        private System.Windows.Forms.Button butAbgxUnprocessed;
        private System.Windows.Forms.Button butAbgxAll;
        public System.Windows.Forms.Button butGamesInactive;
        public System.Windows.Forms.Button butGameActives;
        private System.Windows.Forms.ToolStripMenuItem toolStripInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Control.TextProgressBar progressMain;
        private Control.TextProgressBar progressSub;
        private BrightIdeasSoftware.ObjectListView fastListView;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListView listYoutube;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button butClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuExport;

    }
}