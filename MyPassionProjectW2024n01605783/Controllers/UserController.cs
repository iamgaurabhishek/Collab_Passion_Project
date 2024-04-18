using MyPassionProjectW2024n01605783.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyPassionProjectW2024n01605783.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        // GET: User/List
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static UserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
        }

        public ActionResult List()
        {
            // Description: Retrieves a list of users from the API and displays them
            // Parameters: None
            // Returns: View containing a list of users
            // semester 2
            // assume we only can talk to the API through an HTTP request using an HTTP client in C# to gather the User data.
            //

            // we have our http client object


            //set the path to the resource
            string url = "Userdata/listUsers";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // we should try to digest this response into something we can use
            // digest it into an User data transfer object
            List<UserDto> Users = response.Content.ReadAsAsync<List<UserDto>>().Result;

            return View(Users);
        }

        public ActionResult Details(int id)
        {
            // Description: Retrieves details of a specific user based on the provided ID from the API and displays them
            // Parameters: ID of the user to retrieve details for
            // Returns: View containing details of the specified user
            //objective: communicate with our User data api to retrieve one User
            //curl https://localhost:44301/api/Userdata/findUser/{id}

            string url = "Userdata/findUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            Debug.WriteLine("User to do : ");
            Debug.WriteLine(selectedUser.UserName);


            return View(selectedUser);
        }

        public ActionResult Error()
        {
            // Description: Displays an error view
            // Parameters: None
            // Returns: Error view
            return View();
        }

        // GET: User/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            // Description: Displays a form to create a new user
            // Parameters: None
            // Returns: View for creating a new user
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(User User)
        {
            // Description: Sends a request to the API to create a new user
            // Parameters: User object containing details of the user to be created
            // Returns: Redirects to the list of users after creation if successful, otherwise redirects to an error view
        
            //objective: add a new User into our system using the API
            //curl -H "Content-Type:application/json" -d @User.json https://localhost:44324/api/Userdata/addUser 
            string url = "Userdata/addUser";


            string jsonpayload = jss.Serialize(User);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: User/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            // Description: Displays a form to edit details of a specific user
            // Parameters: ID of the user to edit
            // Returns: View for editing the specified user
            //grab the User information

            //objective: communicate with our User data api to retrieve one User
            //curl https://localhost:44324/api/Userdata/findUser/{id}

            string url = "userdata/findUser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            //Debug.WriteLine("User received : ");
            //Debug.WriteLine(selectedUser.UserName);

            return View(selectedUser);
        }

        // POST: User/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, User User)
        {
            // Description: Sends a request to the API to update details of a specific user
            // Parameters: ID of the user to update, User object containing updated details
            // Returns: Redirects to the details view of the user after update if successful, otherwise redirects to an error view
            try
            {
                Debug.WriteLine("The new User info is:");
                Debug.WriteLine(User.UserName);
                Debug.WriteLine(User.UserDescription);
       
                //serialize into JSON
                //Send the request to the API

                string url = "userdata/UpdateUser/" + id;


                string jsonpayload = jss.Serialize(User);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/UserData/UpdateUser/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            // Description: Displays a confirmation view before deleting a specific user
            // Parameters: ID of the user to delete
            // Returns: Confirmation view for deleting the specified user
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selectedUser);
        }

        // POST: User/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            // Description: Sends a request to the API to delete a specific user
            // Parameters: ID of the user to delete
            // Returns: Redirects to the list of users after deletion if successful, otherwise redirects to an error view
            string url = "userdata/deleteuser/" + id;
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