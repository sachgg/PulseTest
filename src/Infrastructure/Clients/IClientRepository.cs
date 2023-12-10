using Domain.Clients;

namespace Infrastructure.Clients;

public interface IClientRepository
{
    Task<bool> ClientExists(Guid id);
    Task<List<Client>> GetAll();
    Task<Client> Get(Guid id);
    Task<Guid> Create(Client client);
    Task Delete(Client client);
    Task Update(Client client);
}