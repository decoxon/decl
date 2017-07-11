using System.Collections.Generic;
using System.Text;
using decl.Dice;

namespace decl.Expressions
{
    internal class DiceRoll : BinaryOperatorExpression
    {
        private List<Die> dice;
        public List<Die> Dice { get { return dice; } }

        public DiceRoll(IExpression numberOfDice, IExpression numberOfSides)
            : base(numberOfDice, numberOfSides)
        {
            operatorString = "D";
            dice = new List<Die>();
        }

        public override int Evaluate()
        {
            int numberOfDice = LeftOperand.Evaluate();
            int numberOfSides = RightOperand.Evaluate();

            for (var i = 0; i < numberOfDice; i++)
            {
                dice.Add(new ContiguousNumericDie(numberOfSides));
            }

            int result = 0;

            foreach (ContiguousNumericDie die in dice)
            {
                result += die.Roll();
            }

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dice.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append(" + ");
                }

                sb.Append(dice[i].Result);
            }

            return string.Format("{0} ({1})", base.ToString(), sb.ToString());
        }
    }
}
