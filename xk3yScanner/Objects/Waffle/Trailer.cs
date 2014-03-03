using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace xk3yScanner.Objects.Waffle
{
    public class Trailer
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("url")]
        public string Url { get; set; }
        [XmlAttribute("author")]
        public string Author { get; set; }
        [XmlText]
        public string Title { get; set; }
    }
}
