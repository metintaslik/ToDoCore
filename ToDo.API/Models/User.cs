using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.API.Models
{
    public class User
    {
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NameSurname { get; set; }
    }
}
