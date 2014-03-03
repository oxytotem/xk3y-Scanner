using System;
using System.Diagnostics;

namespace xk3yScanner.Classes.Processors.Helpers
{
    public class ExeProcessor
    {
        public delegate void LineEvent(string line);

        public delegate void FinishEvent();

        public event LineEvent OnLine;
        public event LineEvent OnError;
        public event FinishEvent OnFinish;

        private Process process;
        private bool end;

        public void DoLine(string line)
        {
            if (OnLine != null)
                OnLine(line);
        }

        public void DoError(string line)
        {
            if (OnError != null)
                OnError(line);
        }
        public void DoFinish()
        {
            if (OnFinish != null)
                OnFinish();
        }
        // usage
        public void Restart()
        {
            process.Kill();
            process.Exited -= process_Exited;
            Start(ipath,iarguments);
        }
        private string ipath;
        private string iarguments;
        public void Start(string path, string arguments)
        {
            end = false;
            process=new Process();
            ipath = path;
            iarguments = arguments;
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Exited += process_Exited;
            process.EnableRaisingEvents = true;
            process.Start();
            process.ErrorDataReceived+=process_ErrorDataReceived;
            //process.OutputDataReceived+=process_OutputDataReceived;
            //process.BeginOutputReadLine();
            process.BeginErrorReadLine();

/*
            _manager = new ProcessIoManager(process);
            _manager.StderrTextRead += _manager_StderrTextRead;
            _manager.StdoutTextRead += _manager_StdoutTextRead;
            _manager.StartProcessOutputRead();*/
        }

        void _manager_StdoutTextRead(string text)
        {
            DoLine(text);
        }

        void _manager_StderrTextRead(string text)
        {
            DoError(text);
        }

        void process_Exited(object sender, EventArgs e)
        {
            end = true;
            DoFinish();
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if ((e==null) || (e.Data==null))
                return;
            string str = e.Data.Replace("\r\n", string.Empty);
            if (str.Length>0)
                DoError(str);
        }
        
        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if ((e == null) || (e.Data == null))
                return;
            string str = e.Data.Replace("\r\n", string.Empty);
            if (str.Length > 0)
                DoError(str);
        }
        public bool Ended()
        {
            if (process != null)
            {
                return process.HasExited;
            }
            return end;
        }

        public void Kill()
        {
            if (process!=null)
                process.Kill();
            end = true;
        }

        public void WaitTillEnd()
        {
            process.WaitForExit();
            end = true;
        }

    }
}
