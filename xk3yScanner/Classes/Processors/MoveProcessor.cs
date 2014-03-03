using System;
using System.Collections.Generic;
using System.IO;
using xk3yScanner.Classes.Processors.Helpers;

namespace xk3yScanner.Classes.Processors
{
    public class MoveProcessor : BaseProcessor
    {
        private MoveAsync _processor;
        public MoveProcessor() : base(1)
        {
        }
        public override void Start(List<Game> games)
        {
            Cnt = 0;
            _processor = new MoveAsync();
            _processor.OnError += _processor_OnError;
            _processor.OnProgress += _processor_OnProgress;
            GameCnt = games.Count;
            base.Start(games);
        }
        void _processor_OnProgress(Game g, int percent)
        {
            if (Abort)
            {
                _processor.Abort = true;
            }
            DoStatusUpdate(string.Format("Moving {0} {1}/{2}...", g.Title, Cnt+1, GameCnt), string.Empty, Cnt, GameCnt, percent, 100);
        }

        void _processor_OnError(Game g, string error)
        {
            DoStatusUpdate(string.Format("Error Moving {0} {1}/{2}...", g.Title,Cnt+1,GameCnt), error, Cnt, GameCnt, -1, -1);
        }

        internal override void RetrieveThread(Game g)
        {
            string destpath= Properties.Settings.Default.InActiveFolder;
            if (g.GameDirectoy == destpath)
                destpath = Properties.Settings.Default.ActiveFolder;
            int a = g.BasePath.LastIndexOf("\\");
            string oldFullIsoPath = string.Empty;
            if (a >= 0)
                oldFullIsoPath = g.BasePath.Substring(0, a);
            string left = g.BasePath.Substring(g.GameDirectoy.Length+1);

            string destination = Path.Combine(destpath, left);
            string destinationdir = string.Empty;
            a = destination.LastIndexOf("\\");
            if (a >= 0)
            {
                destinationdir = destination.Substring(0, a);
                try
                {
                    Directory.CreateDirectory(destinationdir);
                }
                catch (Exception)
                {
                }
            }
            _processor.Abort = false;
            if (File.Exists(g.XmlPath))
                _processor.FileMove(g, g.XmlPath,destination+".xml");
            if (File.Exists(g.MdsPath))
                _processor.FileMove(g, g.MdsPath, destination + ".mds");
            if (File.Exists(g.DvdPath))
                _processor.FileMove(g, g.DvdPath, destination + ".dvd");
            if (File.Exists(g.Cover1Path))
                _processor.FileMove(g, g.Cover1Path, destination + ".jpg");
            if (File.Exists(g.Cover2Path))
                _processor.FileMove(g, g.Cover2Path, destination + "-cover.jpg");
            if (File.Exists(g.BannerPath))
                _processor.FileMove(g, g.BannerPath, destination + "-banner.jpg");
            if (File.Exists(g.FullIsoPath))
                _processor.FileMove(g, g.FullIsoPath, destination + ".iso");
            if ((!string.IsNullOrEmpty(oldFullIsoPath)) && (Directory.Exists(oldFullIsoPath) && Directory.GetFileSystemEntries(oldFullIsoPath).Length == 0))
                Directory.Delete(oldFullIsoPath);
            g.BasePath = destination;
            g.FullIsoPath = destination + ".iso";
            g.Active = !g.Active;
            g.GameDirectoy = destpath;
        }
    }
}
