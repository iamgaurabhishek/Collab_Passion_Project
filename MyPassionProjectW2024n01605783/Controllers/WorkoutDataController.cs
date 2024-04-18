using MyPassionProjectW2024n01605783.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MyPassionProjectW2024n01605783.Controllers
{
    public class WorkoutDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/WorkoutData/ListWorkouts
        // output a list of Workout in system.
        [HttpGet]
        [Route("api/Workoutdata/listWorkouts")]
        public List<WorkoutDto> ListWorkouts()
        {
            // Description: Retrieves a list of workouts from the database and maps them to WorkoutDto objects
            // Parameters: None
            // Returns: List of WorkoutDto objects containing details of each workout in the system
            List<Workout> Workouts = db.Workouts.ToList();

            List<WorkoutDto> WorkoutDtos = new List<WorkoutDto>();
            Workouts.ForEach(
                Workout => WorkoutDtos.Add(new WorkoutDto()
                {
                    WorkoutId = Workout.WorkoutId,
                    WorkoutName = Workout.WorkoutName,
                    WorkoutDay = Workout.WorkoutDay,

                    WorkoutStatus = Workout.WorkoutStatus,
                    WorkoutDescription = Workout.WorkoutDescription,

                    UserName = Workout.User.UserName,
                    UserId = Workout.User.UserId
                }));

            return WorkoutDtos;
        }
        // GET: api/WorkoutData/FindWorkout/5
        [ResponseType(typeof(Workout))]
        [HttpGet]
        [Route("api/Workoutdata/findWorkout/{id}")]
        public IHttpActionResult FindWorkout(int id)
        {
            // Description: Retrieves details of a specific workout based on the provided ID
            // Parameters: ID of the workout to retrieve details for
            // Returns: IHttpActionResult containing details of the specified workout as a WorkoutDto object if found, otherwise returns NotFound
            Workout Workout = db.Workouts.Find(id);
            WorkoutDto WorkoutDto = new WorkoutDto()
            {
                WorkoutId = Workout.WorkoutId,
                WorkoutName = Workout.WorkoutName,
                WorkoutDescription = Workout.WorkoutDescription,

                WorkoutDay = Workout.WorkoutDay,
                WorkoutStatus = Workout.WorkoutStatus,

                UserName = Workout.User.UserName,
                UserId = Workout.User.UserId
            };
            if (Workout == null)
            {
                return NotFound();
            }

            return Ok(WorkoutDto);
        }

        // POST: api/WorkoutData/UpdateWorkout/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/WorkoutData/UpdateWorkout/{id}")]
        public IHttpActionResult UpdateWorkout(int id, Workout Workout)
        {
            // Description: Updates an existing workout in the database with the provided ID
            // Parameters: ID of the workout to update, Workout object containing updated details
            // Returns: IHttpActionResult with NoContent status code if successful, BadRequest if the provided ID does not match the workout's ID, NotFound if the workout does not exist, or BadRequest if the model state is invalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Workout.WorkoutId)
            {

                return BadRequest();
            }

            db.Entry(Workout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/WorkoutData/AddWorkout
        [ResponseType(typeof(Workout))]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/WorkoutData/AddWorkout")]
        public IHttpActionResult AddWorkout(Workout Workout)
        {
            // Description: Adds a new workout to the database
            // Parameters: Workout object containing details of the workout to be added
            // Returns: IHttpActionResult with Ok status code if successful, BadRequest if the model state is invalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Workouts.Add(Workout);
            db.SaveChanges();

            return Ok();
        }

        // POST: api/WorkoutData/DeleteWorkout/5
        [ResponseType(typeof(Workout))]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/WorkoutData/DeleteWorkout/{id}")]
        public IHttpActionResult DeleteWorkout(int id)
        {
            // Description: Deletes a workout from the database based on the provided ID
            // Parameters: ID of the workout to delete
            // Returns: IHttpActionResult with Ok status code if successful, NotFound if the workout does not exist
            Workout Workout = db.Workouts.Find(id);
            if (Workout == null)
            {
                return NotFound();
            }

            db.Workouts.Remove(Workout);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            // Description: Disposes of the database context when the controller is disposed
            // Parameters: Boolean indicating whether or not to dispose of managed resources
            // Returns: None
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkoutExists(int id)
        {
            // Description: Checks if a workout with the provided ID exists in the database
            // Parameters: ID of the workout to check for existence
            // Returns: Boolean indicating whether or not the workout exists in the database
            return db.Workouts.Count(e => e.WorkoutId == id) > 0;
        }
    }
}
