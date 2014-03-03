using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using System.Windows.Forms;
using xk3yScanner.Classes;

namespace xk3yScanner
{
    public partial class Edit : Form
    {
        public Game Game { get; set; }
        
        private byte[] cover;
        private byte[] banner;

        public Edit(Game g)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.xkey;
            Game = g;

        }

        private void Edit_Load(object sender, EventArgs e)
        {
            this.ClientSize = Properties.Settings.Default.editSize;
            textTitle.Text = Game.Title ?? string.Empty;
            textGenre.Text = Game.Genre ?? string.Empty;
            textDeveloper.Text = Game.Developer ?? string.Empty;
            textPublisher.Text = Game.Publisher ?? string.Empty;
            textTrailer.Text = Game.Trailer ?? string.Empty;
            textDescription.Text = Game.Summary ?? string.Empty;
            if (Game.Cover != null)
                picCover.Image = Utils.ImageFromByteArray(Game.Cover);
            if (Game.Banner != null)
                picBanner.Image = Utils.ImageFromByteArray(Game.Banner);
            cover = Game.Cover;
            banner = Game.Banner;
            if (Game.Abgx != null)
            {
                labLTVersion.Text = Game.Abgx.LTouchVersion==AbgStatus.LTVersion.LT10 ? "1":"2";
                labType.Text = Game.Abgx.DiskVersion.ToString();
                labSize.Text = Game.Abgx.Size.ToString();
                labRegion.Text = Game.Abgx.GetRegions();
                labWave.Text = Game.Abgx.Wave;
                labKernel.Text = Game.Abgx.MinKernel;
                labMedia.Text = Game.Abgx.MediaId;
                GetCRC(labXEX,Game.Abgx.XEXCRC);
                labXEX.LinkColor = labXEX.ActiveLinkColor = labXEX.VisitedLinkColor = labXEX.ForeColor;
                GetCRC(labVideo,Game.Abgx.VideoCRC);
                GetCRC(labSS,Game.Abgx.SSCRC);
                GetCRC(labPFI,Game.Abgx.PFICRC);
                GetCRC(labDMI,Game.Abgx.DMICRC);
                GetCRC(labGame,Game.Abgx.GameCRC);
                labAnyDVD.Text = Game.Abgx.AnyDVDCorruption ? "Yes" : "No";
                labAnyDVD.ForeColor = Game.Abgx.AnyDVDCorruption ? Color.Red : Color.Green;
                labDate.Text = Game.Abgx.LastCheck.ToString();
                if ((Game.Abgx.Error) || (labAnyDVD.ForeColor == Color.Red) || (labXEX.ForeColor == Color.Red)
                        || (labVideo.ForeColor == Color.Red) || (labSS.ForeColor == Color.Red)
                        || (labPFI.ForeColor == Color.Red) || (labDMI.ForeColor == Color.Red)
                        || (labGame.ForeColor == Color.Red))
                {
                    textStatus.ForeColor = Color.Red;
                    textStatus.Text = "Stealth failed\r\n" + Game.Abgx.ErrorString;
                }
                else if ((labXEX.ForeColor == Color.Goldenrod)
                        || (labVideo.ForeColor == Color.Goldenrod) || (labSS.ForeColor == Color.Goldenrod)
                        || (labPFI.ForeColor == Color.Goldenrod) || (labDMI.ForeColor == Color.Goldenrod)
                        || (labGame.ForeColor == Color.Goldenrod))
                {
                    textStatus.ForeColor = Color.Goldenrod;
                    textStatus.Text = "Stealth OK but unverified";
                }
                else
                {
                    textStatus.ForeColor = Color.Green;
                    textStatus.Text = "Stealth OK";
                }
            }

        }
        private void GetCRC(Label lab, AbgStatus.CRCInfo info)
        {
            string text = info.Crc.ToString("X8");
            switch (info.Status)
            {
                case AbgStatus.CRCStatus.Verified:
                    text += " verified";
                    lab.ForeColor = Color.Green;
                    break;
                case AbgStatus.CRCStatus.Unverified:
                    text += " unverified";
                    lab.ForeColor = Color.Goldenrod;
                    break;
                default:
                    text += " bad";
                    lab.ForeColor = Color.Red;
                    break;
            }
            lab.Text = text;
        }
        public byte[] AskForImage(int width, int height)
        {
            try
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.gfxPath))
                    openFileDialog.InitialDirectory = Properties.Settings.Default.gfxPath;
                if (openFileDialog.ShowDialog()==DialogResult.OK)
                {
                    if (File.Exists(openFileDialog.FileName))
                    {
                        byte[] b = File.ReadAllBytes(openFileDialog.FileName);
                        MemoryStream ms=new MemoryStream(b);
                        Image im=Bitmap.FromStream(ms);
                        ms.Dispose();
                        Properties.Settings.Default.gfxPath = openFileDialog.InitialDirectory;
                        if ((im.Width>width) || (im.Height>height))
                        {
                            Bitmap bm=new Bitmap(width,height,PixelFormat.Format24bppRgb);
                            Graphics g = Graphics.FromImage(bm);
                            g.CompositingMode=CompositingMode.SourceCopy;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode=InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.DrawImage(im,new Rectangle(0,0,width,height),new Rectangle(0,0,im.Width,im.Height),GraphicsUnit.Pixel);
                            g.Dispose();
                            return Utils.ConvertToJpg(bm);
                        }
                        return b;
                    }
                }
            }
            catch(Exception)
            {
                
            }
            return null;
        }
        private void picCover_Click(object sender, EventArgs e)
        {
            byte[] b = AskForImage(219, 300);
            if (b!=null)
            {
                cover = b;
                picCover.Image=Utils.ImageFromByteArray(b);
            }

        }

        private void picBanner_Click(object sender, EventArgs e)
        {
            byte[] b = AskForImage(420, 95);
            if (b != null)
            {
                banner = b;
                picBanner.Image = Utils.ImageFromByteArray(b);
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            bool RenameNeed = false;
            if (!string.IsNullOrEmpty(textTitle.Text))
            {
                if (Game.Title != textTitle.Text)
                    RenameNeed = true;
                Game.Title = textTitle.Text;
            }
            if (!string.IsNullOrEmpty(textGenre.Text))
            {
                if ((Game.Genre != textGenre.Text) && (Properties.Settings.Default.FolderStructure == 2))
                    RenameNeed = true;
                Game.Genre = textGenre.Text;
            }
            if (!string.IsNullOrEmpty(textDeveloper.Text))
                Game.Developer = textDeveloper.Text;
            if (!string.IsNullOrEmpty(textPublisher.Text))
                Game.Publisher = textPublisher.Text;
            if (!string.IsNullOrEmpty(textTrailer.Text))
                Game.Trailer = textTrailer.Text;
            if (!string.IsNullOrEmpty(textDescription.Text))
                Game.Summary = textDescription.Text;
            if (cover != null)
                Game.Cover = cover;
            if (banner != null)
                Game.Banner = banner;
            if (RenameNeed)
                Game.RenameWithTitle(Properties.Settings.Default.FolderStructure, !Properties.Settings.Default.menuRename);
            DialogResult = DialogResult.OK;
            this.Close();

        }

        private void Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
             Properties.Settings.Default.editSize=this.ClientSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            banner = Game.GenBanner();
            picBanner.Image = Utils.ImageFromByteArray(banner);
        }

        private void labXEX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if ((Game.Abgx!=null) && (Game.Abgx.XEXCRC != null))
            {
                string url = string.Format("http://www.abgx360.net/verified.php?f=pressings&q={0}",
                                           Game.Abgx.XEXCRC.Crc.ToString("X8"));
                Process.Start(url);
            }
        }




    }
}
