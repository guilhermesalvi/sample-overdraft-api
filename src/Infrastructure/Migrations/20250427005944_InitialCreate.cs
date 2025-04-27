using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PersonType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAccountActive = table.Column<bool>(type: "bit", nullable: false),
                    CustomerAssetsHeld = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    ApprovedOverdraftLimit = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    SelfDeclaredLimit = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    UsedDaysInCurrentCycle = table.Column<int>(type: "int", nullable: false),
                    UsedOverLimit = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    RegularInterestDueInCurrentCycle = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    OverLimitInterestDueInCurrentCycle = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    OverLimitFixedFeeDueInCurrentCycle = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    LatePaymentInterestDueInCurrentCycle = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    LatePaymentPenaltyDueInCurrentCycle = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    CreditTaxDueInCurrentCycle = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    FixedCreditTaxDueInCurrentCycle = table.Column<decimal>(type: "decimal(28,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractAgreements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsContractAgreementActive = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SignatureDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAgreements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsContractActive = table.Column<bool>(type: "bit", nullable: false),
                    GracePeriodDays = table.Column<int>(type: "int", nullable: false),
                    MonthlyInterestRate = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    MonthlyOverLimitInterestRate = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    OverLimitFixedFee = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    MonthlyLatePaymentInterestRate = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    LatePaymentPenaltyRate = table.Column<decimal>(type: "decimal(28,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyLimitUsageEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    ApprovedOverdraftLimit = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    UsedOverLimit = table.Column<decimal>(type: "decimal(28,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyLimitUsageEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyChargeSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ApprovedOverdraftLimit = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    UsedDaysInCurrentCycle = table.Column<int>(type: "int", nullable: false),
                    GracePeriodDays = table.Column<int>(type: "int", nullable: false),
                    TotalRegularInterestDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    TotalOverLimitInterestDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    TotalOverLimitFixedFeeDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    TotalLatePaymentInterestDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    TotalLatePaymentPenaltyDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    TotalCreditTaxDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    TotalFixedCreditTaxDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    TotalDue = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    CapitalizedPrincipal = table.Column<decimal>(type: "decimal(28,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyChargeSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinAssetsHeld = table.Column<decimal>(type: "decimal(28,10)", nullable: false),
                    MaxAssetsHeld = table.Column<decimal>(type: "decimal(28,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConditions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Id",
                table: "Accounts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_IsAccountActive",
                table: "Accounts",
                column: "IsAccountActive");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAgreements_AccountId",
                table: "ContractAgreements",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAgreements_Id",
                table: "ContractAgreements",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractAgreements_IsContractAgreementActive",
                table: "ContractAgreements",
                column: "IsContractAgreementActive");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Id",
                table: "Contracts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_IsContractActive",
                table: "Contracts",
                column: "IsContractActive");

            migrationBuilder.CreateIndex(
                name: "IX_DailyLimitUsageEntries_AccountId",
                table: "DailyLimitUsageEntries",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyLimitUsageEntries_Id",
                table: "DailyLimitUsageEntries",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyLimitUsageEntries_ReferenceDate",
                table: "DailyLimitUsageEntries",
                column: "ReferenceDate");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyChargeSnapshots_AccountId",
                table: "MonthlyChargeSnapshots",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyChargeSnapshots_Id",
                table: "MonthlyChargeSnapshots",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyChargeSnapshots_ReferenceDate",
                table: "MonthlyChargeSnapshots",
                column: "ReferenceDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConditions_ContractId",
                table: "ProductConditions",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConditions_Id",
                table: "ProductConditions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductConditions_MaxAssetsHeld",
                table: "ProductConditions",
                column: "MaxAssetsHeld");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConditions_MinAssetsHeld",
                table: "ProductConditions",
                column: "MinAssetsHeld");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ContractAgreements");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "DailyLimitUsageEntries");

            migrationBuilder.DropTable(
                name: "MonthlyChargeSnapshots");

            migrationBuilder.DropTable(
                name: "ProductConditions");
        }
    }
}
