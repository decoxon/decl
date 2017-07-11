namespace decl.Expressions
{
    internal class Number : IExpression
    {
        private int number;

        public Number(int number)
        {
            this.number = number;
        }

        public int Evaluate()
        {
            return number;
        }

        public override string ToString()
        {
            return number.ToString();
        }
    }
}
