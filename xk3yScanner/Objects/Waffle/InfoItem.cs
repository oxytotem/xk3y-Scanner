
using System.Xml.Serialization;

namespace xk3yScanner.Objects.Waffle
{
    public class InfoItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
