namespace XkeyBrew.Utils.Shared
{
    using System;
    using System.Text;

    public static class ISharedMethods
    {
        public static string ConverByteToHex(byte[] value)
        {
            StringBuilder builder = new StringBuilder(value.Length * 2);
            foreach (byte num in value)
            {
                builder.Append(num.ToString("X02"));
            }
            return builder.ToString();
        }

        public static bool IsObjectEmptyOrNull(object value)
        {
            return (((value == null) || (value == DBNull.Value)) || string.IsNullOrEmpty((value == null) ? null : value.ToString()));
        }
    }
}

