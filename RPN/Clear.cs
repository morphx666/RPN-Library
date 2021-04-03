﻿using System.Collections.Generic;

namespace RPN {
    [RPNFunctionAttr(nameof(Clear))]
    public class Clear : Function {
        public Clear() {
            base.ArgumentCount = 1;
            base.Symbols = new string[] { nameof(Clear).ToUpper() };
        }

        public override void ExecuteInternal(Stack<string> stack) {
            stack.Clear();
        }
    }
}