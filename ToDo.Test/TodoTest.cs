using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.API.Controllers;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.Test
{
    public class TodoTest
    {
        private Mock<ICoreService> service;
        private List<Todo> todos;

        [SetUp]
        public void Setup()
        {
            service = new Mock<ICoreService>();
            todos = new List<Todo>
            {
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateDateTime = DateTime.Now,
                    Title = "Cryptocurrency",
                    Content = "Bitcoin",
                    Alert = DateTime.Now.AddDays(-5),
                    IsItDone = false,
                    IsPinned = true,
                    PinnedDateTime = DateTime.Now,
                },
                new Todo
                {
                   Id = Guid.NewGuid().ToString(),
                    CreateDateTime = DateTime.Now,
                    Title = "ToDoCore API",
                    Content = "Application Unit Test",
                    Alert = DateTime.Now,
                    IsItDone = true,
                    IsPinned = true,
                    PinnedDateTime = DateTime.Now,
                },
                new Todo
                {
                   Id = Guid.NewGuid().ToString(),
                    CreateDateTime = DateTime.Now,
                    Title = "Cryptocurrency",
                    Content = "Etherium",
                    Alert = DateTime.Now,
                    IsItDone = false,
                    IsPinned = true,
                    PinnedDateTime = DateTime.Now
                }
            };
        }

        [Test]
        public void TestTodo()
        {
            service.Setup(x => x.GetTodo(todos[0])).Returns(new ResponseModel<Todo> { Error = false, Entity = todos[0] });

            var controller = new ValuesController(service.Object);
            var result = controller.GetTodo(todos[0]).Entity;

            Assert.IsTrue(result.CreateDateTime.ToShortDateString() == DateTime.Now.ToShortDateString());
            Assert.IsFalse(result.IsPinned == false);
        }

        [Test]
        public void TestTodos()
        {
            service.Setup(x => x.GetTodos()).Returns(new ResponseModel<List<Todo>> { Error = false, Entity = todos });

            var controller = new ValuesController(service.Object);
            var list = controller.GetTodos().Entity;

            Assert.IsTrue(list.Count == 3);
            Assert.IsFalse(list.All(c => c.IsPinned == false));
        }

        [Test]
        public async Task TestAddTodoAsync()
        {
            var todo = new Todo
            {
                Id = Guid.NewGuid().ToString(),
                CreateDateTime = DateTime.Now,
                Title = "ToDo Core Application",
                Content = "Set Unit Test",
                IsPinned = true,
                IsItDone = true,
                Alert = DateTime.Now,
                PinnedDateTime = DateTime.Now,
            };
            service.Setup(x => x.CreateOrUpdateTodoAsync(todo)).ReturnsAsync(new ResponseModel<Todo> { Error = false, Entity = todo });

            var controller = new ValuesController(service.Object);
            var result = await controller.PostTodo(todo);

            Assert.IsEmpty(result.Entity.CategoryId ?? string.Empty);
            Assert.IsNull(result.Entity.CategoryId);
        }
    }
}
