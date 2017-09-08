using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class Or : BinaryOperator
    {
        public Or(IExpression leftOperand, IExpression rightOperand, string format = "{0} || {1}")
            : base(leftOperand, rightOperand, format) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            IExpressionResult left = LeftOperand.Evaluate(context);
            IExpressionResult right = RightOperand.Evaluate(context);

            if (left.Type == ExpressionType.Truth && right.Type == ExpressionType.Truth)
            {
                result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, (left.Value.Equals(Truth.TRUE) || right.Value.Equals(Truth.TRUE)) ? Truth.TRUE : Truth.FALSE, new List<IExpressionResult>() { left, right });
                return result;
            }

            throw new Exception(String.Format("Cannot compare expressions of type {0} and {1}", left.Type.ToString(), right.Type.ToString()));
        }
    }
}
