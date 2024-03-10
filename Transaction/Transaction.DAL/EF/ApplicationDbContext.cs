using Microsoft.EntityFrameworkCore;

namespace Transaction.DAL.EF;

public class ApplicationDbContext : DbContext
{
    public DbSet<Entities.Transaction> Transactions { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

}
