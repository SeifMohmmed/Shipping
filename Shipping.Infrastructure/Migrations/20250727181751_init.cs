using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shipping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinWeight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxWeight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CostPerKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CitySettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StandardShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PickUpShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitySettings_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickupPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CanceledOrder = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DeductionTypes = table.Column<int>(type: "int", nullable: true),
                    DeductionCompanyFromOrder = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalWeight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OrderCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOutOfCityShipping = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrderTypes = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourierId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CitySettingId = table.Column<int>(type: "int", nullable: false),
                    ShippingTypeId = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_CitySettings_CitySettingId",
                        column: x => x.CitySettingId,
                        principalTable: "CitySettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_ShippingTypes_ShippingTypeId",
                        column: x => x.ShippingTypeId,
                        principalTable: "ShippingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialCityCosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CitySettingId = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialCityCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialCityCosts_AspNetUsers_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialCityCosts_CitySettings_CitySettingId",
                        column: x => x.CitySettingId,
                        principalTable: "CitySettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialCourierRegions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialCourierRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialCourierRegions_AspNetUsers_CourierId",
                        column: x => x.CourierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialCourierRegions_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourierReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourierReports_AspNetUsers_CourierId",
                        column: x => x.CourierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourierReports_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01961d25-b4da-7184-a2a8-765486bd4857", "EAE00686-2608-4516-AD1B-F96CD87C475E", new DateTime(2025, 7, 27, 21, 17, 49, 395, DateTimeKind.Local).AddTicks(2129), false, "Admin", "ADMIN" },
                    { "01961d25-b4da-71e9-a488-1b8db232e984", "1420D50C-F54D-4503-88E8-A2EFA3BD7137", new DateTime(2025, 7, 27, 21, 17, 49, 407, DateTimeKind.Local).AddTicks(4403), false, "Merchant", "MERCHANT" },
                    { "01961d25-b4da-75a5-a1f4-a7aa10e421ed", "386C6E14-D0FD-40FF-80D0-74B419360EF0", new DateTime(2025, 7, 27, 21, 17, 49, 407, DateTimeKind.Local).AddTicks(4124), false, "Courier", "COURIER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BranchId", "CanceledOrder", "ConcurrencyStamp", "CreatedAt", "DeductionCompanyFromOrder", "DeductionTypes", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PickupPrice", "RegionId", "SecurityStamp", "StoreName", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0195d439-9ca1-7873-9c14-a4bc1c201593", 0, null, null, null, "0195d43b-a808-757b-9c3e-bf90c6091133", new DateTime(2025, 7, 27, 21, 17, 49, 471, DateTimeKind.Local).AddTicks(8629), null, null, "Seif123@gmail.com", false, "Seif Admin", false, false, null, "SEIF123@GMAIL.COM", "SEIF123@GMAIL.COM", "AQAAAAIAAYagAAAAEDd/7v58/Pv27l5DvvI5krdZuN7OUbl5liki7lV/DFu/EtCm3EE8yuE2uloGVlVesg==", null, false, null, null, "0195d43be3f271878cc37be7dfc34361", null, false, "Seif123@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "Permissions:ViewPermissions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 2, "permissions", "Permissions:AddPermissions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 3, "permissions", "Permissions:UpdatePermissions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 4, "permissions", "Permissions:DeletePermissions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 5, "permissions", "Settings:ViewSettings", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 6, "permissions", "Settings:AddSettings", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 7, "permissions", "Settings:UpdateSettings", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 8, "permissions", "Settings:DeleteSettings", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 9, "permissions", "Bank:ViewBank", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 10, "permissions", "Bank:AddBank", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 11, "permissions", "Bank:UpdateBank", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 12, "permissions", "Bank:DeleteBank", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 13, "permissions", "MoneySafe:ViewMoneySafe", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 14, "permissions", "MoneySafe:AddMoneySafe", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 15, "permissions", "MoneySafe:UpdateMoneySafe", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 16, "permissions", "MoneySafe:DeleteMoneySafe", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 17, "permissions", "Branches:ViewBranches", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 18, "permissions", "Branches:AddBranches", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 19, "permissions", "Branches:UpdateBranches", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 20, "permissions", "Branches:DeleteBranches", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 21, "permissions", "Employees:ViewEmployees", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 22, "permissions", "Employees:AddEmployees", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 23, "permissions", "Employees:UpdateEmployees", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 24, "permissions", "Employees:DeleteEmployees", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 25, "permissions", "Merchants:ViewMerchants", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 26, "permissions", "Merchants:AddMerchants", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 27, "permissions", "Merchants:UpdateMerchants", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 28, "permissions", "Merchants:DeleteMerchants", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 29, "permissions", "Couriers:ViewCouriers", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 30, "permissions", "Couriers:AddCouriers", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 31, "permissions", "Couriers:UpdateCouriers", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 32, "permissions", "Couriers:DeleteCouriers", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 33, "permissions", "Regions:ViewRegions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 34, "permissions", "Regions:AddRegions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 35, "permissions", "Regions:UpdateRegions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 36, "permissions", "Regions:DeleteRegions", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 37, "permissions", "Cities:ViewCities", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 38, "permissions", "Cities:AddCities", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 39, "permissions", "Cities:UpdateCities", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 40, "permissions", "Cities:DeleteCities", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 41, "permissions", "Orders:ViewOrders", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 42, "permissions", "Orders:AddOrders", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 43, "permissions", "Orders:UpdateOrders", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 44, "permissions", "Orders:DeleteOrders", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 45, "permissions", "OrderReports:ViewOrderReports", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 46, "permissions", "OrderReports:AddOrderReports", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 47, "permissions", "OrderReports:UpdateOrderReports", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 48, "permissions", "OrderReports:DeleteOrderReports", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 49, "permissions", "Accounts:ViewAccounts", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 50, "permissions", "Accounts:AddAccounts", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 51, "permissions", "Accounts:UpdateAccounts", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 52, "permissions", "Accounts:DeleteAccounts", "01961d25-b4da-7184-a2a8-765486bd4857" },
                    { 53, "permissions", "Orders:ViewOrders", "01961d25-b4da-75a5-a1f4-a7aa10e421ed" },
                    { 54, "permissions", "Orders:UpdateOrders", "01961d25-b4da-75a5-a1f4-a7aa10e421ed" },
                    { 55, "permissions", "Orders:ViewOrders", "01961d25-b4da-71e9-a488-1b8db232e984" },
                    { 56, "permissions", "Orders:AddOrders", "01961d25-b4da-71e9-a488-1b8db232e984" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "01961d25-b4da-7184-a2a8-765486bd4857", "0195d439-9ca1-7873-9c14-a4bc1c201593" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegionId",
                table: "AspNetUsers",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_RegionId",
                table: "Branches",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_CitySettings_RegionId",
                table: "CitySettings",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierReports_CourierId",
                table: "CourierReports",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierReports_OrderId",
                table: "CourierReports",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId",
                table: "Orders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CitySettingId",
                table: "Orders",
                column: "CitySettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RegionId",
                table: "Orders",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingTypeId",
                table: "Orders",
                column: "ShippingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId",
                table: "Products",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCityCosts_CitySettingId",
                table: "SpecialCityCosts",
                column: "CitySettingId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCityCosts_MerchantId",
                table: "SpecialCityCosts",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCourierRegions_CourierId",
                table: "SpecialCourierRegions",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCourierRegions_RegionId",
                table: "SpecialCourierRegions",
                column: "RegionId");
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
                name: "CourierReports");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SpecialCityCosts");

            migrationBuilder.DropTable(
                name: "SpecialCourierRegions");

            migrationBuilder.DropTable(
                name: "WeightSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CitySettings");

            migrationBuilder.DropTable(
                name: "ShippingTypes");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
