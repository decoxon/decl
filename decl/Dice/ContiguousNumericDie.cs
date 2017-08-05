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

        public override int Roll()
        {
            Random rng = new Random(Guid.NewGuid().GetHashCode());

            Result = rng.Next(minResult, minResult + Sides);

            HasBeenRolled = true;

            return Result;
        }
    }
}
