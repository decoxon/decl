using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class Or : BinaryOperator
    {
        public Or(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            // Evaluate in turn to preserve short-circuiting behaviour.
            IExpressionResult left = LeftOperand.Evaluate(context).As(ExpressionType.Truth);

            if (left.Value.Equals(Truth.TRUE))
            {
                result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, Truth.TRUE, new List<IExpressionResult>() { left });
                return result;
            }

            IExpressionResult right = RightOperand.Evaluate(context).As(ExpressionType.Truth);

            if (right.Value.Equals(Truth.TRUE))
            {
                result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, Truth.TRUE, new List<IExpressionResult>() { left, right });
                return result;
            }

            result = new ExpressionResult(this.GetType().Name, ExpressionType.Truth, Truth.FALSE, new List<IExpressionResult>() { left, right });
            return result;
        }
    }
}
