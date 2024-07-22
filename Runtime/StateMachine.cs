using System;
using System.Collections.Generic;

namespace DatLycan.Packages.StateMachine {
    public class StateMachine : IDisposable {
        public event Action<IState> OnStateSet = delegate { };
        public event Action<IState, IState> OnStateChanged = delegate { };
        
        private StateNode current;
        private readonly Dictionary<IState, StateNode> nodes = new();
        private readonly HashSet<ITransition> absoluteTransitions = new();

        public IState CurrentState => current?.State;

        public StateMachine(IState entryState) {
            SetState(GetOrAddNode(entryState).State);
            StateMachineManager.RegisterStateMachine(this);
        }
        
        public void Update() {
            ITransition transition = GetTransition();
            if (transition != null) {
                ChangeState(transition.To);
            }

            current?.State?.Update();
        }
        
        public void FixedUpdate() => current?.State?.FixedUpdate();
        public void LateUpdate() => current?.State?.LateUpdate();

        public void SetState(IState state) {
            current = nodes[state];
            current?.State?.OnEnter();
            
            OnStateSet.Invoke(state);
        }
        
        private void ChangeState(IState state) {
            if (state == current?.State) return;

            IState previousState = current?.State;
            IState nextState = nodes[state].State;
            
            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state];
            
            OnStateChanged.Invoke(previousState, nextState);
        }

        private ITransition GetTransition() {
            foreach (ITransition transition in absoluteTransitions) {
                if (transition.Condition.Evaluate()) return transition;
            }

            foreach (ITransition transition in current.Transitions) {
                if (transition.Condition.Evaluate()) return transition;            
            }

            return null;
        }

        private StateNode GetOrAddNode(IState state) {
            if (nodes.TryGetValue(state, out StateNode node)) return node;
            
            node = new StateNode(state);
            nodes.Add(state, node);
            return node;
        }
        
        public void AddTransition(IState from, IState to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        public void AddTransition(IState from, ITransition transition) {
            GetOrAddNode(from).AddTransition(transition.To, transition.Condition);
        }
        
        public void AddAbsoluteTransition(IState to, IPredicate condition) {
            absoluteTransitions.Add(new Transition(to, condition));
        }
        public void AddAbsoluteTransition(ITransition transition) {
            absoluteTransitions.Add(transition);
        }      
        
        
        private bool disposed;
        
        ~StateMachine() => Dispose(false);

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposed) return;
            
            if (disposing) {
                StateMachineManager.UnregisterStateMachine(this);
            }

            disposed = true;
        }
        
        
        private class StateNode {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state) {
                State = state;
                Transitions = new();
            }

            public void AddTransition(IState to, IPredicate condition) => Transitions.Add(new Transition(to, condition));
        }
    }
}
