using ContactListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListAPI.Services
{
    public interface IContactsRepository
    {
        IEnumerable<Person> GetAllPeople();

        Person GetPersonById(int id);

        Person AddPerson(Person contact);

        void DeletePerson(int Id);

        IEnumerable<Person> FindPersonByName(string filter);
    }
}
