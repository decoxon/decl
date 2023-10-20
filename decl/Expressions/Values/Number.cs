using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
  class Number : ValueExpression
  {
    private decimal number;

    public Number(string number)
    {
      if (!Decimal.TryParse(number, out this.number))
      {
        throw new Exception(String.Format("Invalid number value {0}", number));
      }
    }

    public override IExpressionResult Evaluate(Thing context)
    {
      result = new ExpressionResult(this.GetType().Name, ExpressionType.Number, number.ToString("G29"));
      return result;
    }

    public override string ToString()
    {
      return number.ToString();
    }
  }
}
