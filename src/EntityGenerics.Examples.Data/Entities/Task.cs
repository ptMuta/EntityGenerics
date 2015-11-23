using System;
using EntityGenerics.Core.Abstractions;

namespace EntityGenerics.Examples.Data.Entities
{
    public class Task : IEntity<int>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime? LastChangeAt { get; set; }
    }
}