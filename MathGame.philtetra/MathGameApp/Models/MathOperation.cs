namespace MathGameApp.Models;
public class MathOperation
{
	private Func<int, int, int>? operation;
	public bool Answered { get; private set; }
	public char Operator { get; private set; }
	public int OperandA { get; private set; }
	public int OperandB { get; private set; }
	private int result;
	private readonly Random rand;
	public int RandomUpperLimit;
	public readonly MathOperationOption SelectedOption;
	private readonly static List<int> primes = new()
	{
		2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97
	};

	public MathOperation(MathOperationOption option)
	{
		RandomUpperLimit = 51; // == MathGame.Difficulty.Easy

		rand = new Random(42);

		this.SelectedOption = option;
		switch (this.SelectedOption)
		{
			case MathOperationOption.Addition:
				this.operation = operationsDict[Function.Sum];
				this.Operator = operatorsDict[Function.Sum];
				break;

			case MathOperationOption.Subtraction:
				this.operation = operationsDict[Function.Difference];
				this.Operator = operatorsDict[Function.Difference];
				break;

			case MathOperationOption.Multiplication:
				this.operation = operationsDict[Function.Product];
				this.Operator = operatorsDict[Function.Product];
				break;

			case MathOperationOption.Division:
				this.operation = operationsDict[Function.Quotient];
				this.Operator = operatorsDict[Function.Quotient];
				break;

			case MathOperationOption.Random:
				SetRandomOperation();
				break;
		}

		GenerateNext();
	}

	public void Answer(int answer)
	{
		if (answer == this.result)
		{
			this.Answered = true;
		}
	}

	public void GenerateNext()
	{
		if (this.SelectedOption == MathOperationOption.Random)
		{
			SetRandomOperation();
		}

		this.Answered = false;
		this.OperandA = rand.Next(0, RandomUpperLimit);
		this.OperandB = rand.Next(1, RandomUpperLimit);
		if (this.SelectedOption == MathOperationOption.Division)
		{
			if (primes.Contains(this.OperandA))
			{
				this.OperandB = rand.Next(2) switch
				{
					0 => this.OperandA,
					_ => 1
				};
			}
			else
			{
				while (this.OperandA % this.OperandB != 0)
				{
					this.OperandB = this.rand.Next(1, 101);
				}
			}
		}
		this.result = this.operation!(this.OperandA, this.OperandB);
	}

	private void SetRandomOperation()
	{
		int index = rand.Next(0, operationsDict.Count);
		this.operation = operationsDict.ElementAt(index).Value;
		this.Operator = operatorsDict.ElementAt(index).Value;
	}

	private static readonly Dictionary<Function, Func<int, int, int>> operationsDict = new()
	{
		[Function.Sum] = Sum,
		[Function.Difference] = Difference,
		[Function.Product] = Product,
		[Function.Quotient] = Quotient
	};
	private static readonly Dictionary<Function, char> operatorsDict = new()
	{
		[Function.Sum] = '+',
		[Function.Difference] = '-',
		[Function.Product] = '*',
		[Function.Quotient] = '/'
	};
	private static int Sum(int a, int b) => a + b;
	private static int Difference(int a, int b) => a - b;
	private static int Product(int a, int b) => a * b;
	private static int Quotient(int a, int b) => a / b;

	private enum Function
	{
		Sum = 1,
		Difference = 2,
		Product = 3,
		Quotient = 4
	}
}
