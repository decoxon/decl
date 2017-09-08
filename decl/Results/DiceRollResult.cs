using declang.Dice;
using System;
using System.Collections.Generic;
using System.Text;
using declang.Expressions;

namespace declang
{
    public class DiceRollResult : ExpressionResult
    {
        private List<Die> dice;

        public List<Die> Dice => dice;

        public DiceRollResult(string operationType, ExpressionType type, string value, List<Die> dice, IEnumerable<IExpressionResult> componentResults = null) 
            : base(operationType, type, value, componentResults)
        {
            this.dice = dice;
        }

        public override string ToResultDetailString(int depth = 0)
        {
            string spacing = GetSpacingString(depth);
            StringBuilder detailString = GetBasicValueStringBuilder(spacing);
            
            foreach(Die die in dice)
            {
                detailString.Append(spacing);
                detailString.Append("(");
                detailString.Append(die.ToString());
                detailString.AppendLine(")");
            }

            AddComponentResultDetailStrings(detailString, depth + 1);

            return detailString.ToString();
        }
    } 
}
