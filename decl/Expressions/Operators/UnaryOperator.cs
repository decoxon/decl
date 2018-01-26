using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class UnaryOperator : Operator
    {
        public IExpression Operand { get; }

        public UnaryOperator(IExpression operand) : base()
        {
            Operand = operand;
        }

        public override string ToString()
        {
            return String.Format(format, Operand.ToString());
        }
    }
}
