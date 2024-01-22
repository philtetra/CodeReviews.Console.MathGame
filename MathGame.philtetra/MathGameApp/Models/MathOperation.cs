using System.Runtime.CompilerServices;

namespace MathGameApp.Models;
public class MathOperation
{
	private Func<int, int, int>? operation;
	public bool Answered { get; private set; }
	public char Operator { get; private set; }
	public int OperandA { get; private set; }
	public int OperandB { get; private set; }
	public int Result { get; private set; }
	public static readonly Random NumberGen;

	private MathOperationOption selectedOption;
	public MathOperationOption SelectedOption
	{
		get => selectedOption;
		set
		{
			selectedOption = value;
			SetOperation(MathOperationOptionToFunctionEnum(value));
		}
	}

	private int randomUpperLimit;
	public int RandomUpperLimit
	{
		get => randomUpperLimit;
		set
		{
			if (value > primes[primes.Count - 1])
			{
				randomUpperLimit = primes[primes.Count - 1];
			}
			else
			{
				randomUpperLimit = value;
			}
			GenerateNext();
		}
	}

	static MathOperation()
	{
		int seed = (int)MathF.Abs(DateTime.Now.Ticks / 100);
		NumberGen = new Random(seed);
	}

	public MathOperation(MathOperationOption option)
	{
		this.randomUpperLimit = 33; // == MathGame.Difficulty.Easy

		this.SelectedOption = option;
		SetOperation(MathOperationOptionToFunctionEnum(option));
		GenerateNext();
	}

	private void SetOperation(FunctionEnum function)
	{
		this.operation = operationsDict[function];
		this.Operator = operatorsDict[function];
	}

	public static FunctionEnum MathOperationOptionToFunctionEnum(MathOperationOption option)
	{
		return option switch
		{
			MathOperationOption.Addition => FunctionEnum.Sum,
			MathOperationOption.Subtraction => FunctionEnum.Difference,
			MathOperationOption.Multiplication => FunctionEnum.Product,
			MathOperationOption.Division => FunctionEnum.Quotient,
			_ => getRandomFunctionEnum()// MathOperationOption.Random
		};

		FunctionEnum getRandomFunctionEnum()
		{
			var functionEnumArr = Enum.GetValuesAsUnderlyingType(typeof(FunctionEnum));
			int index = NumberGen.Next(0, functionEnumArr.Length);
			return (FunctionEnum)functionEnumArr.GetValue(index)!;
		}
	}

	public void Answer(int answer) => this.Answered = answer == this.Result;

	public void GenerateNext()
	{
		if (this.SelectedOption == MathOperationOption.Random)
		{
			SetRandomOperation();
		}

		this.Answered = false;

		int lowerLimit = 0;
		if (this.randomUpperLimit > 64)
		{
			lowerLimit = this.randomUpperLimit / 3;
		}
		this.OperandA = NumberGen.Next(lowerLimit, this.randomUpperLimit);
		this.OperandB = NumberGen.Next(lowerLimit + 1, this.randomUpperLimit);
		if (this.SelectedOption == MathOperationOption.Division)
		{
			if (primes.Contains(this.OperandA))
			{
				this.OperandB = NumberGen.Next(2) switch
				{
					0 => this.OperandA,
					_ => 1
				};
			}
			else
			{
				while (this.OperandA % this.OperandB != 0)
				{
					lowerLimit = this.OperandA == 1 ? 1 : 2;
					this.OperandB = NumberGen.Next(lowerLimit, this.OperandA);
				}
			}
		}
		this.Result = this.operation!(this.OperandA, this.OperandB);
	}

	private void SetRandomOperation()
	{
		var randOp = GetRandomOperation(NumberGen);
		this.operation = randOp.Operation;
		this.Operator = randOp.Operator;
	}

	private static (Func<int, int, int> Operation, char Operator) GetRandomOperation(Random rand)
	{
		int index = rand.Next(0, operationsDict.Count);
		return (operationsDict.ElementAt(index).Value,
			operatorsDict.ElementAt(index).Value);
	}

	private static readonly Dictionary<FunctionEnum, Func<int, int, int>> operationsDict = new()
	{
		[FunctionEnum.Sum] = Sum,
		[FunctionEnum.Difference] = Difference,
		[FunctionEnum.Product] = Product,
		[FunctionEnum.Quotient] = Quotient
	};
	private static readonly Dictionary<FunctionEnum, char> operatorsDict = new()
	{
		[FunctionEnum.Sum] = '+',
		[FunctionEnum.Difference] = '-',
		[FunctionEnum.Product] = '*',
		[FunctionEnum.Quotient] = '/'
	};
	private static int Sum(int a, int b) => a + b;
	private static int Difference(int a, int b) => a - b;
	private static int Product(int a, int b) => a * b;
	private static int Quotient(int a, int b) => a / b;
	private readonly static List<int> primes = new()
	{
		2, 3, 5, 7, 11, 13, 17,
		19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67,
		71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113,
		127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
		179, 181, 191, 193, 197, 199, 211, 223, 227, 229,
		233, 239, 241, 251, 257, 263, 269, 271, 277, 281,
		283, 293, 307, 311, 313, 317, 331, 337, 347, 349,
		353, 359, 367, 373, 379, 383, 389, 397, 401, 409,
		419, 421, 431, 433, 439, 443, 449, 457, 461, 463,
		467, 479, 487, 491, 499, 503, 509, 521, 523, 541,
		547, 557, 563, 569, 571, 577, 587, 593, 599, 601,
		607, 613, 617, 619, 631, 641, 643, 647, 653, 659,
		661
	};

	public enum FunctionEnum
	{
		Sum = 1,
		Difference = 2,
		Product = 3,
		Quotient = 4
	}
}