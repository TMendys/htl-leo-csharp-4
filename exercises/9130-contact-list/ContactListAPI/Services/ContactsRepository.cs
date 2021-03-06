using ContactListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListAPI.Services
{
    public class ContactsRepository : IContactsRepository
    {
        private List<Person> persons = new();

        public Person AddPerson(Person person)
        {
            persons.Add(person);
            return person;
        }

        public void DeletePerson(int Id)
        {
            Person person = persons.FirstOrDefault(c => c.Id == Id);
            if(person is null)
            {
                throw new ArgumentException("Could find person by Id.");
            }

            persons.Remove(person);
        }

        public IEnumerable<Person> FindPersonByName(string filter) 
            => persons.Where(c => c.FirstName.Contains(filter) || c.LastName.Contains(filter));

        public IEnumerable<Person> GetAllPeople() => persons;

        public Person GetPersonById(int id) => persons.FirstOrDefault(x => x.Id == id);
    }
}
