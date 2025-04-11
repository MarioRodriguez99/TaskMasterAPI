using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.Data;
using TaskMasterAPI.Models.DTOs;
using TaskMasterAPI.Models.Entities;

namespace TaskMasterAPI.Services
{
    public class TaskService
    {
        private readonly ApDbContext _context;

        public TaskService(ApDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksForUser(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskById(int id, int userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TaskItem> CreateTask(TaskDTO taskDto, int userId)
        {
            var task = new TaskItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Priority = taskDto.Priority,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<TaskItem?> UpdateTask(int id, UpdateTaskDTO taskDto, int userId)
        {
            var task = await GetTaskById(id, userId);
            if (task == null) return null;

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Priority = taskDto.Priority;
            task.IsCompleted = taskDto.IsCompleted;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTask(int id, int userId)
        {
            var task = await GetTaskById(id, userId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleTaskCompletion(int id, int userId)
        {
            var task = await GetTaskById(id, userId);
            if (task == null) return false;

            task.IsCompleted = !task.IsCompleted;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}