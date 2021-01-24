using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Models.DB
{
    public partial class Login
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
