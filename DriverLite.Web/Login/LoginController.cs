using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DriverLite.Web.Login
{
    public class LoginDetails
    {
        public string Email { get; set; }
        public string Vehicle { get; set; }
        public string Password { get; set; }
    }

    public class LoginController : ApiController
    {
        public LoginController()
        {
            Debug.WriteLine("LoginController()");
        }

        [HttpGet]
        public IHttpActionResult Status()
        {
            return Ok("Running! " + DateTimeOffset.Now.ToString("O"));
        }

        [HttpPost]
        public HttpResponseMessage Login(LoginDetails loginDetails)
        {
            Debug.WriteLine(loginDetails.Email + ", " + loginDetails.Vehicle + ", " + loginDetails.Password);

            return Request.CreateResponse(HttpStatusCode.OK, new { Version = "1.0.0" });
        }
    }
}