﻿using Example.Models;
using Example.Repositories;

namespace Example.Migration.Discoverers;

public class ClaimDiscoverer(ClaimRepository repository) : IDiscoverer
{
    public void Discover(MigrationContext context)
    {
        foreach (var debtor in context.GetDiscoveredEntitiesOfType<Debtor>())
        {
            var claims = repository.GetByDebtorId(debtor.Id);
            foreach (var claim in claims)
            {
                if (!context.IsDiscovered(claim))
                {
                    context.DiscoverEntity(claim);
                }
            }
        }
    }
}