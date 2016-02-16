using System;
using System.Linq;

using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;

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
                                                      join firmCount in firmCounts on new { firmDto.ProjectId, firmDto.CategoryId } equals
                                                          new { firmCount.ProjectId, firmCount.CategoryId }
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
                                    var firmStatistics = q.For<Bit::FirmCategoryStatistics>().AsEnumerable();
                                    var projectStatistics = q.For<Bit::ProjectCategoryStatistics>().AsEnumerable();

                                    var result = from category in categories3.AsEnumerable()
                                                 join firmStat in firmStatistics on new { category.FirmId, category.CategoryId, category.ProjectId } equals new { firmStat.FirmId, firmStat.CategoryId, firmStat.ProjectId } into firmStats
                                                 join projectStat in projectStatistics on new { category.CategoryId, category.ProjectId } equals new { projectStat.CategoryId, projectStat.ProjectId } into projectStats
                                                 let firmStat = firmStats.FirstOrDefault()
                                                 let projectStat = projectStats.FirstOrDefault()
                                                 select new Statistics::FirmCategory3
                                                     {
                                                         ProjectId = category.ProjectId,
                                                         FirmId = category.FirmId,
                                                         CategoryId = category.CategoryId,
                                                         Name = category.Name,
                                                         FirmCount = category.FirmCount,
                                                         Hits = firmStat?.Hits ?? 0,
                                                         Shows = firmStat?.Shows ?? 0,
                                                         AdvertisersShare = Math.Min(1, (float)(projectStat?.AdvertisersCount ?? 0) / category.FirmCount),
                                                     };

                                    return result.AsQueryable();
                                });
                }
            }
        }
    }
}