using System.Collections.Generic;

namespace RPN.OpCodes.Arithmetic {
    [OpCodeAttr(nameof(Add))]
    public class Add : OpCode {
        public Add() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { "+" };
            base.DataTypes = new Types[] { Types.Number, Types.String, Types.Formula };
        }

        public override void ExecuteInternal(Stack<string> stack, Types dataType) {
            if((dataType & Types.String) == Types.String) {
                string v1 = stack.Pop().Replace("\"", "");
                string v2 = stack.Pop().Replace("\"", "");
                stack.Push($"\"{v2}{v1}\"");
            } else if((dataType & Types.Formula) == Types.Formula) {
                string v1 = stack.Pop().Replace("'", "");
                string v2 = stack.Pop().Replace("'", "");
                stack.Push($"'({v2})+({v1})'");
            } else {
                double v1 = double.Parse(stack.Pop());
                double v2 = double.Parse(stack.Pop());
                stack.Push((v2 + v1).ToString());
            }
        }
    }
}