using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlockChainNode
{
    [Serializable]
    public class Blockchain
    {
        public List<Transaction> PendingTransactions = new List<Transaction>();
        public List<Block> Chain { set; get; }
        public int Difficulty { set; get; } = 2;
        public int Reward = 1; //1 cryptocurrency
        private string _port;
        //public long TimeElapsed = 0;

        public Blockchain()
        {
        }

        public Blockchain(string port)
        {
            _port = port;
            InitializeChain();
            if (port == "6001")
            {
                AddTestData();
            }
        }


        public void InitializeChain()
        {
            Chain = new List<Block>();
            AddGenesisBlock();
        }

        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(Difficulty);
            PendingTransactions = new List<Transaction>();
            return block;
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public bool CreateTransaction(Transaction transaction)
        {
            if (Validator.IsValidTransaction(transaction, this) || transaction.FromAddress == "CryptoChain")
            {
                PendingTransactions.Add(transaction);
                return true;
            }
            return false;
        }

        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction("CryptoChain", minerAddress, Reward));
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(this.Difficulty);
            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetBalance(string address)
        {
            int balance = 0;

            for (int i = 0; i < Chain.Count; i++)
            {
                for (int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];

                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }
            return balance;
        }

        public void WriteToXml()
        {
            XmlSerializer s = new XmlSerializer(typeof(Blockchain));
            DateTime dt = DateTime.UtcNow.Date;
            using (FileStream fs = new FileStream($"cryptocoin-{dt.ToString("dd-MM-yyyy")}-{_port}.xml", FileMode.OpenOrCreate))
            {
                s.Serialize(fs, this);
            }
        }

        public static Blockchain LoadFromBackUp()
        {
            Blockchain blockchain = null;
            XmlSerializer s = new XmlSerializer(typeof(Blockchain));
            try
            {
                using (FileStream fs = new FileStream("back-up.xml", FileMode.Open))
                {
                    blockchain = (Blockchain)s.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            return blockchain;
        }

        private void AddTestData()
        {
            foreach (Account item in Validator.Accounts)
            {
                CreateTransaction(new Transaction("CryptoChain", item.UserName, 100));
            }
            ProcessPendingTransactions("miner");
        }
    }
}
