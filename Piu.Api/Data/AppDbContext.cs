using Microsoft.EntityFrameworkCore;

namespace Piu.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
