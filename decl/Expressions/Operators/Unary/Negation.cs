using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class Negation : UnaryOperator
    {
        public Negation(IExpression operand) : base(operand) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            IExpressionResult beforeNegation = Operand.Evaluate(context);

            if(beforeNegation.Type == ExpressionType.Truth)
            {
                result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, beforeNegation.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase) ? "false" : "true", new List<IExpressionResult>() { beforeNegation });
                return result;
            }

            throw new Exception(String.Format("Cannot negate expression of type {0}", beforeNegation.Type));
        }
    }
}
