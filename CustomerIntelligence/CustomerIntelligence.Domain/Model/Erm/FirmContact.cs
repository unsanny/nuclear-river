﻿namespace NuClear.CustomerIntelligence.Domain.Model.Erm
{
    public sealed class FirmContact : IErmObject
    {
        public long Id { get; set; }

        public int ContactType { get; set; }

        public long? FirmAddressId { get; set; }
    }
}