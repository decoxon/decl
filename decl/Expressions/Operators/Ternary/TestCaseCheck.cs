using declang.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace declang.Expressions
{
    internal class TestCaseCheck : ValueExpression
    {
        public const string CHECK_VALUE_SUBSTITUTION_STRING = "#";
        private string checkExpression;

        public TestCaseCheck(string checkExpression)
        {
            this.checkExpression = checkExpression;
        }

        public override IExpressionResult Evaluate(Thing context)
        {
            if (!context.ContainsKey(TestCase.CURRENT_TEST_CASE_KEY))
            {
                throw new Exception($"Current test case result not present in context at key {TestCase.CURRENT_TEST_CASE_KEY}");
            }

            result = Parser.Parse(composeExpression(context[TestCase.CURRENT_TEST_CASE_KEY].Value)).Run(context).As(ExpressionType.Truth);

            return new ExpressionResult(this.GetType().Name, ExpressionType.Truth, result.Value, new List<IExpressionResult> { result });
        }

        private string composeExpression(string currentCaseValue)
        {
            Regex regex = new Regex(CHECK_VALUE_SUBSTITUTION_STRING);

            if (regex.IsMatch(checkExpression))
            {
                return regex.Replace(CHECK_VALUE_SUBSTITUTION_STRING, currentCaseValue);
            }
            else
            {
                try
                {
                    if(Parser.Parse(checkExpression).IsSingleExpressionOfType<ValueExpression>())
                    {
                        return String.Format(ExpressionDefinitions.GetDefinition(ExpressionType.Equal).ToStringFormat, currentCaseValue, checkExpression);
                    }
                }
                // If parsing fails, assume that check expression assumes the current value will be prepended to it.
                catch(Exception) {  }

                return currentCaseValue + checkExpression;
            }
        }
    }
}
