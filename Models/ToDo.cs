using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class ToDo
    {
        [Key]
        public int ToDoId{get; set;}
        [Required]
        public string Title{get; set;}
        [Required]
        public string Description{get; set;}
        [Required]
        public int Duration{get; set;}
        [Required]
        
        public string StringDuration{get; set;}
        public int UserId { get; set; }
        [Required]
        [FutureDate]
        [DataType (DataType.DateTime)]
        [Display(Name="Date")]
        public DateTime ToDoDate{get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public User ToDoCreator{get; set;}
        public List<Banana> Participants{get; set;}

    }
    public class FutureDateAttribute : ValidationAttribute {
        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            DateTime toDoDate = (DateTime) value;
            if (toDoDate < DateTime.Now) 
            {
                return new ValidationResult ("Please Enter a Valid Future Date and Time!");
            } 
            else 
            {
                return ValidationResult.Success;
            }
        }

    }
}