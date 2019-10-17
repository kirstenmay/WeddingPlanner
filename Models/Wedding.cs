using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeddingPlanner.Models;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}
        [Required]
        [MinLength(3)]
        public string WedderOne {get;set;}
        [Required]
        [MinLength(3)]
        public string WedderTwo {get;set;}
        [Required]
        [DataType(DataType.Date)]
        [PastDate]
        public DateTime Date {get;set;}
        [Required]
        public string Address {get;set;}
        public DateTime Created_at {get;set;}
        public DateTime Updated_at {get;set;}
        public int Creator {get;set;}
        public List<Attendee> Guests {get;set;}
    }
}
public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if((DateTime)value <= DateTime.Now)
            {
                return new ValidationResult("Date must be in the future");
            }
            return ValidationResult.Success;
        }
    }