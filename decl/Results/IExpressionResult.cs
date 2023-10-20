using System;
using System.Collections.Generic;
using System.Text;

namespace declang
{
  public interface IExpressionResult
  {
    string OperationType { get; }
    ExpressionType Type { get; }
    string Value { get; }
    IEnumerable<IExpressionResult> ComponentResults { get; }
    string ToResultDetailString(int depth = 0);
    IExpressionResult As(ExpressionType type);
  }
}
