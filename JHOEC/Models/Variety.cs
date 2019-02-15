using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JHOEC.Models
{
    public partial class Variety
    {
        public Variety()
        {
            Plot = new HashSet<Plot>();
        }

        public int VarietyId { get; set; }
        public int? CropId { get; set; }
        [Display(Name="Variety Name")]
        public string Name { get; set; }
        [Display(Name="Crop Name")]
        public Crop Crop { get; set; }
        public ICollection<Plot> Plot { get; set; }
    }
}
