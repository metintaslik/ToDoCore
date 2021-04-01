using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.API.Controllers;
using ToDo.API.Helper;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.Test
{
    class UserTest
    {
        private Mock<ICoreService> service;
        private List<User> userList;

        [SetUp]
        public void Setup()
        {
            service = new Mock<ICoreService>();
            userList = new List<User>
            {
                new User{ Id = Guid.NewGuid().ToString(), CreateDateTime = DateTime.Now, Username = "mrx", Password = Hasher.Hash("123456"), NameSurname = "METIN TASLIK" },
            };
        }

        [Test]
        public void TestLogin()
        {
            service.Setup(x => x.LogIn(userList[0])).Returns(new ResponseModel<User> { Error = false, Entity = userList[0] });

            var controller = new ValuesController(service.Object);
            var result = controller.Login(userList[0]).Entity;

            Assert.IsTrue(result.CreateDateTime.ToShortDateString() == DateTime.Now.ToShortDateString());
            Assert.IsFalse(result.Password == "123456");
        }

        [Test]
        public async Task TestRegisterAsync()
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                CreateDateTime = DateTime.Now,
                Username = "test",
                Password = Hasher.Hash("123456"),
                NameSurname = "TEST TEST"
            };
            service.Setup(x => x.CreateOrUpdateUserAsync(user)).ReturnsAsync(new ResponseModel<User> { Error = false, Entity = user });

            var controller = new ValuesController(service.Object);
            var result = await controller.Register(user);

            Assert.IsTrue(result.Entity.CreateDateTime.ToShortDateString() == DateTime.Now.ToShortDateString());
            Assert.IsFalse(result.Entity.Password == "123456");
        }
    }
}
