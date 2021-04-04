using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Clear))]
    public class Clear : OpCode {
        public Clear() {
            base.ArgumentCount = 0;
            base.Symbols = new string[] { nameof(Clear).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            stack.Clear();
        }
    }
}