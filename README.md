# Memory Game
Yet another memory game made in C# and wpf

## Folder Structure

The project is organized as follows:

```text
src/                            # Contains all source code for the project.
    MemoryGame.UI/              # Holds all user interface files, including XAML layouts and their corresponding C# code-behind files.
        MainWindow.xaml         # Defines the main window's visual layout.
        MainWindow.xaml.cs      # Contains the logic and event handling for the main window.
        App.xaml                # Application entry point and resource definitions.
    MemoryGame.Domain/          # Contains domain-specific logic and models.
        GameLogic.cs            # Core game logic
        Card.cs                 # Card model
    MemoryGame.Application/     # Contains application services and business logic.
        GameManager.cs          # Manages game state and interactions
    MemoryGame.Infrastructure/  # Contains infrastructure-related code, such as data storage and external services.
        DataStorage.cs          # Handles data storage
```
Some of the file is not yet implemented, but the structure is set up for future development.
Additional folders and files may be added as the project grows.