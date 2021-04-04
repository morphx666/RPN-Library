using RPN.OpCodes.Special;
using System.Collections.Generic;

namespace RPN.OpCodes.Probability {
    [OpCodeAttr(nameof(Fact))]
    public class Fact : OpCode {
        public Fact() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Fact).ToUpper(), "!" };
            base.DataTypes = new Types[] { Types.Number, Types.Formula };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            if((dataType & Types.Formula) == Types.Formula) {
                string v1 = stack.Pop().Replace("'", "");
                stack.Push($"'{Symbols[0]}({v1})'");
            } else {
                double v1 = double.Parse(stack.Pop());
                stack.Push(SpecialFunctions.Fact(v1).ToString());
            }
        }
    }
}