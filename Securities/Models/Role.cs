using System.ComponentModel.DataAnnotations;

namespace Securities.Models
{
    public class RoleModel
    {
        public List<Role> RoleDetailLists { get; set; }
    }
    public class Role
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "Enter name, please")]
        public string Name { get; set; }

        public string? Description { get; set; }


        [Required(ErrorMessage = "Choose Status, please")]
        public int Status { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }
    }
}
