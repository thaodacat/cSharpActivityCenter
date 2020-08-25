using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace activityCenter.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required(ErrorMessage="First Name Required")]
        [MinLength(2, ErrorMessage="Must be at least 2 characters!")]
        public string FirstName {get;set;}

        [Required(ErrorMessage="Last Name Required")]
        [MinLength(2, ErrorMessage="Must be at least 2 characters!")]
        public string LastName {get;set;}

        [EmailAddress]
        [Required(ErrorMessage="Email Required")]
        public string Email {get;set;}

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage = "Password must contain at least 1 capital letter, 1 lower case letter, 1 number, and a special character")]
        // [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        // Will not be mapped to your users table!
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
    }
}