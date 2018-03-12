using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Patterns.State
{
    
    public abstract class State
    {

        #region <properties>
        public string ID { get; private set; }
        protected StateManager _manager { get; private set; }
        #endregion

        #region <constructor>
        public State(string id, StateManager manager)
        {
			ID = id;
            _manager = manager;
        }
        #endregion

        #region <methods to override>

        public virtual void OnLeave() { }
        public virtual void OnEnter() { }

       
        #endregion

    }
}
