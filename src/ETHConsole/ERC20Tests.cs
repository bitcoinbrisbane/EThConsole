using System;
using System.Threading.Tasks;

namespace ETHConsole
{
    /// <summary>
    /// Use framework going forward
    /// </summary>
    public class ERC20Tests : IContractTests
    {
        public Nethereum.Web3.Web3 Client { get; private set; }

        public Nethereum.Contracts.Contract Contract { get; private set; }

        public ERC20Tests()
        {
        }

        public async Task<String> GetNameAsync()
        {
            var functionToTest = Contract.GetFunction("name");
            return await functionToTest.CallAsync<String>();
        }
        
        public async Task<UInt64> GetDecimalsAsync()
        {
            var functionToTest = Contract.GetFunction("decimals");
            return await functionToTest.CallAsync<UInt64>();
        }
    }
}
