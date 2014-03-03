using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using XkeyBrew.Utils.Shared;

namespace xk3yScanner.xkeyBrew.IsoGameReader
{
    public class GameInfo
    {
        private string banner = "http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d802%titleid%/1033/banner.png";
        private Image bannerImage = null;
        private string gameDescription;
        private string gameName;
        private string gameUrl = "http://marketplace.xbox.com/en-US/Product/66acd000-77fe-1000-9115-d802%titleid%?nosplash=1";
        private string largeBoxArt = "http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d802%titleid%/1033/boxartlg.jpg";
        private Image largeBoxArtImage = null;
        private string smallBoxArt = "http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d802%titleid%/1033/boxartsm.jpg";
        private Image smallBoxArtImage = null;
        private string title_id;

        public GameInfo(string title_id)
        {
            this.title_id = title_id.ToLower();
            this.GetGameInfo();
        }

        private Image DownloadImage(string url)
        {
            try
            {
                byte[] buffer = new WebClient().DownloadData(url);
                if (buffer != null)
                {
                    MemoryStream stream = new MemoryStream(buffer);
                    return Image.FromStream(stream);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        private void GetGameInfo()
        {
            this.GetInfo();
            this.smallBoxArtImage = this.DownloadImage(this.SmallBoxArt);
            this.largeBoxArtImage = this.DownloadImage(this.LargeBoxArt);
            this.bannerImage = this.DownloadImage(this.Banner);
        }

        private string GetGameName()
        {
            return null;
        }

        private void GetInfo()
        {
            try
            {
                string str = new WebClient { Encoding = Encoding.UTF8 }.DownloadString(this.GameUrl);
                if (!ISharedMethods.IsObjectEmptyOrNull(str))
                {
                    Match match = Regex.Match(str, "<div id=\\\"overview1.*?<div id=\\\"overview2", RegexOptions.Singleline);
                    Match match2 = Regex.Match(match.Value, "<p>.+?</p>", RegexOptions.Singleline);
                    Match match3 = Regex.Match(match.Value, "<img alt=\\\".+?\\\"", RegexOptions.Singleline);
                    this.gameDescription = HttpUtility.HtmlDecode(match2.Value.Replace("<p>", "").Replace("</p>", ""));
                    this.gameName = HttpUtility.HtmlDecode(match3.Value.Remove(match3.Value.Length - 1).Replace("<img alt=\"", ""));
                }
            }
            catch (Exception)
            {
            }
        }

        public string Banner
        {
            get
            {
                return this.banner.Replace("%titleid%", this.title_id);
            }
        }

        public Image BannerImage
        {
            get
            {
                return this.bannerImage;
            }
        }

        public string GameDescription
        {
            get
            {
                return this.gameDescription;
            }
        }

        public XmlDocument GameInfoXml
        {
            get
            {
                XmlDocument document = new XmlDocument();
                XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", "utf-8", null);
                document.AppendChild(newChild);
                XmlElement element = document.CreateElement("GameInfo");
                XmlElement element2 = document.CreateElement("TitleId");
                element2.InnerText = this.TitleId;
                XmlElement element3 = document.CreateElement("Name");
                element3.InnerText = this.GameName;
                XmlElement element4 = document.CreateElement("Description");
                element4.InnerText = this.GameDescription;
                element.AppendChild(element2);
                element.AppendChild(element3);
                element.AppendChild(element4);
                document.AppendChild(element);
                return document;
            }
        }

        public string GameName
        {
            get
            {
                return this.gameName;
            }
        }

        public string GameUrl
        {
            get
            {
                return this.gameUrl.Replace("%titleid%", this.title_id);
            }
        }

        public string LargeBoxArt
        {
            get
            {
                return this.largeBoxArt.Replace("%titleid%", this.title_id);
            }
        }

        public Image LargeBoxArtImage
        {
            get
            {
                return this.largeBoxArtImage;
            }
        }

        public string SmallBoxArt
        {
            get
            {
                return this.smallBoxArt.Replace("%titleid%", this.title_id);
            }
        }

        public Image SmallBoxArtImage
        {
            get
            {
                return this.smallBoxArtImage;
            }
        }

        public string TitleId
        {
            get
            {
                return this.title_id;
            }
            set
            {
                this.title_id = value;
            }
        }
    }
}

