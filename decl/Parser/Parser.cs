using decl.Expressions;
using System;
using System.Collections.Generic;

namespace decl.Parser
{
    /// <summary>
    /// The types of expressions recognised by the <see cref="Parser"/>.
    /// </summary>
    internal enum ExpressionType
    {
        Number,
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Parens,
        DiceRoll
    }

    internal static class Parser
    {
        /// <summary>
        /// Defines the valid characters for expressions
        /// </summary>
        public static Dictionary<ExpressionType, char[]> ExpressionCharacters = new Dictionary<ExpressionType, char[]>
        {
            {ExpressionType.Number,         new char[10] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'} },
            {ExpressionType.Addition,       new char[1]  {'+'} },
            {ExpressionType.Subtraction,    new char[1]  {'-'} },
            {ExpressionType.Multiplication, new char[2]  {'*', 'x'} },
            {ExpressionType.Division,       new char[1]  {'/'} },
            {ExpressionType.Parens,         new char[2]  {'(', ')'} },
            {ExpressionType.DiceRoll,       new char[2]  {'D', 'd'} }
        };

        /// <summary>
        /// Defines the precedence of each type of expression.
        /// </summary>
        public static Dictionary<ExpressionType, int> ExpressionPrecedence = new Dictionary<ExpressionType, int>
        {
            {ExpressionType.Number,         3 },
            {ExpressionType.Addition,       1 },
            {ExpressionType.Subtraction,    1 },
            {ExpressionType.Multiplication, 2 },
            {ExpressionType.Division,       2 },
            {ExpressionType.Parens,         3 },
            {ExpressionType.DiceRoll,       2 }
        };

        /// <summary>
        /// Create an expression tree, ready to be evaluated, for the given expression.
        /// </summary>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>The completed expression tree.</returns>
        public static IExpression GetExpressionTree(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return createExpressionTree(tokeniseExpression(expression));
        }

        /// <summary>
        /// Recursively creates an expression tree for the provided list of tokens.
        /// </summary>
        /// <param name="tokens">The list of Token objects representing the expression created by <see cref="tokeniseExpression(string)"/></param>
        /// <exception cref="Exception">Thrown if a Token has an <see cref="ExpressionType"/> that is not recognised.</exception>
        /// <returns>The completed expression tree.</returns>
        private static IExpression createExpressionTree(List<Token> tokens)
        {
            // Find the right-most, lowest precedence token
            int selectedToken = tokens.Count - 1;

            for (int currentTokenIndex = tokens.Count - 1; currentTokenIndex >= 0; currentTokenIndex--)
            {
                if (tokens[currentTokenIndex].Precedence < tokens[selectedToken].Precedence)
                {
                    selectedToken = currentTokenIndex;
                }
            }

            switch (tokens[selectedToken].Type)
            {
                case ExpressionType.Number:
                    return new Number(Int32.Parse(tokens[selectedToken].Value));
                case ExpressionType.Parens:
                    return new Parens(createExpressionTree(tokeniseExpression(tokens[selectedToken].Value)));
                case ExpressionType.Addition:
                    return new Addition(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Subtraction:
                    return new Subtraction(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Multiplication:
                    return new Multiplication(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.Division:
                    return new Division(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                case ExpressionType.DiceRoll:
                    return new DiceRoll(
                        createExpressionTree(tokens.GetRange(0, selectedToken)),
                        createExpressionTree(tokens.GetRange(selectedToken + 1, tokens.Count - 1 - selectedToken)));
                default:
                    throw new Exception(String.Format("Unrecognised expression type '{0}'.", tokens[selectedToken].Type.ToString()));
            }
        }

        /// <summary>
        /// Converts a string expression into a list of Tokens to be used by <see cref="createExpressionTree(List{Token})"/> 
        /// by looking at each character of the string in turn.
        /// </summary>
        /// <param name="expression">The expression to be tokenised.</param>
        /// <returns>The list of Tokens for the given expression.</returns>
        private static List<Token> tokeniseExpression(string expression)
        {
            List<Token> tokens = new List<Token>();
            string tokenValue;


            for (int currentCharacter = 0; currentCharacter < expression.Length; currentCharacter++)
            {
                ExpressionType type = getExpressionType(expression[currentCharacter]);

                switch (type)
                {
                    case ExpressionType.Number:
                        // Numbers can be multiple characters so we need to find the end of the number.
                        int endOfNumber = currentCharacter;
                        while (endOfNumber < expression.Length && getExpressionType(expression[endOfNumber]) == ExpressionType.Number)
                        {
                            endOfNumber++;
                        }
                        endOfNumber--;
                        tokenValue = expression.Substring(currentCharacter, endOfNumber - currentCharacter + 1);
                        tokens.Add(new Token(type, tokenValue, ExpressionPrecedence[type]));
                        currentCharacter = endOfNumber;
                        break;
                    case ExpressionType.Parens:
                        // Look for matching closing paren
                        int endOfParen = currentCharacter + 1;
                        int nestingLevel = 0;

                        for (; endOfParen < expression.Length; endOfParen++)
                        {
                            if (expression[endOfParen] == ')')
                            {
                                if (nestingLevel == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    nestingLevel--;
                                }
                            }

                            if (expression[endOfParen] == '(')
                            {
                                nestingLevel++;
                            }
                        }

                        tokenValue = expression.Substring(currentCharacter + 1, endOfParen - currentCharacter - 1);
                        tokens.Add(new Token(type, tokenValue, ExpressionPrecedence[type]));
                        currentCharacter = endOfParen;
                        break;
                    case ExpressionType.Addition:
                    case ExpressionType.Subtraction:
                    case ExpressionType.Multiplication:
                    case ExpressionType.Division:
                    case ExpressionType.DiceRoll:
                        tokens.Add(new Token(type, expression.Substring(currentCharacter, 1), ExpressionPrecedence[type]));
                        break;
                }
            }

            return tokens;
        }

        private static ExpressionType getExpressionType(char c)
        {
            foreach (KeyValuePair<ExpressionType, char[]> type in ExpressionCharacters)
            {
                for (int i = 0; i < type.Value.Length; i++)
                {
                    if (c == type.Value[i])
                    {
                        return type.Key;
                    }
                }
            }

            throw new Exception(String.Format("Invalid character '{0}' in expression.", c));
        }
    }
}
