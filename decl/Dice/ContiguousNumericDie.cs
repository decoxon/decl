using System;

namespace declang.Dice
{
  public class ContiguousNumericDie : Die
  {
    private int minResult;

    public ContiguousNumericDie(int sides, int minResult = 1)
        : base(sides)
    {
      this.minResult = minResult;
    }

    public ContiguousNumericDie(ContiguousNumericDie die) : base(die.Sides)
    {
      this.minResult = die.minResult;
    }

    public override int Roll()
    {
      if (!HasBeenRolled)
      {
        var rng = new Random(Guid.NewGuid().GetHashCode());

        Result = rng.Next(minResult, minResult + Sides - 1);

        HasBeenRolled = true;
      }

      return Result;
    }
  }
}
