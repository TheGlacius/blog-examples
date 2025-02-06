using RealWorldSolution.Migration;
using RealWorldSolution.Migration.Discoverers;
using RealWorldSolution.Migration.Migrators;
using RealWorldSolution.Models;
using RealWorldSolution.Repositories;
using Xunit;

namespace RealWorldSolution.Tests;

public class MigrationTests
{
    [Fact]
    public void Migrate_Debtors_And_Claims_From_One_Clerk_To_Another()
    {
        // Arrange
        var lawFirm1 = new LawFirm();
        var lawFirm2 = new LawFirm();
        
        var clerk1 = new Clerk { LawFirmId = lawFirm1.Id };
        var clerk2 = new Clerk { LawFirmId = lawFirm2.Id };

        var debtor1 = new Debtor { ClerkId = clerk1.Id, LawFirmId = lawFirm1.Id };
        var debtor2 = new Debtor { ClerkId = clerk1.Id, LawFirmId = lawFirm1.Id };

        var claim1 = new Claim { DebtorId = debtor1.Id, LawFirmId = lawFirm1.Id };
        var claim2 = new Claim { DebtorId = debtor2.Id, LawFirmId = lawFirm1.Id };

        // Repositories
        var lawFirmRepo = new LawFirmRepository();
        lawFirmRepo.Add(lawFirm1);
        lawFirmRepo.Add(lawFirm2);

        var clerkRepo = new ClerkRepository();
        clerkRepo.Add(clerk1);
        clerkRepo.Add(clerk2);

        var debtorRepo = new DebtorRepository();
        debtorRepo.Add(debtor1);
        debtorRepo.Add(debtor2);

        var claimRepo = new ClaimRepository();
        claimRepo.Add(claim1);
        claimRepo.Add(claim2);

        // Discoverers & Migrators
        var discoverers = new List<IDiscoverer>
        {
            new ClerkDiscoverer(clerkRepo),
            new DebtorDiscoverer(debtorRepo),
            new ClaimDiscoverer(claimRepo)
        };

        var migrators = new List<IMigrator>
        {
            new ClerkMigrator(clerkRepo),
            new DebtorMigrator(debtorRepo),
            new ClaimMigrator(claimRepo)
        };

        // Pre-populated MigrationContext
        var migrationContext = new MigrationContext();
        migrationContext.AddMapping<LawFirm>(lawFirm1.Id, lawFirm2.Id);
        migrationContext.AddMapping<Clerk>(clerk1.Id, clerk2.Id);

        var orchestrator = new Orchestrator(discoverers, migrators);

        // Act
        orchestrator.Orchestrate(migrationContext);

        // Assert
        Assert.Equal(clerk2.Id, debtorRepo.GetById(debtor1.Id).ClerkId);
        Assert.Equal(clerk2.Id, debtorRepo.GetById(debtor2.Id).ClerkId);
        
        Assert.Equal(lawFirm2.Id, debtorRepo.GetById(debtor1.Id).LawFirmId);
        Assert.Equal(lawFirm2.Id, debtorRepo.GetById(debtor2.Id).LawFirmId);
        
        Assert.Equal(lawFirm2.Id, claimRepo.GetById(claim1.Id).LawFirmId);
        Assert.Equal(lawFirm2.Id, claimRepo.GetById(claim2.Id).LawFirmId);

        Assert.Equal(clerk2.Id, debtorRepo.GetById(debtor2.Id).ClerkId);
        Assert.Equal(lawFirm2.Id, debtorRepo.GetById(debtor2.Id).LawFirmId);
        Assert.Equal(lawFirm2.Id, claimRepo.GetById(claim2.Id).LawFirmId);
    }
    
    [Fact]
    public void Migrate_Claim_From_One_Debtor_To_Another()
    {
        // Arrange
        var lawFirm = new LawFirm();
        var clerk = new Clerk { LawFirmId = lawFirm.Id };

        var debtor1 = new Debtor { ClerkId = clerk.Id, LawFirmId = lawFirm.Id };
        var debtor2 = new Debtor { ClerkId = clerk.Id, LawFirmId = lawFirm.Id };

        var claim = new Claim { DebtorId = debtor1.Id, LawFirmId = lawFirm.Id };

        // Repositories
        var lawFirmRepo = new LawFirmRepository();
        lawFirmRepo.Add(lawFirm);

        var clerkRepo = new ClerkRepository();
        clerkRepo.Add(clerk);

        var debtorRepo = new DebtorRepository();
        debtorRepo.Add(debtor1);
        debtorRepo.Add(debtor2);

        var claimRepo = new ClaimRepository();
        claimRepo.Add(claim);

        // Discoverers & Migrators
        var discoverers = new List<IDiscoverer>
        {
            new DebtorDiscoverer(debtorRepo),
            new ClaimDiscoverer(claimRepo)
        };

        var migrators = new List<IMigrator>
        {
            new ClaimMigrator(claimRepo)
        };

        // Pre-populated MigrationContext
        var migrationContext = new MigrationContext();
        migrationContext.AddMapping<LawFirm>(lawFirm.Id, lawFirm.Id);
        migrationContext.AddMapping<Claim>(claim.Id, claim.Id);
        migrationContext.AddMapping<Debtor>(debtor1.Id, debtor2.Id);

        var orchestrator = new Orchestrator(discoverers, migrators);

        // Act
        orchestrator.Orchestrate(migrationContext);

        // Assert
        Assert.Equal(debtor2.Id, claimRepo.GetById(claim.Id).DebtorId);
    }
}