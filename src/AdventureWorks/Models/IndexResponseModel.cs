using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorks.Models
{
    public class IndexResponseModel
    {
        public int Count { get; set; }
        public IEnumerable<UserModel> Users { get; set; }  = Enumerable.Empty<UserModel>();
    }
}