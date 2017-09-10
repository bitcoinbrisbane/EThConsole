using System;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace ETHConsole
{
    class Program : Helper
    {
        private static Nethereum.Web3.Web3 web3;
        
        //private const String DEFAULT_URL = "http://localhost:8545";
        //private const String DEFAULT_URL = "http://quorumnx03.southeastasia.cloudapp.azure.com:8545";
        private const String DEFAULT_URL = "http://192.168.0.103:8545";

        static void Main(string[] args)
        {
            String url = DEFAULT_URL;
            String command = "";

            if (args.Length == 0)
            {
                Console.Write("Geth host?  Or enter for {0}", url);
                url = Console.ReadLine();

                if (String.IsNullOrEmpty(url))
                {
                    url = DEFAULT_URL;
                }

                Console.WriteLine("Node set to {0}", url);

                Console.Write("Command ");
                String[] input = Console.ReadLine().Split(' ');
            }
            else
            {
                url = args[0];
                command = args[1];
            }

            web3 = new Nethereum.Web3.Web3(url);

            switch (command.ToLower())
            {
                case "account":
                    ListAccounts();
                    break;
                case "deploy":
                    String contractHash = DeplyContract("/Users/lucascullen/Projects/Accenture/quorum/src/dot net core/bin/mvc/contracts/", args[2], args[3], args[4]);
                    //String contractHash = DeplyContract("/home/lucascullen/Projects/Accenture/quorum/src/dot net core/bin/mvc/contracts/", "Netting", "0x210f1e4C56D68F63Ab4bf41157bbca253abfeC45", "Test12345");
                    Console.WriteLine(contractHash);
                    MonitorTx(contractHash);
                    break;
                default:
                    throw new ArgumentException();
            }


            //Console.WriteLine("Contract name?");
            //String contractName = Console.ReadLine();

        }

        private static void AccountMenu()
        {
            Console.WriteLine("*** Accounts ***");
            Console.WriteLine("1. List Accounts");
        }

        private static void ListAccounts()
        {
            var accounts = web3.Eth.Accounts.SendRequestAsync().Result;

            for (Int32 i = 0; i < accounts.Length; i++)
            {
                var balance = web3.Eth.GetBalance.SendRequestAsync(accounts[i]).Result;
                Console.WriteLine(accounts[i] + " " + balance.HexValue);
            }
        }

        private static String DeplyContract(String contractPath, String contractName, String account, String password)
        {
            Boolean unlocked = web3.Personal.UnlockAccount.SendRequestAsync(account, password, 60).Result;

            if (unlocked)
            {
                String bytes = GetBytesFromFile(contractPath + contractName + ".bin");
                Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(4000000);
                
                String abi = GetABIFromFile(String.Format("{0}{1}.abi", contractPath, contractName));
                //var x = web3.Eth.DeployContract.EstimateGasAsync<Nethereum.Hex.HexTypes.HexBigInteger>(abi, bytes, account, null).Result;
                return web3.Eth.DeployContract.SendRequestAsync(bytes, account, gas).Result;
            }
            else
            {
                return "unlock failed";
            }
        }

        private static void MonitorTx(String transactionHash)
        {
            var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;

            while (receipt == null)
            {
                Console.WriteLine("Sleeping for 5 seconds");
                System.Threading.Thread.Sleep(5000);
                receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
            }

            Console.WriteLine("Contract address {0} block height {1}", receipt.ContractAddress, receipt.BlockNumber.Value);
        }

        private static async Task ListAccountsAsync()
        {
            var accounts = await web3.Eth.Accounts.SendRequestAsync();

            for (Int32 i = 0; i < accounts.Length; i++)
            {
                var balance = await web3.Eth.GetBalance.SendRequestAsync(accounts[i]);
                Console.WriteLine(accounts[i] + " " + balance.HexValue);
            }
        }
    }
}
