using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Swap))]
    public class Swap : OpCode {
        public Swap() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Swap).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            string v1 = stack.Pop();
            string v2 = stack.Pop();
            stack.Push(v1);
            stack.Push(v2);
        }
    }
}