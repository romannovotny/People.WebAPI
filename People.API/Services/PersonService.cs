using Microsoft.EntityFrameworkCore;
using People.Data.Entities;
using People.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace People.API.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Person> personRepository;

        public PersonService(IRepository<Person> personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<List<Person>> GetAllPerson()
        {
            return await personRepository.FindAll().ToListAsync();
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await personRepository.FindByCondition(person => person.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Person> AddPerson(Person person)
        {
            return await personRepository.CreateAsync(person);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            return await personRepository.UpdateAsync(person);
        }

        public async Task<Person> DeletePerson(Person person)
        {
            return await personRepository.DeleteAsync(person);
        }
    }
}