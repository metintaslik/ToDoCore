using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        public ResponseModel<User> Login([FromBody] User entity)
        {
            return service.LogIn(entity);
        }

        [HttpPost]
        [Route("/User")]
        public async Task<ResponseModel<User>> Register([FromBody] User entity)
        {
            return await service.CreateOrUpdateUserAsync(entity);
        }

        [HttpPost]
        [Route("/Category")]
        public async Task<ResponseModel<Category>> PostCategory([FromBody] Category entity)
        {
            return await service.CreateOrUpdateCategoryAsync(entity);
        }

        [HttpPost]
        [Route("/Todo")]
        public async Task<ResponseModel<Todo>> PostTodo([FromBody] Todo entity)
        {
            return await service.CreateOrUpdateTodoAsync(entity);
        }

        [HttpGet]
        [Route("/Category")]
        public ResponseModel<Category> GetCategory(Category entity)
        {
            return service.GetCategory(entity);
        }

        [HttpGet]
        [Route("/Categories")]
        public ResponseModel<List<Category>> GetCategories()
        {
            return service.GetCategories();
        }

        [HttpGet]
        [Route("/Todo")]
        public ResponseModel<Todo> GetTodo(Todo entity)
        {
            return service.GetTodo(entity);
        }

        [HttpGet]
        [Route("/Todos")]
        public ResponseModel<List<Todo>> GetTodos()
        {
            return service.GetTodos();
        }
    }
}