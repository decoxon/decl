using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class TestCase : TernaryOperator
    {
        public static string CURRENT_TEST_CASE_KEY = "currentTestCaseResult";

        public TestCase(IExpression firstOperand, IExpression secondOperand, IExpression thirdOperand, string format = "{0}:{1}{{ {2} }}") 
            : base(firstOperand, secondOperand, thirdOperand, format) { }

        public override ExpressionResult Evaluate(IDictionary<string, ExpressionResult> context)
        {
            ExpressionResult iterationResult = FirstOperand.Evaluate(context);
            if (Decimal.TryParse(iterationResult.Value, out decimal numIterations))
            {
                int numSuccesses = 0;
                ExpressionResult currentCheckResult = null;
                List<ExpressionResult> componentResults = new List<ExpressionResult>();

                for (var i = 0; i < numIterations; i++)
                {
                    ExpressionResult secondResult = 
                    context[CURRENT_TEST_CASE_KEY] = SecondOperand.Evaluate(context);
                    componentResults.Add(secondResult);
                    currentCheckResult = ThirdOperand.Evaluate(context);

                    if(currentCheckResult.Type == ExpressionType.Truth && currentCheckResult.Value == "true")
                    {
                        numSuccesses++;
                    }

                    componentResults.Add(currentCheckResult);
                }

                result = new ExpressionResult(this.GetType().Name, ExpressionType.Number, numSuccesses.ToString(), componentResults);

                if (context.ContainsKey(CURRENT_TEST_CASE_KEY))
                {
                    context.Remove(CURRENT_TEST_CASE_KEY);
                }

                return result;
            }

            throw new Exception(String.Format("Invalid iteration expression in test case: {0}", iterationResult.Value));
        }
    }
}
