using System;
using System.Collections.Generic;
using System.Text;

namespace declang.Expressions
{
  internal class ExpressionDefinition
  {
    public ExpressionType Type { get; set; }
    public Type ExpressionClass { get; set; }
    public int Precedence { get; set; }
    public char[] TriggerCharacters { get; set; }
    public char[] ValidCharacters { get; set; }
    public string ToStringFormat { get; set; }
    public Dictionary<ExpressionType, Func<ExpressionResult, ExpressionResult>> CastTo { get; set; } = new Dictionary<ExpressionType, Func<ExpressionResult, ExpressionResult>>();

    public bool IsTriggerCharacter(char c)
    {
      for (var i = 0; i < TriggerCharacters.Length; i++)
      {
        if (c == TriggerCharacters[i])
        {
          return true;
        }
      }

      return false;
    }

    public bool IsValidCharacter(char c)
    {
      for (var i = 0; i < ValidCharacters.Length; i++)
      {
        if (c == ValidCharacters[i])
        {
          return true;
        }
      }

      return false;
    }
  }
}
