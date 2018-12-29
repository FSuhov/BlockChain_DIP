using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainNode
{
    class Program
    {
        public static int Port = 6001;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();
        public static Blockchain CryptoCoin = Blockchain.LoadFromBackUp();
        public static string pairURL = "/Blockchain";

        static void Main(string[] args)
        {
            if (args.Length >= 1) Port = int.Parse(args[0]);

            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }

            Console.WriteLine($"Node is running at ::{Port}");

            if (CryptoCoin == null)
            {
                CryptoCoin = new Blockchain(Port.ToString());
            }

            Console.WriteLine("=========================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Display Blockchain");
            Console.WriteLine("3. Display Time elapsed");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL");
                        string userInput = Console.ReadLine();
                        pairURL = userInput + pairURL;
                        Client.Connect($"{pairURL}");
                        break;
                    case 2:
                        Console.WriteLine(JsonConvert.SerializeObject(CryptoCoin, Formatting.Indented));
                        break;
                    case 3:
                        Console.WriteLine(Validator.TimeElapsed);
                        break;
                }

                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }
            Client.Close();
            CryptoCoin.WriteToXml();
        }
    }
}
