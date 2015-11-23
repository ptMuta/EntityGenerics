using System;

namespace EntityGenerics.Examples.Data.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime? LastChangeAt { get; set; }
    }
}