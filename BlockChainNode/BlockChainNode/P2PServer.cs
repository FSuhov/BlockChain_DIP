using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockChainNode
{
    public class P2PServer : WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer wss = null;
        public P2PClient Client = null;

        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }

            else if (e.Data.StartsWith("T"))
            {
                string data = e.Data.Substring(1);
                Transaction transaction = JsonConvert.DeserializeObject<Transaction>(data);
                bool isAdded = Program.CryptoCoin.CreateTransaction(transaction);

                if (!isAdded)
                {
                    Send("Invalid transaction");
                }
                else
                {
                    Send("OK transaction");
                    Program.CryptoCoin.ProcessPendingTransactions($"ws://127.0.0.1:{Program.Port}");
                    //Send(JsonConvert.SerializeObject(Program.CryptoCoin));
                    //if (Client != null)
                    //{
                    //    Client.Broadcast(JsonConvert.SerializeObject(Program.CryptoCoin));
                    //    Client.Send(JsonConvert.SerializeObject(Program.CryptoCoin));
                    //}
                }
            }

            else 
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (newChain.IsValid() && newChain.Chain.Count > Program.CryptoCoin.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.CryptoCoin.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.CryptoCoin = newChain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.CryptoCoin));
                    chainSynched = true;
                }
            }
        }
    }
}
