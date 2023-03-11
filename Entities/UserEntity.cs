using BasketStoreTelegramBot.States;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketStoreTelegramBot.Entities
{
    class UserEntity
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string ChatID { get; set; }
        public int CurrentState { get; set; }

    }
}
