using CoraetionTask.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace CoraetionTask.Models
{
    public class Customer : IEntityBase
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [StringLength(11, ErrorMessage = "Mobile cannot exceed 11 characters")]
        public string Mobile { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }

        [Display(Name = "Order History")]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
