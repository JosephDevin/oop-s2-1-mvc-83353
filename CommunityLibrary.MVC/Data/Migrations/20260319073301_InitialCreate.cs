using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CommunityLibrary.MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Isbn = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Category", "IsAvailable", "Isbn", "Title" },
                values: new object[,]
                {
                    { 1, "Ubaldo Hayes", "Fiction", true, "9856348947658", "Fantastic Fresh Gloves" },
                    { 2, "Laura Little", "Non-Fiction", true, "1628238745952", "Ergonomic Wooden Cheese" },
                    { 3, "Lisandro Spinka", "Technology", true, "5195517737848", "Intelligent Plastic Chicken" },
                    { 4, "Clementina Hackett", "Non-Fiction", true, "1051759309547", "Gorgeous Soft Pants" },
                    { 5, "Danyka Kilback", "Mystery", true, "6610050180665", "Fantastic Granite Ball" },
                    { 6, "Karen MacGyver", "Technology", false, "3433511818548", "Unbranded Steel Car" },
                    { 7, "Mohammad McDermott", "History", false, "1099066873269", "Generic Steel Pants" },
                    { 8, "Virgil Schneider", "Biography", false, "0209823253000", "Intelligent Fresh Salad" },
                    { 9, "Santino Koepp", "Biography", false, "5337674765337", "Fantastic Wooden Table" },
                    { 10, "Elza Koch", "History", false, "5117580276789", "Ergonomic Metal Bike" },
                    { 11, "Eliza McKenzie", "History", false, "4262515882793", "Small Plastic Shirt" },
                    { 12, "Salma Pagac", "Fiction", false, "2885739797135", "Incredible Steel Chicken" },
                    { 13, "Randal Barton", "Fantasy", false, "8260179857344", "Intelligent Soft Car" },
                    { 14, "Brannon Kessler", "Non-Fiction", false, "7852381864251", "Handmade Wooden Tuna" },
                    { 15, "Roy Marks", "Non-Fiction", false, "0270588145930", "Incredible Granite Chair" },
                    { 16, "Easter Waters", "Technology", true, "4698930469590", "Fantastic Steel Pants" },
                    { 17, "Leslie VonRueden", "History", true, "7636313582280", "Licensed Fresh Soap" },
                    { 18, "Kale Fay", "History", true, "6124002954629", "Refined Steel Soap" },
                    { 19, "Luna Medhurst", "Technology", true, "6827435727470", "Generic Soft Keyboard" },
                    { 20, "Wilson Jones", "Fiction", true, "6262346885763", "Gorgeous Concrete Tuna" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Email", "FullName", "Phone" },
                values: new object[,]
                {
                    { 1, "Jed_Robel@yahoo.com", "Anais Stanton", "07040 439440" },
                    { 2, "Salvador_Gerhold61@yahoo.com", "Maia Herman", "07156 889305" },
                    { 3, "Graham_Waters48@hotmail.com", "Eunice Gottlieb", "07178 501486" },
                    { 4, "Delphia24@gmail.com", "Ardith Schinner", "07032 811563" },
                    { 5, "Kylee_Ondricka43@gmail.com", "Rae Nikolaus", "07301 701429" },
                    { 6, "Tina9@yahoo.com", "Constance Smith", "07536 403021" },
                    { 7, "Cordell34@yahoo.com", "Chris Gibson", "07178 004928" },
                    { 8, "Mathew.Bode@hotmail.com", "Lawrence Davis", "07319 211309" },
                    { 9, "Rose70@hotmail.com", "Sage Murphy", "07144 241014" },
                    { 10, "Camryn.Crooks@yahoo.com", "Haylie Osinski", "07770 027203" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "BookId", "DueDate", "LoanDate", "MemberId", "ReturnedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, new DateTime(2025, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 6, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null },
                    { 7, 7, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, null },
                    { 8, 8, new DateTime(2025, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null },
                    { 9, 9, new DateTime(2025, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, null },
                    { 10, 10, new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, null },
                    { 11, 11, new DateTime(2025, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null },
                    { 12, 12, new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { 13, 13, new DateTime(2025, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null },
                    { 14, 14, new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null },
                    { 15, 15, new DateTime(2025, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookId",
                table: "Loans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MemberId",
                table: "Loans",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
