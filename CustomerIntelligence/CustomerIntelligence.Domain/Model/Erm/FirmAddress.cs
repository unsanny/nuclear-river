using NuClear.AdvancedSearch.Common.Metadata.Model;

namespace NuClear.CustomerIntelligence.Domain.Model.Erm
{
    public sealed class FirmAddress : IErmObject
    {
        public FirmAddress()
        {
            IsActive = true;
        }

        public long Id { get; set; }

        public long FirmId { get; set; }

        public long? TerritoryId { get; set; }

        public bool ClosedForAscertainment { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FirmAddress && IdentifiableObjectEqualityComparer<FirmAddress>.Default.Equals(this, (FirmAddress)obj);
        }

        public override int GetHashCode()
        {
            return IdentifiableObjectEqualityComparer<FirmAddress>.Default.GetHashCode(this);
        }
    }
}