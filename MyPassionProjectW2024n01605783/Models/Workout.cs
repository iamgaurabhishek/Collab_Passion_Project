using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyPassionProjectW2024n01605783.Models;

namespace MyPassionProjectW2024n01605783.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public string WorkoutDescription { get; set;}
        public string WorkoutDay { get; set; }

        public string WorkoutStatus { get; set; }

        public ICollection<Exercise> Exercises { get; set; }
   
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
    public class WorkoutDto
    {
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set;}
        public string WorkoutDescription { get; set;}
        public string WorkoutDay { get; set; }
        public string WorkoutStatus { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}

        