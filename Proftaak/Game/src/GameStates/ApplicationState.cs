using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameStates
{
    public abstract class ApplicationState
    {
        public abstract void OnCreate();
        public abstract void OnUpdate(float deltatime);
        public abstract void OnFixedUpdate(float deltatime);
        public abstract void OnDraw(float deltatime);
        public abstract void OnDestroy();

        protected void RequestState(ApplicationState state)
        {
            _requestedState = state;
        }

        public bool IsStateRequested()
        {
            return _requestedState != null;
        }

        public ApplicationState GetRequestedState()
        {
            Debug.Assert(_requestedState != null);
            return _requestedState;
        }


        private ApplicationState _requestedState = null;
    }
}
