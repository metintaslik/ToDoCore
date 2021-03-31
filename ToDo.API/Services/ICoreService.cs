using Couchbase.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.API.Models;

namespace ToDo.API.Services
{
    public interface ICoreService
    {
        ResponseModel<User> LogIn(User entity);
        Task<ResponseModel<User>> CreateOrUpdateUserAsync(User entity);
        Task<ResponseModel<Category>> CreateOrUpdateCategoryAsync(Category entity);
        ResponseModel<Category> GetCategory(Category entity);
        ResponseModel<List<Category>> GetCategories();
        Task<ResponseModel<Todo>> CreateOrUpdateTodoAsync(Todo entity);
        ResponseModel<Todo> GetTodo(Todo entity);
        ResponseModel<List<Todo>> GetTodos();
    }
}
