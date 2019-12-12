using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chatbot
{
    class Program
    {
        private static readonly Database database = new Database();
        private static bool running = true;
        private static string name;

        private static void Main(string[] args)
        {

            var random = new Random();

            var notKnownAnswers = new List<string>();
            Console.WriteLine("Hello Customer, enter your name");

            //Convert first letter to uppercase
            name = Console.ReadLine();
            name = name.First().ToString().ToUpper() + name.Substring(1);

            Startup(name, random.Next(1, 3));

            Console.WriteLine("To exit this conversation at any time, type: 'goodbye'");

            //Main user loop
            while (running)
            {
                var input = Console.ReadLine().ToLower();
                switch (input)
                {
                    case "help":
                        Console.WriteLine("Here is a list of what commands i have:\n" +
                                          "'hammer' \n" +
                                          "'screwdriver' \n" +
                                          "'joke' \n" +
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

                    case "goodbye":
                        running = false;
                        Console.WriteLine("Goodbye, have a nice day, hope i was able to help");
                        break;

                    case "admin":
                        Console.Clear();
                        AdminConsole(notKnownAnswers);
                        break;

                    default:
                        if (input != String.Empty) 
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
            Program.name = name;

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
            foreach (var s in database.HammerList)
            {
                if (database.HammerList.Count != 0 )
                {
                    Console.WriteLine($"{s}\n");
                }
                else
                {
                    Console.WriteLine("No hammers in stock");
                }

            }
        }

        private static void Screwdriver()
        {
            if (database.ScrewdriverList.Count <= 0)
            {
                Console.WriteLine("No screwdrivers in stock at the moment");
            }
            foreach (var s in database.ScrewdriverList)
            {
                    Console.WriteLine($"{s}\n");
            }
        }

        private static async void GetJoke()
        {
            //Disclaimer: This is not my API
            //Link to Chuck Norris joke API
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
                              $"press any key to view notifcations");
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
            Console.WriteLine($"Welcome back {name}! Type 'help' for my commands");
        }
    }
}