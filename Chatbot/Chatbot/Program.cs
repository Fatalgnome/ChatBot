using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Chatbot
{
    class Program
    {
        private static readonly Database Database = new Database();
        private static bool _running = true;
        private static string _name;

        private static void Main(string[] args)
        {

            var random = new Random();
            var adminAccess = false;
            var notKnownAnswers = new List<string>();

            Console.WriteLine("Hello Customer, enter your name");

            _name = Console.ReadLine();

            //Convert first letter to uppercase
            _name = _name?.First().ToString().ToUpper() + _name?.Substring(1);

            if (_name == "Admin")
            {
                adminAccess = true;
            }

            Startup(_name, random.Next(1, 3));

            Console.WriteLine("To exit this conversation at any time, type: 'goodbye'");

            //Main user loop
            while (_running)
            {
                var input = Console.ReadLine()?.ToLower();
                switch (input)
                {
                    case "help":
                        Console.WriteLine("Here is a list of what commands i have:\n" +
                                          "'help' \n" +
                                          "'hammer' \n" +
                                          "'screwdriver' \n" +
                                          "'joke' \n" +
                                          "'weather' \n" +
                                          "'goodbye'\n" +
                                          "'admin'");
                        break;

                    case "hammer":
                        Hammer();
                        break;

                    case "screwdriver":
                        Screwdriver();
                        break;

                    case "joke":
                        GetJoke();
                        break;

                    case "weather":
                        Console.WriteLine("Type your current city or country in english");
                        var city = Console.ReadLine();

                        if (city != string.Empty)
                        {
                            GetNewWeather(city);
                        }
                        else
                        {
                            Console.WriteLine("Type 'weather' to try again");
                        }

                        break;

                    case "goodbye":
                        _running = false;
                        Console.WriteLine("Goodbye, have a nice day, hope i was able to help");
                        break;

                    case "admin":
                        if (adminAccess)
                        {
                            Console.Clear();
                            AdminConsole(notKnownAnswers);
                        }
                        else
                        {
                            Console.WriteLine("You do not have access to this command");
                        }
                        break;

                    default:
                        if (input != string.Empty) 
                        {
                            notKnownAnswers.Add(input);
                        }

                        Console.WriteLine("Dont know the answer to that question");
                        break;
                }
            }

            Console.ReadKey();
        }

        private static void Startup(string name, int random)
        {

            switch (random)
            {
                case 1:
                    Console.WriteLine($"Greetings {name}! Type 'help' for my commands");
                    break;
                case 2:
                    Console.WriteLine($"Bonjour {name}! Type 'help' for my commands");
                    break;
                case 3:
                    Console.WriteLine($"Well met {name}! Type 'help' for my commands");
                    break;
            }
        }

        private static void Hammer()
        {
            Console.WriteLine("Here is a list of hammers we have in stock:\n");

            if (Database.HammerList.Count <= 0)
            {
                Console.WriteLine("No hammers in stock");
            }

            foreach (var s in Database.HammerList)
            {
                if (Database.HammerList.Count != 0 )
                {
                    Console.WriteLine($"{s}\n");
                }

            }
        }

        private static void Screwdriver()
        {
            Console.WriteLine("Here is a list of screwdrivers we have in stock:\n");

            if (Database.ScrewdriverList.Count <= 0)
            {
                Console.WriteLine("No screwdrivers in stock at the moment");
            }

            //This will never run, this is just here if someone decides to add some data
            foreach (var s in Database.ScrewdriverList)
            {
                    Console.WriteLine($"{s}\n");
            }
        }

        private static async void GetJoke()
        {
            //Disclaimer: This is not my API
            //Link to Chuck Norris joke API: http://www.icndb.com/api/
            var baseUrl = "http://api.icndb.com/jokes/random";


            //Get data from API
            try
            {
                using (var client = new HttpClient())
                {
                    using (var res = await client.GetAsync(baseUrl))
                    {
                        using (var content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();

                            if (data != null)
                            {
                                var dataObj = JObject.Parse(data);

                                //First need to get the values, then we can get the joke
                                var valueObj = dataObj["value"];
                                var joke = valueObj["joke"];

                                Console.WriteLine(joke);
                            }
                            else
                            {
                                Console.WriteLine("No data found");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void AdminConsole(List<string> answersList)
        {
            Console.WriteLine("Welcome to the admin console\n" +
                              $"You currently have {answersList.Count} notifications\n" +
                              $"press 'enter' to view notifcations");
            Console.ReadKey();

            if (answersList.Count <= 0)
            {
                Console.WriteLine("No new notifications");
            }

            foreach (var s in answersList)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("To return to the customer console press 'enter'");
            Console.ReadKey();
            Console.WriteLine($"Welcome back {_name}! Type 'help' for my commands");
        }

        private static void GetNewWeather(string city)
        {
            var weather = new Weather();

            weather.CurrentWeather(city);
        }
    }
}