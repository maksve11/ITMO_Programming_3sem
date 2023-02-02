using Reports.BiLayer.Tools;
using Reports.DALayer.Entities.Repositories;
using Reports.DALayer.Models.Accounts;
using Reports.DALayer.Models.Logger;

namespace Reports.BiLayer.Authentication_system;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly List<int> _accounts;

    public AuthenticationManager(ILogger logger)
    {
        _accounts = new List<int>();
        Logger = logger;
    }

    public ILogger Logger { get; }

    public int Registration(IAccount account)
    {
        int tmpToken = account.GetToken();
        if (_accounts.Contains(tmpToken))
            throw new BiException("there's this account yet");
        _accounts.Add(tmpToken);
        string log = $"You have successfully registered!";
        Logger?.LogInformation(this.ToString(), log);
        return tmpToken;
    }

    public bool LogIn(int token)
    {
        if (!_accounts.Contains(token))
        {
            throw new BiException("there's no this account");
        }
        else
        {
            string log = $"You have successfully log in!";
            Logger?.LogInformation(this.ToString(), log);
            return true;
        }
    }
}