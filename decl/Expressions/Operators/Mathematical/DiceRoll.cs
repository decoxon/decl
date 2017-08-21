using System.Collections.Generic;
using System.Text;
using declang.Dice;
using System;

namespace declang.Expressions
{
    internal class DiceRoll : BinaryOperator
    {
        private List<Die> dice;

        public DiceRoll(IExpression numberOfDice, IExpression numberOfSides, string format = "{0}D{1}")
            : base(numberOfDice, numberOfSides, format)
        {
            dice = new List<Die>();
        }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            ExpressionResult left = LeftOperand.Evaluate(context);
            ExpressionResult right = RightOperand.Evaluate(context);

            if (Int32.TryParse(Math.Floor(Convert.ToDecimal(left.Value)).ToString(), out int numberOfDice)
                && Int32.TryParse(Math.Floor(Convert.ToDecimal(right.Value)).ToString(), out int numberOfSides))
            {
                for (var i = 0; i < numberOfDice; i++)
                {
                    dice.Add(new ContiguousNumericDie(numberOfSides));
                }

                int result = 0;

                foreach (ContiguousNumericDie die in dice)
                {
                    result += die.Roll();
                }

                this.result = new DiceRollResult(ExpressionType.Number, result.ToString(), dice, new List<ExpressionResult>() { left, right });
                return this.result;
            }

            throw new Exception(String.Format("Invalid dice operator operands {0} and {1}", LeftOperand.ToString(), RightOperand.ToString()));
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
