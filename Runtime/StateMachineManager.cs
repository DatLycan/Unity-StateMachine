using System.Collections.Generic;

namespace DatLycan.Packages.StateMachine {
    public static class StateMachineManager {
        private static readonly List<StateMachine> stateMachines = new();

        public static void RegisterStateMachine(StateMachine stateMachine) => stateMachines.Add(stateMachine);
        public static void UnregisterStateMachine(StateMachine stateMachine) => stateMachines.Remove(stateMachine);

        public static void FixedUpdate() => stateMachines.ForEach(stateMachine => stateMachine.FixedUpdate());
        public static void Update() => stateMachines.ForEach(stateMachine => stateMachine.Update());
        public static void LateUpdate() => stateMachines.ForEach(stateMachine => stateMachine.LateUpdate());

        public static void Clear() => stateMachines.Clear();
    }
}