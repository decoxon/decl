namespace decl.Expressions
{
    internal class Subtraction : BinaryOperatorExpression
    {
        public Subtraction(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand) { operatorString = " - "; }

        public override int Evaluate()
        {
            return LeftOperand.Evaluate() - RightOperand.Evaluate();
        }
    }
}
