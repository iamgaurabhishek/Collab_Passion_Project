using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProjectW2024n01605783.Models.ViewModel
{
    public class WorkoutList
    {
        public bool IsAdmin { get; set; }
        public IEnumerable<WorkoutDto> Workouts { get; set; }
    }
}