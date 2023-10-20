using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using declang;
using declang.Expressions;

namespace test_declang
{
  [TestClass]
  public class Arithmetic
  {
    #region Addition
    [TestMethod]
    public void IntegerAddition() { Utilities.AssertExpressionResult("4+5", "9", ExpressionType.Number, "Addition", 2); }

    [TestMethod]
    public void DecimalAddition() { Utilities.AssertExpressionResult("4.1+5.1", "9.2", ExpressionType.Number, "Addition", 2); }

    [TestMethod]
    public void NegativeAddition() { Utilities.AssertExpressionResult("-5+-6", "-11", ExpressionType.Number, "Addition", 2); }
    #endregion

    #region Subtraction
    [TestMethod]
    public void IntegerSubtraction() { Utilities.AssertExpressionResult("5-4", "1", ExpressionType.Number, "Subtraction", 2); }

    [TestMethod]
    public void DecimalSubtraction() { Utilities.AssertExpressionResult("4.12-5.12", "-1", ExpressionType.Number, "Subtraction", 2); }

    [TestMethod]
    public void NegativeSubtraction() { Utilities.AssertExpressionResult("-5--6", "1", ExpressionType.Number, "Subtraction", 2); }
    #endregion

    #region Multiplication
    [TestMethod]
    public void IntegerMultiplication() { Utilities.AssertExpressionResult("5*4", "20", ExpressionType.Number, "Multiplication", 2); }

    [TestMethod]
    public void DecimalMultiplication() { Utilities.AssertExpressionResult("4.12*5.12", "21.0944", ExpressionType.Number, "Multiplication", 2); }

    [TestMethod]
    public void NegativeMultiplication() { Utilities.AssertExpressionResult("-5*-6", "30", ExpressionType.Number, "Multiplication", 2); }
    #endregion

    #region Division
    [TestMethod]
    public void IntegerDivision() { Utilities.AssertExpressionResult("5/4", "1.25", ExpressionType.Number, "Division", 2); }

    [TestMethod]
    public void DecimalDivision() { Utilities.AssertExpressionResult("4.12/5.12", "0.8046875", ExpressionType.Number, "Division", 2); }

    [TestMethod]
    public void NegativeDivision() { Utilities.AssertExpressionResult("-5/-6", "0.8333333333333333333333333333", ExpressionType.Number, "Division", 2); }
    #endregion

    #region Modulo
    [TestMethod]
    public void IntegerModulo() { Utilities.AssertExpressionResult("5%4", "1", ExpressionType.Number, "Modulo", 2); }

    [TestMethod]
    public void DecimalModulo() { Utilities.AssertExpressionResult("4.12%5.12", "4.12", ExpressionType.Number, "Modulo", 2); }

    [TestMethod]
    public void NegativeModulo() { Utilities.AssertExpressionResult("-5%-6", "-5", ExpressionType.Number, "Modulo", 2); }
    #endregion
  }
}
