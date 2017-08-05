using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions.Operators
{
    class Assignment : BinaryOperator
    {
        public Assignment(Variable leftOperand, IExpression rightOperand)
            : base(" = ", leftOperand, rightOperand)
        {
        }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            context[((Variable)LeftOperand).Name] = RightOperand.Evaluate(context);
            return LeftOperand.Evaluate(context);
        }
    }
}
