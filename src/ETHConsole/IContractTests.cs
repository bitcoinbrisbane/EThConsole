using System;
namespace ETHConsole
{
    public interface IContractTests
    {
        Nethereum.Web3.Web3 Client { get; }
        
        Nethereum.Contracts.Contract Contract { get; }
    }
}
