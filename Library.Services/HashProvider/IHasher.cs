using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Services.HashProvider
{
    public interface IHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
