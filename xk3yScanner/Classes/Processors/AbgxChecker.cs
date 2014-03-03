using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using xk3yScanner.Classes.Processors.Helpers;

namespace xk3yScanner.Classes.Processors
{
    public class AbgxChecker : BaseProcessor
    {



        public AbgxChecker() : base(1)
        {
        }


        void _processor_OnFinish(Game g, AbgStatus status)
        {
            if (status != null)
            {
                g.Abgx = status;
            }
        }
        void _processor_OnAbgx(Game g, string line, int percent, int max)
        {
            DoStatusUpdate(string.Format("Verifying {0} {1}/{2}...", g.Title, Cnt+1, GameCnt), line, Cnt, GameCnt, percent, max);
        }
        internal override void RetrieveThread(Game g)
        {
            string args = string.Format(Properties.Settings.Default.AbgxArguments, g.FullIsoPath);
            if (File.Exists(g.FullIsoPath))
            {
                AbgxProcessor processor = new AbgxProcessor(g);
                processor.OnAbgx += _processor_OnAbgx;
                processor.OnFinish += _processor_OnFinish;
                processor.Start(Program.Abgx, args);
                while ((!Abort) && (!processor.Ended()))
                {
                    Thread.Sleep(100);
                }
                if (Abort)
                    processor.Kill();
        }
        }
    }
}
