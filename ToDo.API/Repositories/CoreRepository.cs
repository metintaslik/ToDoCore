using Couchbase.Core;
using Couchbase.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDo.API.Helper;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.API.Repositories
{
    public class CoreRepository : ICoreService
    {
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

        public ResponseModel<User> LogIn(User entity, IBucket bucket)
        {
            ResponseModel<User> response = new ResponseModel<User>();
            try
            {
                if (entity == null)
                    CheckEntity(entity);

                var context = new BucketContext(bucket);
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

        public async Task<ResponseModel<User>> CreateOrUpdateUserAsync(User entity, IBucket bucket)
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
                    await bucket.InsertAsync(entity.Id, entity);
                    var userDocument = await bucket.GetDocumentAsync<User>(entity.Id);
                    response.Entity = userDocument.Content;
                }
                else
                {
                    var context = new BucketContext(bucket);
                    var user = context.Query<User>().FirstOrDefault(u => u.Id == entity.Id);
                    if (user == null)
                    {
                        response.Code = 0x0003;
                        response.Error = true;
                        response.Message = "Kullanıcı bilgileri eşleşmedi, lütfen tekrar deneyiniz.";
                    }
                    else
                    {
                        var userDocument = await bucket.GetDocumentAsync<User>(entity.Id);
                        userDocument.Content.NameSurname = entity.NameSurname;
                        userDocument.Content.Username = entity.Username;
                        userDocument.Content.Password = Hasher.Hash(entity.Password);
                        await bucket.UpsertAsync(userDocument.Document);
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

        public async Task<ResponseModel<Category>> CreateOrUpdateCategoryAsync(Category entity, IBucket bucket)
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
                    await bucket.InsertAsync(entity.Id, entity);
                    var categoryDocument = await bucket.GetDocumentAsync<Category>(entity.Id);
                    response.Entity = categoryDocument.Content;
                }
                else
                {
                    var context = new BucketContext(bucket);
                    var category = context.Query<Category>().FirstOrDefault(u => u.Id == entity.Id);
                    if (category == null)
                    {
                        response.Code = 0x0004;
                        response.Error = true;
                        response.Message = "Kategori bilgileri eşleşmedi, lütfen tekrar deneyiniz.";
                    }
                    else
                    {
                        var categoryDocument = await bucket.GetDocumentAsync<Category>(entity.Id);
                        categoryDocument.Content.Name = entity.Name;
                        categoryDocument.Content.IsPinned = entity.IsPinned;
                        categoryDocument.Content.PinnedDateTime = (entity.IsPinned ? DateTime.Now : (DateTime?)null);
                        await bucket.UpsertAsync(categoryDocument.Document);
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

        public async Task<ResponseModel<Todo>> CreateOrUpdateTodoAsync(Todo entity, IBucket bucket)
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
                    await bucket.InsertAsync(entity.Id, entity);
                    var todoDocument = await bucket.GetDocumentAsync<Todo>(entity.Id);
                    response.Entity = todoDocument.Content;
                }
                else
                {
                    var context = new BucketContext(bucket);
                    var category = context.Query<Todo>().FirstOrDefault(u => u.Id == entity.Id);
                    if (category == null)
                    {
                        response.Code = 0x0005;
                        response.Error = true;
                        response.Message = "Yapılacak bilgileri eşleşmedi, lütfen tekrar deneyiniz.";
                    }
                    else
                    {
                        var todoDocument = await bucket.GetDocumentAsync<Todo>(entity.Id);
                        todoDocument.Content.Title = entity.Title;
                        todoDocument.Content.Content = entity.Content;
                        todoDocument.Content.IsPinned = entity.IsPinned;
                        todoDocument.Content.PinnedDateTime = (entity.IsPinned ? DateTime.Now : (DateTime?)null);
                        todoDocument.Content.Alert = entity.Alert;
                        todoDocument.Content.IsItDone = entity.IsItDone;
                        await bucket.UpsertAsync(todoDocument.Document);
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
    }
}