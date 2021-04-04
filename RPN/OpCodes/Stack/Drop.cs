using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Drop))]
    public class Drop : OpCode {
        public Drop() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Drop).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            stack.Pop();
        }
    }
}