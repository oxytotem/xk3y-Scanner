using xk3yScanner.xkeyBrew.BLBinaryReader;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using XkeyBrew.Utils.Shared;

namespace xk3yScanner.xkeyBrew.IsoGameReader
{
    [Serializable]
    public class XeXHeader : IDisposable
    {
        private uint baseVersion;
        private byte discCount;
        private byte discNumber;
        private byte executableType;
        private byte[] short_mediaId;
        private byte platform;
        private byte[] titleId;
        private uint version;
        private DateTime date;
        private byte[] mediaid;
        private uint regioncode;
        private string title = string.Empty;

        private byte[] executioninfo = {0x00, 0x04, 0x00, 0x06 };
        private byte[] basefiletimestamp = { 0x00, 0x01, 0x80, 0x02 };


        public XeXHeader(MyBinaryReader reader)
        {
            long position = reader.BaseStream.Position;

            try
            {
                byte[] b = reader.ReadBytes(4);
                if (Encoding.ASCII.GetString(b) == "XEX2")
                {

                    reader.BaseStream.Seek(position + 20L, SeekOrigin.Begin);
                    uint num = reader.ReadUInt32();

                    UInt32 einfo = BitConverter.ToUInt32(executioninfo, 0);
                    UInt32 tstamp = BitConverter.ToUInt32(basefiletimestamp, 0);

                    for (int i = 0; i < num; i++)
                    {
                        UInt32 val = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
                        uint pos = reader.ReadUInt32();
                        long savepos = reader.BaseStream.Position;
                        if (val == einfo)
                        {
                            reader.BaseStream.Seek((long) position + pos, SeekOrigin.Begin);
                            this.short_mediaId = reader.ReadBytes(4);
                            this.version = reader.ReadUInt32();
                            this.baseVersion = reader.ReadUInt32();
                            this.titleId = reader.ReadBytes(4);
                            this.platform = reader.ReadByte();
                            this.executableType = reader.ReadByte();
                            this.discNumber = reader.ReadByte();
                            this.discCount = reader.ReadByte();
                            reader.BaseStream.Seek(savepos, SeekOrigin.Begin);
                        }
                        else if (val == tstamp)
                        {
                            reader.BaseStream.Seek((long) position + pos + 4, SeekOrigin.Begin);
                            this.date =
                                new DateTime(1970, 1, 1, 0, 0, 0).Add(
                                    TimeSpan.FromTicks((long) reader.ReadUInt32()*TimeSpan.TicksPerSecond));
                            reader.BaseStream.Seek(savepos, SeekOrigin.Begin);
                        }

                    }
                    //Read cert
                    reader.BaseStream.Seek((long) position + 0x10, SeekOrigin.Begin);
                    uint offset = reader.ReadUInt32();
                    reader.BaseStream.Seek((long) position + (long) offset + 0x140, SeekOrigin.Begin);
                    mediaid = reader.ReadBytes(16);
                    reader.BaseStream.Seek((long)position + (long)offset + 0x178, SeekOrigin.Begin);
                    regioncode = reader.ReadUInt32();
                }
                else if (Encoding.ASCII.GetString(b)=="XBEH")
                {
                    reader.EndianType = EndianType.LittleEndian;
                    reader.BaseStream.Seek(position + 0x110, SeekOrigin.Begin);
                    uint pos = reader.ReadUInt32();
                    this.date = new DateTime(1970, 1, 1, 0, 0, 0).Add(TimeSpan.FromTicks((long)reader.ReadUInt32() * TimeSpan.TicksPerSecond));
                    reader.BaseStream.Seek(position + pos+ 0x8,SeekOrigin.Begin);
                    this.titleId=new byte[4];
                    this.titleId[3] = reader.ReadByte();
                    this.titleId[2] = reader.ReadByte();
                    this.titleId[1] = reader.ReadByte();
                    this.titleId[0] = reader.ReadByte();
                    byte[] b2 = reader.ReadBytes(0x40);
                    this.title=Encoding.Unicode.GetString(b2).Replace("\0",string.Empty).Trim();
                    reader.BaseStream.Seek(position + pos + 0xA8, SeekOrigin.Begin);
                    this.discNumber =  this.discCount = (byte)(reader.ReadUInt32() + 1);
                    this.version = reader.ReadUInt32();
                }
                else
                {
                    throw new Exception("Not XEX/XEB file");
                }
               
            }
            catch (Exception exception)
            {
                if (reader!=null)   
                    reader.BaseStream.Seek(position,SeekOrigin.Begin);
                throw exception;
            }
            reader.BaseStream.Seek(position, SeekOrigin.Begin);

        }

        public void Dispose()
        {
            this.short_mediaId = null;
            this.version = 0;
            this.baseVersion = 0;
            this.titleId = null;
            this.platform = 0;
            this.executableType = 0;
            this.discNumber = 0;
            this.discCount = 0;
        }

        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            PropertyInfo[] properties = typeof(XeXHeader).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                builder.AppendLine(info.Name + ": " + info.GetValue(this, null));
            }
            return builder.ToString();
        }

        public uint BaseVersion
        {
            get
            {
                return this.baseVersion;
            }
        }

        public int DiscCount
        {
            get
            {
                return this.discCount;
            }
        }

        public int DiscNumber
        {
            get
            {
                return this.discNumber;
            }
        }

        public string ExecutableType
        {
            get
            {
                return this.executableType.ToString();
            }
        }

        public string ShortMediaId
        {
            get
            {
                if (this.short_mediaId==null)
                    return string.Empty;
                return ISharedMethods.ConverByteToHex(this.short_mediaId);
            }
        }
        public string MediaId
        {
            get
            {
                if (this.mediaid == null)
                    return string.Empty;
                return ISharedMethods.ConverByteToHex(this.mediaid);
            }
        }

        public string RegionCode
        {
            get
            {
                if (regioncode == 0)
                    return string.Empty;
                return this.regioncode.ToString("X8");
            }
        }
        public string Title
        {
            get { return this.title; }
        }
        public string Platform
        {
            get
            {
                return this.platform.ToString();
            }
        }

        public string TitleId
        {
            get
            {
                return ISharedMethods.ConverByteToHex(this.titleId);
            }
        }

        public uint Version
        {
            get
            {
                return this.version;
            }
        }
        public DateTime Date
        {
            get { return this.date; }
        }
    }
}

