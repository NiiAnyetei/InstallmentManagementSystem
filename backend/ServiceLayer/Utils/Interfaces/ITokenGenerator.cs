using DataLayer.Models.Data;
using System.Security.Claims;

namespace ServiceLayer.Utils.Interfaces
{
    public interface ITokenGenerator
    {
        Token GenerateToken(string username);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
