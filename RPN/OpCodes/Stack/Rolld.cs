using System;
using static RPN.RPNStack;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(RollD))]
    public class RollD : OpCode {
        public RollD() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(RollD).ToUpper() };
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            int i1 = int.Parse(rpn.Pop().Token);

            if(i1 > rpn.Count) throw new Exception("Too Few Arguments");
            StackItem[] stk = new StackItem[rpn.Count];
            rpn.CopyTo(stk);
            rpn.Clear();
            for(int i = stk.Length - 1; i > 0; i--) {
                if(i1 - 1 == i) rpn.Push(stk[0]);
                rpn.Push(stk[i]);
            }

            return true;
        }
    }
}