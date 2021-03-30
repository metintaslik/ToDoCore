using Moq;
using NUnit.Framework;
using System;
using ToDo.API.Helper;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.Test
{
    public class UnitTest
    {
        private Mock<ICoreService> service;

        [SetUp]
        public void Setup()
        {
            service = new Mock<ICoreService>();
        }

        [Test]
        public void TestLogin()
        {
            var entity = new User { Username = "mrx", Password = Hasher.Hash("123456") };
            service.Setup(x => x.LogIn(entity))
                .Returns<User>(x => new ResponseModel<User> { Entity = x });
        }

        [Test]
        public void TestCreateOrUpdateUser()
        {
            service.Setup(x => x.CreateOrUpdateUserAsync(new User { Username = "mrx", Password = Hasher.Hash("123456"), NameSurname = "METIN TASLIK" }))
                .ReturnsAsync((ResponseModel<User>)null);
        }

        [Test]
        public void TestCreateOrUpdateCategory()
        {
            service.Setup(x => x.CreateOrUpdateCategoryAsync(new Category { Name = "General", IsPinned = true, PinnedDateTime = DateTime.Now })).Throws(new ArgumentNullException());
        }
    }
}