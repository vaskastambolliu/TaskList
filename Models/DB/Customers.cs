using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.DB
{
    public partial class Customers
    {

        public int CustomersId { get; set; }
        public int CardID { get; set; }
        public int CardNoBarcode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Points { get; set; }
    }
}
