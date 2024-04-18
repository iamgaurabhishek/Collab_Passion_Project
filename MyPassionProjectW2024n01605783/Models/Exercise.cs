using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPassionProjectW2024n01605783.Models
{
    public class Exercise
    {
        // what describe an exercise
        [Key]
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set;}
        public string ExerciseDescription { get; set;}
        public int NumberOfSets {  get; set; }
        // a workout has many exercises
        // WorkoutId in Exercise DB
        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public virtual Workout Workout { get; set; }

    }
    // how we can represent our own version of exercise to serve in a webapi
    public class ExerciseDto
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set;}
        public string ExerciseDescription { get; set; }
        public int NumberOfSets { get; set; }
        // Workout table
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set;}
        public string WorkoutDay {  get; set; }

    }
}