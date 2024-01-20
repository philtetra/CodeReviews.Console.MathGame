namespace MathGameApp.Models;
class MathOperation
{
	public bool Answered { get; private set; }
	private Func<int, int, int> operation;
	public char OperationSign;
	public int a, b;
	public int result;
	private Random rand;
	public readonly MathOperationOption SelectedOption;

	public MathOperation(MathOperationOption option)
	{
		rand = new Random(42);

		switch (option)
		{
			case MathOperationOption.Addition:
				this.operation = Sum;
				this.OperationSign = '+';
				break;

			case MathOperationOption.Subtraction:
				this.operation = Difference;
				this.OperationSign = '-';
				break;

			case MathOperationOption.Multiplication:
				this.operation = Product;
				this.OperationSign = '*';
				break;

			case MathOperationOption.Division:
				this.operation = Quotient;
				this.OperationSign = '/';
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
		this.a = rand.Next(0, 101);
		this.b = rand.Next(0, 101);
		this.result = this.operation(this.a, this.b);
	}

	private void SetRandomOperation()
	{
		int index = rand.Next(0, operationsDict.Count);
		this.operation = operationsDict.ElementAt(index).Value;
	}

	private static readonly Dictionary<Function, Func<int, int, int>> operationsDict = new()
	{
		[Function.Sum] = Sum,
		[Function.Difference] = Difference,
		[Function.Product] = Product,
		[Function.Quotient] = Quotient
	};
	private static int Sum(int a, int b) => a + b;
	private static int Difference(int a, int b) => a - b;
	private static int Product(int a, int b) => a * b;
	private static int Quotient(int a, int b) => a / b;

	private enum Function
	{
		Sum,
		Difference,
		Product,
		Quotient
	}
}
