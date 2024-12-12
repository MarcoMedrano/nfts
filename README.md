# Nft Market Sample

## Introduction
In the block chain world a User is represented by its wallet which is an address in the form of 0xc12345c834DaA099d3f1234567D0D094791A3aa8.
These users can hold NFTs (Non Fungible Tokens) which usually are represented by media used for art, gaming, digital assets and others.

___
### Use Case: User signing
As a blockchain user I want to login into the system by using MetaMask so that I can edit my information.

#### Acceptance Criteria.
* User is able to sign up by using only MeaMask, traditional user and passwords must be avoided.
* User besides its Id should hold a nickname and a profile image.


___
### Use Case: User has an nft gallery
As a blockchain user I want to have a NFT gallery so that I can list visually the ones I own.

#### Acceptance Criteria.
* An Nft should hold its id, name and ipfs image.

___
### Run the app
```shell
docker compose up --build
```
And open http://localhost:8080

Swagger at http://localhost:8080/swagger