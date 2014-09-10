using System;
using System.Collections.Generic;

namespace EFWinFormsExample.Models
{
    public partial class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        public string Address { get; set; }
    }
}
