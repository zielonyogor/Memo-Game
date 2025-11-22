# Memory Game

C# memory game application built with .NET 9.0. UI is created using WPF (Windows Presentation Foundation).

## Database

|  Table | Columns |
|-|-|
|Card|Id, ImagePath|
|UserProfile|Id, UserName|
|GameSession|Id, Date, Duration, GameType, GameMode|
|PlayerGameResult|Id, UserProfileId (FK), GameSessionId (FK), CardsUncovered, IsWinner|