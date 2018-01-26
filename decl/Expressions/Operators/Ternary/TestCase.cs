using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal class TestCase : TernaryOperator
    {
        public static string CURRENT_TEST_CASE_KEY = "_currentTestCaseResult";

        public TestCase(IExpression firstOperand, IExpression secondOperand, IExpression thirdOperand) 
            : base(firstOperand, secondOperand, thirdOperand) { }

        public override IExpressionResult Evaluate(Thing context)
        {
            IExpressionResult iterationResult = FirstOperand.Evaluate(context).As(ExpressionType.Number);
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
                    currentCheckResult = ThirdOperand.Evaluate(context).As(ExpressionType.Truth);

                    if(currentCheckResult.Value == Truth.TRUE)
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
