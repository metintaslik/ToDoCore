using Couchbase;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.Test
{
    public class Tests
    {
        private Mock<ICoreService> service;
        private List<User> users;

        [SetUp]
        public void Setup()
        {
            service = new Mock<ICoreService>();
            users = new List<User>()
            {
                new User
                {
                    Id = "1",
                    CreateDateTime = DateTime.Now,
                    NameSurname = "METIN TASLIK",
                    Username = "mrx1",
                    Password = "123456"
                },
                new User
                {
                    Id = "2",
                    CreateDateTime = DateTime.Now,
                    NameSurname = "METIN TASLIK",
                    Username = "mrx2",
                    Password = "123456"
                },
                new User
                {
                    Id = "3",
                    CreateDateTime = DateTime.Now,
                    NameSurname = "METIN TASLIK",
                    Username = "mrx3",
                    Password = "123456"
                },
                new User
                {
                    Id = "4",
                    CreateDateTime = DateTime.Now,
                    NameSurname = "METIN TASLIK",
                    Username = "mrx4",
                    Password = "123456"
                },
                new User
                {
                    Id = "5",
                    CreateDateTime = DateTime.Now,
                    NameSurname = "METIN TASLIK",
                    Username = "mrx5",
                    Password = "123456"
                },
            };
        }

        [Test]
        public void Test1()
        {
            //service.Setup(x=>x.LogIn(users.FirstOrDefault(x=>x.Id == "1"), null))
            Assert.Pass();
        }
    }
}