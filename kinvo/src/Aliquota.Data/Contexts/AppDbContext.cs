using Aliquota.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aliquota.Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Investiment> Investiments => Set<Investiment>();
    public DbSet<Operation> Operations => Set<Operation>();
}