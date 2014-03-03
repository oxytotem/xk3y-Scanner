using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace xk3yScanner.Classes.Processors.Helpers
{
    public class AbgxProcessor
    {
        public delegate void AbgxEvent(Game g, string line, int percent, int max);

        public delegate void FinishEvent(Game g, AbgStatus status);
        public event AbgxEvent OnAbgx;
        public event FinishEvent OnFinish;
        public void DoAbgx(Game g, string line, int percent, int max)
        {
            if (OnAbgx != null)
                OnAbgx(g, line, percent,max);
        }
        public void DoFinish(Game g, AbgStatus status)
        {
            if (OnFinish != null)
                OnFinish(g, status);
        }
        public Regex KernelVersion = new Regex(".*?Min Kernel Required:(?<kernel>.*)",RegexOptions.Singleline);
        public Regex XEXCrc = new Regex(".*?XEX CRC = (?<crc>.*)", RegexOptions.Singleline);
        public Regex XEXMedia = new Regex(".*?XEX Media ID:(?<id>.*)", RegexOptions.Singleline);
        public Regex RegionCode=  new Regex(".*?Region Code: 0x(?<region>.*)", RegexOptions.Singleline);
        public Regex SSVersion = new Regex(".*?SS Version: (?<version>.*)", RegexOptions.Singleline);
        public Regex SSCrc = new Regex(".*?SS CRC = (?<crc>.*) .*?RawSS = (?<rawcrc>.*)\\)", RegexOptions.Singleline);
        public Regex DMICrc = new Regex(".*?DMI CRC = (?<crc>.*)", RegexOptions.Singleline);
        public Regex PFICrc = new Regex(".*?PFI CRC = (?<crc>.*)", RegexOptions.Singleline);
        public Regex VideoCrc = new Regex(".*?Video CRC = (?<crc>.*).*?\\(V0 = (?<v0crc>.*), V1 = (?<v1crc>.*)\\)", RegexOptions.Singleline);
        public Regex VideoMatch = new Regex("Video CRC (?<match>.*)",RegexOptions.Singleline);
        public Regex V0Match = new Regex   ("V0    CRC (?<match>.*)", RegexOptions.Singleline);
        public Regex V1Match = new Regex ("V1    CRC (?<match>.*)", RegexOptions.Singleline);
        public Regex PFIMatch = new Regex("PFI   CRC (?<match>.*)", RegexOptions.Singleline);
        public Regex DMIMatch = new Regex("DMI   CRC (?<match>.*)", RegexOptions.Singleline);
        public Regex SSMatch = new Regex ("SS    CRC (?<match>.*)", RegexOptions.Singleline);
        public Regex XexMatch = new Regex("Xex   CRC (?<match>.*)", RegexOptions.Singleline);
//        public Regex DMIUnverified = new Regex("DMI is unverified", RegexOptions.Singleline);
//        public Regex SSUnverified = new Regex("SS  is unverified", RegexOptions.Singleline);
        public Regex AnyDVD = new Regex("AnyDVD style corruption was not detected", RegexOptions.Singleline);
        public Regex FoundAnyDVD = new Regex("Found AnyDVD style game data corruption starting at", RegexOptions.Singleline);
        public Regex BadCRC = new Regex("Game partition CRC does not match the verified ini!", RegexOptions.Singleline);
        public Regex GameCRC = new Regex(".*?Game CRC = (?<crc>.*)", RegexOptions.Singleline);
        public Regex Size = new Regex("Size: (?<size>.*) bytes",RegexOptions.Singleline);
        public Regex Topology = new Regex("Checking topology data", RegexOptions.Singleline);
        public Regex Patch = new Regex("Stealth files patched successfully", RegexOptions.Singleline);
        public Regex PFIMatchesKnows = new Regex("PFI matches known data \\((?<data>.*)\\)", RegexOptions.Singleline);
        public Regex VideoMatchesKnows = new Regex("Video partition matches known data \\((?<data>.*)\\)", RegexOptions.Singleline);
        public Regex GamePercent = new Regex("(?<percent>.*?)% .*MB/s", RegexOptions.Singleline);
        public Regex Checking = new Regex("Checking.*", RegexOptions.Singleline);

        private bool end = false;
        private string[] Errors = new string[] { "ERROR","Error" };
        private AbgStatus Status;
        ExeProcessor exeprocess=new ExeProcessor();
        private string lastline;
        private int cnt = 0;
        private Game _game;
        public AbgxProcessor(Game g)
        {
            exeprocess=new ExeProcessor();
            end = false;
            cnt = 0;
            _game = g;
            Status=new AbgStatus();
            Status.SSCRC = new AbgStatus.CRCInfo();
            Status.SSCRC.Status = AbgStatus.CRCStatus.Bad;
            Status.DMICRC = new AbgStatus.CRCInfo();
            Status.DMICRC.Status = AbgStatus.CRCStatus.Bad;
            Status.PFICRC = new AbgStatus.CRCInfo();
            Status.PFICRC.Status = AbgStatus.CRCStatus.Bad;
            Status.XEXCRC = new AbgStatus.CRCInfo();
            Status.XEXCRC.Status = AbgStatus.CRCStatus.Bad;
            Status.VideoCRC = new AbgStatus.CRCInfo();
            Status.VideoCRC.Status = AbgStatus.CRCStatus.Bad;
            Status.V0CRC = new AbgStatus.CRCInfo();
            Status.V0CRC.Status = AbgStatus.CRCStatus.Bad;
            Status.V1CRC = new AbgStatus.CRCInfo();
            Status.V1CRC.Status = AbgStatus.CRCStatus.Bad;
            Status.GameCRC = new AbgStatus.CRCInfo();
            Status.GameCRC.Status = AbgStatus.CRCStatus.Unverified;
            Status.ErrorString = string.Empty;

            Status.Error = false;
            Status.AnyDVDCorruption = false;
            exeprocess.OnError += exeprocess_OnError;
            exeprocess.OnLine += exeprocess_OnLine;
            exeprocess.OnFinish += exeprocess_OnFinish;

        }
        public void Start(string path, string arguments)
        {
            exeprocess.Start(path,arguments);
        }
        void exeprocess_OnFinish()
        {
            if (Status!=null)
                Status.LastCheck = DateTime.Now;
            Thread.Sleep(500);
            DoFinish(_game,Status);
            end = true;

        }

        void exeprocess_OnLine(string line)
        {
            ProcessLine(line);
        }

        void exeprocess_OnError(string line)
        {
            ProcessLine(line);
        }
        public bool Ended()
        {
            return end;
        }

        public void Kill()
        {
            Status = null;
            if (!exeprocess.Ended())
                exeprocess.Kill();
        }

        public void WaitTillEnd()
        {
            exeprocess.WaitTillEnd();
        }


        protected void ProcessLine(string line)
        {
            if ((end) || (Status==null))
                return;
            try
            {
                foreach (string s in Errors)
                {
                    if (line.StartsWith(s))
                    {
                        Status.ErrorString += line;
                        Status.Error = true;
                    }
                }
                while (line.StartsWith("\033"))
                {
                    int idx = line.IndexOf('m');
                    line = line.Substring(idx + 1).Trim();
                }
                Match m;
                if (!Status.Error)
                {
                    m = Patch.Match(line);
                    if (m.Success)
                    {
                        exeprocess.Restart();
                    }
                    m = KernelVersion.Match(line);
                    if (m.Success)
                    {
                        Status.MinKernel = m.Groups["kernel"].Value.Trim();
                    }
                    m = XEXCrc.Match(line);
                    if (m.Success)
                    {
                        Status.XEXCRC.Crc = Convert.ToUInt32(m.Groups["crc"].Value.Trim(), 16);
                        Status.XEXCRC.Status = AbgStatus.CRCStatus.Unverified;
                    }
                    m = XEXMedia.Match(line);
                    if (m.Success)
                        Status.MediaId = m.Groups["id"].Value.Trim();
                    m = RegionCode.Match(line);
                    if (m.Success)
                    {
                        Status.RegionCode = Convert.ToUInt32(m.Groups["region"].Value.Trim(), 16);
                        _game.RegionCode = m.Groups["region"].Value.Trim();
                    }
                    m = SSVersion.Match(line);
                    if (m.Success)
                    {
                        int val = int.Parse(m.Groups["version"].Value.Trim().Substring(0, 1));
                        Status.LTouchVersion = val == 1 ? AbgStatus.LTVersion.LT10 : AbgStatus.LTVersion.LT20;
                    }
                    m = Size.Match(line);
                    if (m.Success)
                    {
                        Status.Size = Convert.ToUInt64(m.Groups["size"].Value.Trim());
                        Status.DiskVersion = AbgStatus.XGDVersion.XGD;
                    }
                    m = Topology.Match(line);
                    if (m.Success)
                    {
                        Status.DiskVersion = Status.Size.ToString().StartsWith("873")
                                                 ? AbgStatus.XGDVersion.XGD3
                                                 : AbgStatus.XGDVersion.AP25;
                    }
                    m = SSCrc.Match(line);
                    if (m.Success)
                    {
                        Status.SSCRC.Crc = Convert.ToUInt32(m.Groups["crc"].Value.Trim(), 16);
                        Status.SSCRC.Status = AbgStatus.CRCStatus.Unverified;
                        Status.RAWSS = Convert.ToUInt32(m.Groups["rawcrc"].Value.Trim(), 16);
                    }

                    m = DMICrc.Match(line);
                    if (m.Success)
                    {
                        Status.DMICRC.Crc = Convert.ToUInt32(m.Groups["crc"].Value.Trim(), 16);
                        Status.DMICRC.Status = AbgStatus.CRCStatus.Unverified;
                    }
                    m = PFICrc.Match(line);
                    if (m.Success)
                    {
                        Status.PFICRC.Crc = Convert.ToUInt32(m.Groups["crc"].Value.Trim(), 16);
                        Status.PFICRC.Status = AbgStatus.CRCStatus.Unverified;
                    }
                    m = VideoCrc.Match(line);
                    if (m.Success)
                    {

                        Status.VideoCRC.Crc = Convert.ToUInt32(m.Groups["crc"].Value.Trim(), 16);
                        Status.V0CRC.Crc = Convert.ToUInt32(m.Groups["v0crc"].Value.Trim(), 16);
                        Status.V1CRC.Crc = Convert.ToUInt32(m.Groups["v1crc"].Value.Trim(), 16);
                        Status.VideoCRC.Status = AbgStatus.CRCStatus.Unverified;
                        Status.V0CRC.Status = AbgStatus.CRCStatus.Unverified;
                        Status.V1CRC.Status = AbgStatus.CRCStatus.Unverified;
                    }
                    else
                    {
                        m = VideoMatch.Match(line);
                        if (m.Success)
                            Status.VideoCRC.Status = m.Groups["match"].Value.Trim().StartsWith("matches") ? AbgStatus.CRCStatus.Verified : AbgStatus.CRCStatus.Bad;
                    }
                    m = V0Match.Match(line);
                    if (m.Success)
                        Status.V0CRC.Status = m.Groups["match"].Value.Trim().StartsWith("matches")
                                                  ? AbgStatus.CRCStatus.Verified
                                                  : AbgStatus.CRCStatus.Bad;
                    m = V1Match.Match(line);
                    if (m.Success)
                        Status.V1CRC.Status = m.Groups["match"].Value.Trim().StartsWith("matches")
                                                  ? AbgStatus.CRCStatus.Verified
                                                  : AbgStatus.CRCStatus.Bad;
                    m = PFIMatch.Match(line);
                    if (m.Success)
                        Status.PFICRC.Status = m.Groups["match"].Value.Trim().StartsWith("matches")
                                                   ? AbgStatus.CRCStatus.Verified
                                                   : AbgStatus.CRCStatus.Bad;
                    m = DMIMatch.Match(line);
                    if (m.Success)
                        Status.DMICRC.Status = m.Groups["match"].Value.Trim().StartsWith("matches")
                                                   ? AbgStatus.CRCStatus.Verified
                                                   : AbgStatus.CRCStatus.Bad;
                    m = SSMatch.Match(line);
                    if (m.Success)
                        Status.SSCRC.Status = m.Groups["match"].Value.Trim().StartsWith("matches")
                                                  ? AbgStatus.CRCStatus.Verified
                                                  : AbgStatus.CRCStatus.Bad;
                    m = XexMatch.Match(line);
                    if (m.Success)
                        Status.XEXCRC.Status = m.Groups["match"].Value.Trim().StartsWith("matches")
                                                   ? AbgStatus.CRCStatus.Verified
                                                   : AbgStatus.CRCStatus.Bad;
                    m = BadCRC.Match(line);
                    if (m.Success)
                    {
                        Status.GameCRC.Status = AbgStatus.CRCStatus.Bad;
                        Status.LastCheck = DateTime.Now;
                    }
                    m = GameCRC.Match(line);
                    if (m.Success)
                    {
                        string crc = m.Groups["crc"].Value.Trim();
                        int a = crc.IndexOf(" ");
                        if (a > 0)
                        {
                            Status.GameCRC.Status = AbgStatus.CRCStatus.Verified;
                            crc = crc.Substring(0, a).Trim();
                        }
                        else
                            Status.GameCRC.Status = AbgStatus.CRCStatus.Unverified;
                        Status.GameCRC.Crc = Convert.ToUInt32(crc, 16);
                        Status.LastCheck = DateTime.Now;
                    }
                    m = AnyDVD.Match(line);
                    if (m.Success)
                        Status.AnyDVDCorruption = false;
                    m = FoundAnyDVD.Match(line);
                    if (m.Success)
                        Status.AnyDVDCorruption = true;
                    m = PFIMatchesKnows.Match(line);
                    if (m.Success)
                        Status.Wave = m.Groups["data"].Value.Trim();
                    m = VideoMatchesKnows.Match(line);
                    if (m.Success)
                        Status.Wave = m.Groups["data"].Value.Trim();
                }
                m = Checking.Match(line);
                if (m.Success)
                {
                    
                    int aa = line.IndexOf("...", System.StringComparison.Ordinal);
                    if (aa >= 0)
                        line = line.Substring(0, aa) + "...";
                    if (line != lastline)
                    {
                        lastline = line;
                        cnt++;
                        DoAbgx(_game, lastline, cnt,115);
                    }
                }
                m = GamePercent.Match(line);
                if (m.Success)
                {
                    int val = Convert.ToInt32(m.Groups["percent"].Value.Trim());
                    DoAbgx(_game, lastline, val + 15, 115);
                }
            }
            catch (Exception e)
            {
                Status.Error = true;
                Status.ErrorString = "Error Checking ISO " + _game.Title + ": " + e.ToString();
            }
         
        }
    }
}
