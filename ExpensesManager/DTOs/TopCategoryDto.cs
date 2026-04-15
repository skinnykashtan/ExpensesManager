namespace ExpensesManager.DTOs;

public class TopCategoryDto
{
    public int Rank {get; set;}
    public string CategoryName {get; set;} = string.Empty;
    public decimal TotalAmount {get; set;}
}