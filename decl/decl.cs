using System;
using System.Collections.Generic;
using System.Text;
using declang.Parsing;
using declang.Expressions;

namespace declang
{
    public static class decl
    {

        public static ExpressionResult Evaluate (string expression, IDictionary<string, ExpressionResult> context = null)
        {
            try
            {
                if (context == null)
                {
                    context = new Dictionary<string, ExpressionResult>();
                }
                return Parser.GetExpressionTree(expression).Evaluate(context);
            }
            catch(Exception e)
            {
                throw new Exception("Exception ocurred during execution", e);
            }
        }
    }
}
