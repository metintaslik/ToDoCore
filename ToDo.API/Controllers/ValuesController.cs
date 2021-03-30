using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICoreService service;
        public ValuesController(ICoreService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("/")]
        public IActionResult Login([FromBody] User entity)
        {
            return Content(JsonConvert.SerializeObject(service.LogIn(entity)), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("/User")]
        public async Task<IActionResult> Register([FromBody] User entity)
        {
            return Content(JsonConvert.SerializeObject(await service.CreateOrUpdateUserAsync(entity)), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("/Category")]
        public async Task<IActionResult> Category([FromBody] Category entity)
        {
            return Content(JsonConvert.SerializeObject(await service.CreateOrUpdateCategoryAsync(entity)), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("/Todo")]
        public async Task<IActionResult> Todo([FromBody] Todo entity)
        {
            return Content(JsonConvert.SerializeObject(await service.CreateOrUpdateTodoAsync(entity)), "application/json", Encoding.UTF8);
        }
    }
}
