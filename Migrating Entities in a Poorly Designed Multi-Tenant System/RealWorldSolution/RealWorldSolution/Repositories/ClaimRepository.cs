using RealWorldSolution.Models;

namespace RealWorldSolution.Repositories;

public class ClaimRepository
{
    private readonly List<Claim> _claims = [];

    public IEnumerable<Claim> GetAll() => _claims;

    public Claim GetById(Guid id) => _claims.Single(c => c.Id == id);

    public IEnumerable<Claim> GetByDebtorId(Guid debtorId) => _claims.Where(c => c.DebtorId == debtorId);

    public void Add(Claim claim) => _claims.Add(claim);
    
    public void Update(Claim claim)
    {
        var index = _claims.FindIndex(c => c.Id == claim.Id);
        if (index != -1)
        {
            _claims[index] = claim;
        }
    }
}