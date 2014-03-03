using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;


namespace xk3yScanner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler (OnProcessExit); 
            DeflateStream ds=new DeflateStream(new MemoryStream(Properties.Resources.abgx360),CompressionMode.Decompress);
            Abgx = Path.GetTempFileName();
            FileStream fs=new FileStream(Abgx,FileMode.Create);
            int r = 0;
            do
            {
                byte[] buf=new byte[4096];
                r = ds.Read(buf, 0, 4096);
                if (r>0)
                    fs.Write(buf,0,r);
            } while (r!=0);
            fs.Close();
            ds.Close();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ActiveFolder))
                Wizard = false;
            Form=new MainForm();
            Application.ThreadException += new ThreadExceptionEventHandler(Form.UnhandledThreadExceptionHandler);
            if (Wizard)
                Form.Enabled = false;
            Application.Run(Form);
        }

        public static string Abgx;


        static void OnProcessExit (object sender, EventArgs e)
        {
            if (File.Exists(Abgx))
            {
                try
                {
                    File.Delete(Abgx);
                }
                catch
                {
                }
            }
        }

        public static bool Wizard = true;
        public static MainForm Form;

        /*            FileStream ds=new FileStream(@"C:\Users\mpiva\Downloads\abgx360_v1.0.6_source (2)\abgx360\abgx360.exe",FileMode.Open);
            MemoryStream ms=new MemoryStream();
            System.IO.Compression.DeflateStream d = new DeflateStream(ms, CompressionMode.Compress,true);
            ds.CopyTo(d);
            d.Close();
            ds.Close();
            ms.Seek(0, SeekOrigin.Begin);
            ds = new FileStream(@"C:\Users\mpiva\Downloads\abgx360_v1.0.6_source (2)\abgx360\abgx360.comp",FileMode.Create);
            ms.CopyTo(ds);
            ms.Close();
            ds.Close();*/
    }
}
