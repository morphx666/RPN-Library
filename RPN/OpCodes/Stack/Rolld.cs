using System;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(RollD))]
    public class RollD : OpCode {
        public RollD() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(RollD).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            int v1 = int.Parse(rpn.Pop());
            if(v1 > rpn.Count) throw new Exception("Too Few Arguments");
            string[] stk = new string[rpn.Count];
            rpn.CopyTo(stk);
            rpn.Clear();
            for(int i = stk.Length - 1; i > 0; i--) {
                if(v1 - 1 == i) rpn.Push(stk[0]);
                rpn.Push(stk[i]);
            }
        }
    }
}