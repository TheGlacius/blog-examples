﻿using RealWorldSolution.Migration.Discoverers;
using RealWorldSolution.Migration.Migrators;

namespace RealWorldSolution.Migration;

public class Orchestrator(IEnumerable<IDiscoverer> discoverers, IEnumerable<IMigrator> migrators)
{
    public void Orchestrate(MigrationContext context)
    {
        // Discovery Phase
        bool newEntitiesDiscovered;
        do
        {
            newEntitiesDiscovered = false;
            foreach (var discoverer in discoverers)
            {
                var initialCount = context.GetEntities().Count();
                discoverer.Discover(context);
                if (context.GetEntities().Count() > initialCount)
                {
                    newEntitiesDiscovered = true;
                }
            }
        } while (newEntitiesDiscovered);

        // Migration Phase
        foreach (var migrator in migrators)
        {
            migrator.Migrate(context);
        }
    }
}