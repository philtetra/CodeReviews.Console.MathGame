﻿using MathGameApp.Models;

var addition = new MathOperation(MathOperationOption.Addition);
var substraction = new MathOperation(MathOperationOption.Subtraction);
var multiplication = new MathOperation(MathOperationOption.Multiplication);
var division = new MathOperation(MathOperationOption.Division);
var randomOperation = new MathOperation(MathOperationOption.Random);

List<string> examplesHistory = new(32);
FillHistoryWithSampleData(200);
ConsoleKeyInfo keyInfo;
do
{
	Console.Clear();
	Console.WriteLine("Math Game\n==============");
	
	for (int i = 1; i <= Enum.GetNames(typeof(MathOperationOption)).Length; i++)
	{
		Console.WriteLine($"{i}. {Enum.GetName(typeof(MathOperationOption), i)}");
	}
	Console.WriteLine("\n6. View history");
	Console.WriteLine("\n0. Quit");
	//Console.WriteLine($"Buffer - width: {Console.BufferWidth}, height: {Console.BufferHeight}");

	Console.CursorVisible = false;
	keyInfo = Console.ReadKey(true);
	Console.CursorVisible = true;

	switch (keyInfo.Key)
	{
		case ConsoleKey.D1:
			DisplayOption(addition);
			break;
		case ConsoleKey.D2:
			DisplayOption(substraction);
			break;
		case ConsoleKey.D3:
			DisplayOption(multiplication);
			break;
		case ConsoleKey.D4:
			DisplayOption(division);
			break;
		case ConsoleKey.D5:
			DisplayOption(randomOperation);
			break;
		case ConsoleKey.D6:
			ViewHistory();
			break;
	}

} while (keyInfo.Key != ConsoleKey.D0);


#region methods
void DisplayOption(MathOperation operation)
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

		answer = Console.ReadLine();
		if (!int.TryParse(answer, out int parsedAnswer))
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
				Console.WriteLine("Press any key to see the next example\nor '0' to return to the menu");
				Console.CursorVisible = false;
				ConsoleKeyInfo keyInfo = Console.ReadKey(true);
				Console.CursorVisible = true;
				if (keyInfo.Key == ConsoleKey.D0) break;
			}
		}

	} while (true);
}

void ViewHistory()
{
	Console.CursorVisible = false;
	Console.SetCursorPosition(Console.WindowWidth / 2, 0);
	Console.WriteLine("History");
	Console.CursorLeft = Console.WindowWidth / 2;
	Console.WriteLine("=======");

	int counter = 0, indents = 1;
	const int columnWidth = 24;
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
	Console.ReadKey();

	Console.CursorVisible = true;
	Console.SetCursorPosition(0, 0);
}

void FillHistoryWithSampleData(int count)
{
	string sample = "33 * 100 = 3300";
	for (int i = 0; i < count; i++)
	{
		examplesHistory.Add(sample);
	}
}

//static bool IsMenuOption(ConsoleKeyInfo keyInfo)
//{
//	return keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.D2
//		|| keyInfo.Key == ConsoleKey.D3 || keyInfo.Key == ConsoleKey.D4
//		|| keyInfo.Key == ConsoleKey.D5;
//}
#endregion



