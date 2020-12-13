using Microsoft.AspNetCore.Diagnostics;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASPNETCore5Demo.Controllers
{
    
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("/error")]
        [Obsolete]
        public IActionResult Index([FromServices] IHostEnvironment webhostingEnviroment)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = feature.Error;
            var IsDev = webhostingEnviroment.IsDevelopment();
            var problemDetails = new ProblemDetail
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = IsDev ? ex.StackTrace : null,
                Title = IsDev ? $"{ex.GetType().Name}:{ex.Message}" : "An error occurred",
                Instance = feature?.Path,
                Content = "this day"
            };
            return StatusCode(problemDetails.Status.Value,problemDetails);
        }
    }



    public class ProblemDetail
    {
        public int? Status { get; set; }
        public string Instance { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Content{ get; set; }
    }
}
