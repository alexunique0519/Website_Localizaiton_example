using OEC06.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OEC06.Models
{
    [MetadataType(typeof(farmMetadata))]
    public partial class farm : IValidatableObject
    {
        OECContext db = new OECContext();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (town == null) town = ""; else town = town.Trim();
            if (county == null) county = ""; else county = county.Trim();
            if (town == "" && county == "")
                yield return new ValidationResult(string.Format(YYTranslations.oneTownOrCountyShouldBeProvided), new [] { "town", "county" });
            if (provinceCode != null)
            {
                provinceCode = provinceCode.ToUpper();
                var province = db.provinces.Find(provinceCode);
                if (province == null)
                    yield return new ValidationResult(string.Format(YYTranslations.provinceNotOnFile, provinceCode), new[] { "provinceCode" });
            }
            if (postalCode != null)
            {
                postalCode = postalCode.ToUpper();
                if (postalCode.Length == 6) postalCode = postalCode.Insert(3, " ");
            }
            if (homePhone != null)
            {
                string newPhone ="";
                foreach (char item in homePhone)
                    if (item >= '0' && item <= '9') newPhone += item;
                if (newPhone.Length != 10)
                    yield return new ValidationResult(string.Format(YYTranslations.homePhoneDigits), new[] { "homePhone" });
                else
                    homePhone = newPhone.Insert(3, "-").Insert(7, "-");
            }
            if (cellPhone != null)
            {
                string newPhone = "";
                foreach (char item in cellPhone)
                    if (item >= '0' && item <= '9') newPhone += item;
                if (newPhone.Length != 10)
                    yield return new ValidationResult(string.Format(YYTranslations.cellPhoneDigits), new[] { "cellPhone" });
                else
                    cellPhone = newPhone.Insert(3, "-").Insert(7, "-");
            }

            if (dateJoined != null && (dateJoined > DateTime.Now))
                    yield return new ValidationResult(string.Format(YYTranslations.XDateCannotBeInFuture, YYTranslations.dateJoined), new[] { "dateJoined" });
            if (lastContactDate != null)
            {
                if (lastContactDate > DateTime.Now)
                    yield return new ValidationResult(string.Format(YYTranslations.XDateCannotBeInFuture, YYTranslations.lastContactDate), new[] { "lastContactDate" });
                if (dateJoined == null)
                    yield return new ValidationResult(string.Format(YYTranslations.dataJoinedMustBeGiven), new[] { "dateJoined, lastContactDate" });
                else if (lastContactDate < dateJoined)
                    yield return new ValidationResult(string.Format(YYTranslations.FarmCannotBeContactedBeforeJoin), new[] { "lastContactDate" });
            }
        }
    }
    public class farmMetadata
    {
        [Display(Name = "farmId", ResourceType=typeof(YYTranslations))]
        [DisplayFormat(DataFormatString = "{0:d5}")]
        public int farmId { get; set; }

        [Required(ErrorMessageResourceType=typeof(YYTranslations), ErrorMessageResourceName="Required")]
        [Display(Name = "name", ResourceType=typeof(YYTranslations))]
        public string name { get; set; }

        [Display(Name = "address", ResourceType=typeof(YYTranslations))]
        public string address { get; set; }

        [Display(Name = "town", ResourceType = typeof(YYTranslations))]
        public string town { get; set; }

        [Display(Name = "county", ResourceType = typeof(YYTranslations))]
        public string county { get; set; }

        
        //[RegularExpression(@"[a-zA-Z]{2}", ErrorMessageResourceType=typeof(YYTranslations), ErrorMessageResourceName="provinceCodeReg")]
        [Remote("ValidateProvinceCode", "Remote")]
        [Display(Name = "provinceCode", ResourceType = typeof(YYTranslations))]
        public string provinceCode { get; set; }

        
        [Display(Name = "postalCode", ResourceType = typeof(YYTranslations))]
        [Required(ErrorMessageResourceType=typeof(YYTranslations), ErrorMessageResourceName="Required")]
        [RegularExpression(@"^[a-zA-Z]\d[a-zA-Z] ?\d[a-zA-Z]\d$", ErrorMessageResourceType=typeof(YYTranslations), ErrorMessageResourceName="postalCodeReg")]
        public string postalCode { get; set; }

        [Display(Name = "homePhone", ResourceType = typeof(YYTranslations))]
        public string homePhone { get; set; }

        [Display(Name = "cellPhone", ResourceType = typeof(YYTranslations))]
        public string cellPhone { get; set; }

        [Display(Name = "direction", ResourceType = typeof(YYTranslations))]
        [DataType(DataType.MultilineText)]
        public string directions { get; set; }

        [Display(Name = "dateJoined", ResourceType = typeof(YYTranslations))]
        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString="{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> dateJoined { get; set; }

        [Display(Name = "lastContactDate", ResourceType = typeof(YYTranslations))]
        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> lastContactDate { get; set; }
    }
}