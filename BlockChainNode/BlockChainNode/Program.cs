using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainNode
{
    class Program
    {
        public static int Port = 6001;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();
        public static Blockchain CryptoCoin = null;
        public static string name = "Unknown";
        public static string pairURL = "";

        static void Main(string[] args)
        {
            if (args.Length >= 1)
                Port = int.Parse(args[0]);

            name = "Server: " + Port;

            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }
            if (name != "Unkown")
            {
                Console.WriteLine($"Miner is ready at {name}");
            }

            CryptoCoin = new Blockchain(Port.ToString());


            Console.WriteLine("=========================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Display Blockchain");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL");
                        pairURL = Console.ReadLine();
                        Client.Connect($"{pairURL}/Blockchain");
                        break;
                    //case 2:
                    //    Console.WriteLine("Please enter the receiver name");
                    //    string receiverName = Console.ReadLine();
                    //    Console.WriteLine("Please enter the amount");
                    //    string amount = Console.ReadLine();
                    //    CryptoCoin.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount)));
                    //    CryptoCoin.ProcessPendingTransactions(name);
                    //    Client.Broadcast(JsonConvert.SerializeObject(CryptoCoin));
                    //    break;
                    case 2:
                        Client.Close();
                        Client = new P2PClient();
                        if (pairURL != "")
                        {
                            Client.Connect($"{pairURL}/Blockchain");
                        }
                        Console.WriteLine("Blockchain");
                        Console.WriteLine(JsonConvert.SerializeObject(CryptoCoin, Formatting.Indented));
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
