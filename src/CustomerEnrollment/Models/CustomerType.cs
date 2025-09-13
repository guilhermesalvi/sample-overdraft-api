namespace CustomerEnrollment.Models;

[Flags]
public enum CustomerType : short
{
    None = 0,
    Individual = 1,
    Business = 2,
    Government = 4,
    NonProfit = 8
}
