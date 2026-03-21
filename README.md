# LuckyFruit
A three-reel slot machine game built in C#
![SlotGif](https://github.com/user-attachments/assets/20149d0b-a73c-4b88-acff-b141cca7a24d)

Independetly spun reels, produce the same symbol in each reel to win. Probability of winning is calculated by multiplying the probability of hitting a certain symbol in each reel.

<img width="476" height="530" alt="LuckyFruitSS1" src="https://github.com/user-attachments/assets/98021f38-f260-4c2b-a825-aa5077a9368f" />

Ex. P(Cherry, Cherry, Cherry) = (7/20) × (7/20) × (7/20) = 0.042875

Total RTP for this slot is 92.81%

Par sheet can be produced via a .CSV file showing probability and statistics for this slot.
<img width="1056" height="799" alt="SlotParSheetSS" src="https://github.com/user-attachments/assets/4a210c32-6302-4c49-8d04-c5a7ad32f753" />

How to run:

Option 1:
Download 'FruitSlot.exe' from the releases page, Press play to begin.

Option 2: *requires .Net 10 SDK*
git clone [https](https://github.com/owen-ingram/LuckyFruit)
cd LuckyFruit
dotnet run

Project Structure

LuckyFruit/
├── Data/ReelDefinitions.cs      Strip definitions and RTP design table
├── Engine/MathEngine.cs         Theoretical RTP calculation
├── Engine/SpinEngine.cs         Random spin and result evaluation
├── Models/                      Symbol, PayTable, ReelStrip
├── Reports/ParSheetGenerator.cs CSV par sheet export
└── UI/                          WinForms UI and animation

Built With
C#  .NET 10  WinForms

