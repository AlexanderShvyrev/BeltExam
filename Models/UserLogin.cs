using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class UserLogin
    {
        [EmailAddress]
        [Required(ErrorMessage="Email is required")]
        public string LoginEmail {get; set;}

        [DataType(DataType.Password)]
        [Required(ErrorMessage="Enter your password")]
        public string LoginPassword { get; set;}
    }
}