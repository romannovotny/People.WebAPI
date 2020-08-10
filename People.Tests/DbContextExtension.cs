using Microsoft.EntityFrameworkCore;
using People.Data;
using People.Data.Entities;

namespace People.Tests
{
    public static class DbContextExtension
    {
        public static void Seed(this PeopleContext dbContext)
        {
            var person1 = PeopleInMemoryContext.TestPerson1();
            var person2 = PeopleInMemoryContext.TestPerson2();
            dbContext.Persons.Add(person1);
            dbContext.Persons.Add(person2);
            dbContext.SaveChanges();
            dbContext.Entry<Person>(person1).State = EntityState.Detached;
            dbContext.Entry<Person>(person2).State = EntityState.Detached;
        }
    }
}