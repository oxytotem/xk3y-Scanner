using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using xk3yScanner.Classes.Processors.Helpers;

namespace xk3yScanner.Classes.Processors
{
    public class GameScanner : BaseProcessor
    {
        public GameScanner() : base(1)
        {
        }

        public void Start(bool noxml)
        {
            Abort = false;
            Finished = false;
            Thread th = new Thread(ScanGamesThread);
            th.Start(noxml);
        }
        public class Iso
        {
            public string basepath;
            public string iso;
            public string path;
            public bool active;
        }

        private List<Iso> isos;

        public void ScanGamesThread(object obj)
        {
            isos = new List<Iso>();
            bool noxml = (bool) obj;
            Cnt = 0;
            int max = 1;
            if (Properties.Settings.Default.InActiveFolders)
                max = 2;

            DoStatusUpdate("Scanning Active Directory...", string.Empty, 0, max, 0, 1);
            ScanGamesInternal(Properties.Settings.Default.ActiveFolder, true, Properties.Settings.Default.ActiveFolder);
            DoStatusUpdate("Scanning Active Directory...", string.Empty, 1, max, 0, 1);
            if (Properties.Settings.Default.InActiveFolders)
            {
                DoStatusUpdate("Scanning Inactive Directory...", string.Empty, 1, max, 0, 1);
                ScanGamesInternal(Properties.Settings.Default.InActiveFolder, false, Properties.Settings.Default.InActiveFolder);
                DoStatusUpdate("Scanning Inactive Directory...", string.Empty, 2, max, 0, 1);
            }
            GameCnt = isos.Count;
            foreach (Iso i in isos)
            {

                if (Abort)
                    break;
                DoStatusUpdate(string.Format("Scanning {2} Directory for ISOS ({0}/{1})", Cnt, GameCnt, i.active ? "Active" : "Inactive"), string.Empty, Cnt, GameCnt, 0, 1);
                ScanIso(i.iso, noxml, i.path, i.active, i.basepath);
                DoStatusUpdate(string.Format("Scanning {2} Directory for ISOS ({0}/{1})", Cnt, GameCnt, i.active ? "Active" : "Inactive"), string.Empty, Cnt, GameCnt, 1, 1);
            }
            DoFinish();
            Finished = true;
        }
        private void ScanIso(string str, bool noxml, string path, bool active, string bbpath)
        {
            try
            {

                string isoname = System.IO.Path.GetFileNameWithoutExtension(str);
                string xmlname = System.IO.Path.Combine(path, isoname + ".xml");
                Game game = null;
                if ((!noxml) && File.Exists(xmlname))
                {
                    try
                    {
                        game = Game.Deserialize(xmlname);
                        if (string.IsNullOrEmpty(game.Type))
                        {
                            string markedtitle = game.Title;
                            game.PopulateIso(str);
                            game.Title = markedtitle;
                        }
                    }
                    catch (Exception)
                    {
                        game = null;
                    }
                }
                if (game == null)
                {
                    game = new Game();
                    if (!game.PopulateIso(str))
                        return;
                    game.Title = isoname;
                }
                else if (string.IsNullOrEmpty(game.MediaId))
                {
                    Game game2 = new Game();
                    if (!game2.PopulateIso(str))
                        return;
                    if (string.IsNullOrEmpty(game.Title))
                        game.Title = game2.Title;
                    game.Date = game2.Date;
                    game.Disc = game2.Disc;
                    game.MediaId = game2.MediaId;
                    game.NumberOfDiscs = game2.NumberOfDiscs;
                    game.TitleId = game2.TitleId;
                    game.Type = game2.Type;
                }

                game.FullIsoPath = str;
                game.BaseName = isoname;    
                game.Active = active;
                game.BasePath = Path.Combine(path, isoname);

                if (game.Cover == null)
                {
                    if (File.Exists(game.Cover1Path))
                        game.Cover = File.ReadAllBytes(game.Cover1Path);
                    else if (File.Exists(game.Cover2Path))
                        game.Cover = File.ReadAllBytes(game.Cover2Path);
                }
                if (game.Banner == null)
                {
                    if (File.Exists(game.BannerPath))
                        game.Banner = File.ReadAllBytes(game.BannerPath);
                }
                game.GameDirectoy = bbpath;
                game.RenameWithTitle(Properties.Settings.Default.FolderStructure, !Properties.Settings.Default.menuRename);
                if (File.Exists(game.FullIsoPath) && game.Size==0)
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(game.FullIsoPath);
                    game.Size = info.Length;
                }
                Cnt++;

                DoGameProcessed(game, Cnt);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Exception scanning {0}: {1}", str, e.ToString()), "Error", MessageBoxButtons.OK);

            }
        }
        private void ScanGamesInternal(string path, bool active, string bpath)
        {
            foreach (string str in Directory.GetDirectories(path))
            {
                ScanGamesInternal(str, active, bpath);
                if (Abort)
                    break;
            }
            foreach (string str in Directory.GetFiles(path, "*.iso"))
            {
                Iso n = new Iso();
                n.iso = str;
                n.path = path;
                n.basepath = bpath;
                n.active = active;
                isos.Add(n);
            }

        }
        internal override void RetrieveThread(Game g)
        {
            throw new NotImplementedException();
        }
    }
}
