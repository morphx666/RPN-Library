using System;
using static RPN.RPNStack;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(DupN))]
    public class DupN : OpCode {
        public DupN() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(DupN).ToUpper() };
        }

        public override bool ExecuteInternal(RPNStack rpn, Types dataType) {
            int i1 = int.Parse(rpn.Pop().Token);

            if(rpn.Count < i1) {
                rpn.Push(i1.ToString(), Types.Integer);
                throw new ArgumentException("Too few arguments");
            }
            StackItem[] stk = new StackItem[i1];
            Array.Copy(rpn.ToArray(), rpn.Count - i1, stk, 0, i1);
            while(--i1 >= 0) rpn.Push(stk[i1]);

            return true;
        }
    }
}