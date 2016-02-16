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
                                                  select new { firmStatistics.CategoryId, firmStatistics.ProjectId, FirmId = (long?)firmStatistics.FirmId, Hits = (int?)firmStatistics.Hits, Shows = (int?)firmStatistics.Shows, AdvertisersCount =(long?)null };

                                    var partTwo = from categoryStatistics in q.For<Bit::ProjectCategoryStatistics>()
                                                  select new { categoryStatistics.CategoryId, categoryStatistics.ProjectId, FirmId = (long?)null, Hits = (int?)null, Shows = (int?)null, AdvertisersCount = (long?)categoryStatistics.AdvertisersCount };

                                    var statistics = partOne.Union(partTwo);

                                    // Объединение данных из двух контекстов происходит в памяти процесса
                                    //var r = categories3.MemoryGroupJoin(statistics,
                                    //                                    x => new StatKey { ProjectId = x.ProjectId, CategoryId = x.CategoryId, FirmId = x.FirmId },
                                    //                                    x => new StatKey { ProjectId = x.ProjectId, CategoryId = x.CategoryId, FirmId = x.FirmId },
                                    //                                    (cat, stats) => stats.FirstOrDefault(x => x.FirmId.HasValue) == null
                                    //                                                        ? stats.FirstOrDefault(x => !x.FirmId.HasValue) == null
                                    //                                                              ? new Statistics::FirmCategory3
                                    //                                                                  {
                                    //                                                                      ProjectId = cat.ProjectId,
                                    //                                                                      FirmId = cat.FirmId,
                                    //                                                                      CategoryId = cat.CategoryId,
                                    //                                                                      Name = cat.Name,
                                    //                                                                      FirmCount = cat.FirmCount,
                                    //                                                                  }
                                    //                                                              : new Statistics::FirmCategory3
                                    //                                                                  {
                                    //                                                                      ProjectId = cat.ProjectId,
                                    //                                                                      FirmId = cat.FirmId,
                                    //                                                                      CategoryId = cat.CategoryId,
                                    //                                                                      Name = cat.Name,
                                    //                                                                      FirmCount = cat.FirmCount,
                                    //                                                                      AdvertisersShare = Math.Min(1, (float) stats.FirstOrDefault(x => !x.FirmId.HasValue).AdvertisersCount / cat.FirmCount),
                                    //                                                                  }
                                    //                                                        : stats.FirstOrDefault(x => !x.FirmId.HasValue) == null
                                    //                                                              ? new Statistics::FirmCategory3
                                    //                                                                  {
                                    //                                                                      ProjectId = cat.ProjectId,
                                    //                                                                      FirmId = cat.FirmId,
                                    //                                                                      CategoryId = cat.CategoryId,
                                    //                                                                      Name = cat.Name,
                                    //                                                                      FirmCount = cat.FirmCount,
                                    //                                                                      Hits = stats.FirstOrDefault(x => x.FirmId.HasValue).Hits.Value,
                                    //                                                                      Shows = stats.FirstOrDefault(x => x.FirmId.HasValue).Shows.Value,
                                    //                                                                  }
                                    //                                                              : new Statistics::FirmCategory3
                                    //                                                                  {
                                    //                                                                      ProjectId = cat.ProjectId,
                                    //                                                                      FirmId = cat.FirmId,
                                    //                                                                      CategoryId = cat.CategoryId,
                                    //                                                                      Name = cat.Name,
                                    //                                                                      FirmCount = cat.FirmCount,
                                    //                                                                      Hits = stats.FirstOrDefault(x => x.FirmId.HasValue).Hits.Value,
                                    //                                                                      Shows = stats.FirstOrDefault(x => x.FirmId.HasValue).Shows.Value,
                                    //                                                                      AdvertisersShare = Math.Min(1, (float)stats.FirstOrDefault(x => !x.FirmId.HasValue).AdvertisersCount / cat.FirmCount),
                                    //                                                                  });
                                    var r = categories3.MemoryGroupJoin(statistics,
                                                                        x => new StatKey { ProjectId = x.ProjectId, CategoryId = x.CategoryId, FirmId = x.FirmId },
                                                                        x => new StatKey { ProjectId = x.ProjectId, CategoryId = x.CategoryId, FirmId = x.FirmId },
                                                                        (cat, stats) => new {cat, stats});

                                    var z = statistics.ToArray();
                                    var rx = r.ToArray();
                                    throw new Exception();
                                });

                    private class StatKey : IComparable<StatKey>
                    {
                        public long ProjectId { get; set; }
                        public long CategoryId { get; set; }
                        public long? FirmId { get; set; }

                        public int CompareTo(StatKey other)
                        {
                            if(ProjectId != other.ProjectId)
                                return ProjectId.CompareTo(other.ProjectId);

                            if (CategoryId != other.CategoryId)
                                return CategoryId.CompareTo(other.CategoryId);

                            // Грязный хак, если хотя бы одно из полей null - считаем совпадение.
                            if (FirmId.HasValue && other.FirmId.HasValue)
                                return FirmId.Value.CompareTo(other.FirmId.Value);

                            return 0;
                        }
                    }
                }
            }
        }
    }
}