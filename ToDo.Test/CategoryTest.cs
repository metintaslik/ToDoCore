using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.API.Controllers;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.Test
{
    public class CategoryTest
    {
        private Mock<ICoreService> service;
        private List<Category> categories;

        [SetUp]
        public void Setup()
        {
            service = new Mock<ICoreService>();
            categories = new List<Category>()
            {
                new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateDateTime = DateTime.Now,
                    Name = "General",
                    IsPinned = true,
                    PinnedDateTime = DateTime.Now,
                },
                new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateDateTime = DateTime.Now,
                    Name = "Today",
                    IsPinned = true,
                    PinnedDateTime = DateTime.Now,
                },
                new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateDateTime = DateTime.Now,
                    Name = "Archive",
                    IsPinned = false,
                    PinnedDateTime = (DateTime?)null
                }
            };
        }

        [Test]
        public void TestCategory()
        {
            service.Setup(x => x.GetCategory(categories[0])).Returns(new ResponseModel<Category> { Error = false, Entity = categories[0] });

            var controller = new ValuesController(service.Object);
            var result = controller.GetCategory(categories[0]).Entity;

            Assert.IsTrue(result.CreateDateTime.ToShortDateString() == DateTime.Now.ToShortDateString());
            Assert.IsFalse(result.IsPinned == false);
        }

        [Test]
        public void TestCategories()
        {
            service.Setup(x => x.GetCategories()).Returns(new ResponseModel<List<Category>> { Error = false, Entity = categories });

            var controller = new ValuesController(service.Object);
            var list = controller.GetCategories().Entity;

            Assert.IsTrue(list.Count == 3);
            Assert.IsFalse(list.All(c => c.IsPinned == true));
        }

        [Test]
        public async Task TestAddCategoryAsync()
        {
            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                CreateDateTime = DateTime.Now,
                Name = "Future",
                IsPinned = true,
                PinnedDateTime = DateTime.Now,
            };
            service.Setup(x => x.CreateOrUpdateCategoryAsync(category)).ReturnsAsync(new ResponseModel<Category> { Error = false, Entity = category });

            var controller = new ValuesController(service.Object);
            var result = await controller.PostCategory(category);

            Assert.IsEmpty(result.Entity.OwnerId ?? string.Empty);
            Assert.IsNull(result.Entity.OwnerId);
        }
    }
}