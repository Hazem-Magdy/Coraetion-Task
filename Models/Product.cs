using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CoraetionTask.Data.Base;

namespace CoraetionTask.Models
{
    public class Product : IEntityBase
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
