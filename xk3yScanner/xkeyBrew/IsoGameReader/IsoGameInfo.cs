using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using XkeyBrew.Utils.IsoGameReader;

namespace xk3yScanner.xkeyBrew.IsoGameReader
{
    public class IsoGameInfo : IDisposable
    {
        private byte[] defaultXexFile;
        private GameInfo gameInfo;
        private IsoType isoType;
        private string path;
        private XeXHeader xexHeader;

        public IsoGameInfo(string path)
        {
            try
            {
                this.path = path;
                Iso iso = new Iso(path);
                this.xexHeader = ObjectCopier.Clone<XeXHeader>(iso.DefaultXeX.XeXHeader);
                this.defaultXexFile = new byte[iso.DefaultXeX.File.Length];
                iso.DefaultXeX.File.CopyTo(this.defaultXexFile, 0);
                this.isoType = iso.IsoType;
                iso.Dispose();
                iso = null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private Image CompressFile(Image image)
        {
            Image image2 = null;
            try
            {
                int num = 0x32000;
                MemoryStream stream = new MemoryStream();
                ImageCodecInfo codecInfo = this.GetCodecInfo();
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 90L);
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = parameter;
                if (codecInfo == null)
                {
                    return image2;
                }
                image.Save(stream, codecInfo, encoderParams);
                for (int i = 5; (stream.Length > num) || (i == 80); i += 5)
                {
                    stream = new MemoryStream();
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (long) (90 - i));
                    image.Save(stream, codecInfo, encoderParams);
                }
                if (stream.Length <= num)
                {
                    image2 = Image.FromStream(stream);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error compressing file to xkey limit", exception);
            }
            return image2;
        }

        private Image CutFrontCover(Image image)
        {
            Image image2;
            try
            {
                int width = 0x1a9;
                int height = image.Height;
                int num4 = image.Width;
                int num5 = image.Height;
                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(num4 - width, 0, width, height), GraphicsUnit.Pixel);
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Jpeg);
                image2 = bitmap;
            }
            catch (Exception exception)
            {
                throw new Exception("Error cutting front cover covers.jqe360.com for iso: " + this.path, exception);
            }
            return image2;
        }

        public void Dispose()
        {
            this.path = string.Empty;
            this.isoType = IsoType.GDF;
            this.xexHeader = null;
            this.xexHeader.Dispose();
            this.defaultXexFile = null;
        }

        public Image DownloadCoverFromJqe360()
        {
            return this.DownloadCoverFromJqe360(false);
        }

        public Image DownloadCoverFromJqe360(bool frontAndBack)
        {
            Image image3;
            try
            {
                string str2;
                string str = "http://covers.jqe360.com/Covers/%titleid%/cover.jpg";
                WebClient client = new WebClient();
                bool flag = false;
                bool flag2 = false;
                byte[] buffer = null;
                try
                {
                    str2 = str.Replace("%titleid%", this.xexHeader.TitleId.ToUpper());
                    buffer = client.DownloadData(str2);
                    flag = true;
                }
                catch
                {
                }
                if (!flag)
                {
                    try
                    {
                        str2 = str.Replace("%titleid%", this.xexHeader.TitleId.ToLower());
                        buffer = client.DownloadData(str2);
                        flag2 = true;
                    }
                    catch
                    {
                    }
                }
                if (flag && flag2)
                {
                    throw new Exception("Didn't find any pictures for this game");
                }
                if (buffer == null)
                {
                    throw new Exception("Didn't find any pictures for this game");
                }
                MemoryStream stream = new MemoryStream(buffer);
                Image image = Image.FromStream(stream);
                if (!frontAndBack)
                {
                    image = this.CutFrontCover(image);
                }
                Image image2 = this.CompressFile(image);
                if (image2 != null)
                {
                    image = image2;
                }
                image3 = image;
            }
            catch (Exception exception)
            {
                throw new Exception("Error downloading cover from covers.jqe360.com for iso: " + this.path, exception);
            }
            return image3;
        }

        public Image DownloadCoverFromXboxCom()
        {
            try
            {
                string address = "http://download.xbox.com/content/images/66acd000-77fe-1000-9115-d802%titleid%/1033/boxartlg.jpg";
                WebClient client = new WebClient();
                address = address.Replace("%titleid%", this.xexHeader.TitleId.ToLower());
                byte[] buffer = client.DownloadData(address);
                if (buffer != null)
                {
                    MemoryStream stream = new MemoryStream(buffer);
                    return Image.FromStream(stream);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error downloading cover from www.xbox.com for iso: " + this.path, exception);
            }
            return null;
        }

        private ImageCodecInfo GetCodecInfo()
        {
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < imageEncoders.Length; i++)
            {
                if (imageEncoders[i].MimeType == "image/jpeg")
                {
                    return imageEncoders[i];
                }
            }
            return null;
        }

        public bool SaveDefaultXexToDisc(string path)
        {
            bool flag;
            try
            {
                System.IO.File.WriteAllBytes(path, this.defaultXexFile);
                flag = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public byte[] DefaultXeXFile
        {
            get
            {
                return this.defaultXexFile;
            }
        }

        public GameInfo GameInfo
        {
            get
            {
                return this.gameInfo;
            }
        }

        public IsoType IsoType
        {
            get
            {
                return this.isoType;
            }
        }

        public XeXHeader XeXHeaderInfo
        {
            get
            {
                return this.xexHeader;
            }
        }
    }
}

