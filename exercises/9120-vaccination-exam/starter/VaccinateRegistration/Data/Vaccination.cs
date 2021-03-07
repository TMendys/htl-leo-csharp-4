using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

namespace VaccinateRegistration.Data
{
    public class Vaccination
    {
        public int Id { get; set; }

        [Required]
        public DateTime VaccinationDate { get; set; }

        [Required]
        public int RegistrationId { get; set; }

        public Registration Registration { get; set; }
    }
}
