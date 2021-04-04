using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Dup2))]
    public class Dup2 : OpCode {
        public Dup2() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Dup2).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            stack.Push("2 DUPN");
        }
    }
}