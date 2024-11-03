using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalAPI.Migrations
{
    public partial class changeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NameInfos_Patients_PatientId",
                table: "NameInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NameInfos",
                table: "NameInfos");

            migrationBuilder.RenameTable(
                name: "NameInfos",
                newName: "NameInfo");

            migrationBuilder.RenameIndex(
                name: "IX_NameInfos_PatientId",
                table: "NameInfo",
                newName: "IX_NameInfo_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NameInfo",
                table: "NameInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NameInfo_Patients_PatientId",
                table: "NameInfo",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NameInfo_Patients_PatientId",
                table: "NameInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NameInfo",
                table: "NameInfo");

            migrationBuilder.RenameTable(
                name: "NameInfo",
                newName: "NameInfos");

            migrationBuilder.RenameIndex(
                name: "IX_NameInfo_PatientId",
                table: "NameInfos",
                newName: "IX_NameInfos_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NameInfos",
                table: "NameInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NameInfos_Patients_PatientId",
                table: "NameInfos",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
