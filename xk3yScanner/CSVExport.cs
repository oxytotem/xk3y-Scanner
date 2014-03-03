using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QiHe.CodeLib.Csv;
using xk3yScanner.Classes;

namespace xk3yScanner
{
    public partial class CSVExport : Form
    {
        public IEnumerable<Game> Games; 
        public CSVExport(IEnumerable<Game> games)
        {
            Games = games;
            InitializeComponent();
            LoadFromSettings();
        }

        public void LoadFromSettings()
        {
            string[] values = Properties.Settings.Default.csvItems.Split(',');
            listCSV.Items.Clear();
            listAvailable.Items.Clear();
            string[] ss = Enum.GetNames(typeof (CsvItems));
            List<string> strs = new List<string>(ss);

            foreach (string s in values)
            {
                if (s == string.Empty)
                    break;
                string val = ss[int.Parse(s)];
                listCSV.Items.Add(val);
                strs.Remove(val);
            }
            foreach (string s in strs)
            {
                listAvailable.Items.Add(s);
            }
        }

        public void SaveFromSetting()
        {
            List<string> ss = new List<string>(Enum.GetNames(typeof(CsvItems)));
            StringBuilder bld=new StringBuilder();
            bool first = true;
            foreach (string s in listCSV.Items)
            {
                int v = ss.IndexOf(s);
                if (!first)
                    bld.Append(",");
                bld.Append(v);
                first = false;
            }
            Properties.Settings.Default.csvItems = bld.ToString();
            Properties.Settings.Default.Save();
        }

        public enum CsvItems
        {
            Title,
            TitleId,
            Genres,
            Date,
            Type,
            Disc,
            Developer,
            Publisher,
            WebPopulated,
            AbgxStatus,
            IsActive,
            Regions,
            XEXCrc,
            GameCrc,
            MediaId,
            XKeyId,
            FullPath,
            PartialPath,
            DateDay,
            DateMonth,
            DateYear,
            NumberOfDiscs,
            Size,
            RegionCode,
            Trailer
        }

        private void butMovIn_Click(object sender, EventArgs e)
        {
            if (listAvailable.SelectedItem != null)
            {
                object item = listAvailable.SelectedItem;
                listCSV.Items.Add(item);
                listAvailable.Items.Remove(item);
            }
        }

        private void butMovOut_Click(object sender, EventArgs e)
        {
            if (listCSV.SelectedItem != null)
            {
                object item = listCSV.SelectedItem;
                listAvailable.Items.Add(item);
                listCSV.Items.Remove(item);
                
            }
        }

        private void butMovDown_Click(object sender, EventArgs e)
        {
            if (listCSV.SelectedItem != null)
            {
                object item = listCSV.SelectedItem;
                int idx = listCSV.Items.IndexOf(item);
                if (idx < listCSV.Items.Count - 1)
                {
                    listCSV.Items.Remove(item);
                    listCSV.Items.Insert(idx + 1,item);
                    listCSV.SelectedItem = item;
                }
            }
        }

        private void butMovUp_Click(object sender, EventArgs e)
        {
            if (listCSV.SelectedItem != null)
            {
                object item = listCSV.SelectedItem;
                int idx = listCSV.Items.IndexOf(item);
                if (idx >0)
                {
                    listCSV.Items.Remove(item);
                    listCSV.Items.Insert(idx - 1, item);
                    listCSV.SelectedItem = item;
                }
            }
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            if (listCSV.Items.Count > 0)
            {
                SaveFileDialog dialog=new SaveFileDialog();
                dialog.DefaultExt = "csv";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SaveCSV(dialog.FileName);
                }
            }
        }

