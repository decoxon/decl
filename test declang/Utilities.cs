using declang;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_declang
{
    internal static class Utilities
    {
        public static void AssertExpressionResult(string expression, string expectedValue, ExpressionType expectedType, string expectedOperation, int numExpectedComponentResults = 0)
        {
            IExpressionResult result = DECL.Evaluate(expression);

            Assert.AreEqual(expectedOperation, result.OperationType);
            Assert.AreEqual(expectedType, result.Type);
            Assert.AreEqual(expectedValue, result.Value);

            if (numExpectedComponentResults > 0)
            {
                Assert.AreEqual(numExpectedComponentResults, result.ComponentResults.Count());
            }
        }
    }
}
