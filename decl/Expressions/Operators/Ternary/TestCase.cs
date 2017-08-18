using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class TestCase : TernaryOperator
    {
        public TestCase(string operatorString, IExpression firstOperand, IExpression secondOperand, IExpression thirdOperand, string format = "{0}:{1}{{ {2} }}") 
            : base(operatorString, firstOperand, secondOperand, thirdOperand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            throw new NotImplementedException();
        }
    }
}
