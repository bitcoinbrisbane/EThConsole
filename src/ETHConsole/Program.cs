﻿using System;
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
                //account = args[1];
                //password = args[2];
            }

            web3 = new Nethereum.Web3.Web3(url);

            if (args.Length > 1)
            {
                String command = args[1];

                switch (command.ToLower())
                {
                    case "account":
                        ListAccounts();
                        break;
                    case "deploy":
                        account = args[3];
                        password = args[4];

                        //"0x210f1e4C56D68F63Ab4bf41157bbca253abfeC45", "Test12345"
                        String contractHash = DeplyContract("/home/lucascullen/Projects/Accenture/quorum/src/dot net core/bin/mvc/contracts/", args[2], account, password);
                        //String contractHash = DeplyContract("/home/lucascullen/Projects/Accenture/quorum/src/dot net core/bin/mvc/contracts/", "Netting", "0x210f1e4C56D68F63Ab4bf41157bbca253abfeC45", "Test12345");
                        Console.WriteLine(contractHash);
                        MonitorTx(contractHash);
                        break;
                }
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
            Boolean unlocked = web3.Personal.UnlockAccount.SendRequestAsync(account, password, 120).Result;

            if (unlocked)
            {
                String bytes = GetBytesFromFile(contractPath + contractName + ".bin");
                Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);
                
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
