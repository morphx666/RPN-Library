using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Mul))]
    public class Mul : Function {
        public Mul() {
            base.ArgumentCount = 2; 
            base.Symbols = new string[] { "*", "×" };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            double v1 = double.Parse(stack.Pop());
            double v2 = double.Parse(stack.Pop());
            stack.Push((v2 * v1).ToString());
        }
    }
}