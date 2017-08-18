using System;
using System.Collections.Generic;

namespace declang.Expressions
{
    internal abstract class BinaryOperator : Operator
    {
        protected IExpression leftOperand;
        protected IExpression rightOperand;

        public IExpression LeftOperand => leftOperand;
        public IExpression RightOperand => rightOperand;

        public BinaryOperator(IExpression leftOperand, IExpression rightOperand, string format = "{0}{1}") 
            : base(format)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
        }

        public override string ToString()
        {
            return String.Format(format, LeftOperand.ToString(), RightOperand.ToString());
        }
    }
}
