using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace xk3yScanner.Classes.Processors.Helpers
{
    public abstract class BaseProcessor
    {

        public delegate void OnStatusUpdateHandler(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount, int rightmaxcount);

        public delegate void OnGameProcessedHandler(Game game, int count);

        public delegate void OnFinishHandler();
                
        public void DoStatusUpdate(string lefttext, string righttext, int leftcount, int leftmaxcount, int rightcount, int rightmaxcount)
        {
            if (OnStatusUpdate != null)
                OnStatusUpdate.Invoke(lefttext, righttext, leftcount, leftmaxcount, rightcount, rightmaxcount);
        }
        public void DoFinish()
        {
            if (OnFinish != null)
                OnFinish.Invoke();
        }
        public void DoGameProcessed(Game game, int count)
        {
            if (OnGameProcessed!=null)
                OnGameProcessed.Invoke(game,count);
        }
        public event OnGameProcessedHandler OnGameProcessed;
        public event OnStatusUpdateHandler OnStatusUpdate;
        public event OnFinishHandler OnFinish;

        internal int Cnt = 0;
        internal int GameCnt=0;
        private int _threadcnt = 0;
        private readonly int _maxthreads = 0;
        
        private object _executorlock = new object();

        public bool Finished { get; set; }
        internal bool Abort { get; set; }
        internal bool Running { get; set; }
        public BaseProcessor(int maxthreads)
        {
            _maxthreads = maxthreads;
            Finished = true;
        }


        public virtual void Start(List<Game> games)
        {
            Cnt = 0;
            GameCnt = games.Count;
            _threadcnt = 0;
            Abort = false;
            Finished = false;
            Thread th = new Thread(ExecutorThread);
            th.Start(games.ToArray());
        }
        public virtual void Stop()
        {
            Abort = true;
            while (!Finished)
            {
                Thread.Sleep(50);
            }
            Abort = false;
        }
        private void ExecutorThread(object obj)
        {
            List<Game> games=new List<Game>((Game[])obj);
            _threadcnt = 0;
            do
            {
                if (games.Count > 0)
                {
                    Game g;
                    lock (_executorlock)
                    {
                        g = games[0];
                        games.RemoveAt(0);
                    }
                    Thread th = new Thread(RetrieveThreadObj);
                    th.Start(g);
                    _threadcnt++;
                }
                while (_threadcnt == _maxthreads)
                {
                    Thread.Sleep(50);
                }
                Thread.Sleep(50);
            } while (((games.Count > 0) || _threadcnt > 0) && (!Abort));
            DoFinish();
            Finished = true;
        }
        private void RetrieveThreadObj(object obj)
        {
            Game g = (Game) obj;
            RetrieveThread(g);
            Cnt++;
            DoGameProcessed(g,Cnt);
            _threadcnt--;
        }
        internal abstract void RetrieveThread(Game g);

    }
}
