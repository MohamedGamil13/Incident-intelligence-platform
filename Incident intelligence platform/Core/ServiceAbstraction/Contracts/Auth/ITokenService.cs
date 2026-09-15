using Domain.Entities.Users;

namespace ServiceAbstraction.Contracts.Auth
{
    internal interface ITokenService
    {
        public (string Token, DateTime Expiration) CreateTokenAsync(ApplicationUser user, IList<string> roles);

    }
}
