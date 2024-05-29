using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HouseRentingSystem.Infrastructure
{
    public static class ClaimsPrincipleExtensions
    {
        public static string? Id(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return id;
        }
    }
}
