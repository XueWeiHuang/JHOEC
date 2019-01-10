using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JHOEC.Models
{
    public partial class Fertilizer
    {
        public Fertilizer()
        {
            TreatmentFertilizer = new HashSet<TreatmentFertilizer>();
        }
        [Required]
        [Display(Name = "Fertilizer Name")]
        public string FertilizerName { get; set; }
        [Display(Name ="OEC Products?")]
        public bool Oecproduct { get; set; }
        public bool Liquid { get; set; }

        public ICollection<TreatmentFertilizer> TreatmentFertilizer { get; set; }
    }
}
