using System.Security.Claims;
using TodoApp.Dto;

namespace TodoApp.Services.Interfaces
{
    public interface IJwtService
    {
        public TokenDto GenerateToken(string id);
        public Boolean ValidateToken(string token);
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
