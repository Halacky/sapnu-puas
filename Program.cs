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
    class Program
    {
        static TelegramBotClient bot;

        static void Main()
        {
            try
            {
                bot = new TelegramBotClient("874202219:AAGGR--lbKolja2R6hweapuysBfQoqbq8gs"); //Указываем токен который выдал FatherBot
                Commands.bot = bot; 
                Console.WriteLine("Бот запущен!");
                bot.OnMessage += Bot_OnMessageAsync;
                //----------------------
                // Обработка нажатий на inline кнопки
                bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                {
                    var messageCall = ev.CallbackQuery.Message;
                    Commands.docs(ev.CallbackQuery.From.Id, ev.CallbackQuery.Data);
                    await bot.EditMessageReplyMarkupAsync(messageCall.Chat.Id, messageCall.MessageId);
                };
                //-------------------------
                bot.StartReceiving();
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("ПРОИЗОШЛО ЧТО-ТО СТРАШНОЕ");
                Console.ReadKey();
            }

        }

        private static async void Bot_OnMessageAsync(object sender, Telegram.Bot.Args.MessageEventArgs e)//Обртаботка текста из чата
        {
            Console.WriteLine("Message! From: " + e.Message.From.Id + "\nText: " + e.Message.Text); 
            var message = e.Message; //сообщение из чата
            var id = message.From.Id; //id пользователя
            if (Commands.containsId(id)) //Если id уже существует в словаре "запросов" 
            {
                switch (Commands.getStatus(id)) //смотрим найденного id статус в словаре 
                {
                    case "full name":
                        string[] fullName = message.Text.Split();
                        await DataBase.addUserData(id,DataBase.dataTypes.last_name,fullName[0]);
                        Thread.Sleep(1000);
                        await DataBase.addUserData(id, DataBase.dataTypes.first_name, fullName[1]);
                        Thread.Sleep(1000);
                        await DataBase.addUserData(id, DataBase.dataTypes.father_name, fullName[2]);
                        Thread.Sleep(2000);
                        // Это поле в бд
                        // Распарсить вход на три слова
                        goto case "aded";

                    case "date":
                        await DataBase.addUserData(id, DataBase.dataTypes.birthdate, message.Text);
                        Thread.Sleep(2000);
                        // Это поле в бд
                        goto case "aded";

                    case "group":
                        await DataBase.addUserData(id, DataBase.dataTypes.ac_group, message.Text);
                        Thread.Sleep(2000);
                        // Это поле в бд
                        goto case "aded";
                    case "aded":
                        if (Commands.checkAboutStatus(DataBase.userData(id)))
                        {
                            Thread.Sleep(2000);
                            var keyboard = new InlineKeyboardMarkup(
                                                new InlineKeyboardButton[][]
                                                {
                                                    // First row
                                                    new [] {
                                                        // First column
                                                        InlineKeyboardButton.WithCallbackData("по месту требования"),

                                                        // Second column
                                                        InlineKeyboardButton.WithCallbackData("соц защита"),
                                                    },
                                                }
                                            );

                            await bot.SendTextMessageAsync(message.Chat.Id, "Уточните ещё раз, какую нужно справку?", replyMarkup: keyboard);
                            Commands.removeStatus(id);
                        }
                        // Если чекер тру, вызываем addBid и удаляем сценарий из словоря
                        break;
                }
            }
            else //если id нет в словаре значит бот ничего не ждет от него
            {
                bool flag = false;
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text) //если сообщение текст
                {
                    string textMes = message.Text.ToLower(); //распознаем команду попутно приводим вводимый текст к нижнему регистру

                    if (textMes == "/start") 
                    {
                        Commands.regNewUser(id);
                        Commands.hello(id);
                        flag = true;
                    }

                    if (textMes == "привет")
                    {
                        Commands.hello(id);
                        flag = true;
                    }
                    if (textMes == "конец пары")
                    {
                        Commands.endClassTime(id);
                        flag = true;
                    }
                    if (textMes == "карта")
                    {
                        Commands.map(id);
                        flag = true;
                    }
                    if (textMes == "сессия")
                    {
                        Commands.setka(id);
                        flag = true;
                    }
                    if (textMes == "расписание")
                    {
                        Commands.timetable(id);
                        flag = true;
                    }
                    // inline buttons
                    if (textMes == "справка") //Если ввели слово справка создаем инлайн кнопки с вариантами
                    {
                        var keyboard = new InlineKeyboardMarkup(
                                                new InlineKeyboardButton[][]
                                                {
                                                    // First row
                                                    new [] {
                                                        // First column
                                                        InlineKeyboardButton.WithCallbackData("по месту требования"),

                                                        // Second column
                                                        InlineKeyboardButton.WithCallbackData("соц защита"),
                                                    },
                                                }
                                            );

                        await bot.SendTextMessageAsync(message.Chat.Id, "Какую тебе нужно справку?", replyMarkup: keyboard);
                        flag = true;
                    }
                   
                    
                    if (!flag||textMes=="/buttons")
                    {
                        var keyboard = new ReplyKeyboardMarkup
                        {
                            Keyboard = new[] {
                                                 new[] // row 1
                                                {
                                                    new KeyboardButton("Привет"),
                                                    new KeyboardButton("Конец пары")
                                                },
                                                new[]
                                                {
                                                    new KeyboardButton("Карта"),
                                                    new KeyboardButton("Расписание") },
                                                new[]
                                                {
                                                    new KeyboardButton("Сессия"),
                                                    new KeyboardButton("Справка")
                                                },
                                            },
                            ResizeKeyboard = true
                        };

                        await bot.SendTextMessageAsync(message.Chat.Id, "Вот тебе кнопки!", replyMarkup: keyboard);
                    }
                }
            }
        }
    }
}