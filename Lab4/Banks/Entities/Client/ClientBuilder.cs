using Banks.Tools;

namespace Banks.Entities.Client;

public class ClientBuilder : IClientBuilder
{
    private readonly Client _client = new Client();

    public IClientBuilder SetNameAndSurname(string? name, string? surname)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            throw new BanksException("Cannot create client without name or surname");
        _client.Name = name;
        _client.Surname = surname;
        return this;
    }

    public IClientBuilder SetAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new BanksException("Cannot create client with null address");
        _client.Address = address;
        return this;
    }

    public IClientBuilder SetPassport(int passportId)
    {
        if (passportId <= 0)
            throw new BanksException("Cannot create client with this passport");
        _client.Passport = passportId;
        return this;
    }

    public Client Build()
    {
        Client client = _client;
        return client;
    }
}