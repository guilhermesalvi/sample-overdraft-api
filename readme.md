# 🏦 Sample Overdraft API

[![CI - Build and Test](https://github.com/guilhermesalvi/sample-overdraft-api/actions/workflows/dotnet.yml/badge.svg)](https://github.com/guilhermesalvi/sample-overdraft-api/actions/workflows/dotnet.yml)

> 🚧 This project is currently under development and subject to change. Please check back later for more updates.

This service is responsible for calculating monthly overdraft charges based on the customer's daily usage.

The calculation runs automatically at the beginning of each month and evaluates the customer’s usage from the **previous month**, including their balance status when the month rolls over.

> ⚠️ **Note:** The business rules applied in this service are based on Brazilian financial regulations and banking practices.

---

## 📋 Business Rules

> 💡 Although charges are accrued daily, the customer is only billed monthly for the total amount due from the previous cycle.

This service applies the following financial rules:

- 🛡️ **Grace Period**  
  A predefined number of days during which no interest is charged on overdraft usage.

- 📈 **Regular Interest**  
  The standard daily interest charged after the grace period ends.

- 💰 **Credit Tax (formerly IOF)**  
  A government-imposed tax applied to financial operations. It includes both a fixed rate and a daily rate, calculated proportionally to the amount and duration of overdraft usage.

- 🚨 **Over-limit Interest**  
  Additional interest applied on any usage that exceeds the overdraft limit.

- ⏰ **Late Payment Interest**  
  Daily interest charged if the customer ends the previous month with a negative balance and does not settle it in the following days.

- ⚠️ **Penalty**  
  A one-time fine applied when a customer rolls over into a new month with an outstanding negative balance.

---

## 📆 Monthly Timeline

```text
[Month N]
  ├── Daily usage is recorded
  └── End of month: system checks if the account ended negative

[Month N+1]
  ├── Day 1: system generates Daily Limit Usage Entries and applies applicable charges
  ├── If balance is still negative → apply penalty + start daily late interest
  └── If balance becomes zero → stop late interest accrual
  ├── Day 1: charges are calculated
  ├── If balance is still negative → apply penalty + start daily late interest
  └── If balance becomes zero → stop late interest accrual
```

---

## 📊 Practical Charging Rules

This table summarizes which charges apply based on the customer's overdraft usage and behavior.

| Situation                                                 | Credit Tax | Regular Interest | Over-limit Interest | Penalty | Late Interest |
|-----------------------------------------------------------|-------------|------------------|---------------------|---------|---------------|
| Did not use overdraft                                     | ❌          | ❌                | ❌                   | ❌       | ❌             |
| Used overdraft within grace and paid before month end     | ✅          | ❌                | ❌                   | ❌       | ❌             |
| Used overdraft, exceeded grace, and paid before month end | ✅          | ✅                | ❌                   | ❌       | ❌             |
| Used overdraft, exceeded limit, and paid before month end | ✅          | ✅                | ✅                   | ❌       | ❌             |
| Used overdraft within grace and didn’t pay by month end   | ✅          | ❌                | ❌                   | ✅       | ✅             |
| Used overdraft, exceeded grace, and didn’t pay            | ✅          | ✅                | ❌                   | ✅       | ✅             |
| Used overdraft, exceeded limit, and didn’t pay            | ✅          | ✅                | ✅                   | ✅       | ✅             |

---

> ℹ️ Although charges are accrued daily, they are only billed to the customer once per month.

## 📘 Glossary

| Term                      | Description                                                                 |
|---------------------------|-----------------------------------------------------------------------------|
| **Principal Amount**       | The amount of overdraft used by the customer                                |
| **Approved Overdraft Limit**        | The agreed overdraft limit on the account                                   |
| **Self-declared Limit**     | The limit optionally defined by the customer for personal control            |
| **Used Over-limit**         | Amount exceeding the approved overdraft limit                               |
| **Grace Period Days**       | Number of days without interest accrual                                     |
| **Interest Rate**           | Daily interest rate applied to principal usage                              |
| **Credit Tax Rate**         | Daily rate used to calculate government-imposed tax on the overdraft        |
| **Fixed Credit Tax Rate**   | Fixed rate tax applied once when the overdraft is first used                |
| **Over-limit Rate**         | Interest rate applied to the over-limit portion                             |
| **Late Payment Rate**       | Interest rate applied after overdue period (post rollover)                  |
| **Penalty Rate**            | One-time fine rate applied when debt rolls over into a new month            |
| **Interest**                | Interest amount due on the reference date                                   |
| **Credit Tax**              | Daily portion of credit tax calculated on the overdraft used                 |
| **Fixed Credit Tax**        | Fixed tax amount charged at first use of overdraft                          |
| **Over-limit Interest**     | Interest charged on amount exceeding the overdraft limit                    |
| **Late Payment Interest**   | Interest charged on overdue balance from previous cycle                     |
| **Late Payment Penalty**    | Fixed penalty amount charged when balance carries into a new month          |
| **Total Charges**           | Total of all charges due for the reference day                              |
| **Contract**                | Defines the terms of overdraft usage between bank and customer              |
| **Account**                 | Represents the customer's overdraft-enabled account and its usage data      |
| **Contract Agreement**      | Formal link between an account and a contract, including signature date     |
| **Daily Limit Usage Entry** | Daily snapshot with state, applied rates, and calculated charges            |

---

## 🧪 Examples

### Case 1: Month Ends with Negative Balance

**Scenario:**  
Customer ends **April** with **-R$100**

- May 1: Balance becomes -R$200
- May 3: Deposit of R$100 → balance becomes -R$100
- May 5: Deposit of R$100 → balance is 0

**What happens:**

- ✅ A **penalty** is applied on May 1st (based on -R$100 from April)
- ✅ **Late payment interest** is charged daily until May 4th
- ❌ No charges after May 5th

### Case 2: Exceeded Grace Period

**Scenario:**  
Contract allows 5 grace days  
Customer uses overdraft for 10 days in April

**What happens:**

- ✅ **Credit Tax** is charged on all 10 days
- ✅ **Regular interest** is charged on all 10 days
- ❌ Grace period does **not** exempt only the first 5 days — once exceeded, **all days** are charged

### Case 3: Stayed Within Grace Period

**Scenario:**  
Grace period = 5 days  
Customer used overdraft for 3 days in April

**What happens:**

- ✅ Only **Credit Tax** is charged
- ❌ No **regular interest**, since usage stayed within grace period
- ❌ No penalty or late interest, since month did not end negative

### Case 4: Used Limit, Then Paid Off Mid-Month

**Scenario:**  
Customer used overdraft from April 1–10  
Balance was zero from April 11 onward

**What happens:**

- ✅ Credit Tax and interest are applied only for the days with negative balance
- ❌ No charges for days with zero balance
- ❌ No penalty or late interest, since balance was cleared before end of the month

### Case 5: Exceeded Overdraft Limit

**Scenario:**  
Customer's overdraft limit is R$500  
On April 12, customer used R$700

**What happens:**

- ✅ Interest is applied on full R$700
- ✅ **Over-limit interest** is applied on the R$200 excess
- ✅ Credit Tax also applies on the full amount used

> ℹ️ These examples illustrate how charges are applied based on real-world overdraft usage patterns and Brazilian banking rules.

---

## 🗂️ Project Structure

```
sample-overdraft-api/
│
├── src/
│   ├── Api/
│   │  ├── Features/
│
├── tests/
│   ├── IntegrationTests/
│   ├── UnitTests/
│
└── Overdraft.sln
```

---

## ⚙️ Technologies Used

- [C#](https://docs.microsoft.com/pt-br/dotnet/csharp/)
- [ASP.NET Core](https://learn.microsoft.com/pt-br/aspnet/core/?view=aspnetcore-9.0&viewFallbackFrom=aspnetcore-9)
- [Serilog](https://github.com/serilog/serilog)
- [Dapper]()
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [xUnit](https://xunit.net/)
- [FluentAssertions](https://github.com/fluentassertions/fluentassertions)
