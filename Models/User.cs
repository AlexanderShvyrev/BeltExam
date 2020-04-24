using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required (ErrorMessage = "First Name Must be Entered")]
        [Display (Name = "First Name : ")]
        [MinLength (2)]
        public string FirstName { get; set; }

        [Required (ErrorMessage = "Last Name Must be Entered")]
        [Display (Name = "Last Name : ")]
        [MinLength (2)]
        public string LastName { get; set; }

    
        [Required (ErrorMessage = "Email Must be Entered")]
        [Display (Name = "Email  : ")]
        [DataType (DataType.EmailAddress)]
        public string Email { get; set; }

        [Required (ErrorMessage = "Password Must be Entered")]
        [Display (Name = "Password  : ")]
        [DataType (DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [RegularExpression(@"^(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z])(?=\D*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]+", ErrorMessage="Password must contain upper case, lower case, number and a special character")]
        public string Password { get; set; }

        [Display (Name = "Confirm Password  : ")]
        [DataType (DataType.Password)]
        [Compare ("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<ToDo> MyToDos{get; set;}
        public List<Banana> MyAnswers{get; set;}
    }
}