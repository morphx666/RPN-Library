using System;
using static RPN.RPNStack;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Roll))]
    public class Roll : OpCode {
        public Roll() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Roll).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            int i1 = int.Parse(rpn.Pop().Token);

            if(i1 > rpn.Count) throw new Exception("Too Few Arguments");
            StackItem[] stk = new StackItem[rpn.Count];
            rpn.CopyTo(stk);
            rpn.Clear();
            for(int i = stk.Length - 1; i >= 0; i--) {
                if((i1 - 1) != i) rpn.Push(stk[i]);
            }
            rpn.Push(stk[i1 - 1]);
        }
    }
}