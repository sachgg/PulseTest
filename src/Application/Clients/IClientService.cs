using Domain.Clients;

namespace Application.Clients;

public interface IClientService
{
    Task<List<Client>> GetAll();
    Task<Client> Get(Guid id);
    Task<Guid> Create(CreateClient client);
    Task<bool> Delete(Guid id);
    Task<bool> Update(Guid id, CreateClient createClient);
}