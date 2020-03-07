using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        public List<User> Users { get; set; }
    }
}
