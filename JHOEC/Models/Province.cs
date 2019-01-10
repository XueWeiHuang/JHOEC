using System;
using System.Collections.Generic;

namespace JHOEC.Models
{
    public partial class Province
    {
        public Province()
        {
            Farm = new HashSet<Farm>();
        }

        public string ProvinceCode { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string RetailTaxName { get; set; }
        public double? RetailTaxRate { get; set; }
        public bool? FederalTaxIncluded { get; set; }

        public ICollection<Farm> Farm { get; set; }
    }
}
