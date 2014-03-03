using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using xk3yScanner.Objects.Waffle;
using Encoder = System.Drawing.Imaging.Encoder;

namespace xk3yScanner.Classes
{
    public class GamesLoader
    {
        private AbgxNameLookup _lookup;
        public GamesLoader()
        {
            _lookup = new AbgxNameLookup();
        }
        private Regex xbox_regex=new Regex(Properties.Settings.Default.xboxRegex,RegexOptions.Singleline);
        private Regex spiffy_regex=new Regex(Properties.Settings.Default.spiffyRegex,RegexOptions.Singleline);
        private Regex youtube_regex=new Regex(Properties.Settings.Default.youtubeRegex,RegexOptions.Singleline);
        public delegate void OnGameScannedHandler(Game game, int count);

        public delegate void OnFinishHandler();

        public delegate void OnStatusUpdateHandler(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount, int rightmaxcount);

        public delegate void OnWebDataLoadedHandler(Game game, int count);

        public event OnGameScannedHandler OnGameScanned;
        public event OnStatusUpdateHandler OnStatusUpdate;
        public event OnWebDataLoadedHandler OnWebDataLoaded;
        public event OnFinishHandler OnFinish;
        public event OnFinishHandler OnScanFinish;

        private int _cnt;
        private int _gamecnt;

        public bool Finished { get; set; }
        public bool Abort { get; set; }

        private class ScanCls
        {
            public string Path { get; set; }
            public string InactivePath { get; set; }
            public bool NoXml { get; set; }

        }


        public class Iso
        {
            public string basepath;
            public string iso;
            public string path;
            public bool active;
        }

        public List<Iso> isos;
        public void ScanGames(string path, string inactivepath,bool noxml)
        {
            isos = new List<Iso>();
            Abort = false;
            Finished = false;
            _cnt = 0;
            ScanCls s=new ScanCls();
            s.Path = path;
            s.NoXml = noxml;
            s.InactivePath = inactivepath;
            Thread th=new Thread(ScanGamesThread);
            th.Start(s);
        }
        public void DoStatusUpdate(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount,
                                   int rightmaxcount)
        {
            if (OnStatusUpdate != null)
                OnStatusUpdate(lefttext, righttext, leftcount, leftmaxcount, rightcount, rightmaxcount);
        }
        private void ScanGamesThread(object obj)
        {
            ScanCls c = (ScanCls)obj;
            DoStatusUpdate("Scanning Active Directory...", string.Empty, 0, 1, 0, 1);
            ScanGamesInternal(c.Path,true,c.Path);
            if (!string.IsNullOrEmpty(c.InactivePath))
            {
                DoStatusUpdate("Scanning Inactive Directory...", string.Empty, 0, 1, 0, 1);
                ScanGamesInternal(c.InactivePath,false,c.InactivePath);            
            }
            int ccnt = 1;
            foreach (Iso i in isos)
            {

                if (Abort)
                    break;
                DoStatusUpdate(string.Format("Scanning {2} ISO ({0}/{1})", ccnt, isos.Count, i.active ? "Active" : "Inactive"), string.Empty, ccnt, isos.Count, 0, 1);
                ScanIso(i.iso,c.NoXml,i.path,i.active,i.basepath);
                DoStatusUpdate(string.Format("Scanning {2} ISO ({0}/{1})", ccnt, isos.Count, i.active ? "Active" : "Inactive"), string.Empty, ccnt, isos.Count, 1, 1);
                ccnt++;
            }
            if (OnScanFinish != null)
                OnScanFinish();
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
                game.RenameWithTitle(bbpath,Properties.Settings.Default.FolderStructure,false);
                _cnt++;
                if (OnGameScanned != null)
                    OnGameScanned.Invoke(game, _cnt);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Exception scanning {0}: {1}", str, e.ToString()), "Error", MessageBoxButtons.OK);

            }
        }
        private void ScanGamesInternal(string path,bool active, string bpath)
        {
            foreach (string str in Directory.GetDirectories(path))
            {
                ScanGamesInternal(str,active,bpath);
                if (Abort)
                    break;
            }
            foreach (string str in Directory.GetFiles(path, "*.iso"))
            {
                Iso n=new Iso();
                n.iso = str;
                n.path = path;
                n.basepath = bpath;
                n.active = active;
                isos.Add(n);
            }

        }
        private class ThreadCls
        {
            public List<Game> Games { get; set; }
            public bool SpiffyCover { get; set; }
            public bool Rename { get; set; }
            public int Type { get; set; }
            public string BasePath { get; set; }
            public string InactivePath { get; set; }
        }
        public class Thread2Cls
        {
            public Game Game { get; set; }
            public bool SpiffyCover { get; set; }
            public bool Rename { get; set; }
            public int Type { get; set; }
            public string BasePath { get; set; }
            public string InactivePath { get; set; }
        }
        public void ProcessGames(IEnumerable<Game> games, bool spiffycover, bool rename, int type,string path, string inactivepath)
        {
            _gamecnt = 0;
            Abort = false;
            Finished = false;
            ThreadCls cls=new ThreadCls();
            cls.Games=new List<Game>();
            cls.Games.AddRange(games);
            cls.SpiffyCover = spiffycover;
            cls.Rename = rename;
            cls.Type = type;
            cls.BasePath = path;
            cls.InactivePath = inactivepath;
            Thread th=new Thread(ExecutorThread);
            th.Start(cls);
        }

