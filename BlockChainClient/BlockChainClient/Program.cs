using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainClient
{
    class Program
    {
        public static int Port = 0;
        
        public static P2PClient Client = new P2PClient();
        
        public static string name = "John Smith";
        public static string serverURL = "";
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                name = args[0];
            }

            if (name != "Unkown")
            {
                Console.WriteLine($"Current user is {name}");
            }

            Console.WriteLine("=========================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Display Blockchain");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL");
                        serverURL = Console.ReadLine();
                        Client.Connect($"{serverURL}/Blockchain");
                        break;
                    case 2:
                        Console.WriteLine("Please enter the receiver name");
                        string receiverName = Console.ReadLine();
                        Console.WriteLine("Please enter the amount");
                        string amount = Console.ReadLine();
                        Transaction transaction = new Transaction(name, receiverName, int.Parse(amount));
                        Client.Send($"{serverURL}/Blockchain", "T"+JsonConvert.SerializeObject(transaction, Formatting.Indented));
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
