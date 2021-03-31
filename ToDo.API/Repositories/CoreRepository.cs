using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.API.Helper;
using ToDo.API.Models;
using ToDo.API.Providers;
using ToDo.API.Services;

namespace ToDo.API.Repositories
{
    public class CoreRepository : ICoreService
    {
        private readonly IBucket usersBucket;
        private readonly IBucket categoriesBucket;
        private readonly IBucket todosBucket;

        public CoreRepository(IUsersBucketProvider uBucket, ICategoriesBucketProvider cBucket, ITodosBucketProvider tBucket)
        {
            usersBucket = uBucket.GetBucket();
            categoriesBucket = cBucket.GetBucket();
            todosBucket = tBucket.GetBucket();
        }

        private ResponseModel<T> CheckEntity<T>(T entity) where T : class
        {
            return new ResponseModel<T>
            {
                Code = 0x0001,
                Error = true,
                Message = "Hatalı istek biçimi."
            };
        }

        private ResponseModel<T> ThrowingCatch<T>(T entity, Exception e) where T : class
        {
            return new ResponseModel<T>
            {
                Code = 0x0002,
                Error = true,
                Message = $"Bir hata meydana geldi. {e.Message}"
            };
        }

        public ResponseModel<User> LogIn(User entity)
        {
            ResponseModel<User> response = new ResponseModel<User>();
            try
            {
                if (entity == null)
                    CheckEntity(entity);

                var context = new BucketContext(usersBucket);
                var user = context.Query<User>().FirstOrDefault(u => u.Username == entity.Username && u.Password == Hasher.Hash(entity.Password));
                if (user == null)
                {
                    response.Code = 0x0003;
                    response.Error = true;
                    response.Message = "Hatalı kullanıcı bilgileri eşleşmedi, lütfen tekrar deneyiniz.";
                    return response;
                }

                response.Entity = user;
                return response;
            }
            catch (Exception e)
            {
                return ThrowingCatch(entity, e);
            }
        }

