using System;
using System.Drawing;
using System.Windows.Forms;

namespace xk3yScanner.Control
{
    public class TextProgressBar : ProgressBar
    {
        private string _progressText;

        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                if (_progressText != value)
                {
                    _progressText = value;
                    Refresh();
                }
            }
        }
        public Color ProgressColor { get; set; }
        public Font ProgressFont { get; set; }
        public ContentAlignment ProgressAlignment { get; set; }

        protected override CreateParams CreateParams
        {
          get
          {
            CreateParams result = base.CreateParams;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT
                && Environment.OSVersion.Version.Major >= 6)
            {
              result.ExStyle |= 0x02000000; // WS_EX_COMPOSITED 
            }

            return result;
          }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x000F)
            {
                using (Graphics g = CreateGraphics())
                using (SolidBrush brush = new SolidBrush(ForeColor))
                {
                    Rectangle rect = ClientRectangle;

                    ProgressBarRenderer.DrawHorizontalBar(g, rect);
                    rect.Inflate(-3, -3);
                    if (Value > 0)
                    {
                        Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width),
                                                       rect.Height);
                        ProgressBarRenderer.DrawHorizontalChunks(g, clip);
                    }
                    if (!string.IsNullOrEmpty(ProgressText) && (ProgressFont != null))
                    {

                        SizeF len = g.MeasureString(ProgressText, ProgressFont);
                        int x;
                        int y;
                        switch (ProgressAlignment)
                        {
                            case ContentAlignment.TopRight:
                            case ContentAlignment.TopLeft:
                            case ContentAlignment.TopCenter:
                                y = rect.Y + 1;
                                break;
                            case ContentAlignment.MiddleRight:
                            case ContentAlignment.MiddleLeft:
                            case ContentAlignment.MiddleCenter:
                                y = rect.Y + Convert.ToInt32((rect.Height - len.Height) / 2);
                                break;
                            default:
                                y = Convert.ToInt32(rect.Y + rect.Height - 1 - len.Height);
                                break;
                        }
                        switch (ProgressAlignment)
                        {
                            case ContentAlignment.TopLeft:
                            case ContentAlignment.MiddleLeft:
                            case ContentAlignment.BottomLeft:
                                x = rect.X + 1;
                                break;
                            case ContentAlignment.TopCenter:
                            case ContentAlignment.MiddleCenter:
                            case ContentAlignment.BottomCenter:
                                x = rect.X+Convert.ToInt32((rect.Width - len.Width) / 2);
                                break;
                            default:
                                x = Convert.ToInt32(rect.X + rect.Width - 1 - len.Width);
                                break;
                        }
                        Point location = new Point(x, y);
                        g.DrawString(ProgressText, ProgressFont, new SolidBrush(ProgressColor), location);
                    }

                }
            }
        }
        public TextProgressBar()
        {
            ProgressColor = SystemColors.WindowText;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            ProgressFont = new Font("Microsoft Sans Serif", 8.25F);
            ProgressAlignment = ContentAlignment.MiddleCenter;
            this.DoubleBuffered = true;
        }

        
    }
}
