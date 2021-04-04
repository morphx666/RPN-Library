using System.Collections.Generic;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Inv))]
    public class Inv : OpCode {
        public Inv() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Inv).ToUpper() };
            base.DataTypes = new Types[] { Types.Number, Types.Formula };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            if((dataType & Types.Formula) == Types.Formula) {
                string v1 = stack.Pop().Replace("'", "");
                stack.Push($"'1/({v1})'");
            } else {
                double v1 = double.Parse(stack.Pop());
                stack.Push((1 / v1).ToString());
            }
        }
    }
}