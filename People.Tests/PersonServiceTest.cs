using People.API.Services;
using People.Data.Entities;
using People.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace People.Tests
{
    public class PersonServiceTest
    {
        private readonly PeopleInMemoryContext context;

        public PersonServiceTest()
        {
            context = new PeopleInMemoryContext();
        }

        [Fact]
        public async Task Should_GetAllPersons()
        {
            var personService = CreateService();

            var result = await personService.GetAllPerson();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Person>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPersonById_WithWrongId_Should_Return_Null()
        {
            var personService = CreateService();

            var result = await personService.GetPersonById(4);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPersonById_Should_GetSpecificPerson()
        {
            var personService = CreateService();

            var result = await personService.GetPersonById(1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Person>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(PeopleInMemoryContext.TestPerson1().Firstname, result.Firstname);
        }

        [Fact]
        public async Task AddPerson_Should_AddNewPerson()
        {
            var personService = CreateService();
            var person = PeopleInMemoryContext.TestPerson3();

            await personService.AddPerson(person);

            Assert.Equal(3, person.Id);
        }

        [Fact]
        public async Task Should_UpdatePerson()
        {
            var personService = CreateService();
            var person = await personService.GetPersonById(1);

            Assert.IsAssignableFrom<Person>(person);
            person.Firstname = "UpdatedName";

            await personService.UpdatePerson(person);
            var updatedPerson = await personService.GetPersonById(1);

            Assert.Equal(updatedPerson.Firstname, person.Firstname);
        }

        [Fact]
        public async Task Should_DeletePerson()
        {
            var personService = CreateService();
            var person = await personService.GetPersonById(1);

            Assert.IsAssignableFrom<Person>(person);

            await personService.DeletePerson(person);
            var updatedPerson = await personService.GetPersonById(1);

            Assert.Null(updatedPerson);
        }

        private PersonService CreateService()
        {
            var repository = new Repository<Person>(context.Context);
            return new PersonService(repository);
        }
    }
}