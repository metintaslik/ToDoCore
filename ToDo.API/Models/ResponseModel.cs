using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.API.Models
{
    public class ResponseModel<T> where T : class
    {
        public bool Error { get; set; } = false;
        public int Code { get; set; }
        public string Message { get; set; }
        public T Entity { get; set; }
    }
}