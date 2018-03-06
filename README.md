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
