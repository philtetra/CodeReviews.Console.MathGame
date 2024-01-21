
using static MathGameApp.Models.MathGame;

namespace MathGameApp.Models;

public class MathOperationCollection
{
	public readonly MathOperation Addition;
	public readonly MathOperation Substraction;
	public readonly MathOperation Multiplication;
	public readonly MathOperation Division;
	public readonly MathOperation Random;

	public MathOperationCollection()
	{
		this.Addition = new MathOperation(MathOperationOption.Addition);
		this.Substraction = new MathOperation(MathOperationOption.Subtraction);
		this.Multiplication = new MathOperation(MathOperationOption.Multiplication);
		this.Division = new MathOperation(MathOperationOption.Division);
		this.Random = new MathOperation(MathOperationOption.Random);
	}

	public void SetRandomUpperLimits(int upperLimit)
	{
		this.Addition.RandomUpperLimit = upperLimit;
		this.Substraction.RandomUpperLimit = upperLimit;
		this.Multiplication.RandomUpperLimit = upperLimit;
		this.Division.RandomUpperLimit = upperLimit;
		this.Random.RandomUpperLimit = upperLimit;
	}
}
