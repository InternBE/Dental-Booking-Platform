using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class fixerror : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_TreatmentPlans_TreatmentPlanId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_UserId",
                table: "Appointment");

            migrationBuilder.AddColumn<int>(
                name: "ClinicId1",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentPlansId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ClinicId1",
                table: "Appointment",
                column: "ClinicId1");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_TreatmentPlansId",
                table: "Appointment",
                column: "TreatmentPlansId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_UserId1",
                table: "Appointment",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Clinic_ClinicId1",
                table: "Appointment",
                column: "ClinicId1",
                principalTable: "Clinic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_TreatmentPlans_TreatmentPlanId",
                table: "Appointment",
                column: "TreatmentPlanId",
                principalTable: "TreatmentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_TreatmentPlans_TreatmentPlansId",
                table: "Appointment",
                column: "TreatmentPlansId",
                principalTable: "TreatmentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_UserId",
                table: "Appointment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_UserId1",
                table: "Appointment",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Clinic_ClinicId1",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_TreatmentPlans_TreatmentPlanId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_TreatmentPlans_TreatmentPlansId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_UserId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_UserId1",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ClinicId1",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_TreatmentPlansId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_UserId1",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ClinicId1",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "TreatmentPlansId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Appointment");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_TreatmentPlans_TreatmentPlanId",
                table: "Appointment",
                column: "TreatmentPlanId",
                principalTable: "TreatmentPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_UserId",
                table: "Appointment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
