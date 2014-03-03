using xk3yScanner.xkeyBrew.BLBinaryReader;
using System.IO;

namespace xk3yScanner.xkeyBrew.DvdReader
{
    internal class VTSM_PGCI_LU_MENU
    {
        private bool isGameMenu = false;
        private int sector;
        private int startByteVTSM_PGCI;

        public VTSM_PGCI_LU_MENU(byte[] array, int numberOfMenu)
        {
            MemoryStream s = new MemoryStream(array);
            MyBinaryReader reader = new MyBinaryReader(s);
            reader.Skip(8 + (numberOfMenu * 8));
            this.isGameMenu = reader.ReadByte() == 0;
            reader.Skip(3);
            this.startByteVTSM_PGCI = reader.ReadInt32B();
            reader.BaseStream.Seek((long) (this.startByteVTSM_PGCI + 0xe8), SeekOrigin.Begin);
            short num = reader.ReadInt16B();
            reader.BaseStream.Seek((long) (this.startByteVTSM_PGCI + num), SeekOrigin.Begin);
            reader.Skip(8);
            this.sector = reader.ReadInt32B();
        }

        public bool IsGameMenu
        {
            get
            {
                return this.isGameMenu;
            }
            set
            {
                this.isGameMenu = value;
            }
        }

        public int Sector
        {
            get
            {
                return this.sector;
            }
            set
            {
                this.sector = value;
            }
        }

        public int StartByteVTSM_PGCI
        {
            get
            {
                return this.startByteVTSM_PGCI;
            }
            set
            {
                this.startByteVTSM_PGCI = value;
            }
        }
    }
}

