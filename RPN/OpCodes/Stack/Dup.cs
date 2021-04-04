using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Dup))]
    public class Dup : OpCode {
        public Dup() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Dup).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            stack.Push(stack.Peek());
        }
    }
}