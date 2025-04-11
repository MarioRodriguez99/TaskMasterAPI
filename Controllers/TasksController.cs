using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskMasterAPI.Services;
using TaskMasterAPI.Models.DTOs;
using TaskMasterAPI.Models.Responses;
using System.Security.Claims;

namespace TaskMasterAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var userId = GetCurrentUserId();
            var tasks = await _taskService.GetAllTasksForUser(userId);
            return Ok(new ApiResponse(true, "Tasks retrieved", tasks));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var userId = GetCurrentUserId();
            var task = await _taskService.GetTaskById(id, userId);

            if (task == null)
            {
                return NotFound(new ApiResponse(false, "Task not found"));
            }

            return Ok(new ApiResponse(true, "Task retrieved", task));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(false, "Invalid data", ModelState));
            }

            var userId = GetCurrentUserId();
            var task = await _taskService.CreateTask(taskDto, userId);

            return CreatedAtAction(nameof(GetTask), new { id = task.Id },
                new ApiResponse(true, "Task created", task));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDTO taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(false, "Invalid data", ModelState));
            }

            var userId = GetCurrentUserId();
            var task = await _taskService.UpdateTask(id, taskDto, userId);

            if (task == null)
            {
                return NotFound(new ApiResponse(false, "Task not found"));
            }

            return Ok(new ApiResponse(true, "Task updated", task));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _taskService.DeleteTask(id, userId);

            if (!result)
            {
                return NotFound(new ApiResponse(false, "Task not found"));
            }

            return Ok(new ApiResponse(true, "Task deleted"));
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> ToggleTaskCompletion(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _taskService.ToggleTaskCompletion(id, userId);

            if (!result)
            {
                return NotFound(new ApiResponse(false, "Task not found"));
            }

            return Ok(new ApiResponse(true, "Task completion status updated"));
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
