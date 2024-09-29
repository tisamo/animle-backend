using Animle.Models;

namespace Animle.Classes
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<Quiz>? Quizzes { get; set; } = null;
    }
}