# 🏦 Sample Overdraft API

[![CI - Build and Test](https://github.com/guilhermesalvi/sample-overdraft-api/actions/workflows/dotnet.yml/badge.svg)](https://github.com/guilhermesalvi/sample-overdraft-api/actions/workflows/dotnet.yml)

> 🚧 This project is currently under development and subject to change. Please check back later for more updates.

This service is responsible for calculating monthly overdraft charges based on the customer's daily usage.

The calculation runs automatically at the beginning of each month and evaluates the customer’s usage from the **previous
month**, including their balance status when the month rolls over.

> ⚠️ **Note:** The business rules applied in this service are based on Brazilian financial regulations and banking
> practices.

---

## 📋 Business Rules

This service applies the following financial rules:

- 🛡️ **Grace Period**  
  A predefined number of days during which no interest is charged on overdraft usage.

- 📈 **Regular Interest**  
  The standard daily interest charged after the grace period ends.

- 💸 **IOF Tax**  
  A federal tax (Imposto sobre Operações Financeiras) applied to financial operations in Brazil, calculated daily.

- 🚨 **Over-limit Interest (AD)**  
  Known locally as *"Adiantamento a Depositante"*, this interest is charged on any usage that exceeds the customer’s
  overdraft limit.

- ⏰ **Late Payment Interest (Mora)**  
  Daily interest charged if the customer ends the previous month with a negative balance and does not settle it in the
  following days.

- ⚠️ **Penalty (Multa)**  
  A one-time fine applied when a customer rolls over into a new month with an outstanding negative balance.

---

## 📆 Monthly Timeline

```text
[Month N]
  ├── Daily usage is recorded
  └── End of month: system checks if the account ended negative

[Month N+1]
  ├── Day 1: charges are calculated
  ├── If balance is still negative → apply penalty + start daily mora
  └── If balance becomes zero → stop late interest accrual
```

---

## 📊 Practical Charging Rules

This table summarizes which charges apply based on the customer's overdraft usage and behavior.

| Situation                                                 | IOF | Regular Interest | Over-limit Interest | Penalty | Late Interest |
|-----------------------------------------------------------|-----|------------------|---------------------|---------|---------------|
| Did not use overdraft                                     | ❌   | ❌                | ❌                   | ❌       | ❌             |
| Used overdraft within grace and paid before month end     | ✅   | ❌                | ❌                   | ❌       | ❌             |
| Used overdraft, exceeded grace, and paid before month end | ✅   | ✅                | ❌                   | ❌       | ❌             |
| Used overdraft, exceeded limit, and paid before month end | ✅   | ✅                | ✅                   | ❌       | ❌             |
| Used overdraft within grace and didn’t pay by month end   | ✅   | ❌                | ❌                   | ✅       | ✅             |
| Used overdraft, exceeded grace, and didn’t pay            | ✅   | ✅                | ❌                   | ✅       | ✅             |
| Used overdraft, exceeded limit, and didn’t pay            | ✅   | ✅                | ✅                   | ✅       | ✅             |

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
- ✅ **Late payment interest (mora)** is charged daily until May 4th
- ❌ No charges after May 5th

---

### Case 2: Exceeded Grace Period

**Scenario:**  
Contract allows 5 grace days  
Customer uses overdraft for 10 days in April

**What happens:**

- ✅ **IOF** is charged on all 10 days
- ✅ **Regular interest** is charged on all 10 days
- ❌ Grace period does **not** exempt only the first 5 days — once exceeded, **all days** are charged

---

### Case 3: Stayed Within Grace Period

**Scenario:**  
Grace period = 5 days  
Customer used overdraft for 3 days in April

**What happens:**

- ✅ Only **IOF** is charged
- ❌ No **regular interest**, since usage stayed within grace period
- ❌ No penalty or mora, since month did not end negative

---

### Case 4: Used Limit, Then Paid Off Mid-Month

**Scenario:**  
Customer used overdraft from April 1–10  
Balance was zero from April 11 onward

**What happens:**

- ✅ IOF and interest are calculated only for April 1–10
- ❌ No charges for days with zero balance
- ❌ No penalty or mora, since balance was cleared before end of the month

---

### Case 5: Exceeded Overdraft Limit

**Scenario:**  
Customer's overdraft limit is R$500  
On April 12, customer used R$700

**What happens:**

- ✅ Interest is applied on full R$700
- ✅ **Over-limit interest (AD)** is applied on the R$200 excess
- ✅ IOF also applies on the full amount used

> ℹ️ These examples illustrate how charges are applied based on real-world overdraft usage patterns and Brazilian
> banking rules.

---

## 🗂️ Project Structure

```
sample-overdraft-api/
│
├── src/
│   ├── Api/
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