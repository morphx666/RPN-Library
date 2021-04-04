using System;
using System.Collections.Generic;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Sqrt))]
    public class Sqrt : OpCode {
        public Sqrt() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Sqrt).ToUpper(), "√" };
            base.DataTypes = new Types[] { Types.Number, Types.Formula };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            if((dataType & Types.Formula) == Types.Formula) {
                string v1 = stack.Pop().Replace("'", "");
                stack.Push($"'√({v1})'");
            } else {
                double v1 = double.Parse(stack.Pop());
                stack.Push(Math.Sqrt(v1).ToString());
            }
        }
    }
}