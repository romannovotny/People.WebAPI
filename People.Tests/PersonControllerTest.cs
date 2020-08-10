using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using People.API.Automapper;
using People.API.Controllers;
using People.API.Models.DTOs;
using People.API.Services;
using People.Data.Entities;
using People.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace People.Tests
{
    public class PersonControllerTest
    {
        private readonly PersonController controller;
        private readonly PeopleInMemoryContext context;

        public PersonControllerTest()
        {
            context = new PeopleInMemoryContext();
            var config = new MapperConfiguration(opts => opts.AddProfile<MappingProfile>());
            var logger = new Mock<ILogger<PersonController>>();
            controller = new PersonController(CreateService(), config.CreateMapper(), logger.Object);
        }

        [Fact]
        public async Task GetPersons_Should_Return_AllItems()
        {
            var actionResult = await controller.GetPersons();
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var persons = Assert.IsAssignableFrom<IEnumerable<PersonDto>>(okResult.Value);

            Assert.NotNull(persons);
            Assert.Equal(2, persons.Count());
        }

        [Fact]
        public async Task GetPerson_Should_Return_Person()
        {
            var actionResult = await controller.GetPerson(1);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var person = Assert.IsAssignableFrom<PersonDto>(okResult.Value);

            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(person);
            Assert.IsAssignableFrom<PersonDto>(person);
            Assert.Equal(1, person.Id);
            Assert.Equal(PeopleInMemoryContext.TestPerson1().Firstname, person.Firstname);
        }

        [Fact]
        public async Task CreatePerson_WithNull_Should_Return_BadRequest()
        {
            var actionResult = await controller.CreatePerson(null);

            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreatePerson_WithNotValidData_Should_Return_BadRequest()
        {
            controller.ModelState.AddModelError("fakeError", "fakeError");

            var actionResult = await controller.CreatePerson(PeopleInMemoryContext.TestPersonDto());

            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreatePerson_Should_Add_NewPerson()
        {
            var newPerson = PeopleInMemoryContext.TestPersonDto();
            var actionResult = await controller.CreatePerson(newPerson);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var person = Assert.IsAssignableFrom<PersonDto>(createdResult.Value);

            Assert.NotNull(person);
            Assert.Equal(3, person.Id);
            Assert.Equal(newPerson.Firstname, person.Firstname);
        }

        [Fact]
        public async Task UpdatePerson_WithNull_Should_Return_BadRequest()
        {
            var actionResult = await controller.UpdatePerson(1, null);

            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task UpdatePerson_WithNotValidData_Should_Return_BadRequest()
        {
            controller.ModelState.AddModelError("fakeError", "fakeError");

            var actionResult = await controller.UpdatePerson(1, PeopleInMemoryContext.TestPersonDto());

            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task UpdatePerson_WithWrongId_Should_Return_NotFound()
        {
            var actionResult = await controller.UpdatePerson(10, PeopleInMemoryContext.TestPersonDto());

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task UpdatePerson_Should_UpdatePerson()
        {
            var actionResult = await controller.GetPerson(1);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var personToUpdate = Assert.IsAssignableFrom<PersonDto>(okResult.Value);

            Assert.NotNull(personToUpdate);

            personToUpdate.Firstname = "UpdatedName";

            await controller.UpdatePerson(personToUpdate.Id, personToUpdate);
            var actionResult2 = await controller.GetPerson(1);
            var okResult2 = Assert.IsType<OkObjectResult>(actionResult2.Result);
            var updatedPerson = Assert.IsAssignableFrom<PersonDto>(okResult2.Value);

            Assert.NotNull(updatedPerson);
            Assert.Equal(updatedPerson.Firstname, personToUpdate.Firstname);
        }

        [Fact]
        public async Task DeletePerson_WithWrongId_Should_Return_NotFound()
        {
            var actionResult = await controller.DeletePerson(10);
            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task DeletePerson_Should_DeletePerson()
        {
            await controller.DeletePerson(1);
            var actionResult = await controller.GetPerson(1);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        private PersonService CreateService()
        {
            var repository = new Repository<Person>(context.Context);
            return new PersonService(repository);
        }
    }
}