using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MoneyTrain.Buzz
{
    internal class SubscriptionStorage
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _subscriptionList =
            new ConcurrentDictionary<string, HashSet<string>>();

        internal IEnumerable<string> GetAllSubscriberForMessageType(Type type)
        {
            HashSet<string> subscribers;
            _subscriptionList.TryGetValue(type.ToString(), out subscribers);
            return subscribers ?? new HashSet<string>();
        }

        internal void Register(string endpoint, string messageType)
        {
            _subscriptionList.AddOrUpdate(messageType,
                                          s => new HashSet<string> {endpoint},
                                          (s, list) =>
                                              {
                                                  list.Add(endpoint);
                                                  return list;
                                              });
        }
    }
}