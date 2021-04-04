using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Depth))]
    public class Depth : OpCode {
        public Depth() {
            base.ArgumentCount = 0;
            base.Symbols = new string[] { nameof(Depth).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            stack.Push(stack.Count.ToString());
        }
    }
}