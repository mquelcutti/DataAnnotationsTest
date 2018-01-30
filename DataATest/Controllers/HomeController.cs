using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataATest.ViewModels;


using System;
using Microsoft.Extensions.Logging;

namespace IconicChannels.Controllers
{
    /// <summary>
    /// HomeController Class
    /// </summary>
    public class HomeController : Controller
    {
       

        //use if we need to use  controller specific wording
        //ensure resource key uses the controller class name in the DB
        //private readonly IStringLocalizer<HomeController> _homeLocalizer;


        
        /// <summary>
     

        /// <summary>
        /// Index 
        /// </summary>
        /// <returns>ViewResult</returns>
        [AllowAnonymous]
        public ViewResult Index()
        {
            

            return View();
        }

        /// <summary>
        ///  Contact 
        /// </summary>
        /// <returns>ViewResult</returns>
        [HttpGet("contact")]
        public IActionResult Contact()
        {

            return View();
        }

        /// <summary>
        /// Emails service to send details supplied via the model
        /// </summary>
        /// <param name="model">The contact model </param>
        /// <returns>IActionResult</returns>
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {

            return View();
        }

       

        
    }
}
