using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JHOEC.Models
{
    public partial class Crop
    {
        public Crop()
        {
            Variety = new HashSet<Variety>();
        }

        public int CropId { get; set; }
        [Display(Name ="Crop Name")]
        public string Name { get; set; }
        public string Image { get; set; }

        public ICollection<Variety> Variety { get; set; }
    }
}
