using xk3yScanner.xkeyBrew.BLBinaryReader;
using System.Collections.Generic;
using System.IO;

namespace xk3yScanner.xkeyBrew.DvdReader
{
    internal class VTSM_PGCI_LU_MENUS : List<VTSM_PGCI_LU_MENU>
    {
        private int endByteOfLU_MENUS;
        private short numberOfMenus;

        public VTSM_PGCI_LU_MENUS(byte[] array)
        {
            MemoryStream s = new MemoryStream(array);
            MyBinaryReader reader = new MyBinaryReader(s);
            this.numberOfMenus = reader.ReadInt16B();
            reader.Skip(2);
            this.endByteOfLU_MENUS = reader.ReadInt32B();
            for (int i = 0; i < this.numberOfMenus; i++)
            {
                VTSM_PGCI_LU_MENU item = new VTSM_PGCI_LU_MENU(array, i);
                base.Add(item);
            }
        }

        public int EndByteOfLU_MENUS
        {
            get
            {
                return this.endByteOfLU_MENUS;
            }
            set
            {
                this.endByteOfLU_MENUS = value;
            }
        }

        public short NumberOfMenus
        {
            get
            {
                return this.numberOfMenus;
            }
            set
            {
                this.numberOfMenus = value;
            }
        }
    }
}

