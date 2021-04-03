using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Drop))]
    public class Drop : Function {
        public Drop() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Drop).ToUpper() };
        }

        public override bool Execute(Stack<string> stack) {
            try {
                stack.Pop();
                return true;
            } catch {
                return false;
            }
        }
    }
}