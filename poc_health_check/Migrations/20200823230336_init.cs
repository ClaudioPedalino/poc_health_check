using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace poc_health_check.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.Sql(@"insert into Employees (id, firstname, lastname, age, isactive) values ('e8cba84d-5d67-4fe9-896e-a7cbe4e599b0', 'Sileas', 'Kleingrub', 25, 1);
                                insert into Employees (id, firstname, lastname, age, isactive) values ('b66e56d6-8041-4abf-8697-44e0646c33d6', 'Vanessa', 'Turpey', 70, 1);
                                insert into Employees (id, firstname, lastname, age, isactive) values ('75e005c2-9b9e-4615-9247-d524b45dba41', 'Chicky', 'Crop', 74, 1);
                                insert into Employees (id, firstname, lastname, age, isactive) values ('01e6726e-3a43-469d-aec7-5f8305b27f6a', 'Ervin', 'Dady', 43, 0);
                                insert into Employees (id, firstname, lastname, age, isactive) values ('57dcbf67-4346-45e8-9bef-930b8baba19c', 'Rodrick', 'Whitehall', 53, 1);
                                insert into Employees (id, firstname, lastname, age, isactive) values ('50be8395-cf1b-4175-8a72-361d77acbd40', 'Alessandro', 'Frowen', 20, 0);
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
