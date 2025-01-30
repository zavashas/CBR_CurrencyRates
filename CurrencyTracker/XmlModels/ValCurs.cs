using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyTracker.XmlModels
{
    [XmlRoot("ValCurs")]
    public class ValCurs
    {
        [XmlAttribute("Date")]
        public string Date { get; set; }
        [XmlElement("Valute")]
        public List<Valute> Valutes { get; set; }
    }
}
