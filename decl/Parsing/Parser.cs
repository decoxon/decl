using declang.Expressions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace declang.Parsing
{
    internal static class Parser
    {
        /// <summary>
        /// Create an expression tree, ready to be evaluated, for the given expression.
        /// </summary>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>The completed expression tree.</returns>
        public static Script Parse(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return new Script(createExpressionTrees(Tokeniser.Tokenise(expression)));
        }

        public static IExpression ParseSingleStatement(string statement, bool throwOnMultiline = true)
        {
            if (String.IsNullOrEmpty(statement))
            {
                throw new ArgumentNullException("expression");
            }

            var statements = Tokeniser.Tokenise(statement);

            if (statements.Count < 1)
            {
                throw new Exception(String.Format("No statements provided: {0}", statement));
            }

            if (statements.Count > 1 && throwOnMultiline)
            {
                throw new Exception(String.Format("Multiple statements provided to ParseSingleStatement: {0}", statement));
            }

            return createExpressionTree(statements[0]);
        }

        private static List<IExpression> createExpressionTrees(List<List<Token>> statements)
        {
            if (statements.Count <= 0)
            {
                return new List<IExpression>() { new Word("") };
            }

            var expressions = new List<IExpression>();

            foreach (var statement in statements)
            {
                expressions.Add(createExpressionTree(statement));
            }

            return expressions;
        }

        /// <summary>
        /// Recursively creates an expression tree for the provided list of tokens.
        /// </summary>
        /// <param name="tokens">The list of Token objects representing the expression created by <see cref="tokeniseExpression(string)"/></param>
        /// <exception cref="Exception">Thrown if a Token has an <see cref="ExpressionType"/> that is not recognised.</exception>
        /// <returns>The completed expression tree.</returns>
        private static IExpression createExpressionTree(List<Token> tokens)
        {
            if (tokens.Count <= 0)
            {
                return new Word("");
            }

            // Find the right-most, lowest precedence token
            var selectedToken = tokens.Count - 1;

            for (var currentTokenIndex = tokens.Count - 1; currentTokenIndex >= 0; currentTokenIndex--)
            {
                if (tokens[currentTokenIndex].Precedence < tokens[selectedToken].Precedence)
                {
                    selectedToken = currentTokenIndex;
                }
            }

            switch (tokens[selectedToken].Type)
            {
                case ExpressionType.Thing:
                case ExpressionType.Number:
                case ExpressionType.Variable:
                case ExpressionType.Truth:
                case ExpressionType.Word:
                    return (IExpression)typeof(Parser)
                        .GetRuntimeMethod("createValueExpression", new Type[] { typeof(ExpressionType), typeof(string) })
                        .MakeGenericMethod(ExpressionDefinitions.GetDefinition(tokens[selectedToken].Type).ExpressionClass)
                        .Invoke(null, new object[] {
                            tokens[selectedToken].Type,
                            tokens[selectedToken].Value
                        }
                    );
                case ExpressionType.Accessor:
                case ExpressionType.Addition:
                case ExpressionType.Subtraction:
                case ExpressionType.Multiplication:
                case ExpressionType.Division:
                case ExpressionType.Modulo:
                case ExpressionType.DiceRoll:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.And:
                case ExpressionType.Or:
                    return (IExpression)typeof(Parser)
                        .GetRuntimeMethod("createBinaryOperatorExpression", new Type[] { typeof(ExpressionType), typeof(List<Token>), typeof(List<Token>) })
                        .MakeGenericMethod(ExpressionDefinitions.GetDefinition(tokens[selectedToken].Type).ExpressionClass)
                        .Invoke(null, new object[] {
                            tokens[selectedToken].Type,
                            tokens.GetRange(0, selectedToken),
                            tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)
                        }
                    );
                case ExpressionType.Parens:
                    var innerExpression = Tokeniser.Tokenise(tokens[selectedToken].Value);

                    if (innerExpression.Count <= 0)
                    {
                        throw new Exception(String.Format("Empty Parens expression"));
                    }

                    if (innerExpression.Count > 1)
                    {
                        throw new Exception(String.Format("Cannot have multiple statements in parentheses: {0}", tokens[selectedToken].Value));
                    }

                    return new Parens(createExpressionTree(innerExpression[0]));
                case ExpressionType.GreaterThanOrEqual:
                    return new Or(
                        new GreaterThan(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))),
                        new Equal(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))));
                case ExpressionType.LessThanOrEqual:
                    return new Or(
                        new LessThan(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))),
                        new Equal(
                            createExpressionTree(tokens.GetRange(0, selectedToken)),
                            createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken))));
                case ExpressionType.Negation:
                    return new Negation(createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Assignment:
                    IExpression leftOperand = createExpressionTree(tokens.GetRange(0, selectedToken));
                    return new Assignment(
                        leftOperand.ToVariable(),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.TestCase:
                    IExpression firstOperand = createExpressionTree(tokens.GetRange(0, selectedToken));

                    var testCaseCheckLocation = selectedToken + 1;

                    while (testCaseCheckLocation < tokens.Count - 1 && tokens[testCaseCheckLocation].Type != ExpressionType.TestCaseCheck)
                    {
                        testCaseCheckLocation++;
                    }

                    if (testCaseCheckLocation - selectedToken < 2 || tokens[testCaseCheckLocation].Type != ExpressionType.TestCaseCheck)
                    {
                        throw new Exception("No check or case found.");
                    }

                    IExpression secondOperand = createExpressionTree(tokens.GetRange(selectedToken + 1, testCaseCheckLocation - selectedToken - 1));
                    IExpression thirdOperand = new TestCaseCheck(tokens[testCaseCheckLocation].Value);

                    return new TestCase(firstOperand, secondOperand, thirdOperand);
                default:
                    throw new Exception(String.Format("Unrecognised expression type '{0}'.", tokens[selectedToken].Type.ToString()));
            }
        }

        public static IExpression createValueExpression<T>(ExpressionType type, string value)
            where T : ValueExpression
        {
            return (T)Activator.CreateInstance(typeof(T), value);
        }

        public static IExpression createUnaryOperatorExpression<T>(ExpressionType type, List<Token> operand)
            where T : UnaryOperator
        {
            return (T)Activator.CreateInstance(typeof(T), createExpressionTree(operand));
        }

        public static IExpression createBinaryOperatorExpression<T>(ExpressionType type, List<Token> leftOperand, List<Token> rightOperand)
            where T : BinaryOperator
        {
            return (T)Activator.CreateInstance(typeof(T), createExpressionTree(leftOperand), createExpressionTree(rightOperand));
        }

        public static IExpression createTernaryOperatorExpression<T>(ExpressionType type, List<Token> firstOperand, List<Token> secondOperand, List<Token> thirdOperand)
            where T : TernaryOperator
        {
            return (T)Activator.CreateInstance(typeof(T), firstOperand, secondOperand, thirdOperand);
        }
    }
}
