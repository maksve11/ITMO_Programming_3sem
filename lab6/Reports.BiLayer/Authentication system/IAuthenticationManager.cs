using Reports.DALayer.Entities.Sources;
using Reports.DALayer.Models.Accounts;

namespace Reports.BiLayer.Authentication_system;

public interface IAuthenticationManager
{
    public int Registration(IAccount account);
    public bool LogIn(int token);
}