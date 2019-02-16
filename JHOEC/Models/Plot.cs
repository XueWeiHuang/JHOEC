using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JHOEC.Models
{
    public partial class Plot
    {
        public Plot()
        {
            Treatment = new HashSet<Treatment>();
        }

        public int PlotId { get; set; }
        public int? FarmId { get; set; }
        public int? VarietyId { get; set; }
        [Display (Name ="Date Planted")]
        public DateTime? DatePlanted { get; set; }
        [Display(Name = "Date Harvested")]
        public DateTime? DateHarvested { get; set; }
        [Display(Name = "Planting Rate")]
        public int? PlantingRate { get; set; }
        [Display(Name = "Planting Rate By Pounds")]
        public bool PlantingRateByPounds { get; set; }
        [Display(Name = "Row Width")]
        public int? RowWidth { get; set; }
        [Display(Name = "Pattern Repeats")]
        public int? PatternRepeats { get; set; }
        [Display(Name = "Organic Matter")]
        public double? OrganicMatter { get; set; }
        public double? BicarbP { get; set; }
        public double? Potassium { get; set; }
        public double? Magnesium { get; set; }
        public double? Calcium { get; set; }
        public double? PHsoil { get; set; }
        public double? PHbuffer { get; set; }
        public double? Cec { get; set; }
        [Display(Name = "Percent Base Saturation K")]
        public double? PercentBaseSaturationK { get; set; }
        [Display(Name = "Percent Base Saturation Mg")]
        public double? PercentBaseSaturationMg { get; set; }
        [Display(Name = "Percent Base Saturation Ca")]
        public double? PercentBaseSaturationCa { get; set; }
        [Display(Name = "Percent Base Saturation H")]
        public double? PercentBaseSaturationH { get; set; }
        public string Comments { get; set; }

        public Farm Farm { get; set; }
        public Variety Variety { get; set; }
        public ICollection<Treatment> Treatment { get; set; }
    }
}
