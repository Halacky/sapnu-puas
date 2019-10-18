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

    static class DataBase
    {
        public enum dataTypes
        {
            id, last_name, first_name, father_name, birthdate, number, ac_group, mail, vk_id
        }
        //-------
        // Пользователи
        // var id_str = message.From.Id;
        public static async Task<string[]> userData(int userId)
        {

            string str = null;
            string phpUrl = "users/get_data.php";
            string[] args = { "id=" + userId };
            str = await phpResult(phpUrl, args);

            string[] res = str.Remove(str.Length - 1, 1).Split('~');
            return res;
        }
        public static async Task<bool> addUserData(int userId, dataTypes data, string value)
        {
            try
            {
                string phpUrl = "users/add_info.php";
                string[] args = { "id=" + userId, "type=" + dataTypeToText(data), "value=" + value };
                await phpResult(phpUrl, args);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<string[]> getDate()
        {
            string[] args = { "heh" };
            string phpUrl = "date.php";
            string str = await phpResult(phpUrl, args);
            string[] res = str.Split();
            return res;
        }
        public static async void addUser(int userId)
        {
            string phpUrl = "users/add_user.php";
            string[] args = { "id=" + userId };
            await phpResult(phpUrl, args);
        }
        public static async void regUser(int userId, string keyWord)
        {
            string phpUrl = "";
        }
        //---------
        // Справки
        public static async void addBid(int userId, string bidType)
        {
            string[] args = { "id=" + userId, "bid_type=" + bidType };
            string phpUrl = "bids/add_bid.php";
            await phpResult(phpUrl, args);
        }
        // Результат типа
        // string[] res = [ "Справка соц защита - готова" , "Справка по месту требования - неготова" ];
        public static async Task<string[]> getBidsStatus(int userId)
        {
            string phpUrl = "";
            return null;
        }
        //----------
        // Мероприятия
        // Возвращает массив названий мероприятий
        public static async Task<string[]> getEventsList()
        {
            string phpUrl = "";
            return null;
        }
        public static async Task<string[]> getEventInfo(string eventName)
        {
            string phpUrl = "";
            return null;
        }
        private static string dataTypeToText(dataTypes dT)
        {
            switch (dT)
            {
                case dataTypes.id:
                    return "id";
                case dataTypes.last_name:
                    return "last_name";
                case dataTypes.first_name:
                    return "first_name";
                case dataTypes.father_name:
                    return "father_name";
                case dataTypes.birthdate:
                    return "birthdate";
                case dataTypes.number:
                    return "number";
                case dataTypes.ac_group:
                    return "ac_group";
                case dataTypes.mail:
                    return "mail";
                case dataTypes.vk_id:
                    return "vk_id";
                default:
                    return null;
            }
        }

        private static async Task<string> phpResult(string phpUrl, string[] args)
        {
            // build request

            string url = "https://istu-bot.000webhostapp.com/" + phpUrl + "?";
            for (int i = 0; i < args.Length - 1; i++)
            {
                url += args[i] + "&";
            }
            url += args[args.Length - 1];
            Console.WriteLine(url);
            //--------------
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            string result = null;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            response.Close();
            return result;
        }
    }
}
