using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Profile.Models
{
    [MetadataType(typeof(Profile_Validations))]
    public partial class Profile
    {
        //
    }

    public class Profile_Validations
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a name")]
        [MinLength(2)]
        [MaxLength(30)]
        public string first_name { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter last name")]
        [MinLength(2)]
        [MaxLength(30)]
        public string last_name { get; set; }
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter a username")]
        [MinLength(2)]
        [MaxLength(30)]
        public string user_name { get; set; }
        [Display(Name = "Website")]
        public string website { get; set; }
        [Display(Name = "Biography")]
        [MaxLength(500)]
        public string biography { get; set; }
        [Display(Name = "Email Address")]
        [EmailAddress]
        [Required(ErrorMessage = "Please enter an email address")]
        public string email_address { get; set; }
        [Display(Name = "Phone Number")]

        public string phone_number { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Display(Name = "Profile Picture")]
        public Nullable<int> profile_picture { get; set; }

    }

    [MetadataType(typeof(User_Validations))]
    public partial class User
    {
        //
    }

    public class User_Validations
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter your username")]
        [MinLength(1)]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(8, ErrorMessage = "Password must be minimum of 8 characters")]
        public string password { get; set; }
    }

    [MetadataType(typeof(Address_Validations))]
    public partial class Address
    {
        //
    }

    public class Address_Validations
    {
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Street Address")]
        public string street_address { get; set; }

        [Display(Name = "City")]
        public string city { get; set; }

        [Display(Name = "Province")]
        public string province { get; set; }

        [Display(Name = "Postal Code")]
        public string postal_code { get; set; }

        /*
        [Display(Name = "Country Code")]
        public string country_code { get; set; }
        */
    }
}