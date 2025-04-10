using System.ComponentModel.DataAnnotations;

namespace TaskMasterAPI.Models.DTOs
{
    public class TaskDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Range(1, 3)]
        public int Priority { get; set; } = 2;
    }

    public class UpdateTaskDTO : TaskDTO
    {
        public bool IsCompleted { get; set; }
    }

}
