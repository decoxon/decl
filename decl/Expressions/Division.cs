namespace decl.Expressions
{
    internal class Division : BinaryOperatorExpression
    {
        public Division(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand) { operatorString = " / "; }

        public override int Evaluate()
        {
            return LeftOperand.Evaluate() / RightOperand.Evaluate();
        }
    }
}
