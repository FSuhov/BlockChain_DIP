using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainNode
{
    public static class Validator
    {
        public static long TimeElapsed = 0;

        public static bool IsValidTransaction(Transaction transaction, Blockchain blockchain)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            if (transaction.FromAddress == transaction.ToAddress)
            {
                timer.Stop();
                TimeElapsed += timer.ElapsedTicks;
                return false;
            }

            if (!IsValidAccount(transaction.FromAddress) 
                || !IsValidAccount(transaction.ToAddress))
            {
                timer.Stop();
                TimeElapsed += timer.ElapsedTicks;
                return false;
            }

            int balance = 0;

            int i = blockchain.Chain.Count - 1;
            int j;
            do
            {
                for (j = 0; j < blockchain.Chain[i].Transactions.Count; j++)
                {
                    if (blockchain.Chain[i].Transactions[j].ToAddress == transaction.FromAddress)
                    {
                        balance += blockchain.Chain[i].Transactions[j].Amount;
                    }

                    if (blockchain.Chain[i].Transactions[j].FromAddress == transaction.FromAddress)
                    {
                        balance += blockchain.Chain[i].Transactions[j].Change;
                        i = 1;
                        break;
                    }
                }

                i--;

            } while (i > 0);

            transaction.Change = balance - transaction.Amount;
            timer.Stop();
            TimeElapsed += timer.ElapsedTicks;
            

            return balance >= transaction.Amount;
        }

        private static bool IsValidAccount(string name)
        {
            Account account = Accounts.FirstOrDefault(a => a.UserName == name);
            return account != null;
        }


        public static List<Account> Accounts = new List<Account>() // 20 accounts
        {
            new Account("John Smith"),
            new Account("Sarah Vega"),
            new Account("Lee Zend"),
            new Account("Alex Cheng"),
            new Account("Maria Dupont"),
            new Account("Richard Gladston"),
            new Account("Michel Dobbs"),
            new Account("Veronica Helm"),
            new Account("Theodor Kalpaduculus"),
            new Account("Fanny May"),
            new Account("July Strobson"),
            new Account("Samuel Bishop"),
            new Account("Ann Johanson"),
            new Account("Tina De Luka"),
            new Account("Susanne Miura"),
            new Account("Gleb Popov"),
            new Account("Peter Savchenko"),
            new Account("Melissa Hury"),
            new Account("Gerard Ritter"),
            new Account("Scarlet Manson")
        };
    }
}
