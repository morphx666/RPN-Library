using System;
using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Pick))]
    public class Pick : OpCode {
        public Pick() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { nameof(Pick).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            int v1 = int.Parse(stack.Pop());
            if(v1 > stack.Count) throw new Exception("Too few arguments");
            string[] stk = new string[stack.Count];
            stack.CopyTo(stk, 0);
            stack.Push(stk[v1 - 1]);
        }
    }
}