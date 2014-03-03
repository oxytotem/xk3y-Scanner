using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using BrightIdeasSoftware;
using xk3yScanner.Objects.Waffle;
using xk3yScanner.xkeyBrew.IsoGameReader;
using Encoder = System.Drawing.Imaging.Encoder;
using GameInfo = xk3yScanner.Objects.Waffle.GameInfo;

namespace xk3yScanner.Classes
{
    [Serializable]
    [XmlRoot("gameinfo")]
    public class Game : GameInfo
    {
        private string[] months = new[]
                                            {
                                                "January",
                                                "February",
                                                "March",
                                                "April",
                                                "May",
                                                "June",
                                                "July",
                                                "August",
                                                "September",
                                                "October",
                                                "November",
                                                "December"
                                            };

        [XmlIgnore] 
        public Visibility Visibility=new Visibility();

        [XmlIgnore] 
        public ListViewItem ListItem { get; private set; }

        [XmlIgnore]
        public AbgStatus Abgx { get; set; }

        public bool PopulateIso(string isopath)
        {
            this.Iso = new Iso(isopath);
            if (this.Iso.DefaultXeX == null)
                return false;
            this.Date = this.Iso.DefaultXeX.XeXHeader.Date;
            this.Disc = this.Iso.DefaultXeX.XeXHeader.DiscNumber;
            this.NumberOfDiscs = this.Iso.DefaultXeX.XeXHeader.DiscCount;
            this.TitleId = this.Iso.DefaultXeX.XeXHeader.TitleId;
            this.Type = Enum.GetName(typeof(IsoType),this.Iso.IsoType).ToUpper();
            this.RegionCode = this.Iso.DefaultXeX.XeXHeader.RegionCode;
            this.MediaId = this.Iso.DefaultXeX.XeXHeader.MediaId;
            this.WebPopulated = false;
            return true;
        }
        public bool PopulateDate(string isopath)
        {
            this.Iso = new Iso(isopath);
            if (this.Iso.DefaultXeX == null)
                return false;
            this.Date = this.Iso.DefaultXeX.XeXHeader.Date;           
            return true;
        }
        [XmlIgnore]
        public string GameDirectoy { get; set; }
        [XmlIgnore]
        public long Size { get; set; }

        private string GetDisks()
        {
            if (Disc == 9999 && NumberOfDiscs == 9999)
                return "Expansion";
            return Disc.ToString() + " of " + NumberOfDiscs.ToString();
        }
        [XmlIgnore]
        [OLVColumn("Active", MinimumWidth = 62, MaximumWidth = 62, Width = 62, DisplayIndex = 12, TextAlign = HorizontalAlignment.Center, CheckBoxes = false, IsEditable = false)]
        public bool Active { get; set; }

        [OLVColumn("Disc", MinimumWidth = 44, MaximumWidth = 44, Width = 44, DisplayIndex = 7, TextAlign = HorizontalAlignment.Center)]
        public string DiskFormat
        {
            get { return GetDisks(); }
        }



        
        public static Game Deserialize(Stream xml)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof (Game));
                Game game = (Game) serializer.Deserialize(xml);
                game.LoadInfoItems();
                game.WebPopulated = true;
                return game;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static Game Deserialize(string xml)
        {
            if (File.Exists(xml))
            {
                FileStream stream = File.OpenRead(xml);
                Game game=Deserialize(stream);
                stream.Close();
 
                return game;
            }
            return null;
        }
        public bool SaveAll()
        {
            if (Save())
            {
                SaveCover();
                SaveBanner();
                return true;
            }
            return false;
        }

