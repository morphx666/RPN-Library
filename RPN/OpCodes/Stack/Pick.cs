using System;
using static RPN.RPNStack;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Pick))]
    public class Pick : OpCode {
        public Pick() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Pick).ToUpper() };
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            int i1 = int.Parse(rpn.Pop().Token);
            if(i1 > rpn.Count) throw new Exception("Too Few Arguments");
            StackItem[] stk = new StackItem[rpn.Count];
            rpn.CopyTo(stk);
            rpn.Push(stk[i1 - 1]);

            return true;
        }
    }
}