using Microsoft.EntityFrameworkCore.Migrations;

namespace VaccinateRegistration.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Vaccinations_BookedVaccinationId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_BookedVaccinationId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "BookedVaccinationId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "VaccinationId",
                table: "Registrations");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinations_RegistrationId",
                table: "Vaccinations",
                column: "RegistrationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccinations_Registrations_RegistrationId",
                table: "Vaccinations",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccinations_Registrations_RegistrationId",
                table: "Vaccinations");

            migrationBuilder.DropIndex(
                name: "IX_Vaccinations_RegistrationId",
                table: "Vaccinations");

            migrationBuilder.AddColumn<int>(
                name: "BookedVaccinationId",
                table: "Registrations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VaccinationId",
                table: "Registrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_BookedVaccinationId",
                table: "Registrations",
                column: "BookedVaccinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Vaccinations_BookedVaccinationId",
                table: "Registrations",
                column: "BookedVaccinationId",
                principalTable: "Vaccinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
