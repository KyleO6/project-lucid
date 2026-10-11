// Original HLUnityCore.Runtime.dll Hardlight.Utils.DictionaryWithNew`2 (020002a6).
// Complete 33-method source recovery; original generic definition pointers are zero.
// 76 MethodSpecs include 66 shared functions per CPU; ten have no direct function record.
// OBJECT/OBJECT and fully-shared-generic transports remain distinct original contexts.
// Complete native ranges below are half-open; original symbols and raw bytes are in evidence.
// Source is research preservation, not current emission, native fault parity, or provider/runtime credit.
// TValue:new() only; nongeneric Add/Contains/Remove/CopyTo deliberately throw.
// 0x0600111e System.Collections.Generic.ICollection`1<TKey> get_Keys() | arm64 classInst=317 [0xf29e34,0xf29ee0); arm64 classInst=5416 [0xf2b260,0xf2b30c); x86_64 classInst=317 [0xf39a40,0xf39af0); x86_64 classInst=5416 [0xf3af30,0xf3afe0)
// 0x0600111f System.Collections.Generic.ICollection`1<TValue> get_Values() | arm64 classInst=317 [0xf29ee0,0xf29f8c); arm64 classInst=5416 [0xf2b30c,0xf2b3b8); x86_64 classInst=317 [0xf39af0,0xf39ba0); x86_64 classInst=5416 [0xf3afe0,0xf3b090)
// 0x06001120 System.Int32 get_Count() | arm64 classInst=317 [0xf29f8c,0xf2a034); arm64 classInst=5416 [0xf2b3b8,0xf2b460); x86_64 classInst=317 [0xf39ba0,0xf39c50); x86_64 classInst=5416 [0xf3b090,0xf3b140)
// 0x06001121 System.Boolean get_IsReadOnly() | arm64 classInst=317 [0xf2a034,0xf2a0e0); arm64 classInst=5416 [0xf2b460,0xf2b50c); x86_64 classInst=317 [0xf39c50,0xf39d00); x86_64 classInst=5416 [0xf3b140,0xf3b1f0)
// 0x06001122 System.Boolean get_IsFixedSize() | arm64 classInst=317 [0xf2a0e0,0xf2a0e8); arm64 classInst=5416 [0xf2b50c,0xf2b514); x86_64 classInst=317 [0xf39d00,0xf39d10); x86_64 classInst=5416 [0xf3b1f0,0xf3b200)
// 0x06001123 System.Boolean get_IsSynchronized() | arm64 classInst=317 [0xf2a0e8,0xf2a0f0); arm64 classInst=5416 [0xf2b514,0xf2b51c); x86_64 classInst=317 [0xf39d10,0xf39d20); x86_64 classInst=5416 [0xf3b200,0xf3b210)
// 0x06001124 System.Object System.Collections.ICollection.get_SyncRoot() | arm64 classInst=317 [0xf2a0f0,0xf2a0f8); arm64 classInst=5416 [0xf2b51c,0xf2b524); x86_64 classInst=317 [0xf39d20,0xf39d30); x86_64 classInst=5416 [0xf3b210,0xf3b220)
// 0x06001125 System.Collections.ICollection System.Collections.IDictionary.get_Keys() | arm64 classInst=317 [0xf2a0f8,0xf2a1d4); arm64 classInst=5416 [0xf2b524,0xf2b600); x86_64 classInst=317 [0xf39d30,0xf39e10); x86_64 classInst=5416 [0xf3b220,0xf3b300)
// 0x06001126 System.Collections.ICollection System.Collections.IDictionary.get_Values() | arm64 classInst=317 [0xf2a1d4,0xf2a2b0); arm64 classInst=5416 [0xf2b600,0xf2b6dc); x86_64 classInst=317 [0xf39e10,0xf39ef0); x86_64 classInst=5416 [0xf3b300,0xf3b3e0)
// 0x06001127 System.Void .ctor() | arm64 classInst=317 [0xf2a2b0,0xf2a320); arm64 classInst=5416 [0xf2b6dc,0xf2b750); x86_64 classInst=317 [0xf39ef0,0xf39f70); x86_64 classInst=5416 [0xf3b3e0,0xf3b450)
// 0x06001128 System.Void .ctor(System.Collections.Generic.IEqualityComparer`1<TKey> comparer) | arm64 classInst=317 [0xf2a320,0xf2a398); arm64 classInst=5416 [0xf2b750,0xf2b7cc); x86_64 classInst=317 [0xf39f70,0xf39ff0); x86_64 classInst=5416 [0xf3b450,0xf3b4d0)
// 0x06001129 System.Void .ctor(System.Int32 capacity) | arm64 classInst=317 [0xf2a398,0xf2a410); arm64 classInst=5416 [0xf2b7cc,0xf2b848); x86_64 classInst=317 [0xf39ff0,0xf3a070); x86_64 classInst=5416 [0xf3b4d0,0xf3b550)
// 0x0600112a System.Void .ctor(System.Int32 capacity, System.Collections.Generic.IEqualityComparer`1<TKey> comparer) | arm64 classInst=317 [0xf2a410,0xf2a498); arm64 classInst=5416 [0xf2b848,0xf2b8d4); x86_64 classInst=317 [0xf3a070,0xf3a100); x86_64 classInst=5416 [0xf3b550,0xf3b5e0)
// 0x0600112b TValue get_Item(TKey key) | arm64 classInst=317 [0xf2a498,0xf2a4cc); arm64 classInst=5416 [0xf2b8d4,0xf2ba10); x86_64 classInst=317 [0xf3a100,0xf3a140); x86_64 classInst=5416 [0xf3b5e0,0xf3b6e0)
// 0x0600112c System.Void set_Item(TKey key, TValue value) | arm64 classInst=317 [0xf2a4cc,0xf2a580); arm64 classInst=5416 [0xf2ba10,0xf2bbd4); x86_64 classInst=317 [0xf3a140,0xf3a200); x86_64 classInst=5416 [0xf3b6e0,0xf3b890)
// 0x0600112d System.Object get_Item(System.Object key) | arm64 classInst=317 [0xf2a580,0xf2a62c); arm64 classInst=5416 [0xf2bbd4,0xf2bd10); x86_64 classInst=317 [0xf3a200,0xf3a2b0); x86_64 classInst=5416 [0xf3b890,0xf3b990)
// 0x0600112e System.Void set_Item(System.Object key, System.Object value) | arm64 classInst=317 [0xf2a62c,0xf2a7d0); arm64 classInst=5416 [0xf2bd10,0xf2bef4); x86_64 classInst=317 [0xf3a2b0,0xf3a450); x86_64 classInst=5416 [0xf3b990,0xf3bb50)
// 0x0600112f System.Void Add(TKey key, TValue value) | arm64 classInst=317 [0xf2a7d0,0xf2a884); arm64 classInst=5416 [0xf2bef4,0xf2c0b8); x86_64 classInst=317 [0xf3a450,0xf3a510); x86_64 classInst=5416 [0xf3bb50,0xf3bd00)
// 0x06001130 System.Boolean ContainsKey(TKey key) | arm64 classInst=317 [0xf2a884,0xf2a93c); arm64 classInst=5416 [0xf2c0b8,0xf2c214); x86_64 classInst=317 [0xf3a510,0xf3a5c0); x86_64 classInst=5416 [0xf3bd00,0xf3be50)
// 0x06001131 System.Boolean Remove(TKey key) | arm64 classInst=317 [0xf2a93c,0xf2a9f4); arm64 classInst=5416 [0xf2c214,0xf2c370); x86_64 classInst=317 [0xf3a5c0,0xf3a670); x86_64 classInst=5416 [0xf3be50,0xf3bfa0)
// 0x06001132 System.Boolean TryGetValue(TKey key, out TValue value) | arm64 classInst=317 [0xf2a9f4,0xf2abac); arm64 classInst=5416 [0xf2c370,0xf2c624); x86_64 classInst=317 [0xf3a670,0xf3a820); x86_64 classInst=5416 [0xf3bfa0,0xf3c240)
// 0x06001133 System.Void Add(System.Collections.Generic.KeyValuePair`2<TKey, TValue> kvp) | arm64 classInst=317 [0xf2abac,0xf2ac60); arm64 classInst=5416 [0xf2c624,0xf2c744); x86_64 classInst=317 [0xf3a820,0xf3a8e0); x86_64 classInst=5416 [0xf3c240,0xf3c350)
// 0x06001134 System.Void Add(System.Object key, System.Object value) | arm64 classInst=317 [0xf2ac60,0xf2ac98); arm64 classInst=5416 [0xf2c744,0xf2c77c); x86_64 classInst=317 [0xf3a8e0,0xf3a920); x86_64 classInst=5416 [0xf3c350,0xf3c390)
// 0x06001135 System.Void Clear() | arm64 classInst=317 [0xf2ac98,0xf2ad44); arm64 classInst=5416 [0xf2c77c,0xf2c828); x86_64 classInst=317 [0xf3a920,0xf3a9d0); x86_64 classInst=5416 [0xf3c390,0xf3c440)
// 0x06001136 System.Boolean Contains(System.Collections.Generic.KeyValuePair`2<TKey, TValue> kvp) | arm64 classInst=317 [0xf2ad44,0xf2adf8); arm64 classInst=5416 [0xf2c828,0xf2c94c); x86_64 classInst=317 [0xf3a9d0,0xf3aa90); x86_64 classInst=5416 [0xf3c440,0xf3c550)
// 0x06001137 System.Boolean Contains(System.Object key) | arm64 classInst=317 [0xf2adf8,0xf2ae30); arm64 classInst=5416 [0xf2c94c,0xf2c984); x86_64 classInst=317 [0xf3aa90,0xf3aad0); x86_64 classInst=5416 [0xf3c550,0xf3c590)
// 0x06001138 System.Void CopyTo(System.Collections.Generic.KeyValuePair`2<TKey, TValue>[] array, System.Int32 arrayIndex) | arm64 classInst=317 [0xf2ae30,0xf2aee4); arm64 classInst=5416 [0xf2c984,0xf2ca38); x86_64 classInst=317 [0xf3aad0,0xf3ab90); x86_64 classInst=5416 [0xf3c590,0xf3c650)
// 0x06001139 System.Boolean Remove(System.Collections.Generic.KeyValuePair`2<TKey, TValue> kvp) | arm64 classInst=317 [0xf2aee4,0xf2af98); arm64 classInst=5416 [0xf2ca38,0xf2cb5c); x86_64 classInst=317 [0xf3ab90,0xf3ac50); x86_64 classInst=5416 [0xf3c650,0xf3c760)
// 0x0600113a System.Collections.Generic.IEnumerator`1<System.Collections.Generic.KeyValuePair`2<TKey, TValue>> GetEnumerator() | arm64 classInst=317 [0xf2af98,0xf2b040); arm64 classInst=5416 [0xf2cb5c,0xf2cc04); x86_64 classInst=317 [0xf3ac50,0xf3ad00); x86_64 classInst=5416 [0xf3c760,0xf3c810)
// 0x0600113b System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() | arm64 classInst=317 [0xf2b040,0xf2b0e8); arm64 classInst=5416 [0xf2cc04,0xf2ccac); x86_64 classInst=317 [0xf3ad00,0xf3adb0); x86_64 classInst=5416 [0xf3c810,0xf3c8c0)
// 0x0600113c System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator() | arm64 classInst=317 [0xf2b0e8,0xf2b1f0); arm64 classInst=5416 [0xf2ccac,0xf2cdb4); x86_64 classInst=317 [0xf3adb0,0xf3aeb0); x86_64 classInst=5416 [0xf3c8c0,0xf3c9c0)
// 0x0600113d System.Void Remove(System.Object key) | arm64 classInst=317 [0xf2b1f0,0xf2b228); arm64 classInst=5416 [0xf2cdb4,0xf2cdec); x86_64 classInst=317 [0xf3aeb0,0xf3aef0); x86_64 classInst=5416 [0xf3c9c0,0xf3ca00)
// 0x0600113e System.Void CopyTo(System.Array array, System.Int32 index) | arm64 classInst=317 [0xf2b228,0xf2b260); arm64 classInst=5416 [0xf2cdec,0xf2ce24); x86_64 classInst=317 [0xf3aef0,0xf3af30); x86_64 classInst=5416 [0xf3ca00,0xf3ca40)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Hardlight.Utils
{
    public class DictionaryWithNew<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary where TValue : new()
    {
        private IDictionary<TKey, TValue> dictionary;

        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;
        public int Count => dictionary.Count;
        public bool IsReadOnly => dictionary.IsReadOnly;
        public bool IsFixedSize => false;
        public bool IsSynchronized => false;
        object ICollection.SyncRoot => null;
        ICollection IDictionary.Keys => dictionary.Keys as ICollection;
        ICollection IDictionary.Values => dictionary.Values as ICollection;

        public DictionaryWithNew()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        public DictionaryWithNew(IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public DictionaryWithNew(int capacity)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public DictionaryWithNew(int capacity, IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set
            {
                dictionary[key] = value;
            }
        }

        public object this[object key]
        {
            get
            {
                return this[(TKey)key];
            }
            set
            {
                dictionary[(TKey)key] = (TValue)value;
            }
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
            {
                value = new TValue();
                Add(key, value);
            }
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
            return ((IDictionary)dictionary).GetEnumerator();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
