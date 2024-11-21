using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Models;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Data
{
    public class AddDbContext : DbContext
    {
public AddDbContext(DbContextOptions<AddDbContext> options): base(options){ }
       public DbSet<Employee> Employees { get; set; }
       public  DbSet<Claim> Claims { get; set; }
    }
}
