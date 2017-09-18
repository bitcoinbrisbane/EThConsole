using System;
namespace ETHConsole
{
    public class ContractSettings : Settings
    {
        public String ABIPath { get; set; }
        
        public String ContractName { get; set; }
        
        public String ContractAddress { get; set; }
        
        public ContractSettings()
        {
        }
        
        public ContractSettings(String[] inputs)
        {
            Url = inputs[0];
            Account = inputs[1];
            ContractAddress = inputs[2];
        }
    }
}
