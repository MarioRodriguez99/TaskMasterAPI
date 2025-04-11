using System.ComponentModel.DataAnnotations;

namespace TaskMasterAPI.Models.DTOs
{
    public class UpdateTaskDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Range(1, 3)]
        public int Priority { get; set; }

        public bool IsCompleted { get; set; }
    }
}
