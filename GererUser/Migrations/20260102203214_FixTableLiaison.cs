using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GererUser.Migrations
{
    /// <inheritdoc />
    public partial class FixTableLiaison : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ArticleCommande",
                columns: table => new
                {
                    ArticlesId = table.Column<int>(type: "int", nullable: false),
                    CommandeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCommande", x => new { x.ArticlesId, x.CommandeId });
                    table.ForeignKey(
                        name: "FK_ArticleCommande_Articles_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleCommande_Commandes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "Commandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCommande_CommandeId",
                table: "ArticleCommande",
                column: "CommandeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleCommande");

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
    }
}
