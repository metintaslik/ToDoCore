using Couchbase.Core;
using System.Threading.Tasks;
using ToDo.API.Models;

namespace ToDo.API.Services
{
    public interface ICoreService
    {
        ResponseModel<User> LogIn(User entity, IBucket bucket);
        Task<ResponseModel<User>> CreateOrUpdateUserAsync(User entity, IBucket bucket);
        Task<ResponseModel<Category>> CreateOrUpdateCategoryAsync(Category entity, IBucket bucket);
        Task<ResponseModel<Todo>> CreateOrUpdateTodoAsync(Todo entity, IBucket bucket);
    }
}