        private int threadcnt = 0;
        private object executorlock = new object();
        private const int MAX_THREADS = 4;

        private void ExecutorThread(object obj)
        {
            ThreadCls cls = (ThreadCls) obj;
            threadcnt = 0;
            do
            {
                if (cls.Games.Count > 0)
                {
                    Thread2Cls cls2 = new Thread2Cls();
                    lock (executorlock)
                    {
                        cls2.Game = cls.Games[0];
                        cls.Games.RemoveAt(0);
                    }
                    cls2.Rename = cls.Rename;
                    cls2.SpiffyCover = cls.SpiffyCover;
                    cls2.Type = cls.Type;
                    cls2.BasePath = cls.BasePath;
                    cls2.InactivePath = cls.InactivePath;
                    Thread th = new Thread(RetrieveThread);
                    th.Start(cls2);
                    threadcnt++;
                }
                while (threadcnt==MAX_THREADS)
                {
                    Thread.Sleep(50);
                }
                Thread.Sleep(50);
            } while (((cls.Games.Count > 0) || threadcnt>0) && (!Abort));
            Finished = true;
            if (this.OnFinish!=null)
                OnFinish.Invoke();
        }
        public void RetrieveThread(object obj)
        {
            Thread2Cls cls = (Thread2Cls) obj;
            if (RetrieveWebInfo(cls.Game,cls.SpiffyCover))
            {
                cls.Game.RenameWithTitle(cls.Game.Active ? cls.BasePath : cls.InactivePath, cls.Type,!cls.Rename);
                _gamecnt++;
                if (OnWebDataLoaded != null)
                    OnWebDataLoaded.Invoke(cls.Game, _gamecnt);
            }
            threadcnt--;
        }
        private void Status(Game game,string str)
        {
            DoStatusUpdate(string.Empty,string.Format("{0}: {1}", game.Title, str),-1,-1,0,1);
        }
        public void SetGameVisibility()
        {
            
        }


