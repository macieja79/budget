using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Patterns.State
{
   

    public class StateManager
    {



        #region <members>
        List<State> _listStates = new List<State>();
		Stack<string> _stackSetStateTypes = new Stack<string>();
        bool _isPreviousSet = false;
        string _defaultStateId = "";
      
        #endregion

        #region <constructor>
        public StateManager()
        {


           
         

        }


        #endregion

        #region <properties
        public State CurrentState { get; private set; }

        

        #endregion

        #region <private methods>
        State GetState(string  id)
        {
            State state = _listStates.Find(s => s.ID == id);
            return state;
        }
        #endregion

        #region <public methods>

        
        public void SetCurrentState(string id)
        {

            if (null != CurrentState)
            {
                CurrentState.OnLeave();

                if (!_isPreviousSet)
                    _stackSetStateTypes.Push(CurrentState.ID);
            }

			CurrentState = GetState(id);

            if (null != CurrentState)
                CurrentState.OnEnter();
        }

        public void SetPreviousState()
        {

            string previousStateType = _stackSetStateTypes.Pop();

            _isPreviousSet = true;
            SetCurrentState(previousStateType);
            _isPreviousSet = false;
        }

       

        public void SetDefaultState()
        {
			// TODO: implementacja domyslnego stanu
            SetCurrentState(_defaultStateId);
        }

        public void AddState(State state)
        {
            if (_listStates.Count == 0) _defaultStateId = state.ID;
            _listStates.Add(state);
        }


        #endregion









    }
}
