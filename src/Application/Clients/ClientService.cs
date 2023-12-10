using Domain.Clients;
using Infrastructure.Clients;

namespace Application.Clients;

public sealed class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Create(CreateClient client)
    {
        return await _repository.Create(MapCreateClientToClient(client));
    }

    public async Task<bool> Delete(Guid id)
    {
        var client = await _repository.Get(id);
        if (client == null)
            return false;

        await _repository.Delete(client);
        return true;
    }

    public async Task<List<Client>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Client> Get(Guid id)
    {
        return await _repository.Get(id);
    }

    public async Task<bool> Update(Guid id, CreateClient createClient)
    {
        var clientExists = await _repository.ClientExists(id);

        if (!clientExists)
            return false;

        var client = MapCreateClientToClient(createClient);
        client.Id = id;

        await _repository.Update(client);
        return true;
    }

    private static Client MapCreateClientToClient(CreateClient createClient)
        => new ()
            {
                Email = createClient.Email,
                Forename = createClient.Forename,
                Surname = createClient.Surname
            };
}