using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdventureWorks.Models
{
    [Table("User")]
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
}