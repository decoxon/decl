namespace decl.Expressions
{
    internal class Addition : BinaryOperatorExpression
    {
        public Addition(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand) { operatorString = " + "; }

        public override int Evaluate()
        {
            return LeftOperand.Evaluate() + RightOperand.Evaluate();
        }

        
    }
}