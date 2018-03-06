using Neo.SmartContract.Framework;
using Neo.VM;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Numerics;

/**
 * This is the missing subject registry for the TakeMeHome application
 *
 */

namespace NeoContract1
{
    public class RegistryContract : SmartContract
    {

        public static readonly byte[] owner = "ASfFNunqtv73UDidxz5Z5fi5fYHAZb3NrQ".ToScriptHash();

        public static object Main(string operation, params object[] args)
        {
            switch (operation)
            {
                case "create":
                    return Create((byte[])args[0], (byte[]) args[1]);

                case "get":
                    return Get((byte[])args[0]);

                case "delete":
                    return Delete((byte[])args[0]);

                default:
                    return false;
            }
        }

        // Create an entry using Beacon ID as key, and reward contract address as value
        private static object Create(byte[] key, byte[] value)
        {
            
            Storage.Put(Storage.CurrentContext, key, value);
            return true;
        }

        // Get the smart contract address by using Beacon ID
        private static object Get(byte[] key)
        {
            return Storage.Get(Storage.CurrentContext, key);
        }

        // Delete the entry by key
        private static object Delete(byte[] key)
        {
            // Only the owner of the registry can delete the registry entry
            // Ideally we should also allow the creator to delete it, 
            // We will implement this in the next stage.
            if (!Runtime.CheckWitness(owner))
            {
                Runtime.Log("failed to verify");
                return false;
            }
            Storage.Delete(Storage.CurrentContext, key);
            return true;
        }

    }
}
