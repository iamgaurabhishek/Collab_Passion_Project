using MyPassionProjectW2024n01605783.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace MyPassionProjectW2024n01605783.Controllers
{
    
    public class BlogController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BlogController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
        }
        // GET: Blog/List
        public ActionResult List()
        {
            // Description: Retrieves a list of blogs from the API and displays them.
            // Parameters: None
            // Returns: View containing a list of blogs
            string url = "blogdata/listblogs";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // we should try to digest this response into something we can use
            // digest it into an Blog data transfer object
            List<BlogDto> Blogs = response.Content.ReadAsAsync<List<BlogDto>>().Result;

            return View(Blogs);
        }
        public ActionResult Details(int id)
        {
            // Description: Retrieves details of a specific blog based on the provided ID from the API and displays them.
            // Parameters: ID of the blog to retrieve.
            // Returns: View containing details of the specified blog.
            //objective: communicate with our Blog data api to retrieve one Blog
            //curl https://localhost:44301/api/blogdata/findblog/{id}

            string url = "blogdata/findblog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            BlogDto selectedblog = response.Content.ReadAsAsync<BlogDto>().Result;
            Debug.WriteLine("Blog to do : ");
            Debug.WriteLine(selectedblog.BlogHeading);


            return View(selectedblog);
        }

        public ActionResult Error()
        {
            // Description: Displays an error view.
            // Parameters: None
            // Returns: Error View
            return View();
        }

        // GET: Blog/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            // Description: Displays a form to create a new blog
            // Parameters: None
            // Returns: View for creating a new blog
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Blog blog)
        {
            // Description: Sends a request to the API to create a new blog
            // Parameters: Blog object containing details of the blog to be created
            // Returns: Redirects to the list of blogs after creation
          
            //objective: add a new Blog into our system using the API
            //curl -H "Content-Type:application/json" -d @Blog.json https://localhost:44324/api/blogdata/addblog 
            string url = "blogdata/addblog";


            string jsonpayload = jss.Serialize(blog);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            return RedirectToAction("List");

        }

        // GET: Blog/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            // Description: Displays a form to edit details of a specific blog
            // Parameters: ID of the blog to edit
            // Returns: View for editing the specified blog

            //grab the Blog information

            //objective: communicate with our Blog data api to retrieve one Blog
            //curl https://localhost:44324/api/blogdata/findblog/{id}

            string url = "blogdata/findblog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BlogDto selectedblog = response.Content.ReadAsAsync<BlogDto>().Result;

            return View(selectedblog);
        }

        // POST: Blog/Update/5

        [HttpPost]
        [Route("Blog/Update/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Blog blog)
        {
            // Description: Sends a request to the API to update details of a specific blog
            // Parameters: ID of the blog to update, Blog object containing updated details
            // Returns: Redirects to the list of blogs after update
            try
            {
                string url = "blogdata/Updateblog/" + id;
                //serialize into JSON
                string jsonpayload = jss.Serialize(blog);
                Debug.WriteLine(jsonpayload);
                //Send the request to the API
                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // the below code is redirecting the page to "List" page.
                return RedirectToAction("List/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Blog/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            // Description: Displays a confirmation view before deleting a specific blog
            // Parameters: ID of the blog to delete
            // Returns: Confirmation view for deleting the specified blog
            string url = "blogdata/findblog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BlogDto selectedblog = response.Content.ReadAsAsync<BlogDto>().Result;
            return View(selectedblog);
        }

        // POST: Blog/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            // Description: Sends a request to the API to delete a specific blog
            // Parameters: ID of the blog to delete
            // Returns: Redirects to the list of blogs after deletion, or to an error view if deletion fails
            string url = "blogdata/deleteblog/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}