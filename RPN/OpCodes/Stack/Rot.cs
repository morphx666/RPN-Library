using static RPN.RPNStack;

namespace RPN.OpCodes.Stack {
    [OpCodeAttr(nameof(Rot))]
    public class Rot : OpCode {
        public Rot() {
            ArgumentCount = 2;
            Symbols = new string[] { nameof(Rot).ToUpper() };
        }

        public override void ExecuteInternal(RPNStack rpn, Types dataType) {
            StackItem[] stk = new StackItem[rpn.Count];
            rpn.CopyTo(stk);
            rpn.Clear();
            for(int i = stk.Length - 2; i >= 0; i--) rpn.Push(stk[i]);
            rpn.Push(stk[^1]);
        }
    }
}