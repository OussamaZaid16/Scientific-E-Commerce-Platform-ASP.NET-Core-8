using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GererUser.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommandeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommandeId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CommandeId",
                table: "Articles",
                column: "CommandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Commandes_CommandeId",
                table: "Articles",
                column: "CommandeId",
                principalTable: "Commandes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Commandes_CommandeId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CommandeId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CommandeId",
                table: "Articles");
        }
    }
}
