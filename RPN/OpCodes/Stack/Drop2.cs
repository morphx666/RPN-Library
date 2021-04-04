using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Drop2))]
    public class Drop2 : OpCode {
        public Drop2() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Drop2).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            stack.Push("2 DROPN");
        }
    }
}