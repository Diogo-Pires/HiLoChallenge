var player1 = new PlayerInfo();
var game = new HiLoGame(player1);

game.WelcomeMessage();
game.StartGame();
game.EndGame();

/// <summary>
/// Class responsible for create a new player
/// </summary>
class PlayerInfo
{
    /// <summary>
    /// It holds player's ordinal number in a game
    /// </summary>
    public short NumberInGame { get; set; }

    /// <summary>
    /// Player's name
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The number to be discovered
    /// </summary>
    public int MysteryNumber { get; private set; }

    /// <summary>
    /// The number of attempts of a player in a match
    /// </summary>
    public int Attempts { get; private set; }

    public PlayerInfo()
    {
        Name = "Player";
        Attempts = 0;
    }

    /// <summary>
    /// It generates a new number to be discovered from min to max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void GenerateMysteryNumber(short min, short max) =>
        MysteryNumber = new Random().Next(min, max +1);

    /// <summary>
    /// Adds a new attempt to a player in a match
    /// </summary>
    public void AddAttempt() =>
        Attempts++;
}

/// <summary>
/// Class responsible for create a new game instance
/// </summary>
class HiLoGame
{
    private const string _YES = "Y";

    /// <summary>
    /// It holds min value to be discovered
    /// </summary>
    public short Min { get; private set; }

    /// <summary>
    /// It holds max value to be discovered
    /// </summary>
    public short Max { get; private set; }

    private PlayerInfo _player1;
    private PlayerInfo? _player2;

    public HiLoGame(PlayerInfo player1)
    {
        Min = 1;
        Max = 100;

        AddFirstPlayer(player1);
    }

    /// <summary>
    /// Displays the welcome message and configure the number of players
    /// </summary>
    public void WelcomeMessage()
    {
        Console.WriteLine("Welcome to the Number Guessing Game!");

        Console.Write($"Will another person play({_YES},N)?");
        var willAnotherPersonPlay = Console.ReadLine()?.ToLower().Trim() == _YES.ToLower().Trim();

        if (willAnotherPersonPlay)
        {
            AddSecondPlayer(new PlayerInfo());
        }
    }

    /// <summary>
    /// It ends a match and ask if we should start another one
    /// </summary>
    public void EndGame()
    {
        Console.WriteLine("Thanks for playing!");
        Console.WriteLine("===================================================");
        Console.Write($"Would you like to play it again({_YES},N)?");
        var shouldWeStartItAgain = Console.ReadLine()?.ToLower().Trim() == _YES.ToLower().Trim();

        if (shouldWeStartItAgain)
        {
            AddFirstPlayer(new PlayerInfo());

            WelcomeMessage();
            StartGame();
            EndGame();
        }
    }

    /// <summary>
    /// It starts a new game
    /// </summary>
    public void StartGame()
    {
        var currentPlayer = _player1;

        while (true)
        {
            Console.WriteLine($"Turn: {currentPlayer.Name}{currentPlayer.NumberInGame}");
            Console.Write("Enter your guess: ");

            if (!int.TryParse(Console.ReadLine(), out int guess))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }

            currentPlayer.AddAttempt();

            if (guess < Min || guess > Max)
            {
                Console.WriteLine("Your guess is out of range. Try again.");
            }
            else if (guess < currentPlayer.MysteryNumber)
            {
                Console.WriteLine("HI");
            }
            else if (guess > currentPlayer.MysteryNumber)
            {
                Console.WriteLine("LO");
            }
            else
            {
                Console.WriteLine($"{currentPlayer.Name}{currentPlayer.NumberInGame} guessed the mystery number {currentPlayer.MysteryNumber} in {currentPlayer.Attempts} attempts!");
                break;
            }

            currentPlayer = (currentPlayer.NumberInGame == _player1.NumberInGame && _player2 is not null) ? _player2 : _player1;
        }
    }

    /// <summary>
    /// Adds the first player
    /// </summary>
    /// <param name="player"></param>
    private void AddFirstPlayer(PlayerInfo player)
    {
        _player1 = player;
        _player1.NumberInGame = 1;
        _player1.GenerateMysteryNumber(Min, Max);
    }

    /// <summary>
    /// Adds the second player
    /// </summary>
    /// <param name="player"></param>
    private void AddSecondPlayer(PlayerInfo player)
    {
        if(_player2 is not null)
        {
            Console.WriteLine($"Cannot add more than 2 players! This only allows max 2 players.");
            return;
        }

        _player2 = player;
        _player2.NumberInGame = (short)(_player1.NumberInGame + 1);
        _player2.GenerateMysteryNumber(Min, Max);
    }
}