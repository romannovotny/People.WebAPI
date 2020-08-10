using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using People.API.Models.DTOs;
using People.API.Services;
using People.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace People.API.Controllers
{
    [Route("api/person")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public PersonController(IPersonService personService, IMapper mapper, ILogger<PersonController> logger)
        {
            this.personService = personService;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersons()
        {
            var persons = await personService.GetAllPerson();
            return Ok(mapper.Map<IEnumerable<PersonDto>>(persons));
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            var person = await personService.GetPersonById(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PersonDto>(person));
        }

        // POST: api/Person
        [HttpPost]
        public async Task<ActionResult<PersonDto>> CreatePerson([FromBody] PersonPostDto person)
        {
            if (person == null)
            {
                return BadRequest("Person object is null");
            }

            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid person object sent from client.");
                return BadRequest("Invalid model object");
            }

            var personEntity = mapper.Map<Person>(person);

            await personService.AddPerson(personEntity);

            var createdPerson = mapper.Map<PersonDto>(personEntity);

            return CreatedAtAction("GetPerson", new { id = createdPerson.Id }, createdPerson);
        }

        // PUT: api/Person/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonPostDto person)
        {
            if (person == null)
            {
                logger.LogError("Person object sent from client is null.");
                return BadRequest("Person object is null");
            }

            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid person object sent from client.");
                return BadRequest("Invalid model object");
            }

            var personEntity = await personService.GetPersonById(id);
            if (personEntity == null)
            {
                logger.LogError($"Person with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            mapper.Map(person, personEntity);

            await personService.UpdatePerson(personEntity);

            return NoContent();
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await personService.GetPersonById(id);
            if (person == null)
            {
                logger.LogError($"Person with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await personService.DeletePerson(person);

            return NoContent();
        }
    }
}