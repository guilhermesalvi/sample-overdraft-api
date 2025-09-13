using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerEnrollment.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverdraftAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerType = table.Column<int>(type: "int", nullable: false),
                    IsBankAccountActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverdraftAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OverdraftContracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverdraftAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GracePeriodDays = table.Column<int>(type: "int", nullable: false),
                    MonthlyInterestRate = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    MonthlyOverLimitInterestRate = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    OverLimitFixedFee = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    MonthlyLatePaymentInterestRate = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    LatePaymentPenaltyRate = table.Column<decimal>(type: "decimal(19,8)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsOverdraftContractActive = table.Column<bool>(type: "bit", nullable: false),
                    SignatureDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CanceledAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverdraftContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverdraftContracts_OverdraftAccounts_OverdraftAccountId",
                        column: x => x.OverdraftAccountId,
                        principalTable: "OverdraftAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverdraftAccounts_Id",
                table: "OverdraftAccounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OverdraftAccounts_IsBankAccountActive",
                table: "OverdraftAccounts",
                column: "IsBankAccountActive");

            migrationBuilder.CreateIndex(
                name: "IX_OverdraftContracts_Id",
                table: "OverdraftContracts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OverdraftContracts_IsOverdraftContractActive",
                table: "OverdraftContracts",
                column: "IsOverdraftContractActive");

            migrationBuilder.CreateIndex(
                name: "IX_OverdraftContracts_OverdraftAccountId",
                table: "OverdraftContracts",
                column: "OverdraftAccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverdraftContracts");

            migrationBuilder.DropTable(
                name: "OverdraftAccounts");
        }
    }
}
