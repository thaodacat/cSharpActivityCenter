using System.ComponentModel.DataAnnotations;

namespace activityCenter.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string LoginEmail { get; set;}

        [DataType(DataType.Password)]
        [Required]
        public string LoginPassword {get;set;}
    }
}