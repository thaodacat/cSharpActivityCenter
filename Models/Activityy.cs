using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using activityCenter.Validations;

namespace activityCenter.Models
{
    public class Activityy
    {
        [Key]
        public int ActivityyID {get; set;}


        [Required]
        public string Title {get; set;}

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime Date {get; set;}


        [Required]
        [DataType(DataType.Time)]
        public DateTime Time {get; set;}


        [Required]
        public string Description {get; set;}


        [Required]
        public string Duration {get; set;}


        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        public int UserId {get; set;}
        public User Organizer {get; set;}

        public List<Attend> Guests { get; set; }
    }
}