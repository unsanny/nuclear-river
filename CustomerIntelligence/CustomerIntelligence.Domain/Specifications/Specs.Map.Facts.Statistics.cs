using System;
using System.Linq;

using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;
using NuClear.Utils.Join;

namespace NuClear.CustomerIntelligence.Domain.Specifications
{
    using Bit = NuClear.CustomerIntelligence.Domain.Model.Bit;
    using Facts = NuClear.CustomerIntelligence.Domain.Model.Facts;
    using Statistics = NuClear.CustomerIntelligence.Domain.Model.Statistics;

    public static partial class Specs
    {
        public static partial class Map
        {
            public static partial class Facts
            {
                public static partial class ToStatistics
                {
                    public static readonly MapSpecification<IQuery, IQueryable<Statistics::FirmCategory3>> FirmCategory3 =
                        new MapSpecification<IQuery, IQueryable<Statistics::FirmCategory3>>(
                            q =>
                                {
                                    // Запрос к фактам
                                    var firmDtos = from firm in q.For<Facts::Firm>()
                                                    join project in q.For<Facts::Project>() on firm.OrganizationUnitId equals project.OrganizationUnitId
                                                    join firmAddress in q.For<Facts::FirmAddress>() on firm.Id equals firmAddress.FirmId
                                                    join categoryFirmAddress in q.For<Facts::CategoryFirmAddress>() on firmAddress.Id equals categoryFirmAddress.FirmAddressId
                                                    select new
                                                    {
                                                        FirmId = firm.Id,
                                                        ProjectId = project.Id,
                                                        categoryFirmAddress.CategoryId
                                                    };

                                    var firmCounts = from firm in firmDtos
                                                     group firm by new { firm.ProjectId, firm.CategoryId }
                                                     into grp
                                                     select new
                                                     {
                                                         grp.Key.ProjectId,
                                                         grp.Key.CategoryId,
                                                         Count = grp.Count()
                                                     };

                                    var categories3 = from firmDto in firmDtos.Distinct()
                                                      join firmCount in firmCounts on new { firmDto.ProjectId, firmDto.CategoryId } equals new { firmCount.ProjectId, firmCount.CategoryId }
                                                      join category in q.For<Facts::Category>() on firmDto.CategoryId equals category.Id
                                                      select new Statistics::FirmCategory3
                                                      {
                                                          ProjectId = firmDto.ProjectId,
                                                          FirmId = firmDto.FirmId,
                                                          CategoryId = firmDto.CategoryId,
                                                          Name = category.Name,
                                                          FirmCount = firmCount.Count,
                                                      };

                                    // Запрос к Bit, содержит коcтыль для эмуляции full join между FirmCategoryStatistics и ProjectCategoryStatistics
                                    var partOne = from firmStatistics in q.For<Bit::FirmCategoryStatistics>()
                                                  from categoryStatistics in q.For<Bit::ProjectCategoryStatistics>()
                                                                           .Where(x => x.CategoryId == firmStatistics.CategoryId && x.ProjectId == firmStatistics.ProjectId)
                                                                           .DefaultIfEmpty()
                                                  select new { firmStatistics.CategoryId, firmStatistics.ProjectId, firmStatistics.FirmId, firmStatistics.Hits, firmStatistics.Shows, categoryStatistics.AdvertisersCount };

                                    var partTwo = from categoryStatistics in q.For<Bit::ProjectCategoryStatistics>()
                                                  from firmStatistics in q.For<Bit::FirmCategoryStatistics>()
                                                                              .Where(x => x.CategoryId == categoryStatistics.CategoryId && x.ProjectId == categoryStatistics.ProjectId)
                                                                              .DefaultIfEmpty()
                                                  where firmStatistics == null
                                                  select new { firmStatistics.CategoryId, firmStatistics.ProjectId, firmStatistics.FirmId, firmStatistics.Hits, firmStatistics.Shows, categoryStatistics.AdvertisersCount };

                                    var statistics = partOne.Union(partTwo);

                                    // Объединение данных из двух контекстов происходит в памяти процесса
                                    var r = categories3.MemoryGroupJoin(statistics,
                                                                       x => new StatKey { ProjectId = x.ProjectId, FirmId = x.FirmId, CategoryId = x.CategoryId },
                                                                       x => new StatKey { ProjectId = x.ProjectId, FirmId = x.FirmId, CategoryId = x.CategoryId },
                                                                       (cat, stats) => stats.FirstOrDefault() == null
                                                                                           ? new Statistics::FirmCategory3
                                                                                               {
                                                                                                   ProjectId = cat.ProjectId,
                                                                                                   FirmId = cat.FirmId,
                                                                                                   CategoryId = cat.CategoryId,
                                                                                                   Name = cat.Name,
                                                                                                   FirmCount = cat.FirmCount,
                                                                                               }
                                                                                           : new Statistics::FirmCategory3
                                                                                               {
                                                                                                   ProjectId = cat.ProjectId,
                                                                                                   FirmId = cat.FirmId,
                                                                                                   CategoryId = cat.CategoryId,
                                                                                                   Name = cat.Name,
                                                                                                   FirmCount = cat.FirmCount,
                                                                                                   Hits = stats.FirstOrDefault().Hits,
                                                                                                   Shows = stats.FirstOrDefault().Shows,
                                                                                                   AdvertisersShare = Math.Min(1, (float)stats.FirstOrDefault().AdvertisersCount / cat.FirmCount),
                                                                                               });

                                    return r;
                                });

                    private class StatKey : IComparable<StatKey>
                    {
                        public long ProjectId { get; set; }
                        public long FirmId { get; set; }
                        public long CategoryId { get; set; }

                        public int CompareTo(StatKey other)
                        {
                            if(ProjectId != other.ProjectId)
                                return ProjectId.CompareTo(other.ProjectId);

                            if (FirmId != other.FirmId)
                                return FirmId.CompareTo(other.FirmId);

                            if (CategoryId != other.CategoryId)
                                return CategoryId.CompareTo(other.CategoryId);

                            return 0;
                        }
                    }
                }
            }
        }
    }
}