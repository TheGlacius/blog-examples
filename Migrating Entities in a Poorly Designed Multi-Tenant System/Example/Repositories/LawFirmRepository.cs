using Example.Models;

namespace Example.Repositories;

public class LawFirmRepository
{
    private readonly ICollection<LawFirm> _lawFirms = new List<LawFirm>();

    public IReadOnlyCollection<LawFirm> GetAll() => _lawFirms.ToList().AsReadOnly();

    public LawFirm Get(Guid id) => _lawFirms.Single(tenant => tenant.Id == id);

    public void Add(LawFirm lawFirm) => _lawFirms.Add(lawFirm);

    public void Update(LawFirm lawFirm)
    {
        var existingLawFirm = Get(lawFirm.Id);
        _lawFirms.Remove(existingLawFirm);
        _lawFirms.Add(lawFirm);
    }

    public void Remove(Guid id)
    {
        var lawFirm = Get(id);
        _lawFirms.Remove(lawFirm);
    }
}