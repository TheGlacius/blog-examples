using RealWorldSolution.Models;

namespace RealWorldSolution.Repositories;

public class ClerkRepository
{
    private readonly List<Clerk> _clerks = [];

    public IEnumerable<Clerk> GetAll() => _clerks;

    public Clerk GetById(Guid id) => _clerks.Single(c => c.Id == id);

    public IEnumerable<Clerk> GetByLawFirmId(Guid lawFirmId) => _clerks.Where(c => c.LawFirmId == lawFirmId);

    public void Add(Clerk clerk) => _clerks.Add(clerk);
    
    public void Update(Clerk clerk)
    {
        var index = _clerks.FindIndex(c => c.Id == clerk.Id);
        if (index != -1)
        {
            _clerks[index] = clerk;
        }
    }
}