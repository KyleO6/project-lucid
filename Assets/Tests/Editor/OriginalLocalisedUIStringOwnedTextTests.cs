using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Hardlight;
using Hardlight.Localisation;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public sealed class OriginalLocalisedUIStringOwnedTextTests
{
    [Test]
    public void InactiveOriginalDiscoveryWritesOwnedUguiTextAndEmptyOverrideWithoutLookup()
    {
        CallbackSnapshot callbacks = CallbackSnapshot.Capture();
        callbacks.RequireNotInvoking();
        GameObject owned = null;
        Exception primary = null;
        List<Exception> cleanup = new List<Exception>();
        try
        {
            owned = new GameObject("Owned localisation uGUI contract", typeof(RectTransform));
            owned.SetActive(false);
            Text text = owned.AddComponent<Text>();
            LocalisedUIString localised = owned.AddComponent<LocalisedUIString>();
            Assert.IsFalse(owned.activeSelf);
            Assert.IsFalse(owned.activeInHierarchy);
            Assert.IsFalse(text.isActiveAndEnabled);
            Assert.IsFalse(localised.isActiveAndEnabled);
            callbacks.RequireUnchanged();
            text.text = "owned initial value";
            localised.FindTextTargets();
            using (SerializedObject observed = new SerializedObject(localised))
            {
                observed.Update();
                Assert.AreSame(text, ObjectReference(observed, "m_uguiTextTarget"));
                Assert.IsNull(ObjectReference(observed, "m_tmpTextTarget"));
                Assert.IsNull(ObjectReference(observed, "m_tmpUguiTextTarget"));
                Assert.IsNull(ObjectReference(observed, "m_tmpRTLTextTarget"));
            }
            localised.UpdateString("Owned override <literal> 42");
            Assert.AreEqual("Owned override <literal> 42", text.text);
            localised.UpdateString(string.Empty);
            Assert.AreEqual(string.Empty, text.text);
            Assert.IsFalse(owned.activeInHierarchy);
            Assert.IsFalse(localised.isActiveAndEnabled);
            callbacks.RequireUnchanged();
        }
        catch (Exception error)
        {
            primary = error;
        }
        finally
        {
            try
            {
                if (owned != null) UnityEngine.Object.DestroyImmediate(owned);
            }
            catch (Exception error)
            {
                cleanup.Add(error);
            }
            try
            {
                callbacks.RequireUnchanged();
            }
            catch (Exception error)
            {
                cleanup.Add(error);
            }
        }
        if (primary != null && cleanup.Count != 0)
        {
            cleanup.Insert(0, primary);
            throw new AggregateException("Owned path and cleanup both failed", cleanup);
        }
        if (primary != null) ExceptionDispatchInfo.Capture(primary).Throw();
        if (cleanup.Count != 0) throw new AggregateException("Owned cleanup failed", cleanup);
        // No null/default override, string lookup, active object, lifecycle call,
        // callback invocation, TMP component, font/material write or rendering.
    }

    private static UnityEngine.Object ObjectReference(SerializedObject observed, string name)
    {
        SerializedProperty property = observed.FindProperty(name);
        Assert.IsNotNull(property);
        Assert.AreEqual(SerializedPropertyType.ObjectReference, property.propertyType);
        return property.objectReferenceValue;
    }

    private sealed class CallbackSnapshot
    {
        private readonly FastAction owner;
        private readonly List<Action> invocation;
        private readonly List<Action> added;
        private readonly List<Action> removed;
        private readonly Action[] invocationValues;
        private readonly Action[] addedValues;
        private readonly Action[] removedValues;
        private readonly bool invoking;

        private CallbackSnapshot(FastAction value)
        {
            owner = value;
            if (value == null) return;
            invocation = value.GetInvocationList();
            added = Read<List<Action>>(value, "m_addedList");
            removed = Read<List<Action>>(value, "m_removedList");
            invoking = Read<bool>(value, "m_invocationActive");
            Assert.IsNotNull(invocation);
            Assert.IsNotNull(added);
            Assert.IsNotNull(removed);
            invocationValues = invocation.ToArray();
            addedValues = added.ToArray();
            removedValues = removed.ToArray();
        }

        public static CallbackSnapshot Capture()
        {
            return new CallbackSnapshot(StringTable.StringsTableHandlers);
        }

        public void RequireNotInvoking()
        {
            Assert.IsFalse(invoking, "Foreign invocation cannot receive an owned removal queue entry");
        }

        public void RequireUnchanged()
        {
            Assert.IsTrue(ReferenceEquals(owner, StringTable.StringsTableHandlers));
            if (owner == null) return;
            Assert.AreEqual(invoking, Read<bool>(owner, "m_invocationActive"));
            RequireList(invocation, owner.GetInvocationList(), invocationValues);
            RequireList(added, Read<List<Action>>(owner, "m_addedList"), addedValues);
            RequireList(removed, Read<List<Action>>(owner, "m_removedList"), removedValues);
        }

        private static T Read<T>(FastAction value, string name)
        {
            FieldInfo field = typeof(FastActionBase<FastAction, Action>).GetField(name,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Assert.IsNotNull(field);
            Assert.AreEqual(typeof(T), field.FieldType);
            return (T)field.GetValue(value);
        }

        private static void RequireList(List<Action> original, List<Action> current, Action[] values)
        {
            Assert.IsTrue(ReferenceEquals(original, current));
            Assert.AreEqual(values.Length, current.Count);
            for (int i = 0; i < values.Length; i++)
                Assert.IsTrue(ReferenceEquals(values[i], current[i]));
        }
    }
}
