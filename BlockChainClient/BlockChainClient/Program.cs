using System;
using Newtonsoft.Json;

namespace BlockChainClient
{
    class Program
    {
        public static P2PClient Client = new P2PClient();
        public static string name = "John Smith"; // default
        public static string serverURL = "/Blockchain"; 

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                name = args[0];
            }

            Console.WriteLine($"Current user is {name}");
            

            Console.WriteLine("=========================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 3)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL");
                        string userInput = Console.ReadLine();
                        serverURL = userInput + serverURL;
                        Client.Connect($"{serverURL}");
                        break;
                    case 2:
                        Console.WriteLine("Please enter the receiver name");
                        string receiverName = Console.ReadLine();
                        Console.WriteLine("Please enter the amount");
                        string amount = Console.ReadLine();
                        Transaction transaction = new Transaction(name, receiverName, int.Parse(amount));
                        Client.Send($"{serverURL}", "T"+JsonConvert.SerializeObject(transaction, Formatting.Indented));
                        break;
                }

                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }

            Client.Close();
        }
    }
}
