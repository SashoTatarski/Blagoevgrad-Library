using System.Collections.Generic;

namespace Services.Contracts
{
    public interface IAuthenticationManager
    {      

        List<string> GetAllowedCommands();

        void CheckForExistingUsername(string username);
        
        void LogOut();
    }
}
