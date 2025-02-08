using Example.Models;

namespace Example.Repositories;

public class DebtorRepository
{
    private readonly List<Debtor> _debtors = [];

    public IEnumerable<Debtor> GetAll() => _debtors;

    public Debtor GetById(Guid id) => _debtors.Single(d => d.Id == id);
    public IEnumerable<Debtor> GetByClerkId(Guid clerkId) => _debtors.Where(d => d.ClerkId == clerkId);

    public void Add(Debtor debtor) => _debtors.Add(debtor);
    
    public void Update(Debtor debtor)
    {
        var index = _debtors.FindIndex(c => c.Id == debtor.Id);
        if (index != -1)
        {
            _debtors[index] = debtor;
        }
    }
}