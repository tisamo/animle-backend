using Animle.Classes;
using System.ComponentModel.DataAnnotations;

namespace Animle.SignalR
{
    public class Player
    {
        [Key] public int Id { get; set; }
        public string ConnectionId { get; set; }

        public string Username { get; set; }

        public int Rating { get; set; }

        public DateTime WaitingSince = new();

        public int? Result { get; set; } = 0;

        public List < AnimeFilter> animes { get; set; } = new List< AnimeFilter>();

        public bool PlayerWaitingForMatch { get; set; } = false;
        public Player opponent { get; set; } = null;
        public bool IsPlaying { get; set; } = false;

        public int? QuizId { get; set; } = null;

       public RestartableInterval MatchInterval { get; set; }

        public RestartableInterval TickInterval { get; set; }


    }
}