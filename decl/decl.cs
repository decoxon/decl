using System;
using System.Collections.Generic;
using System.Text;
using declang.Parsing;
using declang.Expressions;

namespace declang
{
    public static class DECL
    {

        public static ExpressionResult Evaluate(string expression, IDictionary<string, ExpressionResult> context = null)
        {
            try
            {
                List<string> statements = new List<string>(expression.Split(Environment.NewLine.ToCharArray()));

                if (context == null)
                {
                    context = new Dictionary<string, ExpressionResult>();
                }
                return Parser.GetExpressionTree(expression).Evaluate(context);
            }
            catch (Exception e)
            {
                throw new Exception("Exception ocurred during execution", e);
            }
        }

        private static ExpressionResult evaluateStatement(string statement, IDictionary<string, ExpressionResult> context)
        {
            return Parser.GetExpressionTree(statement).Evaluate(context);
        }
    }
}
