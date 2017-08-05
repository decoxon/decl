using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class UnaryOperator : Operator
    {
        public IExpression Operand { get; }

        public UnaryOperator(string operatorString, IExpression operand, string format = "{0}{1}") 
            : base(operatorString, format)
        {
            Operand = operand;
        }

        public override string ToString()
        {
            return String.Format(format, operatorString, Operand.ToString());
        }
    }
}
