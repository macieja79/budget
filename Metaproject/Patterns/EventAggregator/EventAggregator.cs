using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Metaproject.Patterns.EventAggregator.Events;

namespace Metaproject.Patterns.EventAggregator
{
	 public class EventAggregator : IEventAggregator
    {

        class Fake : IEventAggregator
        {
            public void PublishEvent<TEventType>(TEventType eventToPublish)
            {
               
            }

            public void SubsribeEvent(object subscriber)
            {
     
            }
        }

        static Fake _empty;
        public static IEventAggregator Empty
        {
            get
            {
                if (null == _empty) _empty = new Fake();
                return _empty;
            }
        }




        private Dictionary<Type, List<WeakReference>> eventSubsribers = new Dictionary<Type, List<WeakReference>>();

        private readonly object lockSubscriberDictionary = new object();

        public EventAggregator()
        {
            
        }

        #region IEventAggregator Members              

        /// <summary>
        /// Publish an event
        /// </summary>
        /// <typeparam name="TEventType"></typeparam>
        /// <param name="eventToPublish"></param>
        public void PublishEvent<TEventType>(TEventType eventToPublish)
        {

            if (eventToPublish is AggregatedEvent)
            {
                AggregatedEvent aggEvent = eventToPublish as AggregatedEvent;
                if (aggEvent.IsHandled)
                    return;
            }

            var subsriberType = typeof(ISubscriber<>).MakeGenericType(typeof(TEventType));

            var subscribers = GetSubscriberList(subsriberType);

            List<WeakReference> subsribersToBeRemoved = new List<WeakReference>();

            foreach (var weakSubsriber in subscribers)
            {
                if (weakSubsriber.IsAlive)
                {
                    var subscriber = (ISubscriber<TEventType>)weakSubsriber.Target;

                    InvokeSubscriberEvent<TEventType>(eventToPublish, subscriber);
                }
                else
                {
                    subsribersToBeRemoved.Add(weakSubsriber);

                }//End-if-else (weakSubsriber.IsAlive)

            }//End-for-each (var weakSubriber in subscribers)


            if (subsribersToBeRemoved.Any())
            {
                lock (lockSubscriberDictionary)
                {
                    foreach (var remove in subsribersToBeRemoved)
                    {
                        subscribers.Remove(remove);

                    }//End-for-each (var remove in subsribersToBeRemoved)

                }//End-lock (lockSubscriberDictionary)

            }//End-if (subsribersToBeRemoved.Any())
        }
     
        /// <summary>
        /// Subribe to an event.
        /// </summary>
        /// <param name="subscriber"></param>
        public void SubsribeEvent(object subscriber)
        {
            lock (lockSubscriberDictionary)
            {
                var subsriberTypes = subscriber.GetType().GetInterfaces()
                                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

                WeakReference weakReference = new WeakReference(subscriber);

                foreach (var subsriberType in subsriberTypes)
                {
                    List<WeakReference> subscribers = GetSubscriberList(subsriberType);
                    if (!subscribers.Contains(weakReference))
                    {
                        subscribers.Add(weakReference);
                    }

                }//End-for-each (var subsriberType in subsriberTypes)

            }//End-lock (lockSubscriberDictionary)
        }

      

        #endregion

        private void InvokeSubscriberEvent<TEventType>(TEventType eventToPublish, ISubscriber<TEventType> subscriber)
        {
            //Synchronize the invocation of method 

            SynchronizationContext syncContext = SynchronizationContext.Current;

            if (syncContext == null)
            {
                syncContext = new SynchronizationContext();

            }//End-if (syncContext == null)

            if (eventToPublish is AggregatedEvent)
            {
                AggregatedEvent aggEvent = eventToPublish as AggregatedEvent;
                if (aggEvent.IsHandled) 
                    return;
            }

            subscriber.OnEventHandler(eventToPublish);

            //syncContext.Post(s => subscriber.OnEventHandler(eventToPublish), null);
        }

        private List<WeakReference> GetSubscriberList(Type subsriberType)
        {
            List<WeakReference> subsribersList = null;

            lock (lockSubscriberDictionary)
            {
                bool found = this.eventSubsribers.TryGetValue(subsriberType, out subsribersList);

                if (!found)
                {
                    //First time create the list.

                    subsribersList = new List<WeakReference>();

                    this.eventSubsribers.Add(subsriberType, subsribersList);

                }//End-if (!found)

            }//End-lock (lockSubscriberDictionary)

            return subsribersList;
        }
    }
}
