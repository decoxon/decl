using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Parsing
{
    internal static class Tokeniser
    {
        private static Dictionary<ExpressionType, ExpressionDefinition> expressionDefs = new Dictionary<ExpressionType, ExpressionDefinition>
        {
            {ExpressionType.Variable,           new ExpressionDefinition { Type = ExpressionType.Variable,              Precedence = 5, TriggerCharacters = new char[53] { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','_' } } },
            {ExpressionType.Number,             new ExpressionDefinition { Type = ExpressionType.Number,                Precedence = 5, TriggerCharacters = new char[11] {  '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' } } },
            {ExpressionType.Truth,              new ExpressionDefinition { Type = ExpressionType.Truth,                 Precedence = 5, TriggerCharacters = new char[0] {  } } },
            {ExpressionType.Word,               new ExpressionDefinition { Type = ExpressionType.Word,                  Precedence = 5, TriggerCharacters = new char[2] { '"', '"' } } },
            {ExpressionType.Parens,             new ExpressionDefinition { Type = ExpressionType.Parens,                Precedence = 5, TriggerCharacters = new char[2] { '(', ')' } } },
            {ExpressionType.Negation,           new ExpressionDefinition { Type = ExpressionType.Negation,              Precedence = 4, TriggerCharacters = new char[1] { '!' } } },
            {ExpressionType.Multiplication,     new ExpressionDefinition { Type = ExpressionType.Multiplication,        Precedence = 3, TriggerCharacters = new char[1] { '*' } } },
            {ExpressionType.Division,           new ExpressionDefinition { Type = ExpressionType.Division,              Precedence = 3, TriggerCharacters = new char[1] { '/' } } },
            {ExpressionType.Modulo,             new ExpressionDefinition { Type = ExpressionType.Modulo,                Precedence = 3, TriggerCharacters = new char[1] { '%' } } },
            {ExpressionType.DiceRoll,           new ExpressionDefinition { Type = ExpressionType.DiceRoll,              Precedence = 3, TriggerCharacters = new char[0] {  } } },
            {ExpressionType.Addition,           new ExpressionDefinition { Type = ExpressionType.Addition,              Precedence = 2, TriggerCharacters = new char[1] { '+' } } },
            {ExpressionType.Subtraction,        new ExpressionDefinition { Type = ExpressionType.Subtraction,           Precedence = 2, TriggerCharacters = new char[1] { '-' } } },
            {ExpressionType.TestCaseCheck,      new ExpressionDefinition { Type = ExpressionType.TestCaseCheck,         Precedence = 2, TriggerCharacters = new char[1] { '{' } } },
            {ExpressionType.LessThan,           new ExpressionDefinition { Type = ExpressionType.LessThan,              Precedence = 1, TriggerCharacters = new char[1] { '<' } } },
            {ExpressionType.GreaterThan,        new ExpressionDefinition { Type = ExpressionType.GreaterThan,           Precedence = 1, TriggerCharacters = new char[1] { '>' } } },
            {ExpressionType.LessThanOrEqual,    new ExpressionDefinition { Type = ExpressionType.LessThanOrEqual,       Precedence = 1, TriggerCharacters = new char[0] {  } } },
            {ExpressionType.GreaterThanOrEqual, new ExpressionDefinition { Type = ExpressionType.GreaterThanOrEqual,    Precedence = 1, TriggerCharacters = new char[0] {  } } },
            {ExpressionType.NotEqual,           new ExpressionDefinition { Type = ExpressionType.NotEqual,              Precedence = 1, TriggerCharacters = new char[0] {  } } },
            {ExpressionType.Equal,              new ExpressionDefinition { Type = ExpressionType.Equal,                 Precedence = 1, TriggerCharacters = new char[0] {  } } },
            {ExpressionType.And,                new ExpressionDefinition { Type = ExpressionType.And,                   Precedence = 1, TriggerCharacters = new char[1] { '&' } } },
            {ExpressionType.Or,                 new ExpressionDefinition { Type = ExpressionType.Or,                    Precedence = 1, TriggerCharacters = new char[1] { '|' } } },
            {ExpressionType.TestCase,           new ExpressionDefinition { Type = ExpressionType.TestCase,              Precedence = 1, TriggerCharacters = new char[1] { ':' } } },
            {ExpressionType.Assignment,         new ExpressionDefinition { Type = ExpressionType.Assignment,            Precedence = 0, TriggerCharacters = new char[1] { '=' } } },
            {ExpressionType.Ignore,             new ExpressionDefinition { Type = ExpressionType.Ignore,                Precedence = 9, TriggerCharacters = new char[2] { ' ', ';' } } },
        };

        private static char[] validIdentifierCharacters = new char[63]
        {
             '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
             'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
             'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
             '_'
        };

        /// <summary>
        /// Converts a string expression into a list of Tokens to be used by <see cref="createExpressionTree(List{Token})"/> 
        /// by looking at each character of the string in turn.
        /// </summary>
        /// <param name="script">The script to be tokenised.</param>
        /// <returns>The list of Tokens for the given expression.</returns>
        public static List<List<Token>> Tokenise(string script)
        {
            List<List<Token>> result = new List<List<Token>>();

            if (String.IsNullOrEmpty(script))
            {
                return result;
            }

            // Add first line of resulting expression list
            List<Token> tokens = addExpressionToResult(result);

            string tokenValue;
            int endOfToken;
            int numDecimalPoints;
            bool useDefaultTokenCreationMethod;

            for (int currentCharacter = 0; currentCharacter < script.Length; currentCharacter++)
            {
                if (Char.IsWhiteSpace(script[currentCharacter]))
                {
                    continue;
                }

                if (script[currentCharacter] == ';')
                {
                    if (currentCharacter < script.Length)
                    {
                        tokens = addExpressionToResult(result);
                        continue;
                    }
                }

                ExpressionType type = getCharacterType(script[currentCharacter]);
                useDefaultTokenCreationMethod = false;

                switch (type)
                {
                    case ExpressionType.Number:
                        // Numbers can be multiple characters so we need to find the end of the number.
                        endOfToken = currentCharacter;
                        numDecimalPoints = 0;
                        while (endOfToken < script.Length
                            && (getCharacterType(script[endOfToken]) == ExpressionType.Number
                                || (endOfToken == currentCharacter && (script[endOfToken] == '-' || script[endOfToken] == '+'))))
                        {
                            // Count decimal points and throw exception if there are more than one.
                            if (script[endOfToken] == '.')
                            {
                                numDecimalPoints++;
                            }

                            if (numDecimalPoints > 1)
                            {
                                throw new Exception(String.Format("Too many decimal points in expression {0}.", script));
                            }

                            endOfToken++;
                        }
                        endOfToken--;

                        tokenValue = script.Substring(currentCharacter, endOfToken - currentCharacter + 1);
                        tokens.Add(new Token(type, tokenValue, expressionDefs[type].Precedence));
                        currentCharacter = endOfToken;
                        break;
                    case ExpressionType.Variable:
                        ExpressionType tokenType = type;
                        endOfToken = currentCharacter;

                        // Check for DiceRoll operator
                        if (script.Length > currentCharacter + 1 && (script[currentCharacter] == 'd' || script[currentCharacter] == 'D')
                            && getCharacterType(script[currentCharacter + 1]) == ExpressionType.Number)
                        {
                            tokenType = ExpressionType.DiceRoll;

                            // To allow users to say "d6" instead of having to type out "1d6", we'll add a 1 when
                            // there isn't a number (or a variable which could become a number) present before the 
                            // D operator because it is a binary operator and therefore requires two operands.
                            if (tokens.Count == 0 || (tokens[tokens.Count - 1].Type != ExpressionType.Number && tokens[tokens.Count - 1].Type != ExpressionType.Variable))
                            {
                                tokens.Add(new Token(ExpressionType.Number, "1", expressionDefs[ExpressionType.Number].Precedence));
                            }
                        }
                        // Check for truth value
                        else if ((script.Length >= currentCharacter + 4 && script.Substring(currentCharacter, 4).Equals("true", StringComparison.CurrentCultureIgnoreCase))
                            || (script.Length >= currentCharacter + 5 && script.Substring(currentCharacter, 5).Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                        {
                            tokenType = ExpressionType.Truth;

                            if (script.Substring(currentCharacter, 4).Equals("true", StringComparison.CurrentCultureIgnoreCase))
                            {
                                tokenValue = "true";
                                endOfToken = currentCharacter + 3;
                            }
                            else
                            {
                                tokenValue = "false";
                                endOfToken = currentCharacter + 4;
                            }
                        }
                        else
                        {
                            // Identifiers can be multiple characters so we need to find the end of the identifier.
                            while (endOfToken < script.Length && isValidNonInitialIdentifierCharacter(script[endOfToken]))
                            {
                                endOfToken++;
                            }
                            endOfToken--;
                        }

                        tokenValue = script.Substring(currentCharacter, endOfToken - currentCharacter + 1);
                        tokens.Add(new Token(tokenType, tokenValue, expressionDefs[tokenType].Precedence));
                        currentCharacter = endOfToken;
                        break;
                    case ExpressionType.Word:
                        int endOfString = findEndOfNestingExpression(script, currentCharacter + 1, '"', '"', true);
                        tokenValue = removeEscapeSequences(script.Substring(currentCharacter + 1, endOfString - currentCharacter - 1));
                        tokens.Add(new Token(type, tokenValue, expressionDefs[type].Precedence));
                        currentCharacter = endOfString;
                        break;
                    case ExpressionType.Parens:
                        int endOfParen = findEndOfNestingExpression(script, currentCharacter + 1, '(', ')');
                        tokenValue = script.Substring(currentCharacter + 1, endOfParen - currentCharacter - 1);
                        tokens.Add(new Token(type, tokenValue, expressionDefs[type].Precedence));
                        currentCharacter = endOfParen;
                        break;
                    case ExpressionType.Negation:
                        // Check for NotEqual operator
                        if (script.Length >= currentCharacter && script[currentCharacter + 1] == '=')
                        {
                            tokens.Add(new Token(ExpressionType.NotEqual, "!=", expressionDefs[ExpressionType.Negation].Precedence));
                            currentCharacter++;
                        }
                        else
                        {
                            useDefaultTokenCreationMethod = true;
                        }
                        break;
                    case ExpressionType.Addition:
                    case ExpressionType.Subtraction:
                        // If this is the first character in the expression or the previous token is an operator then we have a unary
                        // plus or minus operator that should be treated as a number.
                        if (tokens.Count == 0 || isAnOperator(tokens[tokens.Count - 1].Type))
                        {
                            type = ExpressionType.Number;
                            goto case ExpressionType.Number;
                        }
                        else
                        {
                            useDefaultTokenCreationMethod = true;
                        }
                        break;
                    case ExpressionType.Assignment:
                        // Check for Equal operator
                        if (script.Length >= currentCharacter + 1 && script[currentCharacter + 1] == '=')
                        {
                            tokens.Add(new Token(ExpressionType.Equal, "==", expressionDefs[ExpressionType.Equal].Precedence));
                            currentCharacter++;
                        }
                        else
                        {
                            useDefaultTokenCreationMethod = true;
                        }
                        break;
                    case ExpressionType.TestCaseCheck:
                        endOfToken = findEndOfNestingExpression(script, currentCharacter + 1, '{', '}');
                        tokenValue = script.Substring(currentCharacter + 1, endOfToken - currentCharacter - 1);
                        tokens.Add(new Token(type, tokenValue, expressionDefs[type].Precedence));
                        currentCharacter = endOfToken;
                        break;
                    case ExpressionType.And:
                    case ExpressionType.Or:
                        if(currentCharacter + 1 > script.Length || script[currentCharacter + 1] != script[currentCharacter])
                        {
                            throw new Exception(String.Format("Second \"{0}\" expected", script[currentCharacter]));
                        }
                        tokens.Add(new Token(type, script.Substring(currentCharacter, 2), expressionDefs[type].Precedence));
                        currentCharacter++;
                        break;
                    case ExpressionType.LessThan:
                        if (script.Length >= currentCharacter + 1 && script[currentCharacter + 1] == '=')
                        {
                            tokens.Add(new Token(ExpressionType.LessThanOrEqual, "<=", expressionDefs[ExpressionType.LessThanOrEqual].Precedence));
                        }
                        else
                        {
                            useDefaultTokenCreationMethod = true;
                        }
                        currentCharacter++;
                        break;
                    case ExpressionType.GreaterThan:
                        if (script.Length >= currentCharacter + 1 && script[currentCharacter + 1] == '=')
                        {
                            tokens.Add(new Token(ExpressionType.GreaterThanOrEqual, ">=", expressionDefs[ExpressionType.GreaterThanOrEqual].Precedence));
                        }
                        else
                        {
                            useDefaultTokenCreationMethod = true;
                        }
                        currentCharacter++;
                        break;
                    case ExpressionType.TestCase:
                    case ExpressionType.Multiplication:
                    case ExpressionType.Division:
                    case ExpressionType.Modulo:
                    case ExpressionType.DiceRoll:
                        useDefaultTokenCreationMethod = true;
                        break;
                }

                if (useDefaultTokenCreationMethod)
                {
                    tokens.Add(new Token(type, script.Substring(currentCharacter, 1), expressionDefs[type].Precedence));
                }
            }

            // We may end up with a blank final expression if there was extra 
            // whitespace after the last statement of the script.
            if (result.Count > 0 && result[result.Count - 1].Count == 0)
            {
                result.RemoveAt(result.Count - 1);
            }

            return result;
        }

        private static List<Token> addExpressionToResult(List<List<Token>> result)
        {
            List<Token> newExpression = new List<Token>();
            result.Add(newExpression);
            return newExpression;
        }

        private static ExpressionType getCharacterType(char c)
        {
            foreach (KeyValuePair<ExpressionType, ExpressionDefinition> type in expressionDefs)
            {
                if (type.Value.IsTriggerCharacter(c))
                {
                    return type.Key;
                }
            }

            throw new Exception(String.Format("Invalid character '{0}' in expression.", c));
        }

        private static bool isAnOperator(ExpressionType type)
        {
            return type != ExpressionType.Word && type != ExpressionType.Number && type != ExpressionType.Truth && type != ExpressionType.Variable;
        }

        private static int findEndOfNestingExpression(string expression, int startAt, char openingCharacter, char closingCharacter, bool canEscape = false, char escapeChar = '\\')
        {
            int nestingLevel = 0;
            int ending = startAt;

            for (; ending < expression.Length; ending++)
            {
                if (canEscape && expression[ending] == escapeChar)
                {
                    ending++;
                    continue;
                }

                if (expression[ending] == closingCharacter)
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

                if (expression[ending] == closingCharacter)
                {
                    nestingLevel++;
                }
            }

            return ending;
        }

        private static bool isValidNonInitialIdentifierCharacter(char character)
        {
            for (var i = 0; i < validIdentifierCharacters.Length; i++)
            {
                if (character == validIdentifierCharacters[i])
                {
                    return true;
                }
            }

            return false;
        }

        private static string removeEscapeSequences(string unescapeString)
        {
            string result = unescapeString;
            for (int i = 0; i < unescapeString.Length; i++)
            {
                if (unescapeString[i] == '\\')
                {
                    result = result.Remove(i, 1);
                    i++;
                }
            }

            return result;
        }
    }
}
