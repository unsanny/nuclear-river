﻿using System.Collections.Generic;
using System.Linq;

using NuClear.CustomerIntelligence.Domain.Model.CI;
using NuClear.CustomerIntelligence.Domain.Model.Statistics;
using NuClear.River.Common.Metadata.Model.Operations;
using NuClear.Storage.API.Specifications;

namespace NuClear.CustomerIntelligence.Domain.Specifications
{
    public static partial class Specs
    {
        public static partial class Find
        {
            public static partial class CI
            {
                public static FindSpecification<ClientContact> ClientContacts(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<ClientContact>(x => aggregateIds.Contains(x.ClientId));
                }

                public static FindSpecification<FirmActivity> FirmActivities(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<FirmActivity>(x => aggregateIds.Contains(x.FirmId));
                }

                public static FindSpecification<FirmBalance> FirmBalances(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<FirmBalance>(x => aggregateIds.Contains(x.FirmId));
                }

                public static FindSpecification<FirmCategory1> FirmCategories1(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<FirmCategory1>(x => aggregateIds.Contains(x.FirmId));
                }

                public static FindSpecification<FirmCategory2> FirmCategories2(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<FirmCategory2>(x => aggregateIds.Contains(x.FirmId));
                }

                public static FindSpecification<FirmTerritory> FirmTerritories(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<FirmTerritory>(x => aggregateIds.Contains(x.FirmId));
                }

                public static FindSpecification<ProjectCategory> ProjectCategories(IReadOnlyCollection<long> aggregateIds)
                {
                    return new FindSpecification<ProjectCategory>(x => aggregateIds.Contains(x.ProjectId));
                }

                public static FindSpecification<FirmCategory3> FirmCategory3(IReadOnlyCollection<StatisticsKey> entityIds)
                {
                    return new FindSpecification<FirmCategory3>(
                        x => entityIds.Contains(new StatisticsKey { ProjectId = x.ProjectId, CategoryId = x.CategoryId })
                             || entityIds.Contains(new StatisticsKey { ProjectId = x.ProjectId, CategoryId = null }));
                }

                public static FindSpecification<FirmForecast> FirmForecast(IReadOnlyCollection<StatisticsKey> entityIds)
                {
                    return new FindSpecification<FirmForecast>(
                        x => entityIds.Contains(new StatisticsKey { ProjectId = x.ProjectId, CategoryId = null }));
                }
            }
        }
    }
}