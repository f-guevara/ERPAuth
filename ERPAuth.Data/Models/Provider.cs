using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAuth.Data.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public string Code { get; set; } // Unique code for the provider
        public string Name { get; set; }
    }

}
