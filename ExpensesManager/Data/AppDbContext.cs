using System.IO;
using ExpensesManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Transaction> Transactions => Set<Transaction>();
    
}