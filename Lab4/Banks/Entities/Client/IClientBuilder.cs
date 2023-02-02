namespace Banks.Entities.Client;

public interface IClientBuilder
{
    IClientBuilder SetNameAndSurname(string? name, string? surname);
    IClientBuilder SetAddress(string? address);
    IClientBuilder SetPassport(int passportId);
    Client Build();
}