using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace xk3yScanner.Classes
{
    public class ScannerInfo
    {
        public long ActiveFolderSize { get; set; }
        public long InactiveFolderSize { get; set; }
        public long TotalSize { get; set; }
        public int UnVerifiedGames { get; set; }
        public int UnScannedGames { get; set; }
        public int TotalGames { get; set; }
        public long ActiveFolderFreeSpace { get; set; }
        public long InactiveFolderFreeSpace { get; set; }
        public int ActiveGames { get; set; }
        public int InactiveGames { get; set; }
        public bool SameDisk { get; set; }
        private string GetDriveBase(string file)
        {
            if (file.StartsWith("\\\\"))
            {
                return string.Empty;
            }
            int id2 = file.IndexOf(":");
            return file.Substring(0, id2);
        }
        private string ToGBytes(long size)
        {
            double ff = size;
            ff /= 1024*1024*1024;
            return ff.ToString("N2")+"Gb";
        }
        public override string ToString()
        {       
            StringBuilder bld=new StringBuilder();
            if (TotalGames > 0)
            {
                bld.Append("Games: ");
                bld.Append(TotalGames);
                if ((ActiveGames > 0) && (InactiveGames>0))
                {
                    bld.Append(", Active: ");
                    bld.Append(ActiveGames);
                }
                if (InactiveGames > 0)
                {
                    bld.Append(", Inactive: ");
                    bld.Append(InactiveGames);
                }
                if (UnScannedGames > 0)
                {
                    bld.Append(", Unscanned: ");
                    bld.Append(UnScannedGames);
                }
                if (UnVerifiedGames > 0)
                {
                    bld.Append(", Unverified: ");
                    bld.Append(UnVerifiedGames);
                }

            }
            if (ActiveFolderSize > 0)
            {
                bld.Append("   Active Folder Size: ");
                bld.Append(ToGBytes(ActiveFolderSize));
                if ((ActiveFolderFreeSpace > 0) && (!SameDisk))
                {
                    bld.Append(", Free: ");
                    bld.Append(ToGBytes(ActiveFolderFreeSpace));
                }
            }
            if (InactiveFolderSize > 0)
            {
                bld.Append(" Inactive Size: ");
                bld.Append(ToGBytes(InactiveFolderSize));
                if ((InactiveFolderFreeSpace > 0) && (!SameDisk))
                {
                    bld.Append(", Free: ");
                    bld.Append(ToGBytes(InactiveFolderFreeSpace));
                }
            }
            if ((ActiveFolderFreeSpace > 0) && (SameDisk))
            {
                bld.Append(", Free: ");
                bld.Append(ToGBytes(ActiveFolderFreeSpace));
            }

            return bld.ToString();
        }
        public void Populate(List<Game> games)
        {
            ActiveFolderSize = 0;
            InactiveFolderSize = 0;
            TotalSize = 0;
            TotalGames = 0;
            UnScannedGames = 0;
            UnVerifiedGames = 0;
            SameDisk = false;
            ActiveFolderFreeSpace = 0;
            InactiveFolderFreeSpace = 0;
            ActiveGames = 0;
            InactiveGames = 0;
            foreach (Game g in games)
            {
                if (g.Active)
                {
                    ActiveFolderSize += g.Size;
                    ActiveGames++;
                }
                else
                {
                    InactiveFolderSize += g.Size;
                    InactiveGames++;
                }
                TotalSize += g.Size;
                if (g.Abgx == null)
                    UnVerifiedGames++;
                if (!g.WebPopulated)
                    UnScannedGames++;
                TotalGames++;
            }
            string actdriv = GetDriveBase(Properties.Settings.Default.ActiveFolder);
            string inactdriv = string.Empty;
            if (Properties.Settings.Default.InActiveFolders)
                inactdriv = GetDriveBase(Properties.Settings.Default.InActiveFolder);
            if (!string.IsNullOrEmpty(actdriv))
            {
                DriveInfo info = new DriveInfo(actdriv);
                ActiveFolderFreeSpace = info.TotalFreeSpace;
            }
            if (inactdriv == actdriv)
            {
                SameDisk = true;
                InactiveFolderFreeSpace = ActiveFolderFreeSpace;
            }
            else if (!string.IsNullOrEmpty(inactdriv))
            {
                DriveInfo info = new DriveInfo(inactdriv);
                InactiveFolderFreeSpace = info.TotalFreeSpace;
            }
        }
    }
}
