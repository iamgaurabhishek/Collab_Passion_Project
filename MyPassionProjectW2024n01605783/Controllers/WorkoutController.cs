using MyPassionProjectW2024n01605783.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MyPassionProjectW2024n01605783.Models.ViewModel;
namespace MyPassionProjectW2024n01605783.Controllers
{
    public class WorkoutController : Controller
    {
        // GET: Workout/List
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44301/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Workout/List?PageNum={PageNum}
        public ActionResult List(int PageNum = 0)
        {
            // Description: Retrieves a list of workouts from the API and displays them in a view
            // Parameters: None
            // Returns: View containing a list of WorkoutDto objects
            // semester 2
            // assume we only can talk to the API through an HTTP request using an HTTP client in C# to gather the Workout data.
            //

            // we have our http client object

            WorkoutList ViewModel = new WorkoutList();
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

            //set the path to the resource
            string url = "workoutdata/listworkouts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<WorkoutDto> workouts = response.Content.ReadAsAsync<IEnumerable<WorkoutDto>>().Result;

            // -- Start of Pagination Algorithm --

            // Find the total number of players
            int AnimalCount = workouts.Count();
            // Number of players to display per page
            int PerPage = 4;
            // Determines the maximum number of pages (rounded up), assuming a page 0 start.
            int MaxPage = (int)Math.Ceiling((decimal)AnimalCount / PerPage) - 1;

            // Lower boundary for Max Page
            if (MaxPage < 0) MaxPage = 0;
            // Lower boundary for Page Number
            if (PageNum < 0) PageNum = 0;
            // Upper Bound for Page Number
            if (PageNum > MaxPage) PageNum = MaxPage;

            // The Record Index of our Page Start
            int StartIndex = PerPage * PageNum;

            //Helps us generate the HTML which shows "Page 1 of ..." on the list view
            ViewData["PageNum"] = PageNum;
            ViewData["PageSummary"] = " " + (PageNum + 1) + " of " + (MaxPage + 1) + " ";

            // -- End of Pagination Algorithm --

            //Send another request to get the page slice of the full list
            url = "AnimalData/ListAnimalsPage/" + StartIndex + "/" + PerPage;
            response = client.GetAsync(url).Result;

            // Retrieve the response of the HTTP Request
            IEnumerable<WorkoutDto> SelectedAnimalsPage = response.Content.ReadAsAsync<IEnumerable<WorkoutDto>>().Result;

            ViewModel.Workouts = SelectedAnimalsPage;

            return View(ViewModel);
            // we should try to digest this response into something we can use
            // digest it into an Workout data transfer object
        }

        public ActionResult Details(int id)
        {
            // Description: Retrieves details of a specific workout based on the provided ID and displays them in a view
            // Parameters: ID of the workout to retrieve details
            // Returns: View containing details of the specified workout as a WorkoutDto object
            //objective: communicate with our animal data api to retrieve one animal
            //curl https://localhost:44301/api/Workoutdata/findWorkout/{id}

            DetailsWorkout ViewModel = new DetailsWorkout();

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;
            // Retrieve the Workout from the database along with its associated Exercises
            string url = "Workoutdata/findWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            Debug.WriteLine("Workout to do : ");
            Debug.WriteLine(selectedWorkout.WorkoutName);

            ViewModel.SelectedWorkout = selectedWorkout;
            // show associated exercises with this workout
            url = "exercisedata/listexercisesforworokout/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ExerciseDto>ResponsibleExercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;

            ViewModel.ResponsibleExercises = ResponsibleExercises;
            return View(selectedWorkout);
        }

        public ActionResult Error()
        {
            // Description: Displays an error view
            // Parameters: None
            // Returns: Error view
            return View();
        }

        // GET: Workout/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            // Description: Displays a form to create a new workout
            // Parameters: None
            // Returns: View for creating a new workout
            return View();
        }

        // POST: Workout/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Workout Workout)
        {
            // Description: Sends a POST request to the API to add a new workout to the system
            // Parameters: Workout object containing details of the workout to be added
            // Returns: Redirects to the list of workouts if successful, otherwise redirects to an error view
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(workout.WorkoutName);
            //objective: add a new animal into our system using the API
            //curl -H "Content-Type:application/json" -d @animal.json https://localhost:44324/api/animaldata/addanimal 
            string url = "Workoutdata/addWorkout";


            string jsonpayload = jss.Serialize(Workout);

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

        // GET: Workout/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            // Description: Displays a form to edit an existing workout
            // Parameters: ID of the workout to edit
            // Returns: View for editing the specified workout

            //grab the Workout information

            string url = "Workoutdata/findWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            WorkoutDto selectedworkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            //Debug.WriteLine("workout received : ");
            //Debug.WriteLine(selectedworkout.WorkoutName);

            return View(selectedworkout);
        }

        // POST: Workout/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Workout/Update/{id}")]
        public ActionResult Update(int id, Workout workout)
        {
            // Description: Sends a POST request to the API to update an existing workout in the system
            // Parameters: ID of the workout to update, Workout object containing updated details
            // Returns: Redirects to the details view of the updated workout if successful, otherwise returns to the edit view
            try
            {
                Debug.WriteLine("The new animal info is:");
                Debug.WriteLine(workout.WorkoutStatus);
                Debug.WriteLine(workout.UserId);
                Debug.WriteLine(workout.WorkoutDay);

                //serialize into JSON
                //Send the request to the API

                string url = "WorkoutData/UpdateWorkout/" + id;


                string jsonpayload = jss.Serialize(workout);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/AnimalData/UpdateAnimal/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;




                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }
        // GET: workout/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            // Description: Displays a confirmation view before deleting a workout
            // Parameters: ID of the workout to delete
            // Returns: Confirmation view for deleting the specified workout
            string url = "workoutdata/findworkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto selectedworkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            return View(selectedworkout);
        }

        // POST: workout/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            // Description: Sends a POST request to the API to delete a workout from the system
            // Parameters: ID of the workout to delete
            // Returns: Redirects to the list of workouts if successful, otherwise redirects to an error view
            string url = "workoutdata/deleteworkout/" + id;
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