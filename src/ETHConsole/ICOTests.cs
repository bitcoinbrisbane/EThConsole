using System;
namespace ETHConsole
{
    public class ICOTests : IContractTests
    {
        public Nethereum.Web3.Web3 Client { get; private set; }

        public Nethereum.Contracts.Contract Contract { get; private set; }
        
        public ICOTests()
        {
        }
    }
}
