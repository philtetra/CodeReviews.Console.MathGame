namespace MathGameApp.Models;
public class MathGame
{
	public Func<bool> View { get; private set; }
	public Difficulty SelectedDifficulty { get; private set; }
	public MathOperationCollection Operations { get; set; }

	//private readonly List<string> examplesHistory = new(32);
	private readonly List<HistoryRecord> examplesHistory = new(32);

	private string seventhOptionString = string.Empty;
	private MathOperation currentOperation;

	public MathGame(Difficulty difficulty = Difficulty.Easy)
	{
		this.View = ViewMainMenu;
		this.Operations = new();
		this.currentOperation = this.Operations.Addition;
		SetDifficulty(difficulty);
		this.SelectedDifficulty = difficulty;
	}

	public void SetDifficulty(Difficulty difficulty)
	{
		this.Operations.SetRandomUpperLimits((int)difficulty);
		this.SelectedDifficulty = difficulty;
	}

	public void PrintDifficultyInfo()
	{
		Console.Write("Difficulty: ");
		Console.ForegroundColor = GetDifficultyColor(this.SelectedDifficulty);
		Console.WriteLine($"{this.SelectedDifficulty}\n");
		Console.ForegroundColor = ConsoleColor.Gray;
	}

	public bool Play()
	{
		PrintDifficultyInfo();
		MathOperation operation = this.currentOperation;

		if (operation.Answered)
		{
			operation.GenerateNext();
		}

		string example = $"{operation.OperandA} {operation.Operator} {operation.OperandB} = ";
		Console.Write(example);
		int cursorTop = Console.CursorTop, cursorLeft = Console.CursorLeft;
		Console.SetCursorPosition(0, cursorTop + 3);
		Console.WriteLine("'000' to return to the menu");
		Console.SetCursorPosition(cursorLeft, cursorTop);

		string? answer = Console.ReadLine();
		if (answer == "000")
		{
			this.View = ViewMainMenu;
		}
		else if (!int.TryParse(answer, out int parsedAnswer))
		{
			return false;
		}
		else
		{
			operation.Answer(parsedAnswer);

			if (operation.Answered)
			{
				this.examplesHistory.Add(new HistoryRecord(operation, this.SelectedDifficulty));
				//Console.WriteLine("Correct!");
				Console.SetCursorPosition(cursorLeft + answer.Length, cursorTop);
				Console.Write('\t');
				PrintCorrect();
				Console.WriteLine("\n\nPress any key to continue\nor press '0' to return to the menu");
				Console.CursorVisible = false;
				ConsoleKeyInfo keyInfo = Console.ReadKey(true);
				Console.CursorVisible = true;
				if (keyInfo.Key == ConsoleKey.D0) this.View = ViewMainMenu;
			}
		}
		return false;
	}

	private void PrintCorrect()
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write('C');
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write('O');
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write('R');
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write('E');
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.Write('C');
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write('C');
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.Write('T');
		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write('!');
		Console.ForegroundColor = ConsoleColor.Gray;
	}

	public bool ViewMainMenu()
	{
		PrintDifficultyInfo();
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
				this.currentOperation = this.Operations.Addition;
				this.View = Play;
				break;
			case ConsoleKey.D2:
				this.currentOperation = this.Operations.Substraction;
				this.View = Play;
				break;
			case ConsoleKey.D3:
				this.currentOperation = this.Operations.Multiplication;
				this.View = Play;
				break;
			case ConsoleKey.D4:
				this.currentOperation = this.Operations.Division;
				this.View = Play;
				break;
			case ConsoleKey.D5:
				this.currentOperation = this.Operations.Random;
				this.View = Play;
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
			Console.SetCursorPosition(columnWidth * 3, 0);
			Console.WriteLine("Press '1' to erase the history");
		}
		string tooltip = "( 'R' - Random mode )";
		Console.SetCursorPosition(columnWidth * indents, 0);
		Console.Write(tooltip);
		Console.SetCursorPosition(Console.WindowWidth / 2, 0);
		Console.WriteLine("History");
		Console.CursorLeft = Console.WindowWidth / 2;
		Console.WriteLine("=======");

		foreach (HistoryRecord record in examplesHistory)
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
			//Console.WriteLine(record);
			record.Print();
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
		var mathOp = new MathOperation(MathOperationOption.Random);
		var difficultiesEnumArr = Enum.GetValuesAsUnderlyingType(typeof(Difficulty));
		var mathOpOptionsEnumArr = Enum.GetValuesAsUnderlyingType(typeof(MathOperationOption));
		for (int i = 0; i < count; i++)
		{
			int index = MathOperation.NumberGen.Next(0, difficultiesEnumArr.Length);
			Difficulty difficulty = (Difficulty)difficultiesEnumArr.GetValue(index)!;
			index = MathOperation.NumberGen.Next(0, mathOpOptionsEnumArr.Length);
			MathOperationOption mathOpOption = (MathOperationOption)mathOpOptionsEnumArr.GetValue(index)!;
			mathOp.SelectedOption = mathOpOption;
			mathOp.GenerateNext();
			examplesHistory.Add(new HistoryRecord(mathOp, difficulty));
		}
		this.seventhOptionString = "(Sample data generated)";
	}

	public static ConsoleColor GetDifficultyColor(Difficulty difficulty) => difficulty switch
	{
		Difficulty.Easy => ConsoleColor.Green,
		Difficulty.Medium => ConsoleColor.Yellow,
		Difficulty.Hard => ConsoleColor.Magenta,
		Difficulty.Calculator => ConsoleColor.Red,
		_ => ConsoleColor.Gray
	};

	public enum Difficulty
	{
		Easy = 33,
		Medium = 97,
		Hard = 193,
		Calculator = 667
	}
}