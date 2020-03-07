using System;

namespace Library.Services.Contracts
{
    public interface IConsoleRenderer
    {
        string Input();        

        string InputParameters(string parameterName, Func<string, bool> validator);

        string InputParameters(string parameterName);

        void Output(string output);
    }
}
