using declang;
using declang.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_declang
{
    [TestClass]
    public class DiceRolls
    {
        [TestMethod]
        public void DiceRoll()
        {
            IExpressionResult result = DECL.Evaluate("5D6");

            Assert.IsTrue(Int32.Parse(result.Value) >= 5 && Int32.Parse(result.Value) <= 30);
            Assert.AreEqual(ExpressionType.Number, result.Type);
            Assert.AreEqual("DiceRoll", result.OperationType);
        }

        [TestMethod]
        public void DiceRollInvalidFirstOperand()
        {
            var context = new Thing();
            Assert.ThrowsException<InvalidCastException>(() => (new DiceRoll(new Word("test"), new Number("3"))).Evaluate(context));
        }

        [TestMethod]
        public void DiceRollInvalidSecondOperand()
        {
            var context = new Thing();
            Assert.ThrowsException<InvalidCastException>(() => (new DiceRoll(new Number("3"), new Word("test"))).Evaluate(context));
        }


    }
}
