using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirLineManager.dal.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string aircrafts = File.ReadAllText("C:/all_docs/Airline Manager/db/add.aircrafts.sql");
            migrationBuilder.Sql(aircrafts);

            string seats = File.ReadAllText("C:/all_docs/Airline Manager/db/add.seats.sql");
            migrationBuilder.Sql(seats);

            string blukinsert = File.ReadAllText("C:/all_docs/Airline Manager/db/bulkinsert.sql");
            migrationBuilder.Sql(blukinsert);

            string users = File.ReadAllText("C:/all_docs/Airline Manager/db/add.users.sql");
            migrationBuilder.Sql(users);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
