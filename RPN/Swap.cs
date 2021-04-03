using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Swap))]
    public class Swap : Function {
        public Swap() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Swap).ToUpper() };
        }

        public override bool Execute(Stack<string> stack) {
            try {
                string v1 = stack.Pop();
                string v2 = stack.Pop();
                stack.Push(v1);
                stack.Push(v2);
                return true;
            } catch {
                return false;
            }
        }
    }
}