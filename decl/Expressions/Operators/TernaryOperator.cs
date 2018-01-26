using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal abstract class TernaryOperator : Operator
    {
        protected IExpression firstOperand;
        protected IExpression secondOperand;
        protected IExpression thirdOperand;

        public IExpression FirstOperand => firstOperand;
        public IExpression SecondOperand => secondOperand;
        public IExpression ThirdOperand => thirdOperand;

        public TernaryOperator(IExpression firstOperand, IExpression secondOperand, IExpression thirdOperand) : base()
        {
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
            this.thirdOperand = thirdOperand;
        }

        public override string ToString()
        {
            return String.Format(format, firstOperand, secondOperand, thirdOperand);
        }
    }
}
