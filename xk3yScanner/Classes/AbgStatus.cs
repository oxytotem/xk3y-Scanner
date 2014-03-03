using System;
using System.Collections.Generic;
using System.Text;

namespace xk3yScanner.Classes
{
    public class AbgStatus
    {
        public bool Error { get; set; }
        public string ErrorString { get; set; }
        public LTVersion LTouchVersion { get; set; }
        public CRCInfo XEXCRC { get; set; }
        public CRCInfo VideoCRC { get; set; }
        public CRCInfo V0CRC { get; set; }
        public CRCInfo V1CRC { get; set; }
        public CRCInfo PFICRC { get; set; }
        public CRCInfo DMICRC { get; set; }
        public CRCInfo SSCRC { get; set; }
        public CRCInfo GameCRC { get; set; }

        public uint RAWSS { get; set; }
        public ulong RegionCode { get; set; }
        public string MediaId { get; set; }
        public bool AnyDVDCorruption { get; set; }
        public string MinKernel { get; set; }
        public DateTime LastCheck { get; set; }
        public XGDVersion DiskVersion { get; set; }
        public string Wave { get; set; }
        public ulong Size { get; set; }

        public string GetRegions()
        {
            List<string> rgn=new List<string>();
            StringBuilder bld=new StringBuilder();
            if (RegionCode == 0xFFFFFFFF)
            {
                rgn.Add("Region Free");
            }
            else
            {
                if ((RegionCode & 0x00FF0000) == 0x00FF0000)
                    rgn.Add("PAL");
                else if ((RegionCode & 0x00FF0000) == 0x00FE0000)
                    rgn.Add("PAL (Excludes AUS/NZ)");
                else if ((RegionCode & 0x00FF0000) == 0x00010000)
                    rgn.Add("PAL (AUS/NZ Only)");
                else if((RegionCode & 0x00FF0000) != 0x00000000)
                    rgn.Add(string.Format("PAL (Unknown code: {0:2X})",RegionCode>>16&0xFF));
                if ((RegionCode & 0x0000FF00) == 0x0000FF00)
                     rgn.Add("NTSC/J");
                else if ((RegionCode & 0x0000FF00) == 0x0000FD00)
                     rgn.Add("NTSC/J (Excludes China)");
                else if ((RegionCode & 0x0000FF00) == 0x0000FE00)
                    rgn.Add("NTSC/J (Excludes Japan)");
                else if ((RegionCode & 0x0000FF00) == 0x0000FC00)
                    rgn.Add("NTSC/J (Excludes Japan and China)");
                else if ((RegionCode & 0x0000FF00) == 0x00000100)
                     rgn.Add("NTSC/J (Japan Only)");
                else if ((RegionCode & 0x0000FF00) == 0x00000200)
                    rgn.Add("NTSC/J (China Only)");
                else if ((RegionCode & 0x0000FF00) == 0x00000300)
                    rgn.Add("NTSC/J (Japan and China Only)");
                else if((RegionCode & 0x0000FF00) != 0x00000000)
                    rgn.Add(string.Format("NTSC/J (Unknown code: {0:2X})",RegionCode>>8 & 0xFF));
                if ((RegionCode & 0x000000FF) == 0x000000FF)
                    rgn.Add("NTSC/U");
                else if ((RegionCode & 0x000000FF) != 0x00000000)
                    rgn.Add(string.Format("NTSC/U (Unknown code: {0:2X})", RegionCode & 0xFF));
                if ((RegionCode & 0xFF000000) == 0xFF000000)
                    rgn.Add("Other");
                else if ((RegionCode & 0xFF000000) != 0x00000000)
                    rgn.Add(string.Format("Other (Unknown code: {0:2X})", RegionCode >> 24 & 0xFF));
            }
            for(int x=0;x<rgn.Count;x++)
            {
                if (x > 0)
                    bld.Append(", ");
                bld.Append(rgn[x]);
            }
            return bld.ToString();
        }

