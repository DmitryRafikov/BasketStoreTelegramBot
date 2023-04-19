using BasketStoreTelegramBot.States;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketStoreTelegramBot.Entities
{
    class UserEntity
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string ChatID { get; set; }
        public int CurrentState { get; set; }
        [NotMapped]
        public IState State { get; set; }


    }
}
