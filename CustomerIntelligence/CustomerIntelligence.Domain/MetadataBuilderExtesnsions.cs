﻿using System;
using System.Collections.Generic;

using NuClear.CustomerIntelligence.Domain.Model;
using NuClear.River.Common.Metadata.Builders;
using NuClear.River.Common.Metadata.Features;
using NuClear.River.Common.Metadata.Model;
using NuClear.River.Common.Metadata.Model.Operations;
using NuClear.Storage.API.Specifications;

namespace NuClear.CustomerIntelligence.Domain
{
    public static class MetadataBuilderExtesnsions
    {
        public static ImportDocumentMetadataBuilder<TDto> LeadsToProjectStatisticsCalculation<TDto>(this ImportDocumentMetadataBuilder<TDto> builder)
            where TDto : IBitDto
        {
            Func<TDto, IReadOnlyCollection<IOperation>> projector = x => new [] { new RecalculateStatisticsOperation(new StatisticsKey { ProjectId = x.ProjectId }) };
            var specification = new MapSpecification<TDto, IReadOnlyCollection<IOperation>>(projector);
            var feature = new MapSpecificationFeature<TDto, IReadOnlyCollection<IOperation>>(specification);
            return builder.WithFeatures(feature);
        }
    }
}