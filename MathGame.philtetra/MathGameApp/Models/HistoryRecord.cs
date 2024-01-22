using System.Text;

namespace MathGameApp.Models;

public struct HistoryRecord
{
	public int a;
	public int b;
	public int Result;
	public char Operator;
	public MathOperationOption operation;
	public MathGame.Difficulty difficulty;
	private ConsoleColor difficultyColor;
	public HistoryRecord(MathOperation operation, MathGame.Difficulty difficulty)
	{
		this.a = operation.OperandA;
		this.b = operation.OperandB;
		this.Result = operation.Result;
		this.Operator = operation.Operator;
		this.operation = operation.SelectedOption;
		this.difficulty = difficulty;
		this.difficultyColor = MathGame.GetDifficultyColor(difficulty);
	}

	public void Print()
	{
		if (this.operation == MathOperationOption.Random)
		{
			Console.Write("R ");
		}
		Console.ForegroundColor = this.difficultyColor;
		Console.WriteLine($"{ToString()}");
		Console.ForegroundColor = ConsoleColor.Gray;
	}

	public override string ToString() => $"{a} {Operator} {b} = {Result}";
}
