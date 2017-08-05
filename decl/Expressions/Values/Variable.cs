using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    class Variable : IExpression
    {
        private string variableName;

        public string Name => variableName;

        public Variable(string variableName)
        {
            this.variableName = variableName;
        }

        public ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            if (context.ContainsKey(variableName))
            {
                return context[variableName];
            }
            else
            {
                throw new Exception(String.Format("Variable {0} does not exist in this context.", variableName));
            }
        }
    }
}
