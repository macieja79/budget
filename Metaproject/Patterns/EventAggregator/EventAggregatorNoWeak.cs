using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Metaproject.Patterns.EventAggregator
{
	public class EventAggregatorNoWeak : IEventAggregator
    {

       



        private Dictionary<Type, List<object>> eventSubsribers = new Dictionary<Type, List<object>>();

        private readonly object lockSubscriberDictionary = new object();

        #region IEventAggregator Members              

        /// <summary>
        /// Publish an event
        /// </summary>
        /// <typeparam name="TEventType"></typeparam>
        /// <param name="eventToPublish"></param>
        public void PublishEvent<TEventType>(TEventType eventToPublish)
        {
            var subsriberType = typeof(ISubscriber<>).MakeGenericType(typeof(TEventType));

            var subscribers = GetSubscriberList(subsriberType);

            List<object> subsribersToBeRemoved = new List<object>();

            foreach (var weakSubsriber in subscribers)
            {
                    var subscriber = (ISubscriber<TEventType>)weakSubsriber;
                    InvokeSubscriberEvent<TEventType>(eventToPublish, subscriber);
                

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

                object weakReference = subscriber;

                foreach (var subsriberType in subsriberTypes)
                {
                    List<object> subscribers = GetSubscriberList(subsriberType);

                    subscribers.Add(weakReference);

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

            subscriber.OnEventHandler(eventToPublish);

            //syncContext.Post(s => subscriber.OnEventHandler(eventToPublish), null);
        }

        private List<object> GetSubscriberList(Type subsriberType)
        {
            List<object> subsribersList = null;

            lock (lockSubscriberDictionary)
            {
                bool found = this.eventSubsribers.TryGetValue(subsriberType, out subsribersList);

                if (!found)
                {
                    //First time create the list.

                    subsribersList = new List<object>();

                    this.eventSubsribers.Add(subsriberType, subsribersList);

                }//End-if (!found)

            }//End-lock (lockSubscriberDictionary)

            return subsribersList;
        }
    }
}
