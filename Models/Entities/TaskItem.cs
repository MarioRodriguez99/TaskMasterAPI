using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMasterAPI.Models.Entities
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        [Range(1, 3)]
        public int Priority { get; set; } = 2; // 1: Low, 2: Medium, 3: High

        // Relación con usuario
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }  
    }
}
