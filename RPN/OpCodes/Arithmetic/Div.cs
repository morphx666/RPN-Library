using System.Collections.Generic;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Div))]
    public class Div : OpCode {
        public Div() {
            base.ArgumentCount = 2; 
            base.Symbols = new string[] { "/", "÷" };
            base.DataTypes = new Types[] { Types.Number, Types.Formula };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            if((dataType & Types.Formula) == Types.Formula) {
                string v1 = stack.Pop().Replace("'", "");
                string v2 = stack.Pop().Replace("'", "");
                stack.Push($"'({v2})/({v1})'");
            } else {
                double v1 = double.Parse(stack.Pop());
                double v2 = double.Parse(stack.Pop());
                stack.Push((v2 / v1).ToString());
            }
        }
    }
}