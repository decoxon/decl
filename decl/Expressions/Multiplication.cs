namespace decl.Expressions
{
    internal class Multiplication : BinaryOperatorExpression
    {
        public Multiplication(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand) { operatorString = " x "; }

        public override int Evaluate()
        {
            return LeftOperand.Evaluate() * RightOperand.Evaluate();
        }
    }
}
