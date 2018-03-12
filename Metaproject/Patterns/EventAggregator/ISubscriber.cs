﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Patterns.EventAggregator
{
	public interface ISubscriber<TEventType> 
    {
        void OnEventHandler(TEventType e);
    }
}
