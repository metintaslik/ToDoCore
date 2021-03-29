using System;

namespace ToDo.API.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Name { get; set; }
        public DateTime? PinnedDateTime { get; set; }
        public bool IsPinned { get; set; }
    }
}