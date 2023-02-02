using Banks.Entities.Accounts;
using Banks.Tools;

namespace Banks.Entities.Client;

public class Client
{
    private readonly List<Account> _accounts = new List<Account>();
    public Client() { }

    public string? Name { get; protected internal set; }
    public string? Surname { get; protected internal set; }
    public string? Address { get; protected internal set; }
    public int Passport { get; protected internal set; }

    public bool IsVerifiedClient => Passport > 0 && !string.IsNullOrWhiteSpace(Address);
    public void AddAccount(Account account)
    {
        if (_accounts.Contains(account))
            throw new BanksException("There's this account yet");
        _accounts.Add(account);
    }

    public void RemoveAccount(Account account)
    {
        if (!_accounts.Contains(account))
            throw new BanksException("There's no this account");
        _accounts.Remove(account);
    }

    public bool Equals(Client? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Surname == other.Surname && Address == other.Address && Passport == other.Passport;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Client)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Surname);
    }

    public override string ToString()
    {
        return $"Name: {Name} {Surname}\n" +
               $"Address: {Address}\n" +
               $"Passport: {Passport}";
    }
}