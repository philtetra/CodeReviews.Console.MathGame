using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace MathGameApp.Models;

public class MathGame
{
	public Func<bool> View { get; private set; }
	public Difficulty SelectedDifficulty { get; private set; }

	private readonly List<string> examplesHistory = new(32);

	private readonly MathOperation addition;
	private readonly MathOperation substraction;
	private readonly MathOperation multiplication;
	private readonly MathOperation division;
	private readonly MathOperation randomOperation;

	private string seventhOptionString = string.Empty;

	public MathGame(Difficulty difficulty = Difficulty.Easy)
	{
		this.View = ViewMainMenu;
		this.addition = new MathOperation(MathOperationOption.Addition);
		this.substraction = new MathOperation(MathOperationOption.Subtraction);
		this.multiplication = new MathOperation(MathOperationOption.Multiplication);
		this.division = new MathOperation(MathOperationOption.Division);
		this.randomOperation = new MathOperation(MathOperationOption.Random);

		SetDifficulty(difficulty);
		this.SelectedDifficulty = difficulty;
	}

	public void SetDifficulty(Difficulty difficulty)
	{
		this.addition.RandomUpperLimit = (int)difficulty;
		this.substraction.RandomUpperLimit = (int)difficulty;
		this.multiplication.RandomUpperLimit = (int)difficulty;
		this.division.RandomUpperLimit = (int)difficulty;
		this.randomOperation.RandomUpperLimit = (int)difficulty;
		this.SelectedDifficulty = difficulty;
	}

	public void Play(MathOperation operation)
	{
		string? answer;
		do
		{
			Console.Clear();
			if (operation.Answered)
			{
				operation.GenerateNext();
			}

			string example = $"{operation.OperandA} {operation.Operator} {operation.OperandB} = ";
			Console.Write(example);
			int promptTop = Console.CursorTop, promptLeft = Console.CursorLeft;
			Console.SetCursorPosition(0, promptTop + 4);
			Console.WriteLine("'000' to return to the menu");
			Console.SetCursorPosition(promptLeft, promptTop);

			answer = Console.ReadLine();
			if (answer == "000")
			{
				break;
			}
			else if (!int.TryParse(answer, out int parsedAnswer))
			{
				continue;
			}
			else
			{
				operation.Answer(parsedAnswer);

				if (operation.Answered)
				{
					examplesHistory.Add($"{example}{answer}");
					Console.WriteLine("Correct!");
					Console.WriteLine("\nPress any key to continue\nor press '0' to return to the menu");
					Console.CursorVisible = false;
					ConsoleKeyInfo keyInfo = Console.ReadKey(true);
					Console.CursorVisible = true;
					if (keyInfo.Key == ConsoleKey.D0) break;
				}
			}

		} while (true);
	}

	public bool ViewMainMenu()
	{
		Console.Write("Difficulty: ");
		Console.ForegroundColor = GetDifficultyColor(this.SelectedDifficulty);
		Console.WriteLine($"{this.SelectedDifficulty}\n");
		Console.ForegroundColor = ConsoleColor.Gray;
		for (int i = 1; i <= Enum.GetNames(typeof(MathOperationOption)).Length; i++)
		{
			Console.WriteLine($"{i}. {Enum.GetName(typeof(MathOperationOption), i)}");
		}
		Console.WriteLine("\n6. View history");
		Console.WriteLine(this.seventhOptionString);
		Console.WriteLine("8. Change difficulty");
		Console.WriteLine("\n0. Quit");

		Console.CursorVisible = false;
		ConsoleKeyInfo keyInfo = Console.ReadKey(true);
		Console.CursorVisible = true;

		switch (keyInfo.Key)
		{
			case ConsoleKey.D1:
				Play(addition);
				break;
			case ConsoleKey.D2:
				Play(substraction);
				break;
			case ConsoleKey.D3:
				Play(multiplication);
				break;
			case ConsoleKey.D4:
				Play(division);
				break;
			case ConsoleKey.D5:
				Play(randomOperation);
				break;
			case ConsoleKey.D6:
				ViewHistory();
				break;
			case ConsoleKey.D7:
				FillHistoryWithSampleData(200);
				break;
			case ConsoleKey.D8:
				this.View = ViewDifficultySettings;
				break;
			case ConsoleKey.D0:
				return true;
		}
		return false;
	}

	public bool ViewDifficultySettings()
	{
		string[] mathDifficultyNames = Enum.GetNames(typeof(Difficulty));
		Array? difficulties = Enum.GetValuesAsUnderlyingType(typeof(Difficulty));

		Console.WriteLine("Difficulty options:");
		for (int i = 0; i < mathDifficultyNames.Length; i++)
		{
			var difficulty = (Difficulty)difficulties.GetValue(i)!;
			string paddedDifficultyText = mathDifficultyNames[i].PadRight(14);
			Console.Write($"{i + 1}. ");
			Console.ForegroundColor = GetDifficultyColor(difficulty);
			Console.Write(paddedDifficultyText);

			if (difficulty == this.SelectedDifficulty)
			{
				Console.Write('<');
			}
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine();
		}
		Console.WriteLine("\n0. Back to menu");

		Console.CursorVisible = false;
		ConsoleKeyInfo keyInfo = Console.ReadKey(true);
		Console.CursorVisible = true;

		switch (keyInfo.Key)
		{
			case ConsoleKey.D1:
				SetDifficulty(Difficulty.Easy);
				break;
			case ConsoleKey.D2:
				SetDifficulty(Difficulty.Medium);
				break;
			case ConsoleKey.D3:
				SetDifficulty(Difficulty.Hard);
				break;
			case ConsoleKey.D4:
				SetDifficulty(Difficulty.Calculator);
				break;
			case ConsoleKey.D0:
				this.View = ViewMainMenu;
				break;
		}
		return false;
	}

	public void ViewHistory()
	{
		int counter = 0, indents = 1;
		const int columnWidth = 24;

		Console.CursorVisible = false;
		if (this.examplesHistory.Count != 0)
		{
			Console.SetCursorPosition(columnWidth * indents, 0);
			Console.WriteLine("Press '1' to erase the history");
		}
		Console.SetCursorPosition(Console.WindowWidth / 2, 0);
		Console.WriteLine("History");
		Console.CursorLeft = Console.WindowWidth / 2;
		Console.WriteLine("=======");

		foreach (string record in examplesHistory)
		{
			if (++counter + 2 >= Console.BufferHeight)
			{
				Console.CursorTop = 2;
				counter = 1;
				indents++;
			}
			if (columnWidth * indents >= Console.BufferWidth)
			{
				break;
			}
			Console.CursorLeft = columnWidth * indents;
			Console.WriteLine(record);
		}
		ConsoleKeyInfo keyInfo = Console.ReadKey();
		if (keyInfo.Key == ConsoleKey.D1 && this.examplesHistory.Count != 0)
		{
			this.examplesHistory.Clear();
			this.seventhOptionString = string.Empty;
		}

		Console.CursorVisible = true;
		Console.SetCursorPosition(0, 0);
	}

	public void FillHistoryWithSampleData(int count)
	{
		this.seventhOptionString = "(Sample data generated)";
		string sample = "33 * 100 = 3300";
		for (int i = 0; i < count; i++)
		{
			examplesHistory.Add(sample);
		}
	}

	private static ConsoleColor GetDifficultyColor(Difficulty difficulty) => difficulty switch
	{
		 Difficulty.Easy => ConsoleColor.Green,
		 Difficulty.Medium => ConsoleColor.Yellow,
		 Difficulty.Hard => ConsoleColor.Magenta,
		 Difficulty.Calculator => ConsoleColor.Red,
		 _ => ConsoleColor.Gray
	 };

	public enum Difficulty
	{
		Easy = 51,
		Medium = 101,
		Hard = 200,
		Calculator = 500
	}
}
