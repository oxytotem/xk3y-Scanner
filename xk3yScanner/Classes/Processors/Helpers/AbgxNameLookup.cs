using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace xk3yScanner.Classes.Processors.Helpers
{
    public class AbgxNameLookup
    {
        public Dictionary<string, Lookup> lookup = new Dictionary<string, Lookup>();
        public class Lookup
        {
            public string Title;
            public int Disc;
        }
        Regex disks=new Regex("\\(Disc (.*?)\\)");
        public AbgxNameLookup()
        {
            try
            {
                Properties.Settings.Default.abgxGameNameLookup = Utils.GetUrl(Properties.Settings.Default.abgxNameLookUpUrl, null, true, Utils.UAgent, Properties.Settings.Default.abgxGameNameLookupETAG);
                Properties.Settings.Default.Save();
            }
            catch(Utils.NotModifiedExeption)
            {
                
            }
            catch (Exception)
            {
                return;   
            }
            string[] num = Properties.Settings.Default.abgxGameNameLookup.Split('\n');
            foreach(string s in num)
            {
                string[] m = s.Split(',');
                int disc = 1;
                Match match = disks.Match(m[0]);
                if (match.Success)
                    int.TryParse(match.Groups[1].Value, out disc);
                string val = disks.Replace(m[0], string.Empty).Trim();                
                for(int x=1;x<m.Length;x++)
                {
                    Lookup l=new Lookup();
                    l.Title = val;
                    l.Disc = disc;
                    lookup.Add(m[x],l);
                }
            }
        }
        public string GetName(string mediaid, out int disknum)
        {
            if (lookup.ContainsKey(mediaid))
            {
                Lookup l = lookup[mediaid];
                disknum = l.Disc;
                return l.Title;
            }
            disknum = 0;
            return null;
        }
    }
}
