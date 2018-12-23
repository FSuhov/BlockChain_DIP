using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainNode
{
    public static class Validator
    {
        public static List<Account> Accounts = new List<Account>()
        {
            new Account("John Smith"),
            new Account("Sarah Vega"),
            new Account("Lee Zend")
        };

        public static bool IsValidTransaction(Transaction transaction, Blockchain blockchain)
        {
            if (!IsValidAccount(transaction.FromAddress) 
                || !IsValidAccount(transaction.ToAddress))
            {
                return false;
            }

            int balance = 0;

            for (int i = 0; i < blockchain.Chain.Count; i++)
            {
                for (int j = 0; j < blockchain.Chain[i].Transactions.Count; j++)
                {
                    if (blockchain.Chain[i].Transactions[j].FromAddress == transaction.FromAddress)
                    {
                        balance -= blockchain.Chain[i].Transactions[j].Amount;
                    }

                    if (blockchain.Chain[i].Transactions[j].ToAddress == transaction.FromAddress)
                    {
                        balance += blockchain.Chain[i].Transactions[j].Amount;
                    }
                }
            }

            return balance >= transaction.Amount;
        }

        private static bool IsValidAccount(string name)
        {
            Account account = Accounts.FirstOrDefault(a => a.UserName == name);
            return account != null;
        }
    }
}
