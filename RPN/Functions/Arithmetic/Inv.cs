using System.Collections.Generic;

namespace RPN.Functions.Arithmetic {
    [RPNFunctionAttr(nameof(Inv))]
    public class Inv : Function {
        public Inv() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Inv).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            stack.Push((1 / v1).ToString());
        }
    }
}