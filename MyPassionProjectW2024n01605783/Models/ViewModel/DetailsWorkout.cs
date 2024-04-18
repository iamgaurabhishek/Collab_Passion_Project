using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProjectW2024n01605783.Models.ViewModel
{
    public class DetailsWorkout
    {
        public bool IsAdmin { get; set; }
        public WorkoutDto SelectedWorkout { get; set; }
        public IEnumerable<WorkoutDto> ResponsibleExercises { get; set; }
    }
}