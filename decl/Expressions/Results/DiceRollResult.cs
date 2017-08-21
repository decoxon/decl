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

        public DiceRollResult(ExpressionType type, string value, List<Die> dice, IEnumerable<ExpressionResult> componentResults = null) : base(type, value, componentResults)
        {
            this.dice = dice;
        }
    }
}