        public async Task<ResponseModel<User>> CreateOrUpdateUserAsync(User entity)
        {
            ResponseModel<User> response = new ResponseModel<User>();
            try
            {
                if (entity == null)
                    CheckEntity(entity);

                if (entity.Id == null)
                {
                    entity.Id = Guid.NewGuid().ToString();
                    entity.CreateDateTime = DateTime.Now;
                    entity.Password = Hasher.Hash(entity.Password);
                    await usersBucket.InsertAsync(entity.Id, entity);
                    var userDocument = await usersBucket.GetDocumentAsync<User>(entity.Id);
                    response.Entity = userDocument.Content;
                }
                else
                {
                    var context = new BucketContext(usersBucket);
                    var user = context.Query<User>().FirstOrDefault(u => u.Id == entity.Id);
                    if (user == null)
                    {
                        response.Code = 0x0003;
                        response.Error = true;
                        response.Message = "Kullanıcı bilgileri eşleşmedi, lütfen tekrar deneyiniz.";
                    }
                    else
                    {
                        var userDocument = await usersBucket.GetDocumentAsync<User>(entity.Id);
                        userDocument.Content.NameSurname = entity.NameSurname;
                        userDocument.Content.Username = entity.Username;
                        userDocument.Content.Password = Hasher.Hash(entity.Password);
                        await usersBucket.UpsertAsync(userDocument.Document);
                        context.SubmitChanges();
                        response.Entity = userDocument.Content;
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                return ThrowingCatch(entity, e);
            }
        }

        public async Task<ResponseModel<Category>> CreateOrUpdateCategoryAsync(Category entity)
        {
            try
            {
                if (entity == null)
                    CheckEntity(entity);

                ResponseModel<Category> response = new ResponseModel<Category>();
                if (entity.Id == null)
                {
                    entity.Id = Guid.NewGuid().ToString();
                    entity.CreateDateTime = DateTime.Now;
                    entity.PinnedDateTime = (entity.IsPinned ? DateTime.Now : (DateTime?)null);
                    await categoriesBucket.InsertAsync(entity.Id, entity);
                    var categoryDocument = await categoriesBucket.GetDocumentAsync<Category>(entity.Id);
                    response.Entity = categoryDocument.Content;
                }
                else
                {
                    var context = new BucketContext(categoriesBucket);
                    var category = context.Query<Category>().FirstOrDefault(u => u.Id == entity.Id);
                    if (category == null)
                    {
                        response.Code = 0x0004;
                        response.Error = true;
                        response.Message = "Kategori bilgileri eşleşmedi, lütfen tekrar deneyiniz.";
                    }
                    else
                    {
                        var categoryDocument = await categoriesBucket.GetDocumentAsync<Category>(entity.Id);
                        categoryDocument.Content.Name = entity.Name;
                        categoryDocument.Content.IsPinned = entity.IsPinned;
                        categoryDocument.Content.PinnedDateTime = (entity.IsPinned ? DateTime.Now : (DateTime?)null);
                        await categoriesBucket.UpsertAsync(categoryDocument.Document);
                        context.SubmitChanges();
                        response.Entity = categoryDocument.Content;
                    }
                }
                return response;
            }
            catch (Exception e)
            {
                return ThrowingCatch(entity, e);
            }
        }

        public async Task<ResponseModel<Todo>> CreateOrUpdateTodoAsync(Todo entity)
        {
            try
            {
                if (entity == null)
                    CheckEntity(entity);

                ResponseModel<Todo> response = new ResponseModel<Todo>();
                if (entity.Id == null)
                {
                    entity.Id = Guid.NewGuid().ToString();
                    entity.CreateDateTime = DateTime.Now;
                    entity.PinnedDateTime = (entity.IsPinned ? DateTime.Now : (DateTime?)null);
                    await todosBucket.InsertAsync(entity.Id, entity);
                    var todoDocument = await todosBucket.GetDocumentAsync<Todo>(entity.Id);
                    response.Entity = todoDocument.Content;
                }
                else
                {
                    var context = new BucketContext(todosBucket);
                    var category = context.Query<Todo>().FirstOrDefault(u => u.Id == entity.Id);
                    if (category == null)
                    {
                        response.Code = 0x0005;
                        response.Error = true;
                        response.Message = "Yapılacak bilgileri eşleşmedi, lütfen tekrar deneyiniz.";
                    }
                    else
                    {
                        var todoDocument = await todosBucket.GetDocumentAsync<Todo>(entity.Id);
                        todoDocument.Content.Title = entity.Title;
                        todoDocument.Content.Content = entity.Content;
                        todoDocument.Content.IsPinned = entity.IsPinned;
                        todoDocument.Content.PinnedDateTime = (entity.IsPinned ? DateTime.Now : (DateTime?)null);
                        todoDocument.Content.Alert = entity.Alert;
                        todoDocument.Content.IsItDone = entity.IsItDone;
                        await todosBucket.UpsertAsync(todoDocument.Document);
                        context.SubmitChanges();
                        response.Entity = todoDocument.Content;
                    }
                }
                return response;
            }
            catch (Exception e)
            {
                return ThrowingCatch(entity, e);
            }
        }

        public ResponseModel<Category> GetCategory(Category entity)
        {
            var response = new ResponseModel<Category>();
            try
            {
                if (entity == null)
                    CheckEntity(entity);

                var context = new BucketContext(categoriesBucket);
                var category = context.Query<Category>().FirstOrDefault(c => c.Id == entity.Id);
                if (category == null)
                {
                    response.Code = 0x0006;
                    response.Error = true;
                    response.Message = "Kategori bilgisi eşleşmedi, lütfen tekrar deneyiniz.";
                }
                else
                {
                    response.Error = false;
                    response.Entity = category;
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ResponseModel<List<Category>> GetCategories()
        {
            var response = new ResponseModel<List<Category>>();
            try
            {
                var context = new BucketContext(categoriesBucket);
                var categories = context.Query<Category>().ToList();
                response.Error = false;
                response.Entity = categories;
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ResponseModel<Todo> GetTodo(Todo entity)
        {
            var response = new ResponseModel<Todo>();
            try
            {
                if (entity == null)
                    CheckEntity(entity);

                var context = new BucketContext(categoriesBucket);
                var category = context.Query<Todo>().FirstOrDefault(t => t.Id == entity.Id);
                if (category == null)
                {
                    response.Code = 0x0006;
                    response.Error = true;
                    response.Message = "Yapılacak bilgisi eşleşmedi, lütfen tekrar deneyiniz.";
                }
                else
                {
                    response.Error = false;
                    response.Entity = category;
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ResponseModel<List<Todo>> GetTodos()
        {
            var response = new ResponseModel<List<Todo>>();
            try
            {
                var context = new BucketContext(categoriesBucket);
                var categories = context.Query<Todo>().ToList();
                response.Error = false;
                response.Entity = categories;
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}