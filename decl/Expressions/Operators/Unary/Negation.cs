using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class Negation : UnaryOperator
    {
        public Negation(IExpression operand, string format = "!{0}") : base(operand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            ExpressionResult beforeNegation = Operand.Evaluate(context);

            if(beforeNegation.Type == ExpressionType.Truth)
            {
                result = new ExpressionResult(ExpressionType.Truth, beforeNegation.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase) ? "false" : "true", new List<ExpressionResult>() { beforeNegation });
                return result;
            }

            throw new Exception(String.Format("Cannot negate expression of type {0}", beforeNegation.Type));
        }
    }
}
