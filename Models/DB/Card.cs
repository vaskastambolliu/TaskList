using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DB
{
    public partial class Card
    {
        public int CardID { get; set; }
        public DateTime CardNoBarcode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Point { get; set; }
        public string BarCodeDownload { get; set; }

    }
}
