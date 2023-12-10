using Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ClientContext : DbContext
{
    public ClientContext(DbContextOptions<ClientContext> options) 
        : base(options)
    {
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase(databaseName: "ClientDb");
    }

    public DbSet<Client> Clients { get; set; }
}