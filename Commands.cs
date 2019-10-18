using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot
{
    class Commands
    {
        public static TelegramBotClient bot;
        static private Dictionary<int, string> statusList = new Dictionary<int, string>(); //словарь запросов

        public static bool containsId(int id) //метод для проверки наличия id в словаре 
        {
            if (statusList.ContainsKey(id)) //Если в словаре еслть этот id, возвращаем true
                return true;
            else                            //Иначе false
                return false;

        }

        public static void hello(Telegram.Bot.Types.ChatId id) //Обработка команды "Привет"
        {
            bot.SendTextMessageAsync(id, "Hello!");
        }

        public static void endClassTime(Telegram.Bot.Types.ChatId id) //Обработка команды "конец пары"
        {

            string[] timeArr = new string[] { "9,45,00", "11,25,00", "13,05,00", "15,15,00", "16,55,00", "18,35,00" }; //Массив с временеем окончания пар

            for (int i = 1; i < timeArr.Length; i++) //Проходим по всему расписанию 
            {
                var timeArrStr1 = timeArr[i - 1].Split(','); //Разбиваю элемент массива на время
                var today = (DateTime.Now).ToShortDateString().Split('.'); //Получаю дату сегодняшнего дня в формате ДД.ММ.ГГ
                DateTime dateTime = new DateTime(int.Parse(today[2]), int.Parse(today[1]), int.Parse(today[0]), int.Parse(timeArrStr1[0]), //Указываю дату конца первой пары
                    int.Parse(timeArrStr1[1]), int.Parse(timeArrStr1[2]));

                var timeArrStr2 = timeArr[i].Split(',');
                DateTime dateTime1 = new DateTime(int.Parse(today[2]), int.Parse(today[1]), int.Parse(today[0]), int.Parse(timeArrStr2[0]), //Указываю дату конца второй пары
                    int.Parse(timeArrStr2[1]), int.Parse(timeArrStr2[2]));


                if (DateTime.Now > dateTime && DateTime.Now < dateTime1) //Еслли текущее время больше предыдущей пары и меньше текущей пары отнимаем от текущей пары текущее время
                {
                    var dif = dateTime1.Subtract(DateTime.Now); //отнимаю время
                    var res = String.Format("{0}:{1}:{2}", dif.Hours, dif.Minutes, dif.Seconds); //форматирую строку
                    bot.SendTextMessageAsync(id, res);//вывожу в чат бот
                    break;
                }
                if (DateTime.Now < dateTime) //Если текущее время меньше предыдуущей пары , значит мы на этой паре
                {
                    var dif = dateTime.Subtract(DateTime.Now); //отнимаю время
                    var res = String.Format("{0}:{1}:{2}", dif.Hours, dif.Minutes, dif.Seconds); //форматирую строку
                    bot.SendTextMessageAsync(id, res); //вывожу в чат бот
                    break;
                }
                if (i == timeArr.Length - 1) //Если доходим до конца массива значит пары закончились
                {
                    bot.SendTextMessageAsync(id, "Пары закончились"); //говорим о том что  пары закончились
                    break;
                }
            }
        }

        public static void timetable(Telegram.Bot.Types.ChatId id) //Обработка команды "Расписание"
        {
            bot.SendTextMessageAsync(id, "Посмотри вот тут: https://www.istu.edu/schedule/"); //Говорим что пар нет 
        }

        public static void map(Telegram.Bot.Types.ChatId id) //Обработка команды "Карта"
        {
            bot.SendPhotoAsync(id, "https://istu-bot.000webhostapp.com/images/map.jpg"); //Берем фотографию с хоста и выводим ее
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
            var ee = DataBase.getDate();
            String[] twoDates = DataBase.getDate().Result;
            Thread.Sleep(2000);
            var fromSetka = twoDates[0].Split('.');
            DateTime dateFromSetka = new DateTime(int.Parse(fromSetka[2]), int.Parse(fromSetka[1]), int.Parse(fromSetka[0]));
            var fromWeek = twoDates[1].Split('.');
            DateTime dateFromWeek = new DateTime(int.Parse(fromWeek[2]), int.Parse(fromWeek[1]), int.Parse(fromWeek[0]));

            var betweenSetka = dateFromSetka.Subtract(DateTime.Now);
            var betweenWeek = dateFromWeek.Subtract(DateTime.Now);

            bot.SendTextMessageAsync(id, $"Дней до начала сессии осталось: {betweenSetka.Days}. До начала зачетной недели: {betweenWeek.Days}");
        }

        public static void docs(int id, string docsType)
        {
            /////////////////////////////////
            if (checkAboutStatus(DataBase.userData(id)))
            {
                Thread.Sleep(2000);
                DataBase.addBid(id, docsType);
                Thread.Sleep(2000);
                bot.SendTextMessageAsync(id, "Заявка на оформление справки, оформлена! ");
            }
        }
        public static void removeStatus(int id)
        {
            statusList.Remove(id);
        }
        public static string getStatus(int id)  //Гетер для элемента словаря по id
        {
            return statusList[id];
        }

        public static void setStatus(int id, String something) //Добавляем новый статус в словарь
        {
            if (statusList.ContainsKey(id))
            {
                statusList.Remove(id);
                statusList.Add(id, something);
            }
            else
            {
                statusList.Add(id, something);
            }

        }

        public static bool checkAboutStatus(Task<string[]> arrStr) //Массив - данные пользователя
        {

            string[] str = arrStr.Result;
            int id = int.Parse(str[0]); //id пользователя
            if (str[1] == "nul" || str[2] == "nul" || str[3] == "nul") //если поле равно null мы просим заполнить его
            {
                bot.SendTextMessageAsync(id, "Скажи своё ФИО: ");
                setStatus(id, "full name");
                return false;
            }
            if (str[4] == "nul")
            {
                bot.SendTextMessageAsync(id, "Скажи свою дату рождения в формате ДД.ММ.ГГГГ: ");
                setStatus(id, "date");
                return false;
            }
            if (str[6] == "nul")
            {
                bot.SendTextMessageAsync(id, "Скажи свою учебную группу: ");
                setStatus(id, "group");
                return false;
            }
            return true;
        }

        public static void regNewUser(int id) //Добавляем пользователя в бд
        {
            DataBase.addUser(id);
            Thread.Sleep(2000);
        }

    }
}