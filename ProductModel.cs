using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryptlexLicensingApp
{
    public class ProductModel 
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string TotalLicenses { get; set; }
        public string TotalReleases { get; set; }
        public string TotalProductVersions { get; set; }
        public string TotalTrialActivations { get; set; }
        public string CreatedAt { get; set; }


    }
}
