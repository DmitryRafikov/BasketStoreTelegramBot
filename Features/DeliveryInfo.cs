namespace BasketStoreTelegramBot.Features
{
    class DeliveryInfo
    {
        public string Reciever { get; set; }
        public string Address { get; set; }
        public string MailIndex { get; set; }
        public override string ToString()
        {
            return "Имя получателя: " + Reciever + "\n" +
                   "Адрес отправки: " + Address + "\n" +
                   "Почтовый индекс: " + MailIndex + "\n";
        }
    }
}
