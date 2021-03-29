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
        private readonly IBucket usersBucket;
        private readonly IBucket categoriesBucket;
        private readonly IBucket todosBucket;
        public ValuesController(ICoreService service, IBucketProvider bucketProvider)
        {
            this.service = service;
            usersBucket = bucketProvider.GetBucket("users");
            categoriesBucket = bucketProvider.GetBucket("categories");
            todosBucket = bucketProvider.GetBucket("todos");
        }

        [HttpPost]
        [Route("/")]
        public IActionResult Login([FromBody] User entity)
        {
            return Content(JsonConvert.SerializeObject(service.LogIn(entity, usersBucket)), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("/User")]
        public async Task<IActionResult> Register([FromBody] User entity)
        {
            return Content(JsonConvert.SerializeObject(await service.CreateOrUpdateUserAsync(entity, usersBucket)), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("/Category")]
        public async Task<IActionResult> Category([FromBody] Category entity)
        {
            return Content(JsonConvert.SerializeObject(await service.CreateOrUpdateCategoryAsync(entity, categoriesBucket)), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("/Todo")]
        public async Task<IActionResult> Todo([FromBody] Todo entity)
        {
            return Content(JsonConvert.SerializeObject(await service.CreateOrUpdateTodoAsync(entity, todosBucket)), "application/json", Encoding.UTF8);
        }
    }
}