        private void SaveCSV(string fname)
        {

            CsvData data=new CsvData();
            CsvRecord header=new CsvRecord();
            List<CsvItems> items=new List<CsvItems>();
            foreach (string s in listCSV.Items)
            {
                header.Fields.Add(s);
                items.Add((CsvItems)Enum.Parse(typeof (CsvItems), s));
            }
            data.Header = header;
            foreach (Game g in Games)
            {
                CsvRecord record=new CsvRecord();
                data.Records.Add(record);
                foreach (CsvItems c in items)
                {
                    switch (c)
                    {
                        case CsvItems.Title:
                            record.Fields.Add(g.Title);
                            break;
                        case CsvItems.TitleId:
                            record.Fields.Add(g.TitleId);
                            break;
                        case CsvItems.Genres:
                            record.Fields.Add(g.Genre);
                            break;
                        case CsvItems.Date:
                            record.Fields.Add(g.Date.ToString(CultureInfo.CurrentCulture));
                            break;
                        case CsvItems.Type:
                            record.Fields.Add(g.Type);
                            break;
                        case CsvItems.Disc:
                            record.Fields.Add(g.Disc == 9999 ? "Expansion" : g.Disc.ToString(CultureInfo.CurrentCulture));
                            break;
                        case CsvItems.Developer:
                            record.Fields.Add(g.Developer);
                            break;
                        case CsvItems.Publisher:
                            record.Fields.Add(g.Publisher);
                            break;
                        case CsvItems.WebPopulated:
                            record.Fields.Add(g.WebPopulated ? "1" : "0");
                            break;
                        case CsvItems.AbgxStatus:
                            switch (g.AbgxStats)
                            {
                                case 0:
                                    record.Fields.Add("Not Checked");
                                    break;
                                case 1:
                                    record.Fields.Add("Ok");
                                    break;
                                case 2:
                                    record.Fields.Add("Unverified");
                                    break;
                                default:
                                    record.Fields.Add("Error");
                                    break;
                            }
                            break;
                        case CsvItems.IsActive:
                            record.Fields.Add(g.Active ? "1" : "0");
                            break;
                        case CsvItems.Regions:
                            record.Fields.Add(g.Regions);
                            break;
                        case CsvItems.XEXCrc:
                            record.Fields.Add(g.Abgx != null ? g.Abgx.XEXCRC.Crc.ToString("X8") : string.Empty);
                            break;
                        case CsvItems.GameCrc:
                            record.Fields.Add(g.Abgx != null ? g.Abgx.GameCRC.Crc.ToString("X8") : string.Empty);
                            break;
                        case CsvItems.MediaId:
                            record.Fields.Add(g.MediaId);
                            break;
                        case CsvItems.XKeyId:
                            record.Fields.Add(g.ID);
                            break;
                        case CsvItems.FullPath:
                            record.Fields.Add(g.FullIsoPath);
                            break;
                        case CsvItems.PartialPath:
                            record.Fields.Add(g.FullIsoPath.Substring(g.GameDirectoy.Length+1));
                            break;
                        case CsvItems.DateDay:
                            record.Fields.Add(g.Date.Day.ToString(CultureInfo.CurrentCulture));
                            break;
                        case CsvItems.DateMonth:
                            record.Fields.Add(g.Date.Month.ToString(CultureInfo.CurrentCulture));
                            break;
                        case CsvItems.DateYear:
                            record.Fields.Add(g.Date.Year.ToString(CultureInfo.CurrentCulture));
                            break;
                        case CsvItems.NumberOfDiscs:
                            record.Fields.Add(g.NumberOfDiscs < 1
                                ? "1"
                                : (g.NumberOfDiscs==9999) ? "1" : g.NumberOfDiscs.ToString(CultureInfo.CurrentCulture));
                            break;
                        case CsvItems.Size:
                            record.Fields.Add(g.Size.ToString(CultureInfo.CurrentCulture));
                            break;
                        case CsvItems.RegionCode:
                            record.Fields.Add(g.RegionCode);
                            break;
                        case CsvItems.Trailer:
                            record.Fields.Add(g.Trailer);
                            break;
                    }
                }
            }
            File.WriteAllText(fname, CsvEncoder.Encode(data));
            SaveFromSetting();
            this.Close();
        }
    }
}
