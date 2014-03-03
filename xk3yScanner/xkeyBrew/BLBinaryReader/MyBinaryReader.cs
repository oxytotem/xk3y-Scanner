using System;
using System.IO;

namespace xk3yScanner.xkeyBrew.BLBinaryReader
{
    public class MyBinaryReader : BinaryReader
    {
        private EndianType endianType;

        public MyBinaryReader(Stream s) : this(s, EndianType.LittleEndian)
        {
        }

        public MyBinaryReader(Stream s, EndianType endianType) : base(s)
        {
            this.endianType = EndianType.LittleEndian;
            this.endianType = endianType;
        }

        private object convertToBigEndian(Type type)
        {
            byte[] array = null;
            switch (type.FullName)
            {
                case "System.Int16":
                    array = BitConverter.GetBytes(base.ReadInt16());
                    break;

                case "System.Int32":
                    array = BitConverter.GetBytes(base.ReadInt32());
                    break;

                case "System.Int64":
                    array = BitConverter.GetBytes(base.ReadInt64());
                    break;

                case "System.UInt16":
                    array = BitConverter.GetBytes(base.ReadUInt16());
                    break;

                case "System.UInt32":
                    array = BitConverter.GetBytes(base.ReadUInt32());
                    break;

                case "System.UInt64":
                    array = BitConverter.GetBytes(base.ReadUInt64());
                    break;

                case "System.Single":
                    array = BitConverter.GetBytes(base.ReadSingle());
                    break;

                case "System.Double":
                    array = BitConverter.GetBytes(base.ReadDouble());
                    break;
            }
            Array.Reverse(array);
            switch (type.FullName)
            {
                case "System.Int16":
                    return BitConverter.ToInt16(array, 0);

                case "System.Int32":
                    return BitConverter.ToInt32(array, 0);

                case "System.Int64":
                    return BitConverter.ToInt64(array, 0);

                case "System.UInt16":
                    return BitConverter.ToUInt16(array, 0);

                case "System.UInt32":
                    return BitConverter.ToUInt32(array, 0);

                case "System.UInt64":
                    return BitConverter.ToUInt64(array, 0);

                case "System.Double":
                    return BitConverter.ToDouble(array, 0);

                case "System.Single":
                    return BitConverter.ToSingle(array, 0);
            }
            return array;
        }

        public override double ReadDouble()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (double) this.convertToBigEndian(Type.GetType("System.Double"));
            }
            return base.ReadDouble();
        }

        public double ReadDoubleB()
        {
            return (double) this.convertToBigEndian(Type.GetType("System.Double"));
        }

        public override short ReadInt16()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (short) this.convertToBigEndian(Type.GetType("System.Int16"));
            }
            return base.ReadInt16();
        }

        public short ReadInt16B()
        {
            return (short) this.convertToBigEndian(Type.GetType("System.Int16"));
        }

        public override int ReadInt32()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (int) this.convertToBigEndian(Type.GetType("System.Int32"));
            }
            return base.ReadInt32();
        }

        public int ReadInt32B()
        {
            return (int) this.convertToBigEndian(Type.GetType("System.Int32"));
        }

        public override long ReadInt64()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (long) this.convertToBigEndian(Type.GetType("System.Int64"));
            }
            return base.ReadInt64();
        }

        public long ReadInt64B()
        {
            return (long) this.convertToBigEndian(Type.GetType("System.Int64"));
        }

        public override float ReadSingle()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (float) this.convertToBigEndian(Type.GetType("System.Single"));
            }
            return base.ReadSingle();
        }

        public float ReadSingleB()
        {
            return (float) this.convertToBigEndian(Type.GetType("System.Single"));
        }

        public override ushort ReadUInt16()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (ushort) this.convertToBigEndian(Type.GetType("System.UInt16"));
            }
            return base.ReadUInt16();
        }

        public ushort ReadUInt16B()
        {
            return (ushort) this.convertToBigEndian(Type.GetType("System.UInt16"));
        }

        public override uint ReadUInt32()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (uint) this.convertToBigEndian(Type.GetType("System.UInt32"));
            }
            return base.ReadUInt32();
        }

        public uint ReadUInt32B()
        {
            return (uint) this.convertToBigEndian(Type.GetType("System.UInt32"));
        }

        public override ulong ReadUInt64()
        {
            if (this.endianType == EndianType.BigEndian)
            {
                return (ulong) this.convertToBigEndian(Type.GetType("System.UInt64"));
            }
            return base.ReadUInt64();
        }

        public ulong ReadUInt64B()
        {
            return (ulong) this.convertToBigEndian(Type.GetType("System.UInt64"));
        }

        public void Skip(int length)
        {
            for (int i = 0; i < length; i++)
            {
                base.ReadByte();
            }
        }

        public EndianType EndianType
        {
            get
            {
                return this.endianType;
            }
            set
            {
                this.endianType = value;
            }
        }
    }
}

