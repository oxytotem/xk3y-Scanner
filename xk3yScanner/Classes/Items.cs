using System;
using System.Globalization;
using System.Windows.Forms;

namespace xk3yScanner.Classes
{
    public class Items
    {
        public enum Types
        {
            Title,
            TitleId,
            Genres,
            Date,
            Type,
            Disc,
            Developer,
            Publisher,
            WebPopulated,
            AbgxStatus,
            IsActive,
            Regions,
            XEXCrc,
            GameCrc,
            MediaId,
            FullMediaId,
            XKeyId,
            FullPath,
            PartialPath,
            DateDay,
            DateMonth,
            DateYear,
            NumberOfDiscs,
            Size,
            RegionCode,
            Trailer,
            FirstGenre,
            OptDisc,
            OptStrDisc,
            Wave
        }

        public static string Template(Game g, string template)
        {
            string org = template;
            foreach (Types t in Enum.GetValues(typeof(Types)))
            {
                string nam="{"+Enum.GetName(typeof (Types), t)+"}";
                org = org.Replace(nam, FromType(t, g));
            }
            return org.Replace("  ", " ").Replace("  "," ").Trim();
        }
        public static string FromType(Types t, Game g)
        {


                switch (t)
                {
                    case Types.Title:
                        return g.Title;
                    case Types.TitleId:
                        return g.TitleId;
                    case Types.Genres:
                        return g.Genre;
                    case Types.FirstGenre:
                        if (string.IsNullOrEmpty(g.Genre))
                            return string.Empty;
                        string[] s = g.Genre.Split(',');
                        if (s.Length > 0)
                            return s[0].Trim();
                        return string.Empty;
                    case Types.Date:
                        return g.Date.ToString(CultureInfo.CurrentCulture);
                    case Types.Type:
                        return g.Type;
                    case Types.Disc:
                        return g.Disc == 9999 ? "Expansion" : g.Disc.ToString(CultureInfo.CurrentCulture);
                    case Types.OptDisc:
                        if (g.NumberOfDiscs == 1)
                        {
                            return g.Disc == 9999 ? "Expansion" : string.Empty;
                        }
                        return g.Disc == 9999 ? "Expansion" : g.Disc.ToString(CultureInfo.CurrentCulture);
                    case Types.OptStrDisc:
                        if (g.NumberOfDiscs == 1)
                        {
                            return g.Disc == 9999 ? "Expansion" : string.Empty;
                        }
                        return g.Disc == 9999 ? "Expansion" : "Disc " + g.Disc.ToString(CultureInfo.CurrentCulture);
                    case Types.Developer:
                        return g.Developer;
                    case Types.Publisher:
                        return g.Publisher;
                    case Types.WebPopulated:
                        return g.WebPopulated ? "1" : "0";
                    case Types.AbgxStatus:
                        switch (g.AbgxStats)
                        {
                            case 0:
                                return "Not Checked";
                            case 1:
                                return "Ok";
                            case 2:
                                return "Unverified";
                            default:
                                return "Error";
                        }
                    case Types.IsActive:
                        return g.Active ? "1" : "0";
                    case Types.Regions:
                        return g.Regions.Replace("/", "").Replace(", ", "-").Replace("Region Free", "RF");
                    case Types.XEXCrc:
                        return g.Abgx != null ? g.Abgx.XEXCRC.Crc.ToString("X8") : string.Empty;
                    case Types.GameCrc:
                        return g.Abgx != null ? g.Abgx.GameCRC.Crc.ToString("X8") : string.Empty;
                    case Types.MediaId:
                        return g.MediaId.Substring(24, 8).ToUpper();
                    case Types.FullMediaId:
                        return g.MediaId.ToUpper();
                    case Types.XKeyId:
                        return g.ID;
                    case Types.FullPath:
                        return g.FullIsoPath;
                    case Types.PartialPath:
                        return g.FullIsoPath.Substring(g.GameDirectoy.Length + 1);
                    case Types.DateDay:
                        return g.Date.Day.ToString(CultureInfo.CurrentCulture);
                    case Types.DateMonth:
                        return g.Date.Month.ToString(CultureInfo.CurrentCulture);
                    case Types.DateYear:
                        return g.Date.Year.ToString(CultureInfo.CurrentCulture);
                    case Types.NumberOfDiscs:
                        return g.NumberOfDiscs < 1
                            ? "1"
                            : (g.NumberOfDiscs == 9999) ? "1" : g.NumberOfDiscs.ToString(CultureInfo.CurrentCulture);
                    case Types.Size:
                        return g.Size.ToString(CultureInfo.CurrentCulture);
                    case Types.RegionCode:
                        return g.RegionCode;
                    case Types.Trailer:
                        return g.Trailer;
                    case Types.Wave:
                        return g.Abgx != null ? g.Abgx.Wave : string.Empty;
                }
                return string.Empty;

           
        }
    }
}
