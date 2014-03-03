using System;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

using System.Net;
using System.Security.Cryptography;
using System.Text;
using xk3yScanner.Properties;
using Encoder = System.Drawing.Imaging.Encoder;

namespace xk3yScanner.Classes
{
    public class Utils
    {
        public const string UAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.16 (KHTML, like Gecko) Chrome/10.0.648.82 Safari/534.16";

        public static string GetUrl(string Url, string PostData, bool GZip, string uagent)
        {
            return GetUrl(Url, PostData, GZip, uagent, null);
        }
        public class NotModifiedExeption : Exception
        {
            public NotModifiedExeption()
            {
                
            }
        }
        public static string GetUrl(string Url, string PostData, bool GZip, string uagent,string etag)
        {
            try
            {
                HttpWebRequest Http = (HttpWebRequest)WebRequest.Create(Url);
                if (uagent != null)
                    Http.UserAgent = uagent;
                if (GZip)
                    Http.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                if (!String.IsNullOrEmpty(etag))
                    Http.Headers.Add(HttpRequestHeader.IfNoneMatch,etag);
                Http.AllowAutoRedirect = true;
                if (!String.IsNullOrEmpty(PostData))
                {
                    Http.Method = "POST";
                    byte[] lbPostBuffer = Encoding.Default.GetBytes(PostData);

                    Http.ContentLength = lbPostBuffer.Length;

                    Stream PostStream = Http.GetRequestStream();
                    PostStream.Write(lbPostBuffer, 0, lbPostBuffer.Length);
                    PostStream.Close();
                }

                HttpWebResponse WebResponse = (HttpWebResponse)Http.GetResponse();

                //Hardcoded
                if (etag != null)
                {
                    foreach (string str in WebResponse.Headers.AllKeys)
                    {
                        if (str == "ETag")
                        {
                            Properties.Settings.Default.abgxGameNameLookupETAG = WebResponse.Headers["etag"];
                            break;
                        }
                    }
                }
                Stream responseStream = WebResponse.GetResponseStream();
                if (responseStream != null)
                {
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                    Encoding enc = Encoding.UTF8;
                    if (!String.IsNullOrEmpty(WebResponse.CharacterSet))
                        enc = Encoding.GetEncoding(WebResponse.CharacterSet);
                    StreamReader Reader = new StreamReader(responseStream, enc);

                    string Html = Reader.ReadToEnd();

                    WebResponse.Close();
                    responseStream.Close();

                    return Html;
                }
            }

            catch(WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotModified)
                    throw new NotModifiedExeption();
            }
            catch(Exception e)
            {
                
            }
            return String.Empty;
        }
       public static Image ImageFromByteArray(byte[] b)
       {
           try
           {
               if (b == null)
                   return null;
               MemoryStream ms=new MemoryStream(b);
               Image im=Image.FromStream(ms);
               ms.Dispose();
               return im;
           }
           catch
           {
               return null;
           }
           
       }

        public static byte[] Download(string Url, string PostData, bool GZip, string uagent)
        {
            try
            {
                HttpWebRequest Http = (HttpWebRequest)WebRequest.Create(Url);
                if (uagent != null)
                    Http.UserAgent = uagent;
                if (GZip)
                    Http.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                Http.AllowAutoRedirect = true;
                if (!String.IsNullOrEmpty(PostData))
                {
                    Http.Method = "POST";
                    byte[] lbPostBuffer = Encoding.Default.GetBytes(PostData);

                    Http.ContentLength = lbPostBuffer.Length;

                    Stream PostStream = Http.GetRequestStream();
                    PostStream.Write(lbPostBuffer, 0, lbPostBuffer.Length);
                    PostStream.Close();
                }

                HttpWebResponse WebResponse = (HttpWebResponse)Http.GetResponse();

                Stream responseStream = WebResponse.GetResponseStream();
                if (responseStream != null)
                {
                    if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                        responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                    BinaryReader Reader = new BinaryReader(responseStream);
                    byte[] b = Reader.ReadBytes((int)WebResponse.ContentLength);
                    WebResponse.Close();
                    responseStream.Close();
                    return b;
                }
            }
            catch (Exception)
            {
                
            }
          
            return null;
        }
        public static byte[] ConvertToJpg(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            Image img = Image.FromStream(ms);
            ms.Dispose();
            return ConvertToJpg(img);
        }
        public static byte[] ConvertToJpg(Image img)
        {

            EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 85L);

            MemoryStream ms=new MemoryStream();
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = parameter;
            img.Save(ms, GetCodecInfo(), encoderParams);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] result= new byte[ms.Length];
            ms.Read(result,0, (int)ms.Length);
            ms.Close();
            ms.Dispose();
            return result;
        }

        private static ImageCodecInfo GetCodecInfo()
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



        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create();  // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
