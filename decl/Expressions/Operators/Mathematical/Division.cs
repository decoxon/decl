using System;
using System.Collections.Generic;

namespace declang.Expressions
{
    internal class Division : BinaryOperator
    {
        public Division(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            return divide(LeftOperand.Evaluate(context), RightOperand.Evaluate(context));
        }

        private IExpressionResult divide(IExpressionResult leftOperand, IExpressionResult rightOperand)
        {
            IExpressionResult left = leftOperand.As(ExpressionType.Number);
            IExpressionResult right = rightOperand.As(ExpressionType.Number);
            decimal leftDecimal = Decimal.Parse(left.Value);
            decimal rightDecimal = Decimal.Parse(right.Value);
            result = new ExpressionResult(this.GetType().Name, ExpressionType.Number, (leftDecimal / rightDecimal).ToString(), new List<IExpressionResult>() { left, right });
            return result;
        }
    }
}