        public string Serialize()
        {
            StringBuilder builder=new StringBuilder();
            AddVar(builder," Error",Error.ToString(),true);
            AddVar(builder," ErrorString",ErrorString);
            AddVar(builder," LTouchVersion",((int)LTouchVersion).ToString());
            if (XEXCRC!=null)
                AddVar(builder, " XEXCRC", ((int)XEXCRC.Status).ToString()+"|"+XEXCRC.Crc.ToString("X8"));
            if (VideoCRC != null)
                AddVar(builder, " VideoCRC", ((int)VideoCRC.Status).ToString() + "|" + VideoCRC.Crc.ToString("X8"));
            if (V0CRC != null)
                AddVar(builder, " V0CRC", ((int)V0CRC.Status).ToString() + "|" + V0CRC.Crc.ToString("X8"));
            if (V1CRC != null)
                AddVar(builder, " V1CRC", ((int)V1CRC.Status).ToString() + "|" + V1CRC.Crc.ToString("X8"));
            if (PFICRC != null)
                AddVar(builder, " PFICRC", ((int)PFICRC.Status).ToString() + "|" + PFICRC.Crc.ToString("X8"));
            if (DMICRC != null)
                AddVar(builder, " DMICRC", ((int)DMICRC.Status).ToString() + "|" + DMICRC.Crc.ToString("X8"));
            if (SSCRC != null)
                AddVar(builder, " SSCRC", ((int)SSCRC.Status).ToString() + "|" + SSCRC.Crc.ToString("X8"));
            if (GameCRC != null)
                AddVar(builder, " GameCRC", ((int)GameCRC.Status).ToString() + "|" + GameCRC.Crc.ToString("X8"));
            AddVar(builder," RAWSS",RAWSS.ToString("X8"));
            AddVar(builder," RegionCode",RegionCode.ToString("X8"));
            AddVar(builder," MediaId",MediaId);
            AddVar(builder," AnyDVDCorruption", AnyDVDCorruption.ToString());
            AddVar(builder," MinKernel",MinKernel);
            AddVar(builder," LastCheck",LastCheck.ToString());
            AddVar(builder," Wave",Wave);
            AddVar(builder," Size",Size.ToString());
            return builder.ToString();
        }
        private void AddVar(StringBuilder builder, string key, string value, bool first=false)
        {
            if (!first)
                builder.Append(";");
            builder.Append(key);
            builder.Append("=");
            builder.Append(value.Replace(";", "~").Replace("=", "|"));
        }
        public static AbgStatus Deserialize(string sta)
        {
            AbgStatus abgx=new AbgStatus();
            abgx.SSCRC = new AbgStatus.CRCInfo();
            abgx.SSCRC.Status = AbgStatus.CRCStatus.Bad;
            abgx.DMICRC = new AbgStatus.CRCInfo();
            abgx.DMICRC.Status = AbgStatus.CRCStatus.Bad;
            abgx.PFICRC = new AbgStatus.CRCInfo();
            abgx.PFICRC.Status = AbgStatus.CRCStatus.Bad;
            abgx.XEXCRC = new AbgStatus.CRCInfo();
            abgx.XEXCRC.Status = AbgStatus.CRCStatus.Bad;
            abgx.VideoCRC = new AbgStatus.CRCInfo();
            abgx.VideoCRC.Status = AbgStatus.CRCStatus.Bad;
            abgx.V0CRC = new AbgStatus.CRCInfo();
            abgx.V0CRC.Status = AbgStatus.CRCStatus.Bad;
            abgx.V1CRC = new AbgStatus.CRCInfo();
            abgx.V1CRC.Status = AbgStatus.CRCStatus.Bad;
            abgx.GameCRC = new AbgStatus.CRCInfo();
            abgx.GameCRC.Status = AbgStatus.CRCStatus.Unverified;
            string[] strings = sta.Split(';');
            foreach (string s in strings)
            {
                string[] st = s.Split('=');
                st[0] = st[0].Trim();
                switch (st[0])
                {
                    case "Error":
                        abgx.Error = Convert.ToBoolean(st[1]);
                        break;
                    case "ErrorString":
                        abgx.ErrorString = st[1].Replace("~",";").Replace("|","=");
                        break;
                    case "LTouchVersion":
                        abgx.LTouchVersion = (LTVersion) int.Parse(st[1]);
                        break;
                    case "XEXCRC":
                        string[] k1 = st[1].Split('|');
                        abgx.XEXCRC.Status = (CRCStatus) int.Parse(k1[0]);
                        abgx.XEXCRC.Crc = Convert.ToUInt32(k1[1], 16);
                        break;
                    case "VideoCRC":
                        string[] k2 = st[1].Split('|');
                        abgx.VideoCRC.Status = (CRCStatus)int.Parse(k2[0]);
                        abgx.VideoCRC.Crc = Convert.ToUInt32(k2[1], 16);
                        break;
                    case "V0CRC":
                        string[] k3 = st[1].Split('|');
                        abgx.V0CRC.Status = (CRCStatus)int.Parse(k3[0]);
                        abgx.V0CRC.Crc = Convert.ToUInt32(k3[1], 16);
                        break;
                    case "V1CRC":
                        string[] k4 = st[1].Split('|');
                        abgx.V1CRC.Status = (CRCStatus)int.Parse(k4[0]);
                        abgx.V1CRC.Crc = Convert.ToUInt32(k4[1], 16);
                        break;
                    case "PFICRC":
                        string[] k5 = st[1].Split('|');
                        abgx.PFICRC.Status = (CRCStatus)int.Parse(k5[0]);
                        abgx.PFICRC.Crc = Convert.ToUInt32(k5[1], 16);
                        break;
                    case "DMICRC":
                        string[] k6 = st[1].Split('|');
                        abgx.DMICRC.Status = (CRCStatus)int.Parse(k6[0]);
                        abgx.DMICRC.Crc = Convert.ToUInt32(k6[1], 16);
                        break;
                    case "SSCRC":
                        string[] k7 = st[1].Split('|');
                        abgx.SSCRC.Status = (CRCStatus)int.Parse(k7[0]);
                        abgx.SSCRC.Crc = Convert.ToUInt32(k7[1], 16);
                        break;
                    case "GameCRC":
                        string[] k8 = st[1].Split('|');
                        abgx.GameCRC.Status = (CRCStatus)int.Parse(k8[0]);
                        abgx.GameCRC.Crc = Convert.ToUInt32(k8[1], 16);
                        break;
                    case "Size":
                        abgx.Size = Convert.ToUInt64(st[1]);
                        break;
                    case "RAWSS":
                        abgx.RAWSS= Convert.ToUInt32(st[1], 16);
                        break;
                    case "RegionCode":
                        abgx.RegionCode=Convert.ToUInt32(st[1], 16);
                        break;
                    case "MediaId":
                        abgx.MediaId=st[1];
                        break;
                    case "AnyDVDCorruption":
                        abgx.AnyDVDCorruption=Convert.ToBoolean(st[1]);
                        break;
                    case "MinKernel":
                        abgx.MinKernel=st[1];
                        break;
                    case "DiskVersion":
                        abgx.DiskVersion=(XGDVersion)int.Parse(st[1]);
                        break;
                    case "LastCheck":
                        abgx.LastCheck=Convert.ToDateTime(st[1]);
                        break;
                    case "Wave":
                        abgx.Wave=st[1];
                        break;
                }
            }
            return abgx;
        }

