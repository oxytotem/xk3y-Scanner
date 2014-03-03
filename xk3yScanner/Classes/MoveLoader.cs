using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace xk3yScanner.Classes
{
    public class MoveLoader
    {
        private MoveAsync _processor;

        public delegate void OnFinishHandler();

        public delegate void OnStatusUpdateHandler(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount, int rightmaxcount);

        public event OnStatusUpdateHandler OnStatusUpdate;
        public event OnFinishHandler OnFinish;

        private int _cnt;
        private int _gamecnt;
        private string CurrentGame;
        public bool Finished { get; set; }
        public bool Abort { get; set; }


        public string _source;
        public string _destination;


        public void ProcessGames(List<Game> games, string source, string destination)
        {
            _source = source;
            _destination=destination;
            _cnt = 0;

            _gamecnt = 0;
            Abort = false;
            Finished = false;
            Thread th = new Thread(ExecutorThread);
            _processor = new MoveAsync();
            _processor.OnError += _processor_OnError;
            _processor.OnProgress += _processor_OnProgress;
            _processor.Abort = false;
            th.Start(games);
        }

        void _processor_OnProgress(int percent)
        {
            if (Abort)
            {
                _processor.Abort = true;
            }
            if (OnStatusUpdate != null)
                OnStatusUpdate(string.Format("Moving {0} {1}/{2}...", CurrentGame, _cnt, _gamecnt),string.Empty, _cnt, _gamecnt, percent, 100);
        }

        void _processor_OnError(string error)
        {
            if (OnStatusUpdate != null)
                OnStatusUpdate(string.Format("Error Moving {0} {1}/{2}...", CurrentGame, _cnt, _gamecnt), error, _cnt,
                               -1, -1, -1);
        }



        private void ExecutorThread(object obj)
        {
            List<Game> games = (List<Game>)obj;
            _gamecnt = games.Count;
            do
            {
                while (games.Count > 0)
                {
                    _cnt++;
                    Game g = games[0];
                    CurrentGame = g.Title;
                    games.RemoveAt(0);
                    MoveThread(g);
                }
            } while ((games.Count > 0) && (!Abort));
            Finished = true;
            if (this.OnFinish != null)
                OnFinish.Invoke();
        }
        public void MoveThread(Game g)
        {
            if (_source == _destination)
                return;
            string left = _source.Substring(g.BasePath.Length);
            string destination = Path.Combine(_destination, left);
            _processor.Abort = false;
            if (File.Exists(g.XmlPath))
                _processor.FileMove(g.XmlPath,destination+".xml");
            if (File.Exists(g.MdsPath))
                _processor.FileMove(g.MdsPath, destination + ".mds");
            if (File.Exists(g.DvdPath))
                _processor.FileMove(g.DvdPath, destination + ".dvd");
            if (File.Exists(g.Cover1Path))
                _processor.FileMove(g.Cover1Path, destination + ".jpg");
            if (File.Exists(g.Cover2Path))
                _processor.FileMove(g.Cover2Path, destination + "-cover.jpg");
            if (File.Exists(g.BannerPath))
                _processor.FileMove(g.BannerPath, destination + "-banner.jpg");
            if (File.Exists(g.FullIsoPath))
                _processor.FileMove(g.FullIsoPath, destination + ".iso");
            g.BasePath = destination;
            g.FullIsoPath = destination + ".iso";
            g.Active = !g.Active;
        }
    }
}
