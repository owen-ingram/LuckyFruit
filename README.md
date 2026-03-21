# LuckyFruit
A three-reel slot machine game built in C#

<p align="center">
<img src="https://github.com/user-attachments/assets/20149d0b-a73c-4b88-acff-b141cca7a24d" />
</p align="center">

Match the same symbol across all three independently spun reels to win. Win probability is calculated by multiplying each reel's symbol probability together.

<p align="center">
<img width="476" height="530" alt="LuckyFruitSS1" src="https://github.com/user-attachments/assets/98021f38-f260-4c2b-a825-aa5077a9368f" />
</p align="center">

Ex. P(Cherry, Cherry, Cherry) = (7/20) × (7/20) × (7/20) = 0.042875

Total RTP for this slot is 92.81%

Par sheet can be produced via a .CSV file showing probability and statistics for this slot.

<p align="center">
<img width="900" height="600" alt="SlotParSheetSS" src="https://github.com/user-attachments/assets/4a210c32-6302-4c49-8d04-c5a7ad32f753" />
</p align="center">

## How to run:

Option 1:  
Download 'FruitSlot.exe' from the [releases](https://github.com/owen-ingram/LuckyFruit/releases/tag/v1.0) page, Press play to begin.

Option 2:  
*requires .Net 10 SDK*  
git clone https://github.com/owen-ingram/LuckyFruit  
cd LuckyFruit  
dotnet run

## Project Structure
```
FruitSlot/
├── Data/ReelDefinitions.cs      Strip definitions and RTP design table
├── Engine/MathEngine.cs         Theoretical RTP calculation
├── Engine/SpinEngine.cs         Random spin and result evaluation
├── Models/                      Symbol, PayTable, ReelStrip
├── Reports/ParSheetGenerator.cs CSV par sheet export
└── UI/                          WinForms UI and animation
```

Built With  
C#  .NET 10  WinForms

