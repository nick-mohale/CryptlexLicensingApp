using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptlexLicensingApp
{
    public class LicenseModel
    {
        public string ID { get; set; }
        public string Key { get; set; }
        public string productId { get; set; }
        public string Validity { get; set; }
        public string TotalActivations { get; set; }
        public string LeaseDuration { get; set; }
        public OrganizationModel Organization { get; set; }
        public string Name { get; set; }
        public string createdAt { get; set; }
   
    }

    public class OrganizationModel
    {
        public string reseller { get; set; }
        public string name { get; set; } // Organization name property
        public string email { get; set; }
        public string description { get; set; }
        public int allowedUsers { get; set; }
    }
}
