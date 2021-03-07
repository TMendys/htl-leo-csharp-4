using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

namespace VaccinateRegistration.Data
{
    public record GetRegistrationResult(int Id, long Ssn, string FirstName, string LastName);

    public record StoreVaccination(int RegistrationId, DateTime Datetime);

    public class VaccinateDbContext : DbContext
    {
        public VaccinateDbContext(DbContextOptions<VaccinateDbContext> options) : base(options) { }

        // This class is NOT COMPLETE.
        // Todo: Complete the class according to the requirements

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<Vaccination> Vaccinations { get; set; }

        /// <summary>
        /// Import registrations from JSON file
        /// </summary>
        /// <param name="registrationsFileName">Name of the file to import</param>
        /// <returns>
        /// Collection of all imported registrations
        /// </returns>
        public async Task<IEnumerable<Registration>> ImportRegistrations(string registrationsFileName)
        {
            using FileStream jsonFile = File.OpenRead(registrationsFileName);
            var RegistrationResult = await JsonSerializer.DeserializeAsync<IEnumerable<Registration>>(jsonFile);

            if (RegistrationResult is null) throw new ArgumentException("File not found or no data in file");

            AddRange(RegistrationResult);
            await SaveChangesAsync();

            return RegistrationResult;
        }

        /// <summary>
        /// Delete everything (registrations, vaccinations)
        /// </summary>
        public async Task DeleteEverything()
        {
            await Database.ExecuteSqlRawAsync("DELETE from Vaccinations");
            await Database.ExecuteSqlRawAsync("DELETE from Registrations");
        }

        /// <summary>
        /// Get registration by social security number (SSN) and PIN
        /// </summary>
        /// <param name="ssn">Social Security Number</param>
        /// <param name="pin">PIN code</param>
        /// <returns>
        /// Registration result or null if no registration with given SSN and PIN was found.
        /// </returns>
        public async Task<GetRegistrationResult?> GetRegistration(long ssn, int pin)
        {
            var result = await Registrations.FirstOrDefaultAsync(r => r.SocialSecurityNumber == ssn && r.PinCode == pin);

            if (result is null) return null;

            return new GetRegistrationResult(result.Id, result.SocialSecurityNumber, result.FirstName, result.LastName);
        }

        /// <summary>
        /// Get available time slots on the given date
        /// </summary>
        /// <param name="date">Date (without time, i.e. time is 00:00:00)</param>
        /// <returns>
        /// Collection of all available time slots
        /// </returns>
        public async IAsyncEnumerable<DateTime> GetTimeslots(DateTime date)
        {
            List<TimeSpan> timeSlots = TimeSlots().ToList();
            List<DateTime> avaiableTimeSlots = new();

            var vaccinationsDay = await Vaccinations
                .Select(v => v.VaccinationDate)
                .Where(d => d.Date == date.Date).ToListAsync();

            foreach (var time in vaccinationsDay)
            {
                timeSlots.Remove(time.TimeOfDay);
            }

            foreach (var avaiableTime in timeSlots)
            {
                yield return date + avaiableTime;
            }
        }

        private IEnumerable<TimeSpan> TimeSlots()
        {
            TimeSpan timeSlot = new(7,45,0);
            TimeSpan interval = new(0,15,0);
            TimeSpan lastSlot = new(11, 45, 0);

            while (timeSlot < lastSlot)
            {
                timeSlot += interval;

                yield return timeSlot;
            }
        }

        /// <summary>
        /// Store a vaccination
        /// </summary>
        /// <param name="vaccination">Vaccination to store</param>
        /// <returns>
        /// Stored vaccination after it has been written to the database.
        /// </returns>
        /// <remarks>
        /// If a vaccination with the given vaccination.RegistrationID already exists,
        /// overwrite it. Otherwise, insert a new vaccination.
        /// </remarks>
        public async Task<Vaccination> StoreVaccination(StoreVaccination vaccination)
        {
            if(await Vaccinations.AnyAsync(v => v.VaccinationDate == vaccination.Datetime))
            {
                throw new ArgumentException("The date and time is already taken.");
            }

            Vaccination storeOneVaccination = new() { 
                RegistrationId = vaccination.RegistrationId, 
                VaccinationDate = vaccination.Datetime 
            };

            Vaccinations.Add(storeOneVaccination);
            await SaveChangesAsync();

            return storeOneVaccination;
        }
    }
}