        public bool RetrieveWebInfo(Game game, bool spiffycover)
        {
            try
            {
                string spiffytitle=null;
                string spiffy3dcover = Properties.Settings.Default.spiffy3DCover.Replace("{titleid}", game.TitleId.ToUpper());
                string spiffyfrontcover = Properties.Settings.Default.spiffy3DCover.Replace("{titleid}", game.TitleId.ToUpper());

                Status(game, "Loading Xbox.com Title information");
                
                string summary=game.Summary;
                string title = game.Title;
                string coverurl=Properties.Settings.Default.xboxCover.Replace("{titleid}", game.TitleId.ToLower());
                string bannerurl=Properties.Settings.Default.xboxBanner.Replace("{titleid}", game.TitleId.ToLower());
                string developer = game.Developer;
                string publisher = game.Publisher;
                string genre = game.Genre;
                byte[] cover = null;
                byte[] banner = null;
                string markethtml = Utils.GetUrl(Properties.Settings.Default.xboxUri.Replace("{titleid}", game.TitleId.ToLower()), null, true, Utils.UAgent);
                if (!string.IsNullOrEmpty(markethtml))
                {

                    Match m = xbox_regex.Match(markethtml);
                    if (m.Success)
                    {
                        summary = HttpUtility.HtmlDecode(m.Groups[1].Value).Trim();
                        title = HttpUtility.HtmlDecode(m.Groups[2].Value).Trim();
                        coverurl = HttpUtility.HtmlDecode(m.Groups[3].Value).Trim();
                        developer = HttpUtility.HtmlDecode(m.Groups[4].Value).Trim();
                        publisher = HttpUtility.HtmlDecode(m.Groups[5].Value).Trim();
                        genre = HttpUtility.HtmlDecode(m.Groups[6].Value).Trim();
                    }
                    
                }
                else
                {
                    Status(game, "Connection to xbox.com failed, check your internet connection");
                }

                if (spiffycover)
                {
                    cover = Utils.Download(spiffy3dcover, null, true, Utils.UAgent);
                    if (cover!=null)
                        cover = Utils.ConvertToJpg(cover);
                }
                if (cover==null)
                {
                    Status(game, "Loading cover");
                    cover = Utils.Download(coverurl, null, true, Utils.UAgent);
                }


                banner=Utils.Download(bannerurl, null, true, Utils.UAgent);
                if (banner != null)
                {
                    Status(game, "Loading banner");                    
                    banner = Utils.ConvertToJpg(banner);
                }
                //abgx
                int disc = 1;
                string abgxname = _lookup.GetName(game.MediaId,out disc);
                if (abgxname != null)
                {
                    title = abgxname;
                    if (game.NumberOfDiscs < disc)
                        game.NumberOfDiscs = disc;
                    game.Disc = disc;
                }
                //Youtube
                game.Summary = summary;
                game.Title = title;
                if (cover!=null)
                    game.Cover = cover;
                if (banner!=null)
                    game.Banner = banner;
                if (game.Cover == null)
                    game.Cover = Properties.Resources.cover;
                if (game.Banner == null)
                    game.Banner = game.GenBanner();
                game.Developer = developer;
                game.Publisher = publisher;
                game.Genre = genre;
                if (!string.IsNullOrEmpty(title))
                {
                    List<Trailer> trailers=new List<Trailer>();
                    string input = new Regex("(?:[^a-z0-9% ]|(?<=['\"])s)", RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase).Replace(title, string.Empty).Replace(" ","+");
                    string youtubeurl = Properties.Settings.Default.youtubeUri.Replace("{title}", input);
                    Status(game, "Searching for youtube trailers");
                    string youtubexml = Utils.GetUrl(youtubeurl, null, true, Utils.UAgent);
                    if (!string.IsNullOrEmpty(youtubexml))
                    {
                        MatchCollection collection = youtube_regex.Matches(youtubexml);
                        foreach (Match ma in collection)
                        {
                            Trailer tr = new Trailer();
                            tr.Id = ma.Groups[1].Value;
                            tr.Url = Properties.Settings.Default.youtubeviewurl.Replace("{video}", tr.Id);
                            tr.Title = ma.Groups[2].Value;
                            tr.Author = ma.Groups[3].Value;
                            trailers.Add(tr);
                        }

                        game.Trailers = trailers;
                        if (string.IsNullOrEmpty(game.Trailer) && game.Trailers.Count>0)
                            game.Trailer = trailers[0].Url;
                    }
                }
                Status(game, "Game processed");
                game.WebPopulated = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
