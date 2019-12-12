using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chatbot
{
    class Program
    {

        private static void Main(string[] args)
        {
            var running = true;
            Random random = new Random();
            Console.WriteLine("Hello Customer, enter your name");

            Startup(Console.ReadLine(), random.Next(1, 3));

            Console.WriteLine("To exit this conversation at any time, type: 'goodbye'");

            while (running)
            {
                switch (Console.ReadLine())
                {
                    case "help":
                        Console.WriteLine("Here is a list of what commands i have:\n" +
                                          "'hammer' \n" +
                                          "'screwdriver' \n" +
                                          "'something' \n" +
                                          "'joke' \n" +
                                          "'goodbye'");
                        break;

                    case "hammer":
                    case "Hammer":
                        Hammer();
                        break;

                    case "screwdriver":
                    case "Screwdriver":
                        Screwdriver();
                        break;

                    case "joke":
                    case "Joke":
                        getJoke();
                        break;

                    case "goodbye":
                    case "Goodbye":
                        running = false;
                        Console.WriteLine("Goodbye, have a nice day, hope we were able to help");
                        break;

                    default:
                        Console.WriteLine("Command not accepted");
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
            List<string> hammerList = new List<string>();
            hammerList.Add("https://www.harald-nyborg.dk/p2386/bahco-kloefthammer");
            hammerList.Add("https://www.bauhaus.dk/vaerktoj-isenkram/handvaerktoj/hammere/gummihammer-64-mm");
            hammerList.Add("https://www.stark.dk/raptor-750-g-laegtehammer?id=2640-9671199");

            Console.WriteLine("Here is a list of hammers we have in stock:\n");
            foreach (string s in hammerList)
            {
                Console.WriteLine($"{s}\n");
            }
        }

        private static void Screwdriver()
        {
            Console.WriteLine("No screwdrivers in stock");
        }

        private static async void getJoke()
        {
            //Disclaimer: This is not my API
            string baseUrl = "http://api.icndb.com/jokes/random";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            string data = await content.ReadAsStringAsync();

                            if (data != null)
                            {
                                var dataObj = JObject.Parse(data);

                                //First need to get the values, then we can get the joke
                                var value = dataObj["value"];
                                var joke = value["joke"];

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

    }
}