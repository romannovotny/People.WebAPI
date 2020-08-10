using People.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace People.API.Services
{
    public interface IPersonService
    {
        Task<List<Person>> GetAllPerson();

        Task<Person> GetPersonById(int id);

        Task<Person> AddPerson(Person person);

        Task<Person> UpdatePerson(Person person);

        Task<Person> DeletePerson(Person person);
    }
}