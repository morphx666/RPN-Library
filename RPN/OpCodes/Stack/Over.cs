using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Over))]
    public class Over : OpCode {
        public Over() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Over).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            string v1 = stack.Pop();
            string v2 = stack.Pop();
            stack.Push(v2);
            stack.Push(v1);
            stack.Push(v2);
        }
    }
}