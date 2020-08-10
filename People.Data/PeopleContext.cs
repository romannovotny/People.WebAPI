using Microsoft.EntityFrameworkCore;
using People.Data.Entities;

namespace People.Data
{
    public class PeopleContext : DbContext
    {
        public PeopleContext(DbContextOptions<PeopleContext> options)
           : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}