﻿using Example.Migration.Discoverers;
using Example.Migration.Migrators;

namespace Example.Migration;

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
                var initialCount = context.GetDiscoveredCount();
                discoverer.Discover(context);
                if (context.GetDiscoveredCount() > initialCount)
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