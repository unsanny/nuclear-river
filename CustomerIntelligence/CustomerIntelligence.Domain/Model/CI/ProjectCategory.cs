﻿namespace NuClear.CustomerIntelligence.Domain.Model.CI
{
    public sealed class ProjectCategory : ICustomerIntelligenceObject
    {
        public long ProjectId { get; set; }
        
        public long CategoryId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public long? ParentId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj is ProjectCategory && Equals((ProjectCategory)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ProjectId.GetHashCode() * 397) ^ CategoryId.GetHashCode();
            }
        }

        private bool Equals(ProjectCategory other)
        {
            return ProjectId == other.ProjectId && CategoryId == other.CategoryId;
        }
    }
}