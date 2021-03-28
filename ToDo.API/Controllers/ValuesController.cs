using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.API.Helper;
using ToDo.API.Models;

namespace ToDo.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IBucket bucket;
        public ValuesController(IBucketProvider bucketProvider)
        {
            bucket = bucketProvider.GetBucket("users");
        }

        [HttpPost]
        [Route("/")]
        public IActionResult Login([FromBody] User entity)
        {
            var response = new ResponseModel<User>();
            if (entity == null)
            {
                response.Code = 0x0001;
                response.Error = true;
                response.Message = "Hatalı istek biçimi.";
                return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
            }
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://localhost:8091") }
            });
            cluster.Authenticate("Administrator", "123456");
            var bucket = cluster.OpenBucket("users");
            var context = new BucketContext(bucket);
            var user = context.Query<User>().FirstOrDefault(u => u.Username == entity.Username && u.Password == Hasher.Hash(entity.Password));
            if (user == null)
            {
                response.Code = 0x0002;
                response.Error = false;
                response.Message = "Hatalı kullanıcı bilgileri, lütfen tekrar deneyiniz.";
                return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
            }
            response.Error = false;
            response.Entity = user;
            return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register([FromBody] User entity)
        {
            var response = new ResponseModel<User>();
            if (entity == null)
            {
                response.Code = 0x0001;
                response.Error = true;
                response.Message = "Hatalı istek biçimi.";
                return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
            }

            entity.Id = Guid.NewGuid().ToString();
            entity.CreateDateTime = DateTime.Now;
            entity.Password = Hasher.Hash(entity.Password);
            await bucket.InsertAsync(entity.Id, entity);
            response.Error = false;
            response.Entity = entity;
            return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
        }
    }
}
