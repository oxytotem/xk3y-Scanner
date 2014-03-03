using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using BrightIdeasSoftware;
using xk3yScanner.xkeyBrew.IsoGameReader;

namespace xk3yScanner.Objects.Waffle
{
    [XmlRoot("gameinfo")]
    [Serializable]
    public class GameInfo
    {

        [XmlElement("titleid")]
        [OLVColumn(Width = 60, MinimumWidth = 60,MaximumWidth = 60,DisplayIndex = 1)]
        public string TitleId { get; set; }
        [OLVColumn(Width = 150,MinimumWidth = 100, FreeSpaceProportion = 40, FillsFreeSpace = true,DisplayIndex = 0)]
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("summary")]
        public string Summary { get; set; }

        [XmlArray("info")]
        [XmlArrayItem("infoitem")]
        public List<InfoItem> Items { get; set; }

        [XmlArray("otherinfo")]
        [XmlArrayItem("infoitem")]
        public List<InfoItem> OtherItems { get; set; }

        [XmlElement("boxart")]
        public byte[] Cover { get; set; }

        [XmlElement("banner")]
        public byte[] Banner { get; set; }


        [XmlArray("trailers")]
        [XmlArrayItem("trailer")]
        public List<Trailer> Trailers { get; set; }

        [XmlElement("mediaid")]
        public string MediaId { get; set; }
        [OLVColumn("MediaId", Width = 60, MinimumWidth = 60, MaximumWidth = 60, DisplayIndex = 2)]
        public string ShortMediaId
        {
            get
            {
                return MediaId.Substring(24, 8);
            }
        }

        [XmlIgnore]
        public string FullIsoPath { get; set; }

        [XmlIgnore]
        public string BasePath { get; set; }

        [XmlIgnore]
        public string BaseName { get; set; }

        [XmlIgnore]
        public Iso Iso { get; set; }

        [XmlIgnore]
        [OLVColumn("Populated",MaximumWidth = 60,MinimumWidth = 60,Width = 60, DisplayIndex = 10,TextAlign = HorizontalAlignment.Center,CheckBoxes = false,IsEditable = false)]
        public bool WebPopulated { get; set; }


    }
}