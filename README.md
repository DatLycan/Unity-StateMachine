
<h1 align="left">Unity C# State Machine</h1>

<div align="left">

[![Status](https://img.shields.io/badge/status-active-success.svg)]()
[![GitHub Issues](https://img.shields.io/github/issues/datlycan/Unity-StateMachine.svg)](https://github.com/DatLycan/Unity-StateMachine/issues)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](/LICENSE)

</div>

---

<p align="left"> Easily extendable pure C# State Machine implementation for Unity.
    <br> 
</p>

## 📝 Table of Contents

- [Getting Started](#getting_started)
- [Usage](#usage)
- [Acknowledgments](#acknowledgement)

## 🏁 Getting Started <a name = "getting_started"></a>

### Installing

1. Install the [Git Dependency Resolver](https://github.com/mob-sakai/GitDependencyResolverForUnity).
2. Import it in Unity with the Unity Package Manager using this URL:<br>
   ``https://github.com/DatLycan/Unity-StateMachine.git``

## 🎈 Usage <a name="usage"></a>

### Using the IState interface
   ```C#
   public struct FirstState : IState {
       public void OnEnter() => Debug.Log("Entered First State");
       public void FixedUpdate() { }
       public void Update() => Debug.Log("Update First State");
       public void LateUpdate() { }
       public void OnExit() => Debug.Log("Exited First State");
   }
   ```
   ```C#
   public struct SecondState : IState {
       public void OnEnter() => Debug.Log("Entered Second State");
       public void FixedUpdate() { }
       public void Update() => Debug.Log("Update Second State");
       public void LateUpdate() { }
       public void OnExit() => Debug.Log("Exited Second State");
   }
   ```

   ```C#
   public class MyClass : MonoBehaviour {
       private StateMachine stateMachine;
       private bool toggleState;
   
       private void Start() {
           IState firstState = new FirstState();
           IState secondState = new SecondState();
           
           stateMachine = new StateMachine(entryState:firstState);
           
           stateMachine.AddTransition(from:firstState, to:secondState, new Condition(() => toggleState));
           stateMachine.AddTransition(from:secondState, to:firstState, new Condition(() => toggleState));
       }
   
       private void Update() => toggleState = Input.GetKeyDown(KeyCode.Space);
   }
   ```

### Using the StateBuilder
   ```C#
   public class MyClass : MonoBehaviour {
       private StateMachine stateMachine;
       private bool toggleState;
   
       private void Start() {
           IState firstState = new StateBuilder()
               .WithEnter(() => Debug.Log("Entered First State"))
               .WithExit(() => Debug.Log("Exited First State"))
               .WithUpdate(()  => Debug.Log("Update First State"))
               .Build();
   
           IState secondState = new StateBuilder()
               .WithEnter(() => Debug.Log("Entered Second State"))
               .WithExit(() => Debug.Log("Exited Second State"))
               .WithUpdate(()  => Debug.Log("Update Second State"))
               .Build();
           
           stateMachine = new StateMachine(entryState:firstState);
           
           stateMachine.AddTransition(from:firstState, to:secondState, new Condition(() => toggleState));
           stateMachine.AddTransition(from:secondState, to:firstState, new Condition(() => toggleState));
       }
   
       private void Update() => toggleState = Input.GetKeyDown(KeyCode.Space);
   }
   ```

#### Note:
*The [FixedUpdate()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html), [Update()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html) and [LateUpdate()](https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html) methods are getting called by the according subsystem in the standard Unity PlayerLoop, e.g. every frame.*

---



## 🎉 Acknowledgements <a name = "acknowledgement"></a>

- *Inspired by [adammyhre](https://github.com/adammyhre).*
- *Using [mob-sekai's Git Dependency Resolver For Unity](https://github.com/mob-sakai/GitDependencyResolverForUnity)*

