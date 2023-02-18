using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net_test
{
    internal record TableRecord
    {
        // Each field must match the database table column names + type 
        public int Id { get; set; }
        public string Imie { get; set; }
    }
}
