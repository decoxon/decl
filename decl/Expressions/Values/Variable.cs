using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Variable : ValueExpression
    {
        private string variableName;

        public string Name => variableName;

        public Variable(string variableName)
        {
            this.variableName = variableName;
        }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            if (context.ContainsKey(variableName))
            {
                result = context[variableName];
                return new ExpressionResult(this.GetType().Name, result.Type, result.Value, new List<ExpressionResult> { result });
            }
            else
            {
                throw new Exception(String.Format("Variable {0} does not exist in this context.", variableName));
            }
        }
    }
}
