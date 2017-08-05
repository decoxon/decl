using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Number : IExpression
    {
        private decimal number;

        public Number(string number)
        {
            if(!Decimal.TryParse(number, out this.number))
            {
                throw new Exception(String.Format("Invalid number value {0}", number));
            }
        }

        public ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            return new ExpressionResult(ExpressionType.Number, number.ToString());
        }

        public override string ToString()
        {
            return number.ToString();
        }
    }
}
