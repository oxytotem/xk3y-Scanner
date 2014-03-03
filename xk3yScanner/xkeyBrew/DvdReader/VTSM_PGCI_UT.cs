using xk3yScanner.xkeyBrew.BLBinaryReader;
using System;
using System.IO;

namespace xk3yScanner.xkeyBrew.DvdReader
{
    internal class VTSM_PGCI_UT
    {
        private int endByteOfVTSM_PGCI_LU;
        private VTSM_PGCI_LU_MENUS menus;
        private int numberOfVTSM_PGCI_LU;

        public VTSM_PGCI_UT(byte[] array)
        {
            try
            {
                MemoryStream s = new MemoryStream(array);
                MyBinaryReader reader = new MyBinaryReader(s);
                this.numberOfVTSM_PGCI_LU = reader.ReadInt16B();
                reader.Skip(2);
                this.endByteOfVTSM_PGCI_LU = reader.ReadInt32B();
                reader.Skip(4);
                int num = reader.ReadInt32B();
                reader.BaseStream.Seek((long) num, SeekOrigin.Begin);
                this.menus = new VTSM_PGCI_LU_MENUS(reader.ReadBytes(array.Length - num));
            }
            catch (Exception exception)
            {
                throw new Exception("Error searching sectors in DVD MENU", exception);
            }
        }

        public int EndByteOfVTSM_PGCI_LU
        {
            get
            {
                return this.endByteOfVTSM_PGCI_LU;
            }
            set
            {
                this.endByteOfVTSM_PGCI_LU = value;
            }
        }

        public VTSM_PGCI_LU_MENUS Menus
        {
            get
            {
                return this.menus;
            }
            set
            {
                this.menus = value;
            }
        }

        public int NumberOfVTSM_PGCI_LU
        {
            get
            {
                return this.numberOfVTSM_PGCI_LU;
            }
            set
            {
                this.numberOfVTSM_PGCI_LU = value;
            }
        }
    }
}

