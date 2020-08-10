using Microsoft.EntityFrameworkCore;
using People.API.Models.DTOs;
using People.Data;
using People.Data.Entities;
using System;

namespace People.Tests
{
    public class PeopleInMemoryContext
    {
        public PeopleContext Context => InMemoryContext();

        private PeopleContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<PeopleContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var context = new PeopleContext(options);

            context.Seed();

            return context;
        }

        //Test data
        public static Person TestPerson1()
        {
            var person = new Person
            {
                Firstname = "Test1",
                Lastname = "Last",
                Email = "test@mail.com",
                Phone = "123456789"
            };
            return person;
        }

        public static Person TestPerson2()
        {
            var person = new Person
            {
                Firstname = "Test2",
                Lastname = "Pokorny",
                Email = "past@mail.com",
                Phone = "776878909"
            };
            return person;
        }

        public static Person TestPerson3()
        {
            var person = new Person
            {
                Firstname = "Test3",
                Lastname = "Zbezny",
                Email = "bezny@mail.com",
                Phone = "276856509"
            };
            return person;
        }

        public static PersonDto TestPersonDto()
        {
            var person = new PersonDto
            {
                Firstname = "TestDto",
                Lastname = "ZbeznyDto",
                Email = "bezny@mail.com",
                Phone = "32568954"
            };
            return person;
        }
    }
}