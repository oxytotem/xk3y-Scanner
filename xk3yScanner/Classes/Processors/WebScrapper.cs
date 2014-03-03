using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using xk3yScanner.Classes.Processors.Helpers;
using xk3yScanner.Objects.Waffle;

namespace xk3yScanner.Classes.Processors
{
    public class WebScrapper : BaseProcessor
    {
        private AbgxNameLookup _lookup;
        public WebScrapper() : base(4)
        {
            _lookup = new AbgxNameLookup();
        }

        private Regex xbox_regex=new Regex(Properties.Settings.Default.xboxRegex,RegexOptions.Singleline);
        private Regex youtube_regex=new Regex(Properties.Settings.Default.youtubeRegex,RegexOptions.Singleline);


        internal override void RetrieveThread(Game g)
        {
            if (RetrieveWebInfo(g,Properties.Settings.Default.menuPrefer3DCovers))
                g.RenameWithTitle(Properties.Settings.Default.FolderStructure,!Properties.Settings.Default.menuRename);
        }
        private void Status(Game game,string str)
        {
            DoStatusUpdate("Web Scrapping...",string.Format("{0}: {1}", game.Title, str),Cnt,GameCnt, 0,1);
        }


        public bool RetrieveWebInfo(Game game, bool spiffycover)
        {
            try
            {
                string spiffy3dcover = Properties.Settings.Default.spiffy3DCover.Replace("{titleid}", game.TitleId.ToUpper());

                Status(game, "Loading Xbox.com Title information");
                
                string summary=game.Summary;
                string title = game.Title;
                string coverurl=Properties.Settings.Default.xboxCover.Replace("{titleid}", game.TitleId.ToLower());
                string bannerurl=Properties.Settings.Default.xboxBanner.Replace("{titleid}", game.TitleId.ToLower());
                string developer = game.Developer;
                string publisher = game.Publisher;
                string genre = game.Genre;
                byte[] cover = null;

                string place = "en-US";
                uint region = Convert.ToUInt32(game.RegionCode, 16);
                if ((region & 0x000000FF) != 0x00000000)
                    place = "en-US";
                else if ((region & 0x00FF0000) > 0x00010000)
                    place = "en-GB";
                else if ((region & 0x00FF0000) == 0x00010000)
                    place = "en-AU";
                else if ((region & 0x0000FF00) == 0x0000FF00)
                    place = "ja-JP";
                else if ((region & 0x0000FF00) == 0x0000FD00)
                    place = "ja-JP";
                else if ((region & 0x0000FF00) == 0x0000FE00)
                    place = "ja-JP";
                else if ((region & 0x0000FF00) == 0x00000100)
                    place = "ja-JP";
                else if ((region & 0x0000FF00) == 0x00000200)
                    place = "en-HK";
                else if ((region & 0x0000FF00) == 0x00000300)
                    place = "ja-JP";
                else
                    place = "en-US";

                if (Properties.Settings.Default.DefaultStore != string.Empty)
                    place = Properties.Settings.Default.DefaultStore;

                byte[] banner = null;
                List<string> places = new List<string>();
                foreach (string ste in Properties.Settings.Default.xboxStores)
                {
                    string[] spl = ste.Split('|');
                    places.Add(spl[1]);
                }                
                places.Remove(place);

                string url = Properties.Settings.Default.xboxUri.Replace("{place}", place).Replace("{titleid}", game.TitleId.ToLower());
                string markethtml = Utils.GetUrl(url, null, true, Utils.UAgent);
                if (string.IsNullOrEmpty(markethtml))
                {
                    foreach (string p in places)
                    {
                        url = Properties.Settings.Default.xboxUri.Replace("{place}", p).Replace("{titleid}", game.TitleId.ToLower());
                        markethtml = Utils.GetUrl(url, null, true, Utils.UAgent);
                        if (!string.IsNullOrEmpty(markethtml))
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(markethtml))
                {

                    Match m = xbox_regex.Match(markethtml);
                    if (m.Success)
                    {
                        
                        summary = HttpUtility.HtmlDecode(m.Groups["summary"].Value).Trim();
                        title = HttpUtility.HtmlDecode(m.Groups["title"].Value).Trim();
                        coverurl = HttpUtility.HtmlDecode(m.Groups["coverurl"].Value).Trim();
                        developer = HttpUtility.HtmlDecode(m.Groups["developer"].Value).Trim();
                        publisher = HttpUtility.HtmlDecode(m.Groups["publisher"].Value).Trim();
                        genre = HttpUtility.HtmlDecode(m.Groups["genre"].Value).Trim();
                        if (coverurl.Contains("/boxartlg.jpg"))
                            bannerurl = coverurl.Replace("/boxartlg.jpg", "/banner.png");
                        for (int x = 0; x < Properties.Settings.Default.GenreTranslation.Count-1; x += 2)
                        {
                            string org = Properties.Settings.Default.GenreTranslation[x];
                            string dest= Properties.Settings.Default.GenreTranslation[x+1];
                            genre = genre.Replace(org, dest);
                        }
                    }
                    
                }
                else
                {
                    Status(game, "Connection to xbox.com failed, check your internet connection");
                }
                if ((Properties.Settings.Default.chkOverwriteImages) || (game.Cover == null) || (game.Cover.Length == 0))
                {
                    if (spiffycover)
                    {
                        cover = Utils.Download(spiffy3dcover, null, true, Utils.UAgent);
                        if (cover != null)
                            cover = Utils.ConvertToJpg(cover);
                    }
                    if (cover == null)
                    {
                        Status(game, "Loading cover");
                        cover = Utils.Download(coverurl, null, true, Utils.UAgent);
                        if (cover == null)
                        {
                            string oldcover = coverurl;
                            coverurl = Properties.Settings.Default.xboxCover.Replace("{titleid}", game.TitleId.ToLower());
                            if (oldcover != coverurl)
                                cover = Utils.Download(coverurl, null, true, Utils.UAgent);
                            bannerurl = Properties.Settings.Default.xboxBanner.Replace("{titleid}",
                                game.TitleId.ToLower());
                        }
                    }
                }
                if ((Properties.Settings.Default.chkOverwriteImages) || (game.Banner == null) ||
                    (game.Banner.Length == 0))
                {

                    banner = Utils.Download(bannerurl, null, true, Utils.UAgent);
                    if (banner != null)
                    {
                        Status(game, "Loading banner");
                        banner = Utils.ConvertToJpg(banner);
                    }
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
                else if (game.TitleId == "FFED2000")
                {
                    game.NumberOfDiscs = 9999;
                    game.Disc = 9999;
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
                    string input = new Regex("(?:[^a-zA-Z0-9% 一-龠ぁ-ゔァ-ヴーａ-ｚＡ-Ｚ０-９々〆〤]|(?<=['\"])s)", RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase).Replace(title, string.Empty).Replace(" ", "+");
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