        public Status Stats
        { 
            get
            {
                if ((Error) || (AnyDVDCorruption) || (XEXCRC.Status==CRCStatus.Bad)
                    || (VideoCRC.Status==CRCStatus.Bad) || (V0CRC.Status==CRCStatus.Bad) ||(V1CRC.Status==CRCStatus.Bad)
                    || (PFICRC.Status==CRCStatus.Bad) || (DMICRC.Status==CRCStatus.Bad) || (SSCRC.Status==CRCStatus.Bad)
                    || (GameCRC.Status==CRCStatus.Bad))
                    return Status.Bad;
                if ((XEXCRC.Status==CRCStatus.Unverified)
                    || (VideoCRC.Status==CRCStatus.Unverified) || (V0CRC.Status==CRCStatus.Unverified) ||(V1CRC.Status==CRCStatus.Unverified)
                    || (PFICRC.Status==CRCStatus.Unverified) || (DMICRC.Status==CRCStatus.Unverified) || (SSCRC.Status==CRCStatus.Unverified)
                    || (GameCRC.Status==CRCStatus.Unverified))
                    return Status.Warning;
                return Status.Ok;
            }
        }

        public enum LTVersion
        {
            LT10,
            LT20
        }
        public enum XGDVersion
        {
            GDF,
            XGD,
            AP25,
            XGD3
        }
        public class CRCInfo
        {
            public CRCStatus Status { get; set; }
            public uint Crc { get; set; }
        }
        public enum CRCStatus
        {
            Verified,
            Unverified,
            Bad
        }
        public enum Status
        {
            Ok,
            Warning,
            Bad
        }
    }
}
