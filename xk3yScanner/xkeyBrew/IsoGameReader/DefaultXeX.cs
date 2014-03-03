using XkeyBrew.Utils.IsoGameReader;
using xk3yScanner.xkeyBrew.BLBinaryReader;
using System;
using System.IO;
using System.Text;

namespace xk3yScanner.xkeyBrew.IsoGameReader
{
    public class DefaultXeX : IDisposable
    {
        private byte[] file;
        private string fileName = "default.xex";
        private string fileName2 = "default.xbe";

        private byte fileNameLength;
        private uint fileSize;
        private Iso iso;
        private ushort offsetLeftSubTree;
        private ushort offsetRightSubTree;
        private uint startingSectorOfFile;
        private XeXHeader xexHeader;

        public DefaultXeX(Iso iso)
        {
            this.iso = iso;
            this.ExtractInfo();
        }

        public void Dispose()
        {
            this.xexHeader.Dispose();
            this.file = null;
            this.fileNameLength = 0;
            this.fileSize = 0;
            this.startingSectorOfFile = 0;
            this.offsetLeftSubTree = 0;
            this.offsetRightSubTree = 0;
        }

        private void ExtractInfo()
        {
            
            if (this.SearchForDefaultXeX()==-1)
            {
                throw new Exception("Default.xex/xeb was not found");
            }
            try
            {

                this.iso.Reader.EndianType = EndianType.BigEndian;
                this.xexHeader = new XeXHeader(this.iso.Reader);
                this.iso.Reader.EndianType = EndianType.LittleEndian;
            }
            catch (Exception exception)
            {
                throw new Exception("Error while reading XEX/XEB header", exception);
            }
        }

        private long SearchForDefaultXeX()
        {
            try
            {
                this.iso.Reader.BaseStream.Seek((long) ((long)((long)this.iso.IsoInfo.RootDirSector * (long)this.iso.IsoInfo.SectorSize) + (long)this.iso.IsoInfo.RootOffset), SeekOrigin.Begin);
                byte[] buffer = this.iso.Reader.ReadBytes((int) this.iso.IsoInfo.RootDirSize);
                MemoryStream s = new MemoryStream(buffer);
                MyBinaryReader reader = new MyBinaryReader(s);
                while (reader.BaseStream.Position < this.iso.IsoInfo.RootDirSize)
                {
                    ushort num = reader.ReadUInt16();
                    ushort num2 = reader.ReadUInt16();
                    if ((num != 0xffff) && (num2 != 0xffff))
                    {
                        uint num3 = reader.ReadUInt32();
                        uint num4 = reader.ReadUInt32();
                        uint num5 = reader.ReadByte();
                        byte count = reader.ReadByte();
                        string str = Encoding.ASCII.GetString(buffer, (int) reader.BaseStream.Position, count);
                        reader.BaseStream.Seek((long) count, SeekOrigin.Current);
                        long num7 = reader.BaseStream.Position % 4L;
                        if ((reader.BaseStream.Position % 4L) != 0L)
                        {
                            reader.BaseStream.Seek(4L - (reader.BaseStream.Position % 4L), SeekOrigin.Current);
                        }
                        if ((str.ToLower() == this.fileName.ToLower()) || (str.ToLower()==this.fileName2.ToLower()))
                        {
                            this.offsetLeftSubTree = num;
                            this.offsetRightSubTree = num2;
                            this.startingSectorOfFile = num3;
                            this.fileSize = num4;
                            this.fileNameLength = count;
                            this.iso.Reader.BaseStream.Seek(((long)this.iso.IsoInfo.RootOffset + (long)((long)this.startingSectorOfFile * (long)this.iso.IsoInfo.SectorSize)), SeekOrigin.Begin);
                            return iso.Reader.BaseStream.Position;

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error browsing ISO for default.xex/xeb", exception);
            }
            return -1;
        }

        public byte[] File
        {
            get
            {
                return this.file;
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        public byte FileNameLength
        {
            get
            {
                return this.fileNameLength;
            }
        }

        public uint FileSize
        {
            get
            {
                return this.fileSize;
            }
        }

        public ushort OffsetLeftSubTree
        {
            get
            {
                return this.offsetLeftSubTree;
            }
        }

        public ushort OffsetRightSubTree
        {
            get
            {
                return this.offsetRightSubTree;
            }
        }

        public uint StartingSectorOfFile
        {
            get
            {
                return this.startingSectorOfFile;
            }
        }

        public XeXHeader XeXHeader
        {
            get
            {
                return this.xexHeader;
            }
        }
    }
}

