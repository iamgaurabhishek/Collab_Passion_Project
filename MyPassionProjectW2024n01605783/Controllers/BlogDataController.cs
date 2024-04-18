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
    public class BlogDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: api/BlogData/ListBlogs
        // output a list of blog in system.
        [HttpGet]
        [Route("api/blogdata/listblogs")]
        public List<BlogDto> ListBlogs()
        {
            // Description: Retrieves a list of blogs from the database and returns them as a list of BlogDto objects
            // Parameters: None
            // Returns: List of BlogDto objects representing the blogs
            // Retrive all blogs from the database
            List<Blog> Blogs = db.Blogs.ToList();

            // Create a list of store BlogDTO objects
            List<BlogDto> BlogDtos = new List<BlogDto>();
            // Transform each Blog object into BlogDtos and add to the list
            Blogs.ForEach(
                blog => BlogDtos.Add(new BlogDto()
                {
                    BlogId = blog.BlogId,
                    BlogHeading = blog.BlogHeading,
                    BlogAuthor = blog.BlogAuthor,
                    BlogContent = blog.BlogContent,
                    BlogPublishedDate = blog.BlogPublishedDate,
                    BlogShortDescription = blog.BlogShortDescription,

                }));

            // return the list of BlogDto
            return BlogDtos;
        }


        /// <summary>
        /// Returns Blog base on id
        /// </summary>
        /// <param name="BlogId">The ID to find workout</param>
        /// <returns></returns>
        // GET: api/BlogData/FindBlog/5
        [ResponseType(typeof(Blog))]
        [HttpGet]
        [Route("api/blogdata/findblog/{id}")]
        public IHttpActionResult FindBlog(int id)
        {
            // Description: Retrieves details of a specific blog based on the provided ID from the database and returns it as a BlogDto object.
            // Parameters: ID of the blog to retrieve
            // Returns: IHttpActionResult containing the details of the specified blog as a BlogDto object

            Blog blog = db.Blogs.Find(id);
            // gets all the elements of Blog into BlogDto
            BlogDto BlogDto = new BlogDto()
            {
                BlogId = blog.BlogId,
                BlogHeading = blog.BlogHeading,
                BlogAuthor = blog.BlogAuthor,
                BlogContent = blog.BlogContent,
                BlogPublishedDate = blog.BlogPublishedDate,
                BlogShortDescription = blog.BlogShortDescription,

                ExerciseId = blog.Exercise.ExerciseId,
            };

            if (blog == null)
            {
                return NotFound();
            }

            return Ok(BlogDto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        // POST: api/BlogData/UpdateBlog/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/BlogData/UpdateBlog/{id}")]
        public IHttpActionResult UpdateBlog(int id, Blog blog)
        {
            // Description: Updates the details of a specific blog in the database based on the provided ID
            // Parameters: ID of the blog to update, Blog object containing updated details
            // Returns: IHttpActionResult indicating the status of the update operation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.BlogId)
            {

                return BadRequest();
            }

            db.Entry(blog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
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

        // POST: api/BlogData/AddBlog
        [ResponseType(typeof(Blog))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/BlogData/AddBlog")]
        public IHttpActionResult AddBlog(Blog Blog)
        {
            // Description: Adds a new blog to the database
            // Parameters: Blog object containing details of the blog to be added
            // Returns: IHttpActionResult indicating the status of the add operation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Blogs.Add(Blog);
            db.SaveChanges();

            return Ok();
        }

        [ResponseType(typeof(Blog))]
        [HttpGet]
        [Route("api/BlogData/ListBlogsForExercise/{id}")]
        public IHttpActionResult ListBlogsForExercise(int id)
        {
            // Description: Retrieves a list of blogs associated with a specific exercise from the database and returns them as a list of BlogDto objects
            // Parameters: ID of the exercise
            // Returns: IHttpActionResult containing the list of blogs associated with the specified exercise as BlogDto objects
            List<Blog> blogs = db.Blogs.Where(b => b.ExerciseId == id).ToList();
            List<BlogDto> blogDtos = new List<BlogDto>();
            Console.WriteLine(blogs);

            foreach (var blog in blogs)
            {
                blogDtos.Add(new BlogDto()
                {
                    BlogId = blog.BlogId,
                    BlogAuthor = blog.BlogAuthor,
                    BlogContent = blog.BlogContent,
                    BlogHeading = blog.BlogHeading,
                    BlogPublishedDate = blog.BlogPublishedDate,
                    ExerciseId = blog.Exercise.ExerciseId,
                });
            }
            return Ok(blogDtos);
        }

        // POST: api/BlogData/DeleteBlog/5
        [ResponseType(typeof(Blog))]
        [HttpPost]
        [Route("api/BlogData/DeleteBlog/{id}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteBlog(int id)
        {
            // Description: Deletes a specific blog from the database based on the provided ID
            // Parameters: ID of the blog to delete
            // Returns: IHttpActionResult indicating the status of the delete operation
            Blog Blog = db.Blogs.Find(id);
            if (Blog == null)
            {
                return NotFound();
            }

            db.Blogs.Remove(Blog);
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

        private bool BlogExists(int id)
        {
            // Description: Checks if a blog with the provided ID exists in the database
            // Parameters: ID of the blog to check
            // Returns: Boolean indicating whether the blog exists in the database
            return db.Blogs.Count(e => e.BlogId == id) > 0;
        }
    }
}
