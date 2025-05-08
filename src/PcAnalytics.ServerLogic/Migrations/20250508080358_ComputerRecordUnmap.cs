using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcAnalytics.ServerLogic.Migrations
{
    /// <inheritdoc />
    public partial class ComputerRecordUnmap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Computers_ComputerId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_ComputerId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "Records");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComputerId",
                table: "Records",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_ComputerId",
                table: "Records",
                column: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Computers_ComputerId",
                table: "Records",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "Id");
        }
    }
}
