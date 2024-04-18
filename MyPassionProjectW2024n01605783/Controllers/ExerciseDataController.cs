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
    public class ExerciseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/ExerciseData/ListExercises
        // output a list of exercise in system.
        [HttpGet]
        [Route("api/exercisedata/listexercises")]
        public List<ExerciseDto> ListExercises() 
        {
            // Description: Retrieves a list of exercises from the database and returns them as a list of ExerciseDto objects
            // Parameters: None
            // Returns: List of ExerciseDto objects representing the exercises
            // Retrive all exercises from the database
            List<Exercise> Exercises =  db.Exercises.ToList();

            // Create a list of store ExerciseDTO objects
            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();
            // Transform each Exercise object into ExerciseDtos and add to the list
            Exercises.ForEach(
                exercise => ExerciseDtos.Add(new ExerciseDto() 
            {
                ExerciseId = exercise.ExerciseId,
                ExerciseName = exercise.ExerciseName,

                WorkoutName = exercise.Workout.WorkoutName,
                WorkoutDay = exercise.Workout.WorkoutDay,
                WorkoutId = exercise.Workout.WorkoutId,

                NumberOfSets = exercise.NumberOfSets,
                ExerciseDescription = exercise.ExerciseDescription,
               
                }));

            // return the list of ExerciseDto
            return ExerciseDtos;
        }
        

        /// <summary>
        /// Returns Exercise base on id
        /// </summary>
        /// <param name="ExerciseId">The ID to find workout</param>
        /// <returns></returns>
        // GET: api/ExerciseData/FindExercise/5
        [ResponseType(typeof(Exercise))]
        [HttpGet]        
        [Route("api/exercisedata/findexercise/{id}")]
        public IHttpActionResult FindExercise(int id)
        {
            //Description: Retrieves details of a specific exercise based on the provided ID from the database and returns it as an ExerciseDto object
            // Parameters: ID of the exercise to retrieve details 
            // Returns: IHttpActionResult containing the details of the specified exercise as an ExerciseDto object
            Exercise exercise = db.Exercises.Find(id);
            // gets all the elements of Exercise into ExerciseDto
            ExerciseDto ExerciseDto = new ExerciseDto()
            {
                ExerciseId = exercise.ExerciseId,
                ExerciseName = exercise.ExerciseName,

                ExerciseDescription = exercise.ExerciseDescription,

                WorkoutName = exercise.Workout.WorkoutName,
                WorkoutDay = exercise.Workout.WorkoutDay,
                WorkoutId= exercise.Workout.WorkoutId,

                NumberOfSets = exercise.NumberOfSets,
                
            };

            if (exercise == null)
            {
                return NotFound();
            }
            
            return Ok(ExerciseDto);
        }

        /// <summary>
        /// Returns all Exercises in the system associated with a particular workout.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Exercises in the database taking care of a particular workout
        /// </returns>
        /// <param name="id">Workout Primary Key</param>
        /// <example>
        /// GET: api/KeeperData/ListExercisesForWorkout/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExerciseDto))]
        public IHttpActionResult ListExercisesForWorkout(int id)
        {
            List<Exercise> Exercises = db.Exercises.Where(
                k => k.Workout.Any(
                    a => a.AnimalID == id)
                ).ToList();
            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();

            Exercises.ForEach(e => ExerciseDtos.Add(new ExerciseDto()
            {
                ExerciseId = e.ExerciseId,
                ExerciseName = e.ExerciseName,
                ExerciseDescription = e.ExerciseDescription
            }));

            return Ok(ExerciseDtos);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exercise"></param>
        /// <returns></returns>
        // POST: api/ExerciseData/UpdateExercise/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/ExerciseData/UpdateExercise/{id}")]
        public IHttpActionResult UpdateExercise(int id, Exercise exercise)
        {
            // Description: Updates the details of a specific exercise in the database based on the provided ID
            // Parameters: ID of the exercise to update, Exercise object containing updated details
            // Returns: IHttpActionResult indicating the status of the update operation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exercise.ExerciseId)
            {

                return BadRequest();
            }

            db.Entry(exercise).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
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

        // POST: api/ExerciseData/AddExercise
        [ResponseType(typeof(Exercise))]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/ExerciseData/AddExercise")]
        public IHttpActionResult AddExercise(Exercise Exercise)
        {
            // Description: Adds a new exercise to the database
            // Parameters: Exercise object containing details of the exercise to be added
            // Returns: IHttpActionResult indicating the status of the add operation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exercises.Add(Exercise);
            db.SaveChanges();

            return Ok();
        }

        // POST: api/ExerciseData/DeleteExercise/5
        [ResponseType(typeof(Exercise))]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/ExerciseData/DeleteExercise/{id}")]
        public IHttpActionResult DeleteExercise(int id)
        {
            // Description: Deletes a specific exercise from the database based on the provided ID
            // Parameters: ID of the exercise to delete
            // Returns: IHttpActionResult indicating the status of the delete operation
            Exercise Exercise = db.Exercises.Find(id);
            if (Exercise == null)
            {
                return NotFound();
            }

            db.Exercises.Remove(Exercise);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            // Description: Disposes of the resources used by the controller
            // Parameters: Boolean indicating whether disposing has been done
            // Returns: None
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseExists(int id)
        {
            // Description: Checks if an exercise with the provided ID exists in the database
            // Parameters: ID of the exercise to check
            // Returns: Boolean indicating whether the exercise exists in the database
            return db.Exercises.Count(e => e.ExerciseId == id) > 0;
        }
    }
}