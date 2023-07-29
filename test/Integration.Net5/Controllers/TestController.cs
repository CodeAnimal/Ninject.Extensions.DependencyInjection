using System;
using Integration.Net5.Services;
using Integration.Net5.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Net5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController(IServiceB serviceB)
        {
            ServiceB = serviceB;
        }

        public IServiceB ServiceB { get; }

        [HttpGet]
        public ActionResult<string> Get() => ServiceB.GetFromSubService(10);
    }
}
