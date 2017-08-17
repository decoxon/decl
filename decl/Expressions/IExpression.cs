using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal interface IExpression
    {
        ExpressionResult Result { get; }
        ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context);
    }
}
