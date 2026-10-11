// Original HLUnityCore.Runtime Hardlight.DictionaryWithDefault`2: one owner, 34 methods, one field.
// Both shipping CPU shared bodies retained; open definition pointers are all zero.
// Preserve always-true TryGetValue, default indexer, explicit interface returns and unimplemented object operations.
// The five NotImplementedException bodies are genuine shipped throws, not unresolved reconstruction placeholders.
// Managed generic emission, BCL faults and optimizer/runtime parity require separate qualification.
// 0x060000d9 get_Keys: open 0/0; shared arm64 0xf27f18 / x86_64 0xf37bf0.
// 0x060000da get_Values: open 0/0; shared arm64 0xf27fc4 / x86_64 0xf37ca0.
// 0x060000db get_Count: open 0/0; shared arm64 0xf28070 / x86_64 0xf37d50.
// 0x060000dc get_IsReadOnly: open 0/0; shared arm64 0xf28118 / x86_64 0xf37e00.
// 0x060000dd get_IsFixedSize: open 0/0; shared arm64 0xf281c4 / x86_64 0xf37eb0.
// 0x060000de get_IsSynchronized: open 0/0; shared arm64 0xf281cc / x86_64 0xf37ec0.
// 0x060000df System.Collections.ICollection.get_SyncRoot: open 0/0; shared arm64 0xf281d4 / x86_64 0xf37ed0.
// 0x060000e0 System.Collections.IDictionary.get_Keys: open 0/0; shared arm64 0xf281dc / x86_64 0xf37ee0.
// 0x060000e1 System.Collections.IDictionary.get_Values: open 0/0; shared arm64 0xf282b8 / x86_64 0xf37fc0.
// 0x060000e2 .ctor: open 0/0; shared arm64 0xf28394 / x86_64 0xf380a0.
// 0x060000e3 .ctor: open 0/0; shared arm64 0xf28408 / x86_64 0xf38110.
// 0x060000e4 .ctor: open 0/0; shared arm64 0xf28484 / x86_64 0xf38190.
// 0x060000e5 .ctor: open 0/0; shared arm64 0xf28500 / x86_64 0xf38210.
// 0x060000e6 get_Item: open 0/0; shared arm64 0xf2858c / x86_64 0xf382a0.
// 0x060000e7 set_Item: open 0/0; shared arm64 0xf286c8 / x86_64 0xf383a0.
// 0x060000e8 get_Item: open 0/0; shared arm64 0xf2888c / x86_64 0xf38550.
// 0x060000e9 set_Item: open 0/0; shared arm64 0xf289c8 / x86_64 0xf38650.
// 0x060000ea Add: open 0/0; shared arm64 0xf28bac / x86_64 0xf38810.
// 0x060000eb ContainsKey: open 0/0; shared arm64 0xf28d70 / x86_64 0xf389c0.
// 0x060000ec Remove: open 0/0; shared arm64 0xf28ecc / x86_64 0xf38b10.
// 0x060000ed TryGetValue: open 0/0; shared arm64 0xf29028 / x86_64 0xf38c60.
// 0x060000ee Add: open 0/0; shared arm64 0xf291ac / x86_64 0xf38dd0.
// 0x060000ef Add: open 0/0; shared arm64 0xf292cc / x86_64 0xf38ee0.
// 0x060000f0 Clear: open 0/0; shared arm64 0xf29304 / x86_64 0xf38f20.
// 0x060000f1 Contains: open 0/0; shared arm64 0xf293b0 / x86_64 0xf38fd0.
// 0x060000f2 Contains: open 0/0; shared arm64 0xf294d4 / x86_64 0xf390e0.
// 0x060000f3 CopyTo: open 0/0; shared arm64 0xf2950c / x86_64 0xf39120.
// 0x060000f4 Remove: open 0/0; shared arm64 0xf295c0 / x86_64 0xf391e0.
// 0x060000f5 GetEnumerator: open 0/0; shared arm64 0xf296e4 / x86_64 0xf392f0.
// 0x060000f6 System.Collections.IEnumerable.GetEnumerator: open 0/0; shared arm64 0xf2978c / x86_64 0xf393a0.
// 0x060000f7 System.Collections.IDictionary.GetEnumerator: open 0/0; shared arm64 0xf29834 / x86_64 0xf39450.
// 0x060000f8 Remove: open 0/0; shared arm64 0xf2986c / x86_64 0xf39490.
// 0x060000f9 CopyTo: open 0/0; shared arm64 0xf298a4 / x86_64 0xf394d0.
// 0x060000fa FindValue: open 0/0; shared arm64 0xf298dc / x86_64 0xf39510.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Hardlight
{
    public class DictionaryWithDefault<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection
    {
        private IDictionary<TKey, TValue> dictionary;

        public ICollection<TKey> Keys { get { return dictionary.Keys; } }
        public ICollection<TValue> Values { get { return dictionary.Values; } }
        public int Count { get { return dictionary.Count; } }
        public bool IsReadOnly { get { return dictionary.IsReadOnly; } }
        public bool IsFixedSize { get { return false; } }
        public bool IsSynchronized { get { return false; } }
        object ICollection.SyncRoot { get { return null; } }
        ICollection IDictionary.Keys { get { return dictionary.Keys as ICollection; } }
        ICollection IDictionary.Values { get { return dictionary.Values as ICollection; } }

        public DictionaryWithDefault()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        public DictionaryWithDefault(IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public DictionaryWithDefault(int capacity)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public DictionaryWithDefault(int capacity, IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value = default(TValue);
                TryGetValue(key, out value);
                return value;
            }
            set { dictionary[key] = value; }
        }

        public object this[object key]
        {
            get { return this[(TKey)key]; }
            set { dictionary[(TKey)key] = (TValue)value; }
        }

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!dictionary.TryGetValue(key, out value))
                value = default(TValue);
            return true;
        }

        public void Add(KeyValuePair<TKey, TValue> kvp)
        {
            dictionary.Add(kvp);
        }

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> kvp)
        {
            return dictionary.Contains(kvp);
        }

        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> kvp)
        {
            return dictionary.Remove(kvp);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public TValue FindValue(Predicate<TValue> match)
        {
            foreach (TValue value in dictionary.Values)
            {
                if (match(value))
                    return value;
            }
            return default(TValue);
        }
    }
}
