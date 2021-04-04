using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Rot))]
    public class Rot : OpCode {
        public Rot() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Rot).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            string[] stk = new string[stack.Count];
            stack.CopyTo(stk, 0);
            stack.Clear();
            for(int i = stk.Length - 2; i >= 0; i--) stack.Push(stk[i]);
            stack.Push(stk[^1]);
        }
    }
}