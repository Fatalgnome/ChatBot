using System;

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
            while (running)
            {
                Console.WriteLine("To exit this conversation at any time, type: 'goodbye'");
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

                    case "goodbye":
                    case "Goodbye":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Command not accepted");
                        break;
                }

            }

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
            Console.WriteLine("Hammer");
        }

        private static void Screwdriver()
        {
            Console.WriteLine("Screwdriver");
        }

    }
}