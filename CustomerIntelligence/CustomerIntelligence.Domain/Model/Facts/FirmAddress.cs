﻿namespace NuClear.CustomerIntelligence.Domain.Model.Facts
{
    public sealed class FirmAddress : IErmFactObject
    {
        public long Id { get; set; }

        public long FirmId { get; set; }

        public long? TerritoryId { get; set; }
    }
}