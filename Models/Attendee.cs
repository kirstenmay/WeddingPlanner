using System.ComponentModel.DataAnnotations;
using WeddingPlanner.Models;

namespace WeddingPlanner.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeId {get;set;}
        public int UserId {get;set;}
        public int WeddingId {get;set;}
        public User User {get;set;}
        public Wedding Wedding {get;set;}
    }
}