using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Models
{
    [Table("User")]
    public class UserModel
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
    }
}