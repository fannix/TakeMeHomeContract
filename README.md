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
To test our app, We created an corresponding entry for this contract in the registry contract:
70202E016B4FC400203AAE13CC40D7855A2A5EDF -> DD21460AC14E185BDA33BB36B6C37263E391BDA6
The key is `hash(beaconID`) with beaconID as 
id1: 00000000-0000-0000-0000-000000000000 id2: 0 id3: 0.  
And the value is `hash(beaconID + "0") XOR 011ce07245481a06042f039407f6b7737e443e47` (i.e. the reward contract address).
