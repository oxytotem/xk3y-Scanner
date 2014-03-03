
using System.Collections.Generic;


namespace xk3yScanner.Classes
{
    public class Visibility : Dictionary<string,bool>
    {
        public Visibility()
        {
            this.Add("Genre", true);
            this.Add("Discs", true);
            this.Add("Publisher", true);
            this.Add("Developer", true);
            this.Add("Game Date", true);
            this.Add("Game Type", true);
            this.Add("Trailer", true);
            this.Add("Abgx Info",false);
            this.Add("Regions",true);

        }
        public void Update()
        {
            this["Genre"] = Properties.Settings.Default.chkGenre;
            this["Discs"] = Properties.Settings.Default.chkDiscs;
            this["Publisher"] = Properties.Settings.Default.chkPublisher;
            this["Developer"] = Properties.Settings.Default.chkDeveloper;
            this["Game Date"] = Properties.Settings.Default.chkGameDate;
            this["Game Type"] = Properties.Settings.Default.chkGameType;
            this["Trailer"] = Properties.Settings.Default.chkTrailer;
            this["Abgx Info"] = Properties.Settings.Default.chkAbgxInfo;
            this["Regions"] = Properties.Settings.Default.chkRegionCode;
        }


    }
}
