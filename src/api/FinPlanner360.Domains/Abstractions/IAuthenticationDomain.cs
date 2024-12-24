using FinPlanner360.Domains.Entities;
using FinPlanner360.Entities.Authentication;

namespace FinPlanner360.Domains.Abstractions
{
    public interface IAuthenticationDomain
    {
        Task<DomainOutput<LoginOutput>> LoginUserAsync(bool generateToken, DomainInput<LoginInput> loginUser);
    }
}