        public bool Save()
        {
            try
            {
                SaveInfoItems();
                FileStream write = File.Create(XmlPath);
                XmlSerializer serializer = new XmlSerializer(typeof(Game));
                serializer.Serialize(write,this);
                write.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveCover()
        {
            if ((Cover!=null) && (!string.IsNullOrEmpty(BasePath)))
            {
                File.WriteAllBytes(Cover1Path,Cover);
                File.WriteAllBytes(Cover2Path, Cover);
            }
        }

        public void SaveBanner()
        {
            if ((Banner != null) && (!string.IsNullOrEmpty(BasePath)))
            {
                File.WriteAllBytes(BannerPath, Banner);
            }
        }
        private bool SafeRename(string oldname, string newname)
        {
            try
            {
                if (!File.Exists(oldname))
                    return false;
                if (File.Exists(newname))
                    File.Move(newname,newname+".old");
                File.Move(oldname,newname);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public byte[] GenBanner()
        {
            string tit = this.Title;
            Image im = Image.FromStream(new MemoryStream(Properties.Resources.banner));
            if (this.NumberOfDiscs > 1)
                tit += " Disc " + this.Disc;
            Graphics g = Graphics.FromImage(im);
            for (int x = 40; x >= 5; x--)
            {
                Font f = new Font("Arial", x, FontStyle.Bold);
                SizeF ns = g.MeasureString(tit, f);
                if ((ns.Width <= im.Width - 10) && (ns.Height <= im.Height - 5))
                {
                    float cx = (float)Math.Round((im.Width - ns.Width) / 2);
                    float cy = (float)Math.Round((im.Height - ns.Height)/2);
                    g.DrawString(tit, f, new SolidBrush(Color.White), cx - 1, cy - 1);
                    g.DrawString(tit, f, new SolidBrush(Color.White), cx + 1, cy + 1);
                    g.DrawString(tit, f, new SolidBrush(Color.Black), cx, cy);
                    break;
                }
            }
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 85L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            MemoryStream ms = new MemoryStream();
            im.Save(ms, jpgEncoder, myEncoderParameters);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] b=new byte[ms.Length];
            ms.Read(b, 0, (int)ms.Length);
            ms.Close();
            ms.Dispose();
            return b;
        }

        [XmlIgnore]
        [OLVColumn("Regions", MinimumWidth = 70, MaximumWidth = 70, Width = 70, DisplayIndex = 13, TextAlign = HorizontalAlignment.Left, CheckBoxes = false, IsEditable = false)]
        public string Regions
        {
            get
            {
                List<string> rgn = new List<string>();
                StringBuilder bld = new StringBuilder();
                uint regioncode = Convert.ToUInt32(RegionCode, 16);
                if (regioncode == 0xFFFFFFFF)
                {
                    rgn.Add("Region Free");
                }
                else
                {
                    if ((regioncode & 0x00FF0000) == 0x00FF0000)
                        rgn.Add("PAL");
                    else if ((regioncode & 0x00FF0000) == 0x00FE0000)
                        rgn.Add("PAL (Excludes AUS/NZ)");
                    else if ((regioncode & 0x00FF0000) == 0x00010000)
                        rgn.Add("PAL (AUS/NZ Only)");
                    else if ((regioncode & 0x00FF0000) != 0x00000000)
                        rgn.Add(string.Format("PAL (Unknown code: {0:2X})", regioncode >> 16 & 0xFF));
                    if ((regioncode & 0x0000FF00) == 0x0000FF00)
                        rgn.Add("NTSC/J");
                    else if ((regioncode & 0x0000FF00) == 0x0000FD00)
                        rgn.Add("NTSC/J (Excludes China)");
                    else if ((regioncode & 0x0000FF00) == 0x0000FE00)
                        rgn.Add("NTSC/J (Excludes Japan)");
                    else if ((regioncode & 0x0000FF00) == 0x0000FC00)
                        rgn.Add("NTSC/J (Excludes Japan and China)");
                    else if ((regioncode & 0x0000FF00) == 0x00000100)
                        rgn.Add("NTSC/J (Japan Only)");
                    else if ((regioncode & 0x0000FF00) == 0x00000200)
                        rgn.Add("NTSC/J (China Only)");
                    else if ((regioncode & 0x0000FF00) == 0x00000300)
                        rgn.Add("NTSC/J (Japan and China Only)");
                    else if ((regioncode & 0x0000FF00) != 0x00000000)
                        rgn.Add(string.Format("NTSC/J (Unknown code: {0:2X})", regioncode >> 8 & 0xFF));
                    if ((regioncode & 0x000000FF) == 0x000000FF)
                        rgn.Add("NTSC/U");
                    else if ((regioncode & 0x000000FF) != 0x00000000)
                        rgn.Add(string.Format("NTSC/U (Unknown code: {0:2X})", regioncode & 0xFF));
                    if ((regioncode & 0xFF000000) == 0xFF000000)
                        rgn.Add("Other");
                    else if ((regioncode & 0xFF000000) != 0x00000000)
                        rgn.Add(string.Format("Other (Unknown code: {0:2X})", regioncode >> 24 & 0xFF));
                }
                for (int x = 0; x < rgn.Count; x++)
                {
                    if (x > 0)
                        bld.Append(", ");
                    bld.Append(rgn[x]);
                }
                return bld.ToString();
            }
        }

        
        
        public string RegionCode { get; set; }

        public void RenameWithTitle(int type, bool moveonly)
        {
            string newbasename = string.Empty;
            if (moveonly)
                newbasename = BaseName;
            else
            {
                string tit = Classes.Items.Template(this, Properties.Settings.Default.txtTemplate);

                foreach (char c in tit)
                {
                    bool fnd = false;
                    foreach (char d in Path.GetInvalidFileNameChars())
                    {
                        if (d == c)
                        {
                            fnd = true;
                            break;
                        }
                    }
                    if (!fnd)
                        newbasename += c;
                }
            }


            string newbasepath = string.Empty;
            switch (type)
            {
                case 1:
                    char l = newbasename.ToUpper()[0];
                    if ((l < 'A') || (l > 'Z'))
                        l = '0';
                    newbasepath = Path.Combine(Path.Combine(GameDirectoy, l.ToString()), newbasename);
                    break;
                case 2:
                    string genre = "Unknown";
                    string[] genres = this.Genre.Split(new char[] {';', ','}, StringSplitOptions.RemoveEmptyEntries);
                    if (genres.Length > 0)
                        genre = genres[0];
                    string genre2 = string.Empty;
                    foreach (char c in genre)
                    {
                        bool fnd = false;
                        foreach (char d in Path.GetInvalidFileNameChars())
                        {
                            if (d == c)
                            {
                                fnd = true;
                                break;
                            }
                        }
                        if (!fnd)
                            genre2 += c;
                    }
                    newbasepath = Path.Combine(Path.Combine(GameDirectoy,genre2), newbasename);
                    break;
                default:
                    newbasepath = Path.Combine(Path.GetDirectoryName(FullIsoPath),newbasename);
                    break;

            }
            string newfullisopath = newbasepath + ".iso";
            if (String.Compare(FullIsoPath, newfullisopath, System.StringComparison.InvariantCultureIgnoreCase)==0)
                return;
            string oldFullIsoPath = Path.GetDirectoryName(FullIsoPath);
            string dir = Path.GetDirectoryName(newfullisopath);
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch
            {
            }
            if (SafeRename(this.FullIsoPath, newfullisopath))
            {
                SafeRename(XmlPath, newbasepath + ".xml");
                SafeRename(DvdPath, newbasepath + ".dvd");
                SafeRename(MdsPath, newbasepath + ".mds");
                SafeRename(Cover1Path, newbasepath + ".jpg");
                SafeRename(Cover2Path, newbasepath + "-cover.jpg");
                SafeRename(BannerPath, newbasepath + "-banner.jpg");
                BaseName = newbasename;
                BasePath = newbasepath;
                FullIsoPath = newfullisopath;
                try
                {
                    if ((oldFullIsoPath != null) && (Directory.Exists(oldFullIsoPath) && Directory.GetFileSystemEntries(oldFullIsoPath).Length == 0))
                        Directory.Delete(oldFullIsoPath);
                }
                catch (Exception)
                {
                }

            }
        }
        [XmlIgnore]
        public string Cover1Path
        {
            get { return BasePath + ".jpg"; }
        }
        [XmlIgnore]
        public string Cover2Path
        {
            get { return BasePath + "-cover.jpg"; }
        }
        [XmlIgnore]
        public string BannerPath
        {
            get { return BasePath + "-banner.jpg"; }
        }
        [XmlIgnore]
        public string XmlPath
        {
            get { return BasePath + ".xml"; }
        }
        [XmlIgnore]
        public string DvdPath
        {
            get { return BasePath + ".dvd"; }
        }
        [XmlIgnore]
        public string MdsPath
        {
            get { return BasePath + ".mds"; }
        }
        [XmlIgnore]
        public bool Locked { get; set; }



        [XmlElement("type")]
        [OLVColumn("Type", MinimumWidth = 40, MaximumWidth = 40, Width = 40, DisplayIndex = 6, TextAlign = HorizontalAlignment.Center)]
        public string Type { get; set; }

        [XmlElement("date")]
        [OLVColumn("Date",MinimumWidth = 80,MaximumWidth = 80,Width = 80,DisplayIndex = 5, AspectToStringFormat = "{0:yyyy-MM-dd}",TextAlign = HorizontalAlignment.Center)]
        public DateTime Date { get; set; }
                
        private void SplitDiscs(string original)
        {
            int disc = Disc;
            int discnumber = NumberOfDiscs;
            if (original.Contains("Expansion"))
            {
                Disc = 9999;
                NumberOfDiscs = 9999;
                return;
            }
            string[] spl = original.Split(new string[] { "of" }, StringSplitOptions.RemoveEmptyEntries);
            if (spl.Length == 2)
            {
                int.TryParse(spl[0].Trim(), out disc);
                int.TryParse(spl[1].Trim(), out discnumber);
                Disc = disc;
                NumberOfDiscs = discnumber;

            }
        }

        private void LoadInfoItems()
        {
            foreach(string str in Visibility.Keys)
            {
                string result=GetInfoItem(str);
                switch(str)
                {
                    case "Genre":
                        Genre = result;
                        break;
                    case "Discs":
                        if (string.IsNullOrEmpty(result))
                            result = GetInfoItem("Disc");
                        SplitDiscs(result);
                        break;
                    case "Publisher":
                        Publisher = result;
                        break;
                    case "Developer":
                        Developer = result;
                        break;
                    case "Trailer":
                        Trailer = result;
                        break;
                    //case "Game Date":
                      //  Date = Convert.ToDateTime(result);
                    //case "Game Type":
                    //    Type = result;
                    case "Abgx Info":
                        if (result != string.Empty)
                        {
                            Abgx = AbgStatus.Deserialize(result);
                            if (Abgx != null)
                            {
                                if ((Abgx.RegionCode != 0))
                                    RegionCode = Abgx.RegionCode.ToString("X8");
                            }
                        }
                        break;
                    case "RegionCode":
                        if (!string.IsNullOrEmpty(result.Trim()))
                            RegionCode = result;
                        break;
                }

            }

        }
        private void SaveInfoItems()
        {
            try
            {
                Items = null;
                OtherItems = null;
                foreach (string str in Visibility.Keys)
                {
                    bool visible = Visibility[str];
                    switch (str)
                    {
                        case "Genre":
                            SetInfoItem(str, Genre, visible);
                            break;
                        case "Discs":
                            SetInfoItem(str, GetDisks(), visible);
                            break;
                        case "Publisher":
                            SetInfoItem(str, Publisher, visible);
                            break;
                        case "Developer":
                            SetInfoItem(str, Developer, visible);
                            break;
                        case "Trailer":
                            SetInfoItem(str, Trailer, visible);
                            break;
                        case "Game Date":
                            string dta = Date.Day+" "+months[Date.Month-1] + " " + Date.Year.ToString();
                            SetInfoItem(str, dta, visible);
                            break;
                        case "Game Type":
                            SetInfoItem(str, Type == "XBOX1" ? "Xbox 1" : "Xbox 360", visible);
                            break;
                        case "Abgx Info":
                            if (Abgx!=null)
                                SetInfoItem(str,Abgx.Serialize(),visible);
                            break;
                        case "Regions":
                            SetInfoItem(str,Regions,visible);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        [XmlIgnore]
        public int Disc { get; set; }

        [XmlIgnore]
        public int NumberOfDiscs { get; set; }
           
        [XmlIgnore]
        [OLVColumn("Genre",DisplayIndex = 4,MinimumWidth = 60,Width=60,FillsFreeSpace = true,FreeSpaceProportion = 24)]
        public string Genre { get; set; }

        [XmlIgnore]
        [OLVColumn("Developer",DisplayIndex =8,MinimumWidth = 60,Width=60,FillsFreeSpace = true,FreeSpaceProportion = 18)]
        public string Developer { get; set; }

        [XmlIgnore]
        public string Trailer { get; set; }

        [XmlIgnore]
        [OLVColumn("Publisher",DisplayIndex = 9,MinimumWidth = 60,Width=60,FillsFreeSpace = true,FreeSpaceProportion = 18)]
        public string Publisher { get; set; }

        [XmlIgnore]
        [OLVColumn("Abgx",DisplayIndex = 11, MinimumWidth = 40,MaximumWidth = 40,Width=40,TextAlign = HorizontalAlignment.Center)]
        public int AbgxStats
        {
            get
            {
                if (Abgx == null)
                    return 0;
                else
                {
                    if (Abgx.Stats == AbgStatus.Status.Ok)
                        return 1;
                    if (Abgx.Stats == AbgStatus.Status.Warning)
                        return 2;
                    return 3;
                }
            }
        }

        public string ID
        {
            get
            {
                return Utils.GetHashString(this.FullIsoPath.Substring(this.GameDirectoy.Length+1).Replace("\\","/"));
            }
        }

        private string GetInfoItem(string name)
        {
            if ((Items != null) && (Items.Count > 0))
            {
                foreach (InfoItem i in Items)
                {
                    if (i.Name == name)
                    {
                        return i.Value;
                    }
                }
            }
            if ((OtherItems != null) && (OtherItems.Count > 0))
            {
                foreach (InfoItem i in OtherItems)
                {
                    if (i.Name == name)
                    {
                        return i.Value;
                    }
                }
            }
            return string.Empty;
        }
        public void SetInfoItem(string name, string value, bool visibility)
        {
            List<InfoItem> onitems;
            List<InfoItem> offitems;
            if (Items == null)
                Items = new List<InfoItem>();
            if (OtherItems == null)
                OtherItems = new List<InfoItem>();

            if (visibility)
            {
                onitems = Items;
                offitems = OtherItems;
            }
            else
            {

                onitems = OtherItems;
                offitems = Items;
            }
            bool fnd = false;
            foreach (InfoItem i in onitems)
            {
                if (i.Name == name)
                {
                    i.Value = value;
                    fnd = true;
                    break;
                }
            }
            if (!fnd)
            {
                InfoItem ii = new InfoItem();
                ii.Name = name;
                ii.Value = value;
                onitems.Add(ii);
            }
            foreach (InfoItem i in offitems)
            {
                if (i.Name == name)
                {
                    offitems.Remove(i);
                    break;
                }
            }
        }
    }
}
