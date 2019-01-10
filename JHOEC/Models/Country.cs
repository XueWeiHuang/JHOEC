using System;
using System.Collections.Generic;

namespace JHOEC.Models
{
    public partial class Country
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public string PostalPattern { get; set; }
        public string PhonePattern { get; set; }
        public string RetailTaxName { get; set; }
        public double? RetailTaxRate { get; set; }
    }
}
