using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Add))]
    public class Add : Function {
        public Add() {
            base.ArgumentCount = 2;
            base.Symbols = new string[] { "+" };
        }

        public override bool Execute(Stack<string> stack) {
            try {
                double v1 = double.Parse(stack.Pop());
                double v2 = double.Parse(stack.Pop());
                stack.Push((v2 + v1).ToString());
                return true;
            } catch {
                return false;
            }
        }
    }
}