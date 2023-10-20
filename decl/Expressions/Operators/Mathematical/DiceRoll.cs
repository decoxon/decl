using System.Collections.Generic;
using System.Text;
using declang.Dice;
using System;

namespace declang.Expressions
{
    internal class DiceRoll : BinaryOperator
    {
        private List<Die> dice;

        public DiceRoll(IExpression numberOfDice, IExpression numberOfSides)
            : base(numberOfDice, numberOfSides)
        { }

        public override IExpressionResult Evaluate(Thing context)
        {
            dice = new List<Die>();
            IExpressionResult left = LeftOperand.Evaluate(context).As(ExpressionType.Number);
            IExpressionResult right = RightOperand.Evaluate(context).As(ExpressionType.Number);

            var numberOfDice = Decimal.Parse(left.Value);
            var numberOfSides = Convert.ToInt32(Math.Floor(Decimal.Parse(right.Value)));

            for (var i = 0; i < numberOfDice; i++)
            {
                dice.Add(new ContiguousNumericDie(numberOfSides));
            }

            var result = 0M;

            foreach (ContiguousNumericDie die in dice)
            {
                result += die.Roll();
            }

            this.result = new DiceRollResult(this.GetType().Name, ExpressionType.Number, result.ToString("G29"), dice, new List<IExpressionResult>() { left, right });
            return this.result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < dice.Count; i++)
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
