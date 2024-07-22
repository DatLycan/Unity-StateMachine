using System;
using DatLycan.Packages.Utils;

namespace DatLycan.Packages.StateMachine {
    public struct StateBuilder : IBuilder<IState> {
        private Action onEnter;
        private Action fixedUpdate;
        private Action update;
        private Action lateUpdate;
        private Action onExit;

        public StateBuilder WithEnter(Action action) {
            onEnter = action;
            return this;
        }

        public StateBuilder WithFixedUpdate(Action action) {
            fixedUpdate = action;
            return this;
        }

        public StateBuilder WithUpdate(Action action) {
            update = action;
            return this;
        }

        public StateBuilder WithLateUpdate(Action action) {
            lateUpdate = action;
            return this;
        }

        public StateBuilder WithExit(Action action) {
            onExit = action;
            return this;
        }

        public IState Build() => new State(onEnter, fixedUpdate, update, lateUpdate, onExit);

        private readonly struct State : IState {
            private Action onEnter { get; }
            private Action fixedUpdate { get; }
            private Action update { get; }
            private Action lateUpdate { get; }
            private Action onExit { get; }

            public State(Action onEnter, Action fixedUpdate, Action update, Action lateUpdate, Action onExit) {
                this.onEnter = onEnter;
                this.fixedUpdate = fixedUpdate;
                this.update = update;
                this.lateUpdate = lateUpdate;
                this.onExit = onExit;
            }

            public void OnEnter() => onEnter?.Invoke();
            public void FixedUpdate() => fixedUpdate?.Invoke();
            public void Update() => update?.Invoke();
            public void LateUpdate() => lateUpdate?.Invoke();
            public void OnExit() => onExit?.Invoke();
        }
    }
}