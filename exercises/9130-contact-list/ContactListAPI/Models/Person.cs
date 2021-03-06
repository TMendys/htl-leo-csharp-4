using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListAPI.Models
{
    public record Person(string FirstName, string LastName)
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public string Email { get; init; }
    }
}
