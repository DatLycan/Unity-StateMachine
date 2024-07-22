namespace DatLycan.Packages.StateMachine {
    public interface IState {
        public void OnEnter();
        public void FixedUpdate();
        public void Update();
        public void LateUpdate();
        public void OnExit();
    }

    public struct State : IState {
        public void OnEnter() => throw new System.NotImplementedException();
        public void FixedUpdate() => throw new System.NotImplementedException();
        public void Update() => throw new System.NotImplementedException();
        public void LateUpdate() => throw new System.NotImplementedException();
        public void OnExit() => throw new System.NotImplementedException();
    }
}