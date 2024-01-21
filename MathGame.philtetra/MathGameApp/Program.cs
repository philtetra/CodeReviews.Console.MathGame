using MathGameApp.Models;

var mathGame = new MathGame();

bool quitGame = false;
const string author = "by philtetra";
do
{
	Console.Clear();
	PrintHeader();
	quitGame = mathGame.View();

} while (!quitGame);

static void PrintHeader()
{
	Console.ForegroundColor = ConsoleColor.White;
	Console.WriteLine("THE MATH GAME");
	Console.ForegroundColor = ConsoleColor.Blue;
	Console.WriteLine(author.PadLeft(author.Length + 4));
	Console.ForegroundColor = ConsoleColor.Blue;
	Console.WriteLine("================");
	Console.ForegroundColor = ConsoleColor.Gray;
}

