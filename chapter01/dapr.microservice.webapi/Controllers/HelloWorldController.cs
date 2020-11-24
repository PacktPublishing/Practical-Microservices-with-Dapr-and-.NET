using Dapr;
using Microsoft.AspNetCore.Mvc;
using System;

namespace dapr.microservice.webapi.Controllers
{
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet("hello")]
        public ActionResult<string> Get()
        {
            Console.WriteLine("Hello, World.");
            return "Hello, World";
        }
    }
}