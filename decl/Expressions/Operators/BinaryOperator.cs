using System;
using System.Collections.Generic;

namespace declang.Expressions
{
    internal abstract class BinaryOperator : Operator
    {
        public IExpression LeftOperand { get; }
        public IExpression RightOperand { get; }

        public BinaryOperator(string operatorString, IExpression leftOperand, IExpression rightOperand, string format = "{1}{0}{2}") 
            : base(operatorString, format)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public override string ToString()
        {
            return String.Format(format, operatorString, LeftOperand.ToString(), RightOperand.ToString());
        }
    }
}
