using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public class StateMachine
    {
        private bool active;

        public bool isActive {
            get {
                return active;
            }
        }

        public enum UpdateMode {
            FRAME,
            FIXED
        }

        private UpdateMode updateType;

        public UpdateMode updateMode {
            get {
                return updateType;
            }
        }

        public void SetUpdateMode(UpdateMode mode) {
            updateType = mode;
        }

        private State current;

        public State currentState {
            get {
                return current;
            }
        }

        private State idle;

        public State idleState {
            get {
                return idle;
            }
        }

        public void SetIdleState(State state) {

        }

        private void Update() {

        }

        public IEnumerator InternalUpdateLoop() {
            while (true) {
                
            }
        }
    }
}
