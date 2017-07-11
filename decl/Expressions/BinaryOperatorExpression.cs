using System;

namespace decl.Expressions
{
    internal abstract class BinaryOperatorExpression : IExpression
    {
        protected string operatorString = "";

        public IExpression LeftOperand { get; }
        public IExpression RightOperand { get; }

        public BinaryOperatorExpression(IExpression leftOperand, IExpression rightOperand)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public abstract int Evaluate();

        public override string ToString()
        {
            return String.Format("{0}{1}{2}", LeftOperand.ToString(), operatorString, RightOperand.ToString());
        }
    }
}
