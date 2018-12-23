using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainNode
{
    public class Account
    {
        public string UserName { get; set; }

        public Account(string name)
        {
            UserName = name;
        }
    }
}
