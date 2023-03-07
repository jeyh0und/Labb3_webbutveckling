using ButalandIntErasmus.Server.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ButalandIntErasmus.Server.DataAccess;

public class StaffDbContext : DbContext
{
    public DbSet<UserModel> LoginUsers { get; set; }
    public DbSet<StaffModel> Staff { get; set; }
    public StaffDbContext(DbContextOptions options) : base(options)
    {
    }
}