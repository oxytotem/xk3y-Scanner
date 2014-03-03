using System;
using System.Collections.Generic;

using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BrightIdeasSoftware;
using xk3yScanner.Classes;
using xk3yScanner.Classes.Processors;
using xk3yScanner.Classes.Processors.Helpers;
using xk3yScanner.Objects.Waffle;

namespace xk3yScanner
{
    public partial class MainForm : Form
    {



        private GameScanner _scanner;
        private AbgxChecker _abgx;
        private MoveProcessor _move;
        private ScannerInfo _info;
        private WebScrapper _scrapper;

        public void UnhandledThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            this.HandleUnhandledException(e.Exception);
        }

        public void HandleUnhandledException(Exception e)
        {
            ExceptionForm form=new ExceptionForm(e);
            form.ShowDialog(this);
            Application.Exit();
        }


        private Visibility Visibility=new Visibility();
        
        private OLVColumn GetColumn(string name)
        {
            foreach (OLVColumn c in this.fastListView.AllColumns)
            {
                if (string.Compare(c.Text, name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return c;
            }
            return null;
        }

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.xkey;
            //lvwColumnSorter = new ListViewColumnSorter();
            //listView_GameList.ListViewItemSorter = lvwColumnSorter;
            //lvwColumnSorter.SecondaryColumn = 5;
            Generator.GenerateColumns(this.fastListView, typeof (Game));
            this.fastListView.FormatCell += fastListView_FormatCell;
            GetColumn("Active").Renderer=new MappedImageRenderer(new object[] { true,Properties.Resources.Active,false,Properties.Resources.Inactive});
            GetColumn("Abgx").Renderer=new MappedImageRenderer(new object[] { 0, Properties.Resources.None,1,Properties.Resources.Ok,2,Properties.Resources.Warning,3,Properties.Resources.Error});
            GetColumn("Populated").Renderer=new MappedImageRenderer(new object[] { true, Properties.Resources.Green,false,Properties.Resources.None});
            GetColumn("Active").Searchable = false;
            GetColumn("Abgx").Searchable = false;
            GetColumn("Populated").Searchable = false;
            this.fastListView.VirtualMode = false;
            this.fastListView.CustomSorter = delegate(OLVColumn column, SortOrder order)
                                                 {
                                                     switch (column.Text)
                                                     {
                                                         case "Title":
                                                             this.fastListView.ListViewItemSorter=new ColumnComparer(column,order,GetColumn("Disc"),SortOrder.Ascending);
                                                             break;
                                                         case "Genre":
                                                         case "Developer":
                                                         case "Publisher":
                                                             this.fastListView.ListViewItemSorter =
                                                                 new ColumnComparer(column, order, GetColumn("Title"),
                                                                                    SortOrder.Ascending);
                                                             break;
        
                                                         default:
                                                             this.fastListView.ListViewItemSorter=new ColumnComparer(column,order);
                                                             break;

                                                     }
                                                 };
            if (Properties.Settings.Default.InActiveFolders)
            {
                butGameActives.Enabled = true;
                butGamesInactive.Enabled = true;
            }
            else
            {
                butGameActives.Enabled = false;
                butGamesInactive.Enabled = false;
            }
        }

        void fastListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex == 12)
            {
                Game game = (Game) e.Model;
                uint val;
                switch (Properties.Settings.Default.DefaultRegion)
                {
                    case 0:
                        val = 0x000000FF;
                        break;
                    case 1:
                        val = 0x00FF0000;
                        break;
                    default:
                        val = 0x0000FF00;
                        break;
                }
                if (!string.IsNullOrEmpty(game.RegionCode))
                   e.SubItem.BackColor = (Convert.ToUInt32(game.RegionCode,16) & val) == 0 ? Color.Red : Color.LightGreen;
            }
        }

        void _loader_OnScanFinish()
        {
            if (this.InvokeRequired)
            {
                BaseProcessor.OnFinishHandler hand = new BaseProcessor.OnFinishHandler(_loader_OnScanFinish);
                this.BeginInvoke(hand, new object[] { });
                return;
            }
            Free();
            progressMain.ProgressText = "Scanning Done. " + fastListView.Items.Count + " games scanned";
            progressMain.Value = 0;
            progressSub.ProgressText = string.Empty;
            progressSub.Value = 0;
            
            if (fastListView.SelectedIndices.Count == 0)
            {
                fastListView.Focus();
                fastListView.SelectObject(fastListView.GetModelObject(0));
//                    fastListView.GetItem(0)VirtualDataSource.GetNthObject(0), true);
//                fastListView.SelectObject(fastListView.VirtualDataSource.GetNthObject(0), true);
            }
            fastListView.Sort(0);
            GenInfo();
            GenCache();
        }
        void AutoResize()
        {
            
        }
        void GenInfo()
        {
            List<Game> games=new List<Game>();
            foreach (object lv in fastListView.Objects)
            {
                games.Add((Game)lv);
            }
            _info.Populate(games);
            toolStripInfo.Text = _info.ToString();
        }

        void _loader_OnFinish()
        {
            if (this.InvokeRequired)
            {
                BaseProcessor.OnFinishHandler hand = new BaseProcessor.OnFinishHandler(_loader_OnFinish);
                this.BeginInvoke(hand, new object[] { });
                return;
            }
            Free();


            progressMain.ProgressText = "Done";
            progressMain.Value = 0;
            progressSub.ProgressText = string.Empty;
            progressSub.Value = 0;
            GenInfo();
            GenCache();
        }

        public void GenCache()
        {
            List<Game> active=new List<Game>();
            List<Game> inactive=new List<Game>();
            foreach (object lv in fastListView.Objects)
            {
                Game g = (Game) lv;
                if (g.Active)
                    active.Add(g);
                else
                    inactive.Add(g);
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ActiveFolder))
                WriteCache(active, Properties.Settings.Default.ActiveFolder);
            if (!string.IsNullOrEmpty(Properties.Settings.Default.InActiveFolder))
                WriteCache(inactive, Properties.Settings.Default.InActiveFolder);

        }
        public void WriteCache(List<Game> games, string path)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan span = (DateTime.Now.ToLocalTime() - epoch);          
            string filename = Path.Combine(path, ".cache");
            StringBuilder bld=new StringBuilder();
            bld.Append("Date=");
            bld.AppendLine(((long)span.TotalSeconds).ToString());
            bld.Append("Isos=");
            bld.AppendLine(games.Count.ToString());
            foreach (Game g in games)
            {
                bld.Append("Iso=");
                bld.Append(g.ID.ToLower());
                bld.Append("~~");
                bld.Append(g.Title);
                bld.Append("~~");
                bld.Append(g.Disc);
                bld.Append("~~");
                bld.Append(g.Genre);
                bld.Append("~~");
                string p = g.FullIsoPath.Substring(path.Length + 1).Replace("\\", "/");
                string dir = string.Empty;
                int a = p.LastIndexOf("/", p.Length, System.StringComparison.Ordinal);
                if (a> 0)
                {
                    dir = p.Substring(0, a);
                }
                bld.Append(dir);
                bld.Append("~~");                
                bld.AppendLine(p);
            }
            File.WriteAllText(filename,bld.ToString(),Encoding.UTF8);
        }



        void _OnGameProcessed(Game game, int count)
        {
            if (this.InvokeRequired)
            {
                BaseProcessor.OnGameProcessedHandler hand = new BaseProcessor.OnGameProcessedHandler(_OnGameProcessed);
                this.BeginInvoke(hand, new object[] { game, count });
                return;
            }
            game.SaveAll();
            fastListView.RefreshObject(game);
            if (game==SelectedGame)
            {
                PopulateControls(game);
            }

        }



        void _loader_OnGameScanned(Game game,int count)
        {
            if (this.InvokeRequired)
            {
                BaseProcessor.OnGameProcessedHandler hand = new BaseProcessor.OnGameProcessedHandler(_loader_OnGameScanned);
                this.BeginInvoke(hand, new object[] {game, count});
                return;
            }
            fastListView.AddObject(game);

        }
        private void PopulateControls(Game game)
        {
            if (game.Cover!=null)
            {
                MemoryStream ms=new MemoryStream(game.Cover);
                picCover.Image=Bitmap.FromStream(ms);
                ms.Dispose();
            }
            else
            {
                picCover.Image = null;
            }
            if (game.Banner!=null)
            {
                MemoryStream ms = new MemoryStream(game.Banner);
                picBanner.Image = Bitmap.FromStream(ms);
                ms.Dispose();
            }
            else
            {
                picBanner.Image = null;
            }
            if (string.IsNullOrEmpty(game.Summary))
                richDescription.Text = string.Empty;
            else
                richDescription.Text = game.Summary;
            PopulateYoutube(game);
        }

 
        private class ComboG
        {
            public Game Game { get; set; }
            public Trailer Trailer { get; set; }
        }
        private void PopulateYoutube(Game game)
        {
            if (string.IsNullOrEmpty(game.Trailer) && ((game.Trailers==null) || (game.Trailers.Count==0)))
            {
                if (panel1.Controls.Count > 0)
                    panel1.Controls[0].Dispose();
                panel1.Controls.Clear();
                listYoutube.Items.Clear();
            }
            else
            {
                string id = string.Empty;
                if (!string.IsNullOrEmpty(game.Trailer))
                    id = game.Trailer.Substring(game.Trailer.LastIndexOf("/") + 1);
                if ((game.Trailers!=null) && (game.Trailers.Count>0))
                {
                    int pos = 0;
                    listYoutube.Items.Clear();
                    int cnt = 0;
                    foreach(Trailer t in game.Trailers)
                    {
                        ListViewItem item=new ListViewItem(t.Title);
                        item.SubItems.Add(t.Author);
                        ComboG g=new ComboG();
                        g.Game = game;
                        g.Trailer = t;
                        item.Tag = g;
                        listYoutube.Items.Add(item);
                        if (t.Id == id)
                            pos = cnt;
                        cnt++;
                    }
                    listYoutube.Focus();
                    listYoutube.Items[pos].Selected = true;
                    listYoutube.EnsureVisible(pos);
                    listYoutube.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    fastListView.Focus();

                }
                else
                {
                    listYoutube.Items.Clear();
                    Trailer t=new Trailer();
                    t.Id = id;
                    t.Title = game.Trailer;
                    t.Author = string.Empty;
                    t.Url = game.Trailer;
                    ListViewItem item = new ListViewItem(game.Trailer, string.Empty);
                    ComboG g=new ComboG();
                    g.Game = game;
                    g.Trailer = t;
                    item.Tag = g;
                    listYoutube.Items.Add(item);
                    listYoutube.Focus();
                    listYoutube.Items[0].Selected = true;
                    listYoutube.EnsureVisible(0);
                    listYoutube.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    fastListView.Focus();
                }
  

            }
        }
        private void PopulateWebBrowser(string url)
        {
            if (panel1.Controls.Count > 0)
                panel1.Controls[0].Dispose();
            panel1.Controls.Clear();


           string id = url.Substring(url.LastIndexOf("/") + 1);
           WebBrowser web=new WebBrowser();
           web.Dock = DockStyle.Fill;
            web.AllowWebBrowserDrop = false;
            web.AllowNavigation = true;
            web.ScriptErrorsSuppressed = true;
            web.ScrollBarsEnabled = false;
            var sb = new StringBuilder();

            const string YOUTUBE_URL = @"http://www.youtube.com/v/";

            sb.Append("<html>");
            sb.Append("    <head>");
            sb.Append("        <meta name=\"viewport\" content=\"width=device-width; height=device-height;\">");
            sb.Append("    </head>");
            sb.Append("    <body marginheight=\"0\" marginwidth=\"0\" leftmargin=\"0\" topmargin=\"0\" style=\"overflow-y: hidden\">");
            sb.Append("        <object width=\"100%\" height=\"100%\">");
            sb.Append("            <param name=\"movie\" value=\"" + YOUTUBE_URL + id + "?version=3&amp;rel=0\" />");
            sb.Append("            <param name=\"allowFullScreen\" value=\"true\" />");
            sb.Append("            <param name=\"allowscriptaccess\" value=\"always\" />");
            sb.Append("            <embed src=\"" + YOUTUBE_URL + id + "?version=3&amp;rel=0\" type=\"application/x-shockwave-flash\"");
            sb.Append("                   width=\"100%\" height=\"100%\" allowscriptaccess=\"always\" allowfullscreen=\"true\" />");
            sb.Append("        </object>");
            sb.Append("    </body>");
            sb.Append("</html>");
            web.DocumentText = sb.ToString();
            panel1.Controls.Add(web);

        }



        public void Scan(bool noxml)
        {
            fastListView.ClearObjects();
            Busy();
            _scanner.Start(noxml);
        }
        private void menuSelect_Click(object sender, EventArgs e)
        {

            Scan(false);
        }


        public void Busy()
        {
            float siz = tableMain.RowStyles[4].Height;
            tableMain.RowStyles[4].Height = 0;
            tableMain.RowStyles[5].Height = siz;
            butProcessAll.Enabled = false;
            butProcessSelected.Enabled = false;
            butProcessUnprocessed.Enabled = false;
            menuStrip.Enabled = false;
            butCancel.Enabled = true;
        }
        public void Free()
        {
            float siz = tableMain.RowStyles[5].Height;
            tableMain.RowStyles[5].Height = 0;
            tableMain.RowStyles[4].Height = siz;
            butCancel.Enabled = false;
            butProcessAll.Enabled = true;
            butProcessSelected.Enabled = true;
            butProcessUnprocessed.Enabled = true;
            menuStrip.Enabled = true;
            
        }


        public Game SelectedGame = null;

        private void listView_GameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fastListView.SelectedIndices.Count>0)
            {
                ListViewItem final = null;
                ListView.SelectedIndexCollection col = fastListView.SelectedIndices;

                foreach(int item in col)
                {
                     if (fastListView.Items[item].Focused)
                    {
                        final = fastListView.Items[item];
                        break;
                    }
                }
                if (final==null)
                    final = fastListView.Items[col[0]];
                Game game=(Game)fastListView.GetModelObject(final.Index);
                if ((SelectedGame != null) && (game.FullIsoPath == SelectedGame.FullIsoPath))
                    return;
                if (game.Locked)
                    return;
                if (panel1.Controls.Count > 0)
                    panel1.Controls[0].Dispose();
                panel1.Controls.Clear();
                SelectedGame = game;
                PopulateControls(game);
                
            }
        }

        private void butProcessSelected_Click(object sender, EventArgs e)
        {
            
            List<Game> games=new List<Game>();
            foreach (object item in fastListView.SelectedObjects)
                games.Add((Game)item);
            ProcessGames(games);

        }
        private void butProcessAll_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            foreach (object item in fastListView.Objects)
                games.Add((Game)item);
            ProcessGames(games);
        }
        private void butProcessUnprocessed_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            foreach (object item in fastListView.Objects)
            {
                Game game = (Game) item;
                if (!game.WebPopulated)
                    games.Add(game);
            }
            ProcessGames(games);

        }
        private void ProcessGames(List<Game> games)
        {
            Busy();
            _scrapper.Start(games);
        }

        private void VerifyGames(List<Game> games)
        {
            Busy();
            _abgx.Start(games);
        }
        private void MoveGames(List<Game> games)
        {
            Busy();
            _move.Start(games);
        }

        private void butCancel_Click(object sender, EventArgs e)
       {
            
            progressMain.ProgressText = "Canceling Threads...";
            progressSub.ProgressText = string.Empty;
            Thread rh=new Thread(StopThread);
            rh.Start();
       }

        public void StopThread(object obj)
        {   if (!_scanner.Finished)
                _scanner.Stop();
            if (!_scrapper.Finished)
                _scrapper.Stop();
            if (!_abgx.Finished)
                _abgx.Stop();
            if (!_move.Finished)
                _move.Stop();
            
        }
        private void listYoutube_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listYoutube.SelectedItems.Count>0)
            {
                ListViewItem lv = listYoutube.SelectedItems[0];
                ComboG g = (ComboG) lv.Tag;
                if ((g.Game.Trailer==null) || (g.Game.Trailer.ToUpper()!=g.Trailer.Url.ToUpper()) || (panel1.Controls.Count==0))
                {
                    g.Game.Trailer = g.Trailer.Url;
                    PopulateWebBrowser(g.Game.Trailer);
                    g.Game.Save();

                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ClientSize = Properties.Settings.Default.formSize;
            Visibility.Update();
            progressMain.ProgressText = "Updating Name Lookups from abgx.net";
            _scanner=new GameScanner();
            _scanner.OnGameProcessed += _loader_OnGameScanned;
            _scanner.OnStatusUpdate += _loader_OnStatusUpdate;
            _scanner.OnFinish += _loader_OnScanFinish;
            _scrapper=new WebScrapper();
            _scrapper.OnGameProcessed += _OnGameProcessed;
            _scrapper.OnStatusUpdate += _loader_OnStatusUpdate;
            _scrapper.OnFinish += _loader_OnFinish;
            _abgx = new AbgxChecker();
            _abgx.OnFinish += _loader_OnFinish;
            _abgx.OnGameProcessed += _OnGameProcessed;
            _abgx.OnStatusUpdate += _loader_OnStatusUpdate;
            _move = new MoveProcessor();
            _move.OnFinish += _loader_OnFinish;
            _move.OnGameProcessed += _OnGameProcessed;
            _move.OnStatusUpdate += _loader_OnStatusUpdate;
            _info=new ScannerInfo();
            progressMain.ProgressText = "";
            progressSub.ProgressText = "";
            if (Program.Wizard)
                menuSettings_Click(null,null);
    }

        public delegate void LoaderOnUpdate(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount, int rightmaxcount);

        void _loader_OnStatusUpdate(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount, int rightmaxcount)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LoaderOnUpdate(_loader_OnStatusUpdate),
                            new object[] {lefttext, righttext, leftcount, leftmaxcount, rightcount, rightmaxcount});
                return;
            }
            progressMain.ProgressText = lefttext;
            progressSub.ProgressText = righttext;
            if (leftcount != -1)
            {
                progressMain.Maximum = leftmaxcount;
                progressMain.Value = leftcount;
            }
            if (rightcount != -1)
            {
                progressSub.Maximum = rightmaxcount;
                progressSub.Value = rightcount;
                
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.formSize = this.ClientSize;
            Properties.Settings.Default.Save();
        }
        /*
        private void listView_GameList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listView_GameList.Sort();
        }
        */
        private void listView_GameList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item=fastListView.GetItemAt(e.X, e.Y);
            if (item!=null)
            {
                Game g = (Game)fastListView.GetModelObject(item.Index);
                Edit ed=new Edit(g);
                DialogResult res = ed.ShowDialog();
                if (res==DialogResult.OK)
                {
                    fastListView.RefreshObject(g);
                    if (g==SelectedGame)
                        PopulateControls(g);
                    g.SaveAll();
                    GenCache();
                }
            }
        }

        private void menuSelect2_Click(object sender, EventArgs e)
        {


            Scan(true);
        }


        private void butXml_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            if (fastListView.SelectedIndices.Count >0)
            {
                InfoItems inf=new InfoItems();
                if (inf.ShowDialog() == DialogResult.OK)
                {
                    Visibility.Update();
                    foreach (object item in fastListView.SelectedObjects)
                    {
                        Game g = (Game) item;
                        g.Visibility = Visibility;
                        g.Save();
                    }
                }
            }
        }

        private void butAbgxSelected_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            foreach (object item in fastListView.SelectedObjects)
                games.Add((Game)item);
            VerifyGames(games);
        }


        private void butAbgxUnprocessed_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            foreach (object item in fastListView.Objects)
            {
                Game game = (Game)item;
                if (game.Abgx==null)
                    games.Add(game);
            }
            VerifyGames(games);
        }

        private void butAbgxAll_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            foreach (object item in fastListView.Objects)
                games.Add((Game)item);
            VerifyGames(games);
        }

        private void butGamesActive_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            foreach (object item in fastListView.SelectedObjects)
            {
                if (!((Game) item).Active)
                    games.Add((Game) item);
            }
            if (games.Count > 0)
            {
                if (CheckGames(games, false))
                    MoveGames(games);
                else
                {
                    MessageBox.Show("Not enough free space in Active Folder", "Not Enough Space", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            Settings set = new Settings();
            set.ShowDialog();
        }

        private void butGamesInactive_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            foreach (object item in fastListView.SelectedObjects)
            {
                if (((Game)item).Active)
                    games.Add((Game)item);
            }
            if (games.Count > 0)
            {
                if (CheckGames(games, true))
                    MoveGames(games);
                else
                {
                    MessageBox.Show("Not enough free space in Inactive Folder", "Not Enough Space", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        bool CheckGames(List<Game> games, bool inactive)
        {
            
            _info.Populate(games);
            if (_info.SameDisk)
                return true;
            long rsize = _info.TotalSize + (_info.TotalSize/200);
            if (inactive)
            {
                if (rsize > _info.InactiveFolderFreeSpace)
                    return false;
            }
            else
            {
                if (rsize > _info.ActiveFolderFreeSpace)
                    return false;
            }
            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == string.Empty)
            {
                this.fastListView.UseFiltering = false;
            }
            else
            {
                this.fastListView.ModelFilter=new TextMatchFilter(this.fastListView,txtSearch.Text,StringComparison.CurrentCultureIgnoreCase);
                this.fastListView.UseFiltering = true;
            }
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
        }

        private void picCover_DoubleClick(object sender, EventArgs e)
        {
            if (picCover.Image != null)
            {
                CoverForm fr=new CoverForm(picCover.Image);
                fr.Show(this);
            }
        }

        private void picBanner_DoubleClick(object sender, EventArgs e)
        {
            if (picBanner.Image != null)
            {
                BannerForm fr = new BannerForm(picBanner.Image);
                fr.Show(this);
            }
        }

        private void menuExport_Click(object sender, EventArgs e)
        {
            List<Game> games = new List<Game>();
            if (fastListView.Objects != null)
            {
                foreach (object lv in fastListView.Objects)
                {
                    if (lv!=null)
                        games.Add((Game) lv);
                }
            }
            if (games.Count > 0)
            {
                CSVExport csv = new CSVExport(games);
                csv.ShowDialog(this);
            }
        }

    }
}
