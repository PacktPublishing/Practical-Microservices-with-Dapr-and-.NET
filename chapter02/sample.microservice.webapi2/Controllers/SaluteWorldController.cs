using Dapr;
using Microsoft.AspNetCore.Mvc;
using System;

namespace sample.microservice.webapi.Controllers
{
    [ApiController]
    public class SaluteWorldController : ControllerBase
    {
        [HttpGet("salute")]
        public ActionResult<string> Get()
        {
            Console.WriteLine("I salute you, my dear World.");
            return "I salute you, my dear World.";
        }
    }
}