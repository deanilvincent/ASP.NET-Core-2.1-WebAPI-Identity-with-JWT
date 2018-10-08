using System.Threading.Tasks;
using MyApp.Models;

namespace MyApp.Services
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateJwtTokenString(User user);
    }
}
