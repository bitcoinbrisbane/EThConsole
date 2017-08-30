using System;
using System.Threading.Tasks;

namespace ETHConsole
{
    class Program : Helper
    {
        private static Nethereum.Web3.Web3 web3;
        private const String DEFAULT_URL = "http://localhost:8545";

        static void Main(string[] args)
        {
            String url = DEFAULT_URL;
            String account;
            String password;

            if (args.Length == 0)
            {
                Console.WriteLine("Geth host?  Or enter for {0}", url);
                url = Console.ReadLine();

                if (String.IsNullOrEmpty(url))
                {
                    url = DEFAULT_URL;
                }
            }
            else
            {
                url = args[0];
                account = args[1];
                password = args[2];
            }

            web3 = new Nethereum.Web3.Web3(url);

            Console.WriteLine("Contract name?");
            String contractName = Console.ReadLine();

            String contractHash = DeplyContract("/home/lucascullen/Projects/Accenture/quorum/src/bin/dot net core/mvc/contracts/", contractName, "0x210f1e4C56D68F63Ab4bf41157bbca253abfeC45", "Test12345");
            //String contractHash = DeplyContract("/home/lucascullen/Projects/Accenture/quorum/src/dot net core/bin/mvc/contracts/", "Netting", "0x210f1e4C56D68F63Ab4bf41157bbca253abfeC45", "Test12345");
            Console.WriteLine(contractHash);
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
            Boolean unlocked = web3.Personal.UnlockAccount.SendRequestAsync(account, password, 120).Result;

            if (unlocked)
            {
                String bytes = GetBytesFromFile(contractPath + contractName + ".bin");
                Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);
                return web3.Eth.DeployContract.SendRequestAsync(bytes, account, gas).Result;
            }
            else
            {
                return "unlock failed";
            }
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
