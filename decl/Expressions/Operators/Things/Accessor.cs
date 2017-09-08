using System;
using System.Collections.Generic;
using System.Text;
using declang.Parsing;

namespace declang.Expressions
{
    internal class Accessor : BinaryOperator
    {
        public Accessor(IExpression leftOperand, IExpression rightOperand, string format = "{0}.{1}") 
            : base(leftOperand, rightOperand, format)
        {
            if(!(LeftOperand.ToVariable() is Variable) || !(RightOperand.ToVariable() is Variable))
            {
                throw new Exception(String.Format("Invalid operands for accessor operator: {0} {1}", LeftOperand, RightOperand));
            }
        }

        public override IExpressionResult Evaluate(Thing context)
        {
            IExpressionResult container = leftOperand.Evaluate(context);

            if (!(container is Thing))
            {
                throw new Exception(String.Format("Left operand is not a Thing: {0}", leftOperand.Result));
            }

            return ((Thing)container).GetValue(((Variable)rightOperand).Name);
        }

        public override Variable ToVariable()
        {
            return new Variable(base.ToString());
        }
    }
}
