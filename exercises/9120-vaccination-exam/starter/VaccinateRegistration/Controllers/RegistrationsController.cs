using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccinateRegistration.Data;

namespace VaccinateRegistration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly VaccinateDbContext context;

        public RegistrationsController(VaccinateDbContext context)
        {
            this.context = context;
        }

        // This class is NOT COMPLETE.
        // Todo: Complete the class according to the requirements

        [HttpGet]
        public async Task<GetRegistrationResult?> GetRegistration([FromQuery] long ssn, [FromQuery] int pin)
        {
            return await context.GetRegistration(ssn, pin);
        }

        [HttpGet]
        [Route("timeSlots")]
        public IAsyncEnumerable<DateTime> GetTimeslots([FromQuery] DateTime date)
        {
            return context.GetTimeslots(date);
        }
    }
}
