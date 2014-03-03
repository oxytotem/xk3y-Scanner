using xk3yScanner.xkeyBrew.BLBinaryReader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace xk3yScanner.xkeyBrew.DvdReader
{
    public class DvdMenuReadSectors
    {
        private MyBinaryReader br = null;
        private FileStream fs = null;
        private int pathOffset = 0x84;
        private int sectorSize = 0x800;

        public DvdMenuReadSectors(string path)
        {
            try
            {
                this.fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                this.br = new MyBinaryReader(this.fs);
            }
            catch (Exception exception)
            {
                throw new Exception("Error opening iso file", exception);
            }
        }

        public List<byte[]> FillListWithMenuSectors()
        {
            List<byte[]> list = new List<byte[]>();
            Dictionary<string, byte[]> filesWithSectors = this.GetFilesWithSectors();
            foreach (KeyValuePair<string, byte[]> pair in filesWithSectors)
            {
                if (pair.Key.ToUpper().Contains("_0.IFO"))
                {
                    int num = BitConverter.ToInt32(pair.Value, 0);
                    long num2 = num * this.sectorSize;
                    int sectorOfVtsVob = this.GetSectorOfVtsVob(pair.Key);
                    if ((num > 0) && (sectorOfVtsVob > 0))
                    {
                        this.br.BaseStream.Seek(num2 + 0xd0L, SeekOrigin.Begin);
                        int num4 = this.br.ReadInt32B();
                        long offset = num2 + (num4 * this.sectorSize);
                        this.br.BaseStream.Seek(offset, SeekOrigin.Begin);
                        this.br.ReadInt32B();
                        int count = this.br.ReadInt32B();
                        long num7 = offset + count;
                        this.br.BaseStream.Seek(offset, SeekOrigin.Begin);
                        VTSM_PGCI_UT vtsm_pgci_ut = new VTSM_PGCI_UT(this.br.ReadBytes(count));
                        for (int i = 0; i < vtsm_pgci_ut.Menus.Count; i++)
                        {
                            if (((i % 2) == 1) && vtsm_pgci_ut.Menus[i].IsGameMenu)
                            {
                                int num9 = sectorOfVtsVob + vtsm_pgci_ut.Menus[i].Sector;
                                list.Add(BitConverter.GetBytes(num9));
                            }
                        }
                    }
                }
            }
            return list;
        }

        private Dictionary<string, byte[]> GetFilesFromVideoTS(int videoTSLBA)
        {
            Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
            try
            {
                long offset = this.sectorSize * videoTSLBA;
                this.br.BaseStream.Seek(offset, SeekOrigin.Begin);
                int sectorSize = this.sectorSize;
                while (this.br.BaseStream.Position < (offset + sectorSize))
                {
                    if (this.br.ReadByte() > 0)
                    {
                        byte num4 = this.br.ReadByte();
                        byte[] buffer = this.br.ReadBytes(4);
                        int num5 = BitConverter.ToInt32(buffer, 0);
                        this.br.BaseStream.Seek(4L, SeekOrigin.Current);
                        int num6 = this.br.ReadInt32();
                        this.br.BaseStream.Seek(4L, SeekOrigin.Current);
                        string str = Encoding.ASCII.GetString(this.br.ReadBytes(7));
                        byte num7 = this.br.ReadByte();
                        byte num8 = this.br.ReadByte();
                        byte num9 = this.br.ReadByte();
                        short num10 = this.br.ReadInt16();
                        this.br.BaseStream.Seek(2L, SeekOrigin.Current);
                        byte count = this.br.ReadByte();
                        byte[] bytes = this.br.ReadBytes(count);
                        string str2 = Encoding.ASCII.GetString(bytes);
                        if ((count == 1) && (bytes[0] == 0))
                        {
                            str2 = ".";
                            sectorSize = num6;
                        }
                        else if ((count == 1) && (bytes[0] == 1))
                        {
                            str2 = "..";
                        }
                        if ((count % 2) == 0)
                        {
                            this.br.BaseStream.Seek(1L, SeekOrigin.Current);
                        }
                        dictionary.Add(str2.Replace(";1", ""), buffer);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error listing VIDEO_TS", exception);
            }
            return dictionary;
        }

        public Dictionary<string, byte[]> GetFilesWithSectors()
        {
            return this.GetFilesWithSectors(false);
        }

        public Dictionary<string, byte[]> GetFilesWithSectors(bool showLog)
        {
            Dictionary<string, byte[]> input = null;
            int lBAOfVideoTS = this.GetLBAOfVideoTS();
            if (lBAOfVideoTS > 0)
            {
                input = this.GetFilesFromVideoTS(lBAOfVideoTS);
                if (showLog)
                {
                    Form form = new Form {
                        Size = new Size(800, 600)
                    };
                    TextBox box = new TextBox {
                        Multiline = true,
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        Text = this.PrepareLog(input),
                        Font = new Font("Courier New", 9f)
                    };
                    form.Controls.Add(box);
                    form.Show();
                }
            }
            return input;
        }

        private int GetLBAOfVideoTS()
        {
            try
            {
                this.br.BaseStream.Seek((long)((0x10 * (long)this.sectorSize) + (long)this.pathOffset), SeekOrigin.Begin);
                int num2 = this.br.ReadInt32();
                this.br.BaseStream.Seek(4L, SeekOrigin.Current);
                int num3 = this.br.ReadInt32();
                long offset = this.sectorSize * num3;
                this.br.BaseStream.Seek(offset, SeekOrigin.Begin);
                while (this.br.BaseStream.Position < (offset + num2))
                {
                    byte count = this.br.ReadByte();
                    this.br.BaseStream.Seek(1L, SeekOrigin.Current);
                    int num6 = this.br.ReadInt32();
                    short num7 = this.br.ReadInt16();
                    string str = Encoding.ASCII.GetString(this.br.ReadBytes(count));
                    if ((count % 2) == 1)
                    {
                        this.br.BaseStream.Seek(1L, SeekOrigin.Current);
                    }
                    if (str.ToUpper() == "VIDEO_TS")
                    {
                        return num6;
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error searching for VIDEO_TS", exception);
            }
            return 0;
        }

        private int GetSectorOfVtsVob(string ifoName)
        {
            Dictionary<string, byte[]> filesWithSectors = this.GetFilesWithSectors();
            foreach (KeyValuePair<string, byte[]> pair in filesWithSectors)
            {
                if (pair.Key == ifoName.Replace(".IFO", ".VOB"))
                {
                    return BitConverter.ToInt32(pair.Value, 0);
                }
            }
            return -1;
        }

        private string PrepareLog(Dictionary<string, byte[]> input)
        {
            string str = "";
            str = (str + "File name".PadRight(20)) + "Sector".PadRight(20) + "\r\n";
            foreach (KeyValuePair<string, byte[]> pair in input)
            {
                str = str + pair.Key.PadRight(20);
                str = str + BitConverter.ToInt32(pair.Value, 0).ToString().PadRight(20);
                str = str + "\r\n";
            }
            return str;
        }
    }
}

