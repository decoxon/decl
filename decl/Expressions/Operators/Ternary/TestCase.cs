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

        public override IExpressionResult Evaluate(Thing context)
        {
            IExpressionResult iterationResult = FirstOperand.Evaluate(context);
            if (Decimal.TryParse(iterationResult.Value, out decimal numIterations))
            {
                int numSuccesses = 0;
                IExpressionResult currentCheckResult = null;
                List<IExpressionResult> componentResults = new List<IExpressionResult>();

                for (var i = 0; i < numIterations; i++)
                {
                    IExpressionResult secondResult = SecondOperand.Evaluate(context);
                    context[CURRENT_TEST_CASE_KEY] = secondResult;
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
