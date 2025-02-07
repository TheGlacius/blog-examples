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
        var lawFirmRepository = new LawFirmRepository();
        var clerkRepository = new ClerkRepository();
        var debtorRepository = new DebtorRepository();
        var claimRepository = new ClaimRepository();
        
        var sourceLawFirm = new LawFirm();
        lawFirmRepository.Add(sourceLawFirm);
        
        var destinationLawFirm = new LawFirm();
        lawFirmRepository.Add(destinationLawFirm);
        
        var sourceClerkToKeep = CreateClerkWithDebtorsAndClaims(
            sourceLawFirm, 
            clerkRepository, 
            debtorRepository, 
            claimRepository);
        
        var sourceClerkToMigrate = CreateClerkWithDebtorsAndClaims(
            sourceLawFirm, 
            clerkRepository, 
            debtorRepository, 
            claimRepository);

        var destinationClerkToMigrateTo = CreateClerkWithDebtorsAndClaims(
            destinationLawFirm, 
            clerkRepository, 
            debtorRepository, 
            claimRepository);

        var destinationClerkUnaffected = CreateClerkWithDebtorsAndClaims(
            destinationLawFirm, 
            clerkRepository, 
            debtorRepository, 
            claimRepository);

        // Discoverers & Migrators
        var discoverers = new List<IDiscoverer>
        {
            new DebtorDiscoverer(debtorRepository),
            new ClaimDiscoverer(claimRepository)
        };

        var migrators = new List<IMigrator>
        {
            new ClerkMigrator(clerkRepository),
            new DebtorMigrator(debtorRepository),
            new ClaimMigrator(claimRepository)
        };

        // Pre-populated MigrationContext
        var migrationContext = new MigrationContext();
        migrationContext.RegisterMapping(sourceClerkToMigrate, destinationClerkToMigrateTo);

        var orchestrator = new Orchestrator(discoverers, migrators);

        // Act
        orchestrator.Orchestrate(migrationContext);

        // Assert
        
        // Debtors to keep are still associated with Clerk to keep and remain in source law firm
        var debtorsToKeep = debtorRepository
            .GetByClerkId(sourceClerkToKeep.Id);

        foreach (var debtor in debtorsToKeep)
        {
            Assert.Equal(sourceLawFirm.Id, debtor.LawFirmId);
        }
        
        // Source Clerk to migrate has no debtors anymore
        var debtorsOfSourceClerkToMigrate = debtorRepository
            .GetByClerkId(sourceClerkToMigrate.Id);
        
        Assert.Empty(debtorsOfSourceClerkToMigrate);
        
        // Debtors of source clerk to migrate moved to destination clerk to migrate to
        var migratedDebtors = debtorRepository
            .GetByClerkId(destinationClerkToMigrateTo.Id)
            .ToList();

        Assert.Equal(4, migratedDebtors.Count);
        
        foreach (var migratedDebtor in migratedDebtors)
        {
            Assert.Equal(destinationLawFirm.Id, migratedDebtor.LawFirmId);
            
            var claims = claimRepository
                .GetByDebtorId(migratedDebtor.Id)
                .ToList();
            
            Assert.Equal(2, claims.Count);
            
            claims.ForEach(claim 
                => Assert.Equal(destinationLawFirm.Id, claim.LawFirmId));
        }
        
        // Destination clerk that should stay unaffected is unaffected
        var unaffectedDebtors = debtorRepository
            .GetByClerkId(destinationClerkUnaffected.Id)
            .ToList();

        Assert.Equal(2, unaffectedDebtors.Count);
        
        foreach (var unaffectedDebtor in unaffectedDebtors)
        {
            Assert.Equal(destinationLawFirm.Id, unaffectedDebtor.LawFirmId);
            
            var claims = claimRepository
                .GetByDebtorId(unaffectedDebtor.Id)
                .ToList();
            
            Assert.Equal(2, claims.Count);
            
            claims.ForEach(claim 
                => Assert.Equal(destinationLawFirm.Id, claim.LawFirmId));
        }
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

        var clerkRepository = new ClerkRepository();
        clerkRepository.Add(clerk);

        var debtorRepository = new DebtorRepository();
        debtorRepository.Add(debtor1);
        debtorRepository.Add(debtor2);

        var claimRepository = new ClaimRepository();
        claimRepository.Add(claim);

        // Discoverers & Migrators
        var discoverers = new List<IDiscoverer>
        {
            new ClerkDiscoverer(clerkRepository),
            new DebtorDiscoverer(debtorRepository),
            new ClaimDiscoverer(claimRepository)
        };

        var migrators = new List<IMigrator>
        {
            new ClaimMigrator(claimRepository),
            new ClerkMigrator(clerkRepository),
            new DebtorMigrator(debtorRepository)
        };

        // Pre-populated MigrationContext
        var migrationContext = new MigrationContext();
        migrationContext.RegisterMapping(debtor1, debtor2);

        var orchestrator = new Orchestrator(discoverers, migrators);

        // Act
        orchestrator.Orchestrate(migrationContext);

        // Assert
        Assert.Equal(debtor2.Id, claimRepository.GetById(claim.Id).DebtorId);
    }
    
    private static Clerk CreateClerkWithDebtorsAndClaims(
        LawFirm lawFirm, 
        ClerkRepository clerkRepository, 
        DebtorRepository debtorRepository, 
        ClaimRepository claimRepository)
    {
        var clerk = new Clerk { LawFirmId = lawFirm.Id };
        clerkRepository.Add(clerk);

        CreateDebtorWithClaims(clerk, debtorRepository, claimRepository);
        CreateDebtorWithClaims(clerk, debtorRepository, claimRepository);

        return clerk;
    }

    private static Debtor CreateDebtorWithClaims(
        Clerk clerk, 
        DebtorRepository debtorRepository, 
        ClaimRepository claimRepository)
    {
        var debtor = new Debtor { ClerkId = clerk.Id , LawFirmId = clerk.LawFirmId };
        debtorRepository.Add(debtor);

        CreateClaim(debtor, claimRepository);
        CreateClaim(debtor, claimRepository);

        return debtor;
    }

    private static Claim CreateClaim(Debtor debtor, ClaimRepository claimRepository)
    {
        var claim = new Claim { DebtorId = debtor.Id, LawFirmId = debtor.LawFirmId };
        claimRepository.Add(claim);
        return claim;
    }

}