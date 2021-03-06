using ContactListAPI.Models;
using ContactListAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsRepository repository;

        public ContactsController(IContactsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Person>))]
        public IActionResult GetAllPeople() => Ok(repository.GetAllPeople());

        [HttpGet("{id}", Name = nameof(GetPersonById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPersonById(int id)
        {
            var person = repository.GetPersonById(id);

            if (person is null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPerson([FromBody] Person newContact)
        {
            if (newContact.Id < 1)
            {
                return BadRequest("Invalid Id");
            }

            repository.AddPerson(newContact);
            return CreatedAtAction(nameof(GetPersonById), new { id = newContact.Id }, newContact);
        }

        [HttpDelete]
        [Route("{personToDeleteId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePerson(int personToDeleteId) 
        {
            try
            {
                repository.DeletePerson(personToDeleteId);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpGet("search/{filter}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Person>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult FindPersonByName(string filter)
        {
            var contacts = repository.FindPersonByName(filter);

            if (contacts is null) return NotFound();

            return Ok(contacts);
        }
    }
}
