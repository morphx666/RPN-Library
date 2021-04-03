using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Dup))]
    public class Dup : Function {
        public Dup() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Dup).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            stack.Push(stack.Peek());
        }
    }
}