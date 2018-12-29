using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockChainNode;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain blockchain = new Blockchain("6001");
            Transaction transaction = new Transaction("John Smith", "Sarah Vega", 11);
            bool isAdded = blockchain.CreateTransaction(transaction);
            blockchain.ProcessPendingTransactions("node 1");
            transaction = new Transaction("John Smith", "Sarah Vega", 41);
            isAdded = blockchain.CreateTransaction(transaction);
            blockchain.ProcessPendingTransactions("node 1");
            Console.WriteLine(isAdded);



        }
    }
}
