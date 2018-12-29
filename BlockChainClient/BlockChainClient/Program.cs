using System;
using System.Collections.Generic;
using System.Threading;
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
            Console.WriteLine("3. Start Test");
            Console.WriteLine("3. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 4)
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
                    case 3:
                        GenerateTransactions(100);
                        break;
                }

                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }

            Client.Close();
        }

        public static void GenerateTransactions(int times)
        {
            Random rnd = new Random();
            for (int i = 0; i < times; i++)
            {
                string sender = Names[rnd.Next(24)];
                string receiver = Names[rnd.Next(24)];
                int amount = rnd.Next(1, 10);
                Transaction transaction = new Transaction(sender, receiver, amount);
                Client.Send($"{serverURL}", "T" + JsonConvert.SerializeObject(transaction, Formatting.Indented));
                Thread.Sleep(100);
            }
        }

        public static string[] Names =
        {
            "John Smith",
            "Sarah Vega",
            "Lee Zend",
            "Alex Cheng",
            "Maria Dupont",
            "Richard Gladston",
            "Michel Dobbs",
            "Veronica Helm",
            "Theodor Kalpaduculus",
            "Fanny May",
            "July Strobson",
            "Samuel Bishop",
            "Ann Johanson",
            "Tina De Luka",
            "Susanne Miura",
            "Gleb Popov",
            "Peter Savchenko",
            "Melissa Hury",
            "Gerard Ritter",
            "Scarlet Manson",
            "Jody Mayerson",
            "John Abbot",
            "Jim Parsons",
            "Rob Walters",
            "Gina Kaffy"
        }; // 25 names, so sometimes it will generate transactions from/to non-existing accounts
    }
}
