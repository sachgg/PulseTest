using Application.Clients;
using Domain.Clients;
using Infrastructure.Clients;
using Moq;

namespace UnitTests.Clients;

public class ClientTests
{
    private readonly Mock<IClientRepository> _clientRepository;

    public ClientTests()
    {
        _clientRepository = new Mock<IClientRepository>();
    }

    [Fact]
    public async Task ShouldBeAbleToGetAllClients()
    {
        var clients = GetClients();
        _clientRepository.Setup(x => x.GetAll()).ReturnsAsync(clients);

        var clientService = new ClientService(_clientRepository.Object);

        var result = await clientService.GetAll();

        Assert.NotNull(result);
        Assert.Equal(result.Count, clients.Count);
    }

    [Fact]
    public async Task ShouldBeAbleToCreateNewClient()
    {
        var clients = GetClients();
        var originalCount = clients.Count;
        _clientRepository.Setup(x => x.GetAll()).ReturnsAsync(clients);
        _clientRepository.Setup(x => x.Create(It.IsAny<Client>())).ReturnsAsync((Client client) => 
        {
            client.Id = new Guid();
            clients.Add(client);
            return client.Id;
        });

        var clientService = new ClientService(_clientRepository.Object);

        var createClient = new CreateClient
        {
            Email = "e@mail.com",
            Forename = "Fname",
            Surname = "Sname"
        };

        var newId = await clientService.Create(createClient);
        var resultClients = await clientService.GetAll();

        Assert.NotNull(resultClients);
        Assert.NotEqual(originalCount, resultClients.Count);
        Assert.Contains(createClient.Email, resultClients.Select(r => r.Email));
        Assert.Contains(createClient.Forename, resultClients.Select(r => r.Forename));
        Assert.Contains(createClient.Surname, resultClients.Select(r => r.Surname));
    }

    [Fact]
    public async Task ShouldBeAbleToGetClientById()
    {
        var clients = GetClients();
        _clientRepository.Setup(x => x.GetAll()).ReturnsAsync(clients);
        _clientRepository.Setup(x => x.Create(It.IsAny<Client>())).ReturnsAsync((Client client) =>
        {
            client.Id = new Guid();
            clients.Add(client);
            return client.Id;
        });
        _clientRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
        {
            return clients.FirstOrDefault(x => x.Id == id);
        });

        var clientService = new ClientService(_clientRepository.Object);

        var createClient = new CreateClient
        {
            Email = "e@mail.com",
            Forename = "Fname",
            Surname = "Sname"
        };

        var newId = await clientService.Create(createClient);
        var client = await clientService.Get(newId);

        Assert.NotNull(client);
        Assert.Equal(client.Email, createClient.Email);
        Assert.Equal(client.Forename, createClient.Forename);
        Assert.Equal(client.Surname, createClient.Surname);
    }

    [Fact]
    public async Task ShouldBeAbleToUpdateExistingClient()
    {
        var clients = GetClients();
        _clientRepository.Setup(x => x.ClientExists(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>  clients.Any(x => x.Id == id));
        _clientRepository.Setup(x => x.GetAll()).ReturnsAsync(clients);
        _clientRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
        {
            return clients.FirstOrDefault(x => x.Id == id);
        });
        _clientRepository.Setup(x => x.Update(It.IsAny<Client>())).Callback((Client client) =>
        {
            foreach (var record in clients)
            {
                if (record.Id == client.Id)
                {
                    record.Email = client.Email;
                    record.Forename = client.Forename;
                    record.Surname = client.Surname;
                }
            }
        });

        var clientService = new ClientService(_clientRepository.Object);
        var clientId = (await clientService.GetAll()).Select(x => x.Id).FirstOrDefault();

        var createClient = new CreateClient
        {
            Email = "xxx@mail.com",
            Forename = "xForename",
            Surname = "xSurname"
        };

        var result = await clientService.Update(clientId, createClient);
        
        Assert.True(result);
        Assert.Contains(createClient.Email, clients.Select(x => x.Email));
        Assert.Contains(createClient.Forename, clients.Select(x => x.Forename));
        Assert.Contains(createClient.Surname, clients.Select(x => x.Surname));
    }

    [Fact]
    public async Task ShouldBeAbleToDeleteClient()
    {
        var clients = GetClients();
        var originalCount = clients.Count;

        _clientRepository.Setup(x => x.ClientExists(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => clients.Any(x => x.Id == id));
        _clientRepository.Setup(x => x.GetAll()).ReturnsAsync(clients);
        _clientRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
        {
            return clients.FirstOrDefault(x => x.Id == id);
        });        
        _clientRepository.Setup(x => x.Delete(It.IsAny<Client>())).Callback((Client client) =>
        {
            clients.Remove(client);
        });

        var clientService = new ClientService(_clientRepository.Object);
        var clientId = (await clientService.GetAll()).Select(x => x.Id).FirstOrDefault();

        var result = await clientService.Delete(clientId);
        var client = await clientService.Get(clientId);

        Assert.True(result);
        Assert.NotEqual(originalCount, clients.Count);
        Assert.Null(client);
    }

    private static List<Client> GetClients()
    {
        return new List<Client>()
        {
            new()
            {
                Id = new Guid("59df1809-8301-43c7-86b8-5b914dd886d9"),
                Email = "bob@mail.com",
                Forename = "Bob",
                Surname = "Smith"
            },
            new()
            {
                Id = new Guid("f68201ed-3ff2-4d94-8335-72b4a7241db8"),
                Email = "tom@mail.com",
                Forename = "Tom",
                Surname = "Cross"
            },
            new()
            {
                Id = new Guid("f22a57ca-ecb1-4671-abbb-2da04560e537"),
                Email = "jean@mail.com",
                Forename = "Jean",
                Surname = "Green"
            }
        };
    }
}