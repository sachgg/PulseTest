using Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Clients;

public class ClientRepository : IClientRepository
{
    private readonly ClientContext _context;
    public ClientRepository(ClientContext context)
    {
        _context = context;
    }

    public async Task<bool> ClientExists(Guid id)
    {
        return await _context.Clients.AnyAsync(x => x.Id == id);
    }

    public async Task<Guid> Create(Client client)
    {
        await _context.AddAsync(client);
        await _context.SaveChangesAsync();
        return client.Id;
    }

    public async Task Delete(Client client)
    {
        _context.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task<Client> Get(Guid id) 
        => await _context.Clients.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<List<Client>> GetAll()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task Update(Client client)
    {
        var entity = await _context.Clients.SingleOrDefaultAsync(x => x.Id == client.Id);

        entity.Email = client.Email;
        entity.Forename = client.Forename;
        entity.Surname = client.Surname;

        await _context.SaveChangesAsync();
    }
}