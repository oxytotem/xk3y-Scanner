using System;
using System.IO;
using System.Threading;

namespace xk3yScanner.Classes.Processors.Helpers
{
    public class MoveAsync
    {
        public delegate void MoveEvent(Game g, int percent);
        public delegate void MoveError(Game g, string error);

        private bool _deleteorigin;
        private string _source;

        public bool Abort = false;

        public event MoveEvent OnProgress;
        public event MoveError OnError;
        public bool Running = false;

        public void DoProgress(Game g, int percent)
        {
            if (OnProgress != null)
                OnProgress(g, percent);
        }
        public void DoError(Game g, string error)
        {
            if (OnError != null)
                OnError(g, error);
        }
        public delegate void FileCopyDelegate(string sourceFile, string destFile);

        private Game _game;
        public void FileMove(Game g, string sourceFile, string destFile)
        {
            Running = true;
            _game = g;
            FileCopyDelegate del = new FileCopyDelegate(FMove);
            IAsyncResult result = del.BeginInvoke(sourceFile, destFile, CallBackAfterFileCopied, null);
            while (Running)
                Thread.Sleep(100);
            del.EndInvoke(result);
        }
        private string GetBase(string file)
        {
            if (file.StartsWith("\\\\"))
            {
                int idx = file.IndexOf("\\", 2);
                return file.Substring(2, idx - 2);
            }
            int id2 = file.IndexOf(":");
            return file.Substring(0, id2 );
        }
        private void FMove(string sourceFile, string destFile)
        {
            FileStream src = null;
            FileStream destination = null;
            try
            {

                 _source = sourceFile;
                if (GetBase(sourceFile) == GetBase(destFile))
                {
                    DoProgress(_game, 0);
                    File.Move(sourceFile,destFile);
                    _deleteorigin = false;
                }
                else
                {
                    _deleteorigin = true;
                    src = File.OpenRead(sourceFile);
                    long length = src.Length;
                    long start = length;
                    int per = 0;
                    destination = File.OpenWrite(destFile);
                    byte[] buffer = new byte[0x100000];
                    while (start > 0)
                    {
                        if (Abort)
                        {
                            src.Close();
                            destination.Close();
                            return;
                        }
                        int size=start>0x100000 ? 0x100000: (int)start;
                        int read=src.Read(buffer, 0, size);
                        destination.Write(buffer,0,size);
                        start -= read;
                        int newper = 100-(int)(start*100/length);
                        if (newper != per)
                        {
                            per = newper;
                            DoProgress(_game, per);
                        }
                    }
                    src.Close();
                    destination.Close();
                }
            }
            catch (Exception e)
            {
                _deleteorigin = false;
                if (src!=null)
                    src.Close();
                if (destination != null)
                    destination.Close();
                DoError(_game, "Error moving file");
            }
           

        }

        public void CallBackAfterFileCopied(IAsyncResult result)
        {
            if ((_deleteorigin) && (!string.IsNullOrEmpty(_source)))
            {
                File.Delete(_source);
            }
            DoProgress(_game, 100);
            Running = false;
        }
    }
}
