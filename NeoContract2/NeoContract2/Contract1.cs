using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Neo.SmartContract
{
    public class Found : Framework.SmartContract
    {
        //Token Settings
        public static string Url() => "https://fannix.github.io/doge.html";
        public static string Email() => "doge@doge.com";
        public static readonly byte[] Owner = "ASfFNunqtv73UDidxz5Z5fi5fYHAZb3NrQ".ToScriptHash();


        [DisplayName("transfer")]
        public static event Action<byte[], byte[], BigInteger> Transferred;

        public static Object Main(string operation, params object[] args)
        {
            if (Runtime.Trigger == TriggerType.Verification)
            {
                return Runtime.CheckWitness(Owner);
            }
            else if (Runtime.Trigger == TriggerType.Application)
            {
                if (operation == "deploy") return Deploy();
                if (operation == "url") return Url();
                if (operation == "email") return Email();
                if (operation == "submit")
                {
                    if (args.Length != 2) return false;
                    byte[] to = (byte[])args[0];
                    byte[] info = (byte[])args[1];
                    BigInteger count = Storage.Get(Storage.CurrentContext, "count").AsBigInteger();
                    Storage.Put(Storage.CurrentContext, count.AsByteArray(), info);
                    count = count + 1;
                    Storage.Put(Storage.CurrentContext, "count", count);
                    return Transfer("reserve".AsByteArray(), to, 1);
                }
                if (operation == "balanceOf")
                {
                    if (args.Length != 1) return 0;
                    byte[] account = (byte[])args[0];
                    return BalanceOf(account);
                }
            }

            return false;
        }

        // initialization parameters, only once
        public static bool Deploy()
        {
            byte[] count = Storage.Get(Storage.CurrentContext, "count");
            //if (count.Length != 0) return false;
            Storage.Put(Storage.CurrentContext, "count", 0);
            Storage.Put(Storage.CurrentContext, "reserve", 1000);
            return true;
        }

        // function that is always called when someone wants to transfer tokens.
        public static bool Transfer(byte[] from, byte[] to, BigInteger value)
        {
            if (value <= 0) return false;
            BigInteger from_value = Storage.Get(Storage.CurrentContext, from).AsBigInteger();
            if (from_value < value) return false;
            if (from_value == value)
                Storage.Delete(Storage.CurrentContext, from);
            else
                Storage.Put(Storage.CurrentContext, from, from_value - value);
            BigInteger to_value = Storage.Get(Storage.CurrentContext, to).AsBigInteger();
            Storage.Put(Storage.CurrentContext, to, to_value + value);
            Transferred(from, to, value);
            return true;
        }

        // get the account balance of another account with address
        public static BigInteger BalanceOf(byte[] address)
        {
            return Storage.Get(Storage.CurrentContext, address).AsBigInteger();
        }

    }
}