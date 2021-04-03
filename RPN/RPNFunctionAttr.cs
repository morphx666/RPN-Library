using System;

namespace RPN {
    [AttributeUsage(AttributeTargets.Class)]
    public class RPNFunctionAttr : Attribute {
        private string Description { get; init; }

        public RPNFunctionAttr(string description) {
            Description = description;
        }

    }
}
