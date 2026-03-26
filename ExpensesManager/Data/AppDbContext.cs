using ExpensesManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Data;

public class AppDbContext : DbContext
{
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=./Data/app.db");
    }
}