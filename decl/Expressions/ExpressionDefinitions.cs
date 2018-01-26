using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
    internal static class ExpressionDefinitions
    {
        private static Dictionary<ExpressionType, ExpressionDefinition> expressionDefinitions = new Dictionary<ExpressionType, ExpressionDefinition>
        {
            {ExpressionType.Ignore,             new ExpressionDefinition {
                Type = ExpressionType.Ignore,
                ExpressionClass = null,
                Precedence = 9,
                ToStringFormat ="",
                TriggerCharacters = new char[2] { ' ', ';' }
            } },
            {ExpressionType.Variable,           new ExpressionDefinition {
                Type = ExpressionType.Variable,
                ExpressionClass = typeof(Variable),
                Precedence = 6,
                ToStringFormat ="{0}",
                TriggerCharacters = new char[53] { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','_' }
            } },
            {ExpressionType.Number,             new ExpressionDefinition {
                Type = ExpressionType.Number,
                ExpressionClass = typeof(Number),
                Precedence = 6,
                ToStringFormat ="{0}",
                TriggerCharacters = new char[10] {  '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' },
                CastTo = new Dictionary<ExpressionType, Func<ExpressionResult, ExpressionResult>> ()
                {
                    { ExpressionType.Truth, expr => buildCastResult(ExpressionType.Truth, Decimal.Parse(expr.Value) == 0m ? Truth.FALSE : Truth.TRUE, expr ) },
                    { ExpressionType.Word, expr => buildCastResult(ExpressionType.Word, expr.Value, expr) }
                }
            } },
            {ExpressionType.Truth,              new ExpressionDefinition {
                Type = ExpressionType.Truth,
                ExpressionClass = typeof(Truth),
                Precedence = 6,
                ToStringFormat ="{0}",
                TriggerCharacters = new char[0] {  },
                CastTo = new Dictionary<ExpressionType, Func<ExpressionResult, ExpressionResult>> ()
                {
                    { ExpressionType.Number, expr => buildCastResult(ExpressionType.Number, expr.Value == Truth.TRUE ? "1" : "0", expr ) },
                    { ExpressionType.Word, expr => buildCastResult(ExpressionType.Word, expr.Value, expr) }
                }
            } },
            {ExpressionType.Word,               new ExpressionDefinition {
                Type = ExpressionType.Word,
                ExpressionClass = typeof(Word),
                Precedence = 6,
                ToStringFormat ="{0}",
                TriggerCharacters = new char[2] { '"', '"' },
                CastTo = new Dictionary<ExpressionType, Func<ExpressionResult, ExpressionResult>> ()
                {
                    { ExpressionType.Truth, expr => buildCastResult(ExpressionType.Truth, expr.Value.Length > 0 ? Truth.TRUE : Truth.FALSE, expr ) },
                    { ExpressionType.Number, expr => {

                        if(!Decimal.TryParse(expr.Value, out decimal newValue))
                        {
                            throw new Exception($"Cannot cast {expr.Value} from Word to Number.");
                        }

                        return buildCastResult(ExpressionType.Number, newValue.ToString(), expr);
                    }}
                }
            } },
            {ExpressionType.Thing,              new ExpressionDefinition {
                Type = ExpressionType.Thing,
                ExpressionClass = typeof(ThingLiteral),
                Precedence = 6,
                ToStringFormat ="{0}",
                TriggerCharacters = new char[1] { '{' }
            } },
            {ExpressionType.Parens,             new ExpressionDefinition {
                Type = ExpressionType.Parens,
                ExpressionClass = typeof(Parens),
                Precedence = 6,
                ToStringFormat ="({0})",
                TriggerCharacters = new char[2] { '(', ')' }
            } },
            {ExpressionType.Accessor,           new ExpressionDefinition {
                Type = ExpressionType.Accessor,
                ExpressionClass = typeof(Accessor),
                Precedence = 5,
                ToStringFormat ="{0}.{1}",
                TriggerCharacters = new char[1] { '.' }
            } },
            {ExpressionType.Negation,           new ExpressionDefinition {
                Type = ExpressionType.Negation,
                ExpressionClass = typeof(Negation),
                Precedence = 4,
                ToStringFormat ="!{0}",
                TriggerCharacters = new char[1] { '!' }
            } },
            {ExpressionType.Multiplication,     new ExpressionDefinition {
                Type = ExpressionType.Multiplication,
                ExpressionClass = typeof(Multiplication),
                Precedence = 3,
                ToStringFormat ="{0} * {1}",
                TriggerCharacters = new char[1] { '*' }
            } },
            {ExpressionType.Division,           new ExpressionDefinition {
                Type = ExpressionType.Division,
                ExpressionClass = typeof(Division),
                Precedence = 3,
                ToStringFormat ="{0} / {1}",
                TriggerCharacters = new char[1] { '/' }
            } },
            {ExpressionType.Modulo,             new ExpressionDefinition {
                Type = ExpressionType.Modulo,
                ExpressionClass = typeof(Modulo),
                Precedence = 3,
                ToStringFormat ="{0} % {1}",
                TriggerCharacters = new char[1] { '%' }
            } },
            {ExpressionType.DiceRoll,           new ExpressionDefinition {
                Type = ExpressionType.DiceRoll,
                ExpressionClass = typeof(DiceRoll),
                Precedence = 3,
                ToStringFormat ="{0}D{1}",
                TriggerCharacters = new char[0] {  }
            } },
            {ExpressionType.Addition,           new ExpressionDefinition {
                Type = ExpressionType.Addition,
                ExpressionClass = typeof(Addition),
                Precedence = 2,
                ToStringFormat ="{0} + {1}",
                TriggerCharacters = new char[1] { '+' }
            } },
            {ExpressionType.Subtraction,        new ExpressionDefinition {
                Type = ExpressionType.Subtraction,
                ExpressionClass = typeof(Subtraction),
                Precedence = 2,
                ToStringFormat ="{0} - {1}",
                TriggerCharacters = new char[1] { '-' }
            } },
            {ExpressionType.TestCaseCheck,      new ExpressionDefinition {
                Type = ExpressionType.TestCaseCheck,
                ExpressionClass = typeof(TestCaseCheck),
                Precedence = 2,
                ToStringFormat ="",
                TriggerCharacters = new char[1] { '{' }
            } },
            {ExpressionType.LessThan,           new ExpressionDefinition {
                Type = ExpressionType.LessThan,
                ExpressionClass = typeof(LessThan),
                Precedence = 1,
                ToStringFormat ="{0} < {1}",
                TriggerCharacters = new char[1] { '<' }
            } },
            {ExpressionType.GreaterThan,        new ExpressionDefinition {
                Type = ExpressionType.GreaterThan,
                ExpressionClass = typeof(GreaterThan),
                Precedence = 1,
                ToStringFormat ="{0} > {1}",
                TriggerCharacters = new char[1] { '>' }
            } },
            {ExpressionType.LessThanOrEqual,    new ExpressionDefinition {
                Type = ExpressionType.LessThanOrEqual,
                ExpressionClass = null,
                Precedence = 1,
                ToStringFormat ="",
                TriggerCharacters = new char[0] {  }
            } },
            {ExpressionType.GreaterThanOrEqual, new ExpressionDefinition {
                Type = ExpressionType.GreaterThanOrEqual,
                ExpressionClass = null,
                Precedence = 1,
                ToStringFormat ="",
                TriggerCharacters = new char[0] {  }
            } },
            {ExpressionType.NotEqual,           new ExpressionDefinition {
                Type = ExpressionType.NotEqual,
                ExpressionClass = typeof(NotEqual),
                Precedence = 1,
                ToStringFormat ="{0} != {1}",
                TriggerCharacters = new char[0] {  }
            } },
            {ExpressionType.Equal,              new ExpressionDefinition {
                Type = ExpressionType.Equal,
                ExpressionClass = typeof(Equal),
                Precedence = 1,
                ToStringFormat ="{0} == {1}",
                TriggerCharacters = new char[0] {  }
            } },
            {ExpressionType.And,                new ExpressionDefinition {
                Type = ExpressionType.And,
                ExpressionClass = typeof(And),
                Precedence = 1,
                ToStringFormat ="{0} && {1}",
                TriggerCharacters = new char[1] { '&' }
            } },
            {ExpressionType.Or,                 new ExpressionDefinition {
                Type = ExpressionType.Or,
                ExpressionClass = typeof(Or),
                Precedence = 1,
                ToStringFormat ="{0} || {1}",
                TriggerCharacters = new char[1] { '|' }
            } },
            {ExpressionType.TestCase,           new ExpressionDefinition {
                Type = ExpressionType.TestCase,
                ExpressionClass = typeof(TestCase),
                Precedence = 1,
                ToStringFormat ="{0}:{1}{{ {2} }}",
                TriggerCharacters = new char[1] { ':' }
            } },
            {ExpressionType.Assignment,         new ExpressionDefinition { Type = ExpressionType.Assignment,         ExpressionClass = typeof(Assignment),      Precedence = 0, ToStringFormat="{0} = {1}",         TriggerCharacters = new char[1] { '=' } } },
        };

        public static ExpressionDefinition GetDefinition(ExpressionType type)
        {
            return expressionDefinitions.ContainsKey(type) ? expressionDefinitions[type] : null;
        }

        public static ExpressionType GetCharacterType(char c)
        {
            foreach (KeyValuePair<ExpressionType, ExpressionDefinition> type in expressionDefinitions)
            {
                if (type.Value.IsTriggerCharacter(c))
                {
                    return type.Key;
                }
            }

            throw new Exception(String.Format("Invalid character '{0}' in expression.", c));
        }

        public static char[] GetTriggerCharacters(ExpressionType type)
        {
            if (!expressionDefinitions.ContainsKey(type) || expressionDefinitions[type].TriggerCharacters.Length < 1)
            {
                throw new Exception(String.Format("No trigger characters defined for expression type {0}", type.ToString()));
            }

            return expressionDefinitions[type].TriggerCharacters;
        }

        public static string GetToStringFormatForType(Type type)
        {
            foreach (KeyValuePair<ExpressionType, ExpressionDefinition> kvp in expressionDefinitions)
            {
                if (type == kvp.Value.ExpressionClass)
                {
                    return kvp.Value.ToStringFormat;
                }
            }

            return String.Empty;
        }

        private static ExpressionResult buildCastResult(ExpressionType type, string value, IExpressionResult originalResult)
        {
            return new ExpressionResult("Cast", type, value, new List<IExpressionResult>() { originalResult });
        }
    }
}
