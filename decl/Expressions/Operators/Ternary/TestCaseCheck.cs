using declang.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace declang.Expressions
{
    internal class TestCaseCheck : ValueExpression
    {
        public const string substituteString = "#";
        private string checkExpression;

        public TestCaseCheck(string checkExpression)
        {
            this.checkExpression = checkExpression;
        }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            if (!context.ContainsKey(TestCase.CURRENT_TEST_CASE_KEY))
            {
                throw new Exception(String.Format("Current test case result not present in context at key {0}", TestCase.CURRENT_TEST_CASE_KEY));
            }

            if (context[TestCase.CURRENT_TEST_CASE_KEY].Type != ExpressionType.Number)
            {
                throw new Exception(String.Format("Test Case result must be a Number, not {0}", context[TestCase.CURRENT_TEST_CASE_KEY]));
            }

            result = Parser.GetExpressionTree(composeExpression(context[TestCase.CURRENT_TEST_CASE_KEY].Value)).Evaluate(context);

            if (result.Type != ExpressionType.Truth)
            {
                throw new Exception(String.Format("Check must evaluate to Truth value, not {0}", result.Type.ToString()));
            }

            return result;
        }

        private string composeExpression(string currentCaseValue)
        {
            Regex regex = new Regex(substituteString);

            if (regex.IsMatch(checkExpression))
            {
                return regex.Replace(substituteString, currentCaseValue);
            }
            else
            {
                try
                {
                    if(Parser.GetExpressionTree(checkExpression) is ValueExpression)
                    {
                        return currentCaseValue + "==" + checkExpression;
                    }
                }
                // Fall through to standard return if parsing goes wrong.
                catch(Exception e) { }

                return currentCaseValue + checkExpression;
            }
        }
    }
}
