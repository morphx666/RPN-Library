using System;
using System.Collections.Generic;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(DropN))]
    public class DropN : OpCode {
        public DropN() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(DropN).ToUpper() };
            base.DataTypes = new Types[] { Types.Any };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            int v1 = int.Parse(stack.Pop());
            if(stack.Count < v1) {
                stack.Push(v1.ToString());
                throw new ArgumentException("Too few arguments");
            }
            while(--v1 >= 0) stack.Pop();
        }
    }
}