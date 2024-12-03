using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quik_BookingApp.Modal
{
    public class GoogleConfiguration
    {
        public string? ClientID { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class FirebaseConfiguration
    {
        public string? ApiKey { get; set; }
        public string? AuthEmail { get; set; }
        public string? AuthPassword { get; set; }
        public string? Bucket { get; set; }

    }

}
