using Couchbase.Core;
using System.Threading.Tasks;
using ToDo.API.Models;

namespace ToDo.API.Services
{
    public interface ICoreService
    {
        ResponseModel<User> LogIn(User entity);
        Task<ResponseModel<User>> CreateOrUpdateUserAsync(User entity);
        Task<ResponseModel<Category>> CreateOrUpdateCategoryAsync(Category entity);
        Task<ResponseModel<Todo>> CreateOrUpdateTodoAsync(Todo entity);
    }
}
