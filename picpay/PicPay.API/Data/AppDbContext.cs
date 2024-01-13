using Microsoft.EntityFrameworkCore;
using PicPay.API.Models;

namespace PicPay.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}