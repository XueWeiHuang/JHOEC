using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JHClassLibrary;



namespace JHOEC.Models
{
    [ModelMetadataType(typeof(FarmMetadata))]

    public partial class Farm : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            OECContext _context = JHOECContext_Singleton.Context();
            //trim empty spaces for all fields

            if (!string.IsNullOrEmpty(PostalCode))
                PostalCode = PostalCode.Trim();
            if (!string.IsNullOrEmpty(HomePhone))
                HomePhone = HomePhone.Trim();
            if (!string.IsNullOrEmpty(CellPhone))
                CellPhone = CellPhone.Trim();
            if (!string.IsNullOrEmpty(Email))
                Email = Email.Trim();
            if (!string.IsNullOrEmpty(Directions))
                Directions = Directions.Trim();

            //capitalize name
            if (!string.IsNullOrEmpty(Name.Trim()))
            {
                Name.Trim();
                Name = JHValidations.JHCapitalize(Name);

            }
            else
            {
                yield return new ValidationResult("Name cannot be empty strings", new string[] { nameof(Name) });
            }

            //capitalize address
            if (!string.IsNullOrEmpty(Address))
            {
                Address = Address.Trim();
                Address = JHValidations.JHCapitalize(Address);
            }
            //else
            //{
            //    yield return new ValidationResult("Address cannot be empty strings", new string[] { nameof(Address)});
            //}
            //capitalize town
            if (!string.IsNullOrEmpty(Town))
            {
                Town = Town.Trim();
                Town = JHValidations.JHCapitalize(Town);
            }
            //else
            //{
            //    yield return new ValidationResult("Town cannot be empty strings", new string[] { nameof(Town) });
            //}

            //capitalize county
            if (!string.IsNullOrEmpty(County))
            {
                County = County.Trim();
                County = JHValidations.JHCapitalize(County);
            }
            //else
            //{
            //    yield return new ValidationResult("County cannot be empty strings", new string[] { nameof(County) });
            //}

            //either town or county must be provided
            if (string.IsNullOrEmpty(County) && string.IsNullOrEmpty(Town))
            {
                yield return new ValidationResult("Either County or Town must be provided", new[] { nameof(Town), nameof(County) });
            }

            //if email is not provided, address and postal must be provided
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(PostalCode))
            {
                yield return new ValidationResult("Either Email or Address and Postal Code must be provided", new[] { nameof(Address), nameof(PostalCode), nameof(Email) });
            }

            //force province code to upper case
            if (string.IsNullOrEmpty(ProvinceCode))
            {
                ProvinceCode = ProvinceCode.Trim();
                ProvinceCode = ProvinceCode.ToUpper();
            }

            //validate postcode based on country
            if (_context.Province.Where(p => p.ProvinceCode == ProvinceCode).Select(p => p.CountryCode).FirstOrDefault() == "CA")
            {
                var postalCodeCheck = PostalCode;
                var validateResult = false;
                validateResult = JHValidations.JHPostalCodeValidation(ref postalCodeCheck);
                if (validateResult == true)
                {
                    PostalCode = postalCodeCheck;
                }
                else
                {
                    yield return new ValidationResult(" Postal Code is invalid. The format doesnt match 'A3B C4B'", new[] { nameof(PostalCode) });

                }
            }
            else if (_context.Province.Where(p => p.ProvinceCode == ProvinceCode).Select(p => p.CountryCode).FirstOrDefault() == "US")
            {
                var postalCodeCheck = PostalCode;
                var validateResult = false;
                validateResult = JHValidations.JHZipCodeValidation(ref postalCodeCheck);
                if (validateResult == true)
                {
                    PostalCode = postalCodeCheck;
                }
                else
                {
                    yield return new ValidationResult(" Zip Code is invalid. The format doesnt match '123456' or '12345-6789'", new[] { nameof(PostalCode) });
                }
            }

            //last contacted date cannot before join date, AND cannot exist if the farm hasn't joined yet
            if (DateJoined == null && LastContactDate != null)
                yield return new ValidationResult("Last contacted date doesnt exist if the farm hasn't joined yet", new[] { nameof(LastContactDate), nameof(DateJoined) });
            if (DateJoined != null && LastContactDate != null)
            {
                if (DateJoined > LastContactDate)
                    yield return new ValidationResult("Last contaced date cound not exist before the join date", new[] { nameof(LastContactDate), nameof(DateJoined) });
            }

            //validate phone number
            if (string.IsNullOrEmpty(CellPhone) && string.IsNullOrEmpty(HomePhone))
            {
                yield return new ValidationResult("Either cell phone or home phone must be provided", new[] { nameof(CellPhone), nameof(HomePhone) });
            }
            if (!string.IsNullOrEmpty(CellPhone))
            {

                var phoneCheck = CellPhone;
                var validateResult = false;
                validateResult = JHValidations.JHPhoneValidate(ref phoneCheck);
                if (validateResult == true)
                {
                    CellPhone = phoneCheck;
                }
                else
                {
                    yield return new ValidationResult(" Cell phone number is invalid. The format doesnt match '123-456-7890'", new[] { nameof(CellPhone) });

                }
            }
            if (!string.IsNullOrEmpty(HomePhone))
            {
                var phoneCheck = HomePhone;
                var validateResult = false;
                validateResult = JHValidations.JHPhoneValidate(ref phoneCheck);
                if (validateResult == true)
                {
                    HomePhone = phoneCheck;
                }
                else
                {
                    yield return new ValidationResult(" Home phone number is invalid. The format doesnt match '123-456-7890'", new[] { nameof(HomePhone) });
                }
            }
            yield return ValidationResult.Success;
        }
    }
          
    public class FarmMetadata
    {
        public int FarmId { get; set; }
        [Display(Name ="Farm Name")]
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        [Display(Name = "Province Code")]
        [Required]
        [Remote("CheckProvinceCode", "Remotes")]
        public string ProvinceCode { get; set; }
        [Display(Name = "Postal Code")]
        //[JHPostalCodeValidation]
        //[JHZipCodeValidation]

        public string PostalCode { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Directions { get; set; }
        [Display(Name = "Date Joined")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]

        [JHDateNotInFuture]
        public DateTime? DateJoined { get; set; }

        [Display(Name = "Last Contact")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [JHDateNotInFuture]
        public DateTime? LastContactDate { get; set; }
        [Display(Name = "Province")]
        public Province ProvinceCodeNavigation { get; set; }
        public ICollection<Plot> Plot { get; set; }

    }
}
