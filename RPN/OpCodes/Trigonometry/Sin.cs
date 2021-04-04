using System;
using System.Collections.Generic;

namespace RPN.OpCodes.Trigonometry {
    [OpCodeAttr(nameof(Sin))]
    public class Sin : OpCode {
        public Sin() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Sin).ToUpper() };
            base.DataTypes = new Types[] { Types.Number, Types.Formula };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            if((dataType & Types.Formula) == Types.Formula) {
                string v1 = stack.Pop().Replace("'", "");
                stack.Push($"'{Symbols[0]}({v1})'");
            } else {
                double v1 = double.Parse(stack.Pop());
                stack.Push(Math.Sin(v1).ToString());
            }
        }
    }
}