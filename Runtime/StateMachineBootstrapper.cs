using DatLycan.Packages.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace DatLycan.Packages.StateMachine {
    internal static class StateMachineBootstrapper {
        private static PlayerLoopSystem stateMachineFixedUpdateSystem;
        private static PlayerLoopSystem stateMachineUpdateSystem;
        private static PlayerLoopSystem stateMachineLateUpdateSystem;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialize() {
            PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertStateMachineManager<FixedUpdate>(ref currentPlayerLoop, 0, StateMachineManager.FixedUpdate, stateMachineFixedUpdateSystem)) {
                Debug.LogWarning("Unable to register StateMachineManager into the FixedUpdate loop.");
                return;
            }
            
            if (!InsertStateMachineManager<Update>(ref currentPlayerLoop, 0, StateMachineManager.Update, stateMachineUpdateSystem)) {
                Debug.LogWarning("Unable to register StateMachineManager into the Update loop.");
                return;
            }
            
            if (!InsertStateMachineManager<PostLateUpdate>(ref currentPlayerLoop, 0, StateMachineManager.LateUpdate, stateMachineLateUpdateSystem)) {
                Debug.LogWarning("Unable to register StateMachineManager into the LateUpdate loop.");
                return;
            }
            
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayerModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayerModeStateChanged;
#endif
            static void OnPlayerModeStateChanged(PlayModeStateChange state) {
                if (state != PlayModeStateChange.ExitingPlayMode) return;
                
                PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                    
                RemoveStateMachineManager<FixedUpdate>(ref currentPlayerLoop, stateMachineFixedUpdateSystem);
                RemoveStateMachineManager<Update>(ref currentPlayerLoop, stateMachineUpdateSystem);
                RemoveStateMachineManager<PostLateUpdate>(ref currentPlayerLoop, stateMachineLateUpdateSystem);
                    
                PlayerLoop.SetPlayerLoop(currentPlayerLoop);
                StateMachineManager.Clear();
            }
        }

        private static void RemoveStateMachineManager<T>(ref PlayerLoopSystem loop, PlayerLoopSystem system) {
            PlayerLoopUtils.RemoveSystem<T>(ref loop, in system);
        }

        private static bool InsertStateMachineManager<T>(ref PlayerLoopSystem loop, int index, PlayerLoopSystem.UpdateFunction method, PlayerLoopSystem system) {
            system = new PlayerLoopSystem {
                type = typeof(StateMachineManager),
                updateDelegate = method,
                subSystemList = null
            };
            return PlayerLoopUtils.InsertSystem<T>(ref loop, in system, index);
        }
    }
}