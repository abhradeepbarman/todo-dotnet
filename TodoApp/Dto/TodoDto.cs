using System.ComponentModel.DataAnnotations;

namespace TodoApp.Dto
{
    public class CreateTodoRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public required string Description { get; set; } = string.Empty;
    }

    public class UpdateTodoResponseDto
    {
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public required string Description { get; set; } = string.Empty;
    }
}
