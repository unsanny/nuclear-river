﻿using NUnit.Framework;

using Facts = NuClear.CustomerIntelligence.Domain.Model.Facts;
using Erm = NuClear.CustomerIntelligence.Domain.Model.Erm;
using CI = NuClear.CustomerIntelligence.Domain.Model.CI;

// ReSharper disable PossibleUnintendedReferenceComparison
namespace NuClear.CustomerIntelligence.Replication.Tests.Transformation
{
    [TestFixture]
    internal partial class FactDependencyTests
    {
        [Test]
        public void ShouldInitializeProjectIfProjectCreated()
        {
            SourceDb.Has(new Erm::Project { Id = 1, OrganizationUnitId = 2 });

            Transformation.Create(Query, RepositoryFactory)
                          .ApplyChanges<Facts::Project>(1)
                          .VerifyDistinct(Statistics.Operation(1),
                                          Aggregate.Initialize<CI::Project>(1));
        }

        [Test]
        public void ShouldDestroyProjectIfProjectDeleted()
        {
            TargetDb.Has(new Facts::Project { Id = 1, OrganizationUnitId = 2 });

            Transformation.Create(Query, RepositoryFactory)
                          .ApplyChanges<Facts::Project>(1)
                          .VerifyDistinct(Statistics.Operation(1),
                                          Aggregate.Destroy<CI::Project>(1));
        }

        [Test]
        public void ShouldRecalculateDependentAggregatesIfProjectUpdated()
        {
            SourceDb.Has(new Erm::Project { Id = 1, OrganizationUnitId = 2 });

            TargetDb.Has(new Facts::Project { Id = 1, OrganizationUnitId = 1 })
                   .Has(new Facts::Territory { Id = 1, OrganizationUnitId = 1 })
                   .Has(new Facts::Territory { Id = 2, OrganizationUnitId = 2 })
                   .Has(new Facts::Firm { Id = 1, OrganizationUnitId = 1 })
                   .Has(new Facts::Firm { Id = 2, OrganizationUnitId = 2 });

            Transformation.Create(Query, RepositoryFactory)
                          .ApplyChanges<Facts::Project>(1)
                          .VerifyDistinct(Statistics.Operation(1),
                                          Aggregate.Recalculate<CI::Territory>(1),
                                          Aggregate.Recalculate<CI::Firm>(1),
                                          Aggregate.Recalculate<CI::Project>(1),
                                          Aggregate.Recalculate<CI::Territory>(2),
                                          Aggregate.Recalculate<CI::Firm>(2));
        }
    }
}