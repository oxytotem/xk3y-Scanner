using xk3yScanner.xkeyBrew.BLBinaryReader;
using System;
using System.IO;
using System.Text;
using XkeyBrew.Utils.Shared;

namespace xk3yScanner.xkeyBrew.IsoGameReader
{
    public class Iso : IDisposable
    {
        private DefaultXeX defaultXeX;
        private FileStream file;
        private string filePath;
        private XkeyBrew.Utils.IsoGameReader.IsoInfo isoInfo;
        private IsoType isoType;
        private MyBinaryReader reader;

        public Iso(string path) : this(path, true)
        {
        }

        public Iso(string path, bool readData)
        {
            this.filePath = path;
            if (readData)
            {
                this.ReadGameInfo();
            }
        }

        private void CheckIfXbox360Iso()
        {
            if (this.isoType == IsoType.XSF)
            {
                throw new Exception("Provided iso is not XBOX360 iso");
            }
        }

        private bool CheckPath()
        {
            return (!ISharedMethods.IsObjectEmptyOrNull(this.filePath) && System.IO.File.Exists(this.filePath));
        }

        private void CloseFile()
        {
            this.filePath = string.Empty;
            this.reader.Close();
            this.file.Close();
            this.file.Dispose();
        }

        public void Dispose()
        {
            this.CloseFile();
            this.defaultXeX.Dispose();
            this.isoType = IsoType.GDF;
            this.isoInfo = new XkeyBrew.Utils.IsoGameReader.IsoInfo();
        }

        private void OpenIsoFile()
        {
            if (!this.CheckPath())
            {
                throw new Exception("Provided path is not correct");
            }
            try
            {
                this.file = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
                this.reader = new MyBinaryReader(this.file);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void ReadDefaultXex()
        {
            this.defaultXeX = new DefaultXeX(this);
        }

        private void ReadGameInfo()
        {
            this.OpenIsoFile();
            this.ReadIsoData();
            this.CheckIfXbox360Iso();
            this.ReadDefaultXex();
            this.CloseFile();
        }

        private void ReadIsoData()
        {
            try
            {
                this.isoInfo = new XkeyBrew.Utils.IsoGameReader.IsoInfo();
                this.isoInfo.SectorSize = 0x800;
                this.reader.BaseStream.Seek((long) (0x20 * (long)this.isoInfo.SectorSize), SeekOrigin.Begin);
                if (Encoding.ASCII.GetString(this.reader.ReadBytes(20)) == "MICROSOFT*XBOX*MEDIA")
                {
                    this.isoType = IsoType.XSF;
                    this.isoInfo.RootOffset = (uint) this.isoType;
                }
                else
                {
                    this.file.Seek((long)((0x20 * (long)this.isoInfo.SectorSize) + (long)IsoType.GDF), SeekOrigin.Begin);
                    if (Encoding.ASCII.GetString(this.reader.ReadBytes(20)) == "MICROSOFT*XBOX*MEDIA")
                    {
                        this.isoType = IsoType.GDF;
                        this.isoInfo.RootOffset = (uint)this.isoType;
                    }
                    else
                    {
                        this.file.Seek((long)((0x20 * (long)this.isoInfo.SectorSize) + (long)IsoType.XGD3), SeekOrigin.Begin);
                        if (Encoding.ASCII.GetString(this.reader.ReadBytes(20)) == "MICROSOFT*XBOX*MEDIA")
                        {
                            this.isoType = IsoType.XGD3;
                            this.isoInfo.RootOffset = (uint) this.isoType;
                        }
                        else
                        {
                            this.file.Seek((long)((0x20 * (long)this.isoInfo.SectorSize) + (long)IsoType.XBOX1), SeekOrigin.Begin);
                            if (Encoding.ASCII.GetString(this.reader.ReadBytes(20)) == "MICROSOFT*XBOX*MEDIA")
                            {
                                this.isoType = IsoType.XBOX1;
                                this.isoInfo.RootOffset = (uint) this.isoType;
                            }
                            else
                            {
                                throw new Exception("Invalid Iso Type");
                            }
                        }
                    }
                }
                this.reader.BaseStream.Seek((long)((0x20 * (long)this.isoInfo.SectorSize) + (long)this.isoInfo.RootOffset), SeekOrigin.Begin);
                this.isoInfo.Identifier = this.reader.ReadBytes(20);
                this.isoInfo.RootDirSector = this.reader.ReadUInt32();
                this.isoInfo.RootDirSize = this.reader.ReadUInt32();
                this.isoInfo.ImageCreationTime = this.reader.ReadBytes(8);
                this.isoInfo.VolumeSize = (ulong) (this.reader.BaseStream.Length - this.isoInfo.RootOffset);
                this.isoInfo.VolumeSectors = (uint) (this.isoInfo.VolumeSize / ((ulong) this.isoInfo.SectorSize));
            }
            catch (Exception exception)
            {
                throw new Exception("Cannot read ISO info", exception);
            }
        }

        public void SaveRootFolderTree(string path)
        {
            this.OpenIsoFile();
            this.ReadIsoData();
            this.Reader.BaseStream.Seek((long)(((long)this.IsoInfo.RootDirSector * (long)this.IsoInfo.SectorSize) + (long)this.IsoInfo.RootOffset), SeekOrigin.Begin);
            byte[] bytes = this.Reader.ReadBytes((int) this.IsoInfo.RootDirSize);
            System.IO.File.WriteAllBytes(path, bytes);
            this.CloseFile();
        }

        public DefaultXeX DefaultXeX
        {
            get
            {
                return this.defaultXeX;
            }
        }

        public FileStream File
        {
            get
            {
                return this.file;
            }
        }

        public string FilePath
        {
            get
            {
                return this.filePath;
            }
        }

        public XkeyBrew.Utils.IsoGameReader.IsoInfo IsoInfo
        {
            get
            {
                return this.isoInfo;
            }
        }

        public IsoType IsoType
        {
            get
            {
                return this.isoType;
            }
        }

        public MyBinaryReader Reader
        {
            get
            {
                return this.reader;
            }
        }
    }
}

