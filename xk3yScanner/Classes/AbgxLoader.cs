using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace xk3yScanner.Classes
{
    public class AbgxLoader
    {
        private AbgxProcessor _processor;

        public delegate void OnGameScannedHandler(Game game, int count);

        public delegate void OnFinishHandler();

        public delegate void OnStatusUpdateHandler(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount, int rightmaxcount);

        public event OnGameScannedHandler OnGameScanned;
        public event OnStatusUpdateHandler OnStatusUpdate;
        public event OnFinishHandler OnFinish;

        private int _cnt;
        private int _gamecnt;
        private string CurrentGame;

        public bool Finished { get; set; }
        public bool Abort { get; set; }
        public string _path;
        public string _arguments;



        public void ProcessGames(List<Game> games)
        {
            _cnt = 0;
            _path = Program.Abgx;
            _arguments = Properties.Settings.Default.AbgxArguments;
            _gamecnt = 0;
            Abort = false;
            Finished = false;
            Thread th = new Thread(ExecutorThread);
            _processor=new AbgxProcessor();
            _processor.OnAbgx += _processor_OnAbgx;
            th.Start(games);
        }

        void _processor_OnAbgx(string line, int percent, int max)
        {
            if (Abort)
                _processor.Kill();
            if (OnStatusUpdate != null)
                OnStatusUpdate(string.Format("Verifying {0} {1}/{2}...", CurrentGame, _cnt, _gamecnt), line, _cnt-1, _gamecnt, percent, max);
        }
        private int threadcnt = 0;

        private void ExecutorThread(object obj)
        {
            List<Game> games = (List<Game>)obj;
            threadcnt = 0;
            _gamecnt = games.Count;
            do
            {
                while (games.Count > 0)
                {
                    _cnt++;                    
                    Game g = games[0];
                    CurrentGame = g.Title;
                    games.RemoveAt(0);
                    RetrieveThread(g);
                }
            } while (((games.Count > 0) || threadcnt > 0) && (!Abort));
            Finished = true;
            if (this.OnFinish != null)
                OnFinish.Invoke();
        }
        public void RetrieveThread(Game g)
        {
            string args = string.Format(_arguments, g.FullIsoPath);
            if (File.Exists(g.FullIsoPath))
            {
                _processor.Start(_path, args);
                while ((!Abort) && (!_processor.Ended()))
                {
                    Thread.Sleep(100);   
                }
                if (Abort)
                    _processor.Kill();
                else
                {
                    g.Abgx = _processor.Status;
                    if (OnGameScanned != null)
                        OnGameScanned(g, _cnt);
                }
            }

        }
    }
}
