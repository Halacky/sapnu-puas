using System;
using Telegram.Bot;
namespace TestBot1
{
    class Program
    {
        static TelegramBotClient bot;
        static void Main()
        {
            try
            {
                bot = new TelegramBotClient("914360727:AAHQOJKar_N__IViBNuNzJCru_uFUvGTac0");
                Commands.bot = bot;
                Console.WriteLine("Бот запущен!");
                bot.OnMessage += Bot_OnMessage;
                bot.StartReceiving();
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("ПРОИЗОШЛО ЧТО-ТО СТРАШНОЕ");
                Console.ReadKey();
            }

        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine("Message! From: " + e.Message.From.FirstName + "\nText: " + e.Message.Text);
            var message = e.Message;
            var id = message.From.Id;

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                string textMes = message.Text.ToLower();

                if (textMes == "привет")
                {
                    Commands.hello(id);
                }

                if (textMes == "карта")
                {
                    Commands.map(id);
                }
                if (textMes == "сессия")
                {
                    Commands.setka(id);
                }
                if (textMes == "расписание")
                {
                    Commands.timetable(id);
                }
            }
        }
    }

    static class Commands
    {
        public static TelegramBotClient bot;

        public static void hello(Telegram.Bot.Types.ChatId id)
        {
            bot.SendTextMessageAsync(id, "Hello!");
        }

        public static void timetable(Telegram.Bot.Types.ChatId id)
        {
            bot.SendTextMessageAsync(id, "Сегодня у тебя нет пар!");
        }

        public static void map(Telegram.Bot.Types.ChatId id)
        {
            bot.SendPhotoAsync(id, "https://sun9-38.userapi.com/c853528/v853528470/1294ac/feNQQjMlNkc.jpg");
        }
        public static void setka(Telegram.Bot.Types.ChatId id)
        {
            /*
             !!!!!!ФОРМАТ В БД ТИПА 01.01.2000!!!!!!
             * twoDates - массив из БД с двумя датами
             * сплитую по точке эти две даты
             * отнимаю от дат текущую дату 
             * получаю дни
             * вывожу дни
             */
            String[] twoDates = new String[] {"12.12.2019","30.11.2019"};
            var fromSetka = twoDates[0].Split('.');
            DateTime dateFromSetka = new DateTime(int.Parse(fromSetka[2]), int.Parse(fromSetka[1]), int.Parse(fromSetka[0]));
            var fromWeek = twoDates[1].Split('.');
            DateTime dateFromWeek = new DateTime(int.Parse(fromWeek[2]), int.Parse(fromWeek[1]), int.Parse(fromWeek[0]));

            var betweenSetka = dateFromSetka.Subtract(DateTime.Now);
            var betweenWeek = dateFromWeek.Subtract(DateTime.Now);

            bot.SendTextMessageAsync(id, $"Дней до начала сессии осталось: {betweenSetka.Days}. До начала зачетной недели: {betweenWeek.Days}");
        }
    }

    static class DataBase
    {
        // var id_str = message.From.Id;
        public static string[] userData(int id)
        {
            string str = "" + id + "~Иванов~Иван~Иванович~1998-09-03~null~АСУб-16-1~null~null";
            return str.Split('~');
        }
        public static void addBid(int id, string bidType)
        {

        }
    }
}