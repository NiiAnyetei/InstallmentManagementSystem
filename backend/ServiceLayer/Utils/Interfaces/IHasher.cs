using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Utils.Interfaces
{
    public interface IHasher
    {
        public byte[] GenerateSalt();
        public string HashPassword(string password, byte[] salt);
        public bool VerifyPassword(string enteredPassword, string storedHashedPassword, string salt);
    }
}
