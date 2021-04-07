using System;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(DropN))]
    public class DropN : OpCode {
        public DropN() {
            ArgumentCount = 1;
            Symbols = new string[] { nameof(DropN).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            int i1 = int.Parse(rpn.Pop().Token);

            if(rpn.Count < i1) {
                rpn.Push(i1.ToString(), Types.Integer);
                throw new ArgumentException("Too few arguments");
            }
            while(--i1 >= 0) rpn.Pop();
        }
    }
}