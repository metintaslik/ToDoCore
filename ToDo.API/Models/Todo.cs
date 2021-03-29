using System;

namespace ToDo.API.Models
{
    public class Todo
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Alert { get; set; }
        public bool IsPinned { get; set; }
        public DateTime? PinnedDateTime { get; set; }
        public bool IsItDone { get; set; }
    }
}
