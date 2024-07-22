namespace DatLycan.Packages.StateMachine {
    public interface IPredicate {
        public bool Evaluate();
    }
    
    public readonly struct Condition : IPredicate {
        private readonly System.Func<bool> condition;

        public Condition(System.Func<bool> condition) => this.condition = condition;
        public bool Evaluate() => condition.Invoke();
    }
}