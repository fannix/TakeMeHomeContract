# TakeMeHomeContract

## Introduction 

These are the two smart contracts for the project [TakeMeHome](https://fannix.github.io/blockchain/2018/03/04/TakeMeHome.html)

1. The first one is a registry for missing children. This is basically a key-value table. The key is the hash of the beacon ID. The reason we use the hash instead of the plain text of beacon ID is to make sure the people who claims are in the proximity region of the children are not reporting incorrect current location by using fake beacon ID.
The value is the address of the smart contract that encodes the reward rules masked by a modified beacon ID string. This is also to ensure the people who calls the reward contract indeed are in the proximity region.
2. The second one  has several purposes. 
    - It stores the URL of web page which has the photo and description of the missing subject.
    - It stores the email of the contact person.
    - It stores the historical reported location of the children or the pets.
    - It describes how people submitting the location will be rewarded.
    
To see how these smart contracts are used in the Android app, please take a look at <https://github.com/fannix/TakeMeHome>.

## Example
We already deployed the registry contracts at testnet, 
its addresses is 6478509f833cccbbc5a9f70e6d8183065b54b48f.

We also deployed an example of reward contract at 011ce07245481a06042f039407f6b7737e443e47.
To test our app, We created an corresponding key/value entry for this contract in the registry contract:
`70202E016B4FC400203AAE13CC40D7855A2A5EDF -> DD21460AC14E185BDA33BB36B6C37263E391BDA6`.

The key is `RIPEMD160(beaconID)` (we use `id1: 00000000-0000-0000-0000-000000000000 id2: 0 id3: 0` as the example beacon ID).  
And the value is `RIPEMD160(beaconID + "0") XOR SMART_CONTRACT_ADDR` (011ce07245481a06042f039407f6b7737e443e47 is the example reward contract address).

We can query the value using the key by calling the `get` function:

```bash
curl -X POST -i 'http://seed2.neo.org:20332' --data '
{
  "jsonrpc": "2.0",
  "method": "invokefunction",
  "params": ["6478509f833cccbbc5a9f70e6d8183065b54b48f", "get",

             [{"type": "ByteArray", "value": "70202E016B4FC400203AAE13CC40D7855A2A5EDF"}]
             ],
  "id": 3
}
'
```

The result should be:

```bash
{
   "jsonrpc":"2.0",
   "id":3,
   "result":{
      "script":"1470202e016b4fc400203aae13cc40d7855a2a5edf51c103676574678fb4545b0683816d0ef7a9c5bbcc3c839f507864",
      "state":"HALT, BREAK",
      "gas_consumed":"0.189",
      "stack":[
         {
            "type":"ByteArray",
            "value":"dd21460ac14e185bda33bb36b6c37263e391bda6"
         }
      ]
   }
}
```

We can also query information for the reward contract.


```bash
{
  "jsonrpc": "2.0",
  "method": "invokefunction",
  "params": ["011ce07245481a06042f039407f6b7737e443e47", "email",

             []
             ],
  "id": 3
}
```

It will return an email address:

```bash
{
   "jsonrpc":"2.0",
   "id":3,
   "result":{
      "script":"00c105656d61696c67473e447e73b7f60794032f04061a484572e01c01",
      "state":"HALT, BREAK",
      "gas_consumed":"0.111",
      "stack":[
         {
            "type":"ByteArray",
            "value":"646f676540646f67652e636f6d"
         }
      ]
   }
}
```

The email is encoded in a hex code format. We can read the address like this:
0x64 is 'd', 0x6f is 'o'...
After converting the ByteArray to ASCII, the address will be "doge@doge.com".
