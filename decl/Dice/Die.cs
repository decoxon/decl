using System;

namespace declang.Dice
{
  public abstract class Die
  {
    public int Sides { get; }
    public bool HasBeenRolled { get; protected set; }
    public int Result { get; protected set; }

    public Die(int sides)
    {
      Sides = sides;
      HasBeenRolled = false;
    }

    public abstract int Roll();

    public override string ToString()
    {
      return String.Format("D{0}: {1}", Sides, (HasBeenRolled ? Result.ToString() : "Unrolled"));
    }
  }
}
