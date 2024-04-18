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
    public class UserDataController : ApiController
    {
            private ApplicationDbContext db = new ApplicationDbContext();
            // GET: api/UserData/ListUsers
            // output a list of User in system.
            [HttpGet]
            [Route("api/Userdata/listUsers")]
            public List<UserDto> ListUsers()
            {
            // Description: Retrieves a list of users from the database and transforms them into DTOs for API response
            // Parameters: None
            // Returns: List of UserDto objects containing details of users
            List<User> Users = db.Users.ToList();

                List<UserDto> UserDtos = new List<UserDto>();
                Users.ForEach(
                    User => UserDtos.Add(new UserDto()
                    {
                        UserId = User.UserId,
                        UserName = User.UserName,
                        UserAge = User.UserAge,
                        UserWeight = User.UserWeight
                    }));

                return UserDtos;
            }
            // GET: api/UserData/FindUser/5
            [HttpGet]
            [ResponseType(typeof(User))]
            [Route("api/Userdata/findUser/{id}")]
            public IHttpActionResult FindUser(int id)
            {
            // Description: Retrieves details of a specific user based on the provided ID
            // Parameters: ID of the user to retrieve details
            // Returns: IHttpActionResult containing details of the specified user as a UserDto object, or NotFound if the user is not found
            User User = db.Users.Find(id);
                UserDto UserDto = new UserDto()
                {
                    UserId = User.UserId,
                    UserName = User.UserName,
                    UserDescription = User.UserDescription, 
                    UserAge = User.UserAge,
                    UserWeight = User.UserWeight

                };
                if (User == null)
                {
                    return NotFound();
                }

                return Ok(UserDto);
            }

            // POST: api/UserData/UpdateUser/5
            [ResponseType(typeof(void))]
            [Authorize(Roles = "Admin")]
            [HttpPost]
            [Route("api/Userdata/UpdateUser/{id}")]
            public IHttpActionResult UpdateUser(int id, User User)
            {
            // Description: Updates details of a specific user in the database
            // Parameters: ID of the user to update, User object containing updated details
            // Returns: IHttpActionResult indicating success (NoContent) or failure (BadRequest, NotFound) based on the update operation
            if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != User.UserId)
                {

                    return BadRequest();
                }

                db.Entry(User).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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

            // POST: api/UserData/AddUser
            [ResponseType(typeof(User))]
            [Authorize(Roles = "Admin")]
            [HttpPost]
            [Route("api/UserData/AddUser")]
            public IHttpActionResult AddUser(User User)
            {
            // Description: Adds a new user to the database
            // Parameters: User object containing details of the user to be added
            // Returns: IHttpActionResult indicating success (Ok) or failure (BadRequest) based on the addition operation
            if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Users.Add(User);
                db.SaveChanges();

                return Ok();
            }

            // POST: api/UserData/DeleteUser/5
            [ResponseType(typeof(User))]
            [Authorize(Roles = "Admin")]
            [HttpPost]
            [Route("api/UserData/DeleteUser/{id}")]
            public IHttpActionResult DeleteUser(int id)
            {
            // Description: Deletes a specific user from the database
            // Parameters: ID of the user to delete
            // Returns: IHttpActionResult indicating success (Ok) or failure (NotFound) based on the deletion operation
            User User = db.Users.Find(id);
                if (User == null)
                {
                    return NotFound();
                }

                db.Users.Remove(User);
                db.SaveChanges();

                return Ok();
            }

            protected override void Dispose(bool disposing)
            {
            // Description: Disposes the database context when the controller is disposed
            // Parameters: Boolean flag indicating whether to dispose managed resources
            // Returns: Void
            if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private bool UserExists(int id)
            {
            // Description: Checks if a user with the specified ID exists in the database
            // Parameters: ID of the user to check existence fo
            // Returns: True if the user exists, otherwise False
            return db.Users.Count(e => e.UserId == id) > 0;
            }
    }
}