using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib.Csv
{
    public class FieldFormatOption
    {
        public int TotalWidth;
        public bool AlignRight;
        public bool AlwaysQuoted;

        public FieldFormatOption() { }

        public FieldFormatOption(bool alwaysQuoted)
        {
            TotalWidth = 0;
            AlignRight = false;
            AlwaysQuoted = alwaysQuoted;
        }

        public FieldFormatOption(int totalWidth, bool alignRight, bool alwaysQuoted)
        {
            TotalWidth = totalWidth;
            AlignRight = alignRight;
            AlwaysQuoted = alwaysQuoted;
        }

        public static readonly FieldFormatOption Default = new FieldFormatOption();
    }
}
