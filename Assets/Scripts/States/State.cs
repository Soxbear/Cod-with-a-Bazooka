using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public abstract class State
    {
        public StateMachine stateMachine;

        internal void Start() {

        }

        protected abstract void OnStart();

        internal void Update() {

        }

        protected abstract void OnUpdate();

        internal void End() {

        }

        protected abstract void OnEnd();
    }
}
