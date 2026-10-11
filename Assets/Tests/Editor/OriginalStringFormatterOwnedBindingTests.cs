using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Hardlight.UI.Binding;
using Hardlight.UI.Binding.Nodes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public sealed class OriginalStringFormatterOwnedBindingTests
{
    [Test]
    public void InactiveOriginalFormatterReadsSerializedOutputReferencesAndPublicRetarget()
    {
        Assert.IsFalse(Application.isPlaying, "Owned case requires the genuine Editor path");
        List<GameObject> owned = new List<GameObject>();
        Exception primary = null;
        List<Exception> cleanup = new List<Exception>();
        try
        {
            StringFormatter formatSource = CreateInactive(owned, "Owned format source");
            StringFormatter firstSource = CreateInactive(owned, "Owned first argument source");
            StringFormatter secondSource = CreateInactive(owned, "Owned second argument source");
            StringFormatter target = CreateInactive(owned, "Owned reward formatter");
            formatSource.Output.Value = "Rewards {0} / {1}";
            firstSource.Output.Value = "5";
            secondSource.Output.Value = "12";

            using (SerializedObject setup = new SerializedObject(target))
            {
                setup.Update();
                SetReference(setup, "String", formatSource);
                SetReference(setup, "Arg1", firstSource);
                SetReference(setup, "Arg2", firstSource);
                Assert.IsTrue(setup.ApplyModifiedPropertiesWithoutUndo());
                setup.Update();
                RequireReference(setup, "String", formatSource);
                RequireReference(setup, "Arg1", firstSource);
                RequireReference(setup, "Arg2", firstSource);
                RequireUnset(setup, "Arg3");
                RequireUnset(setup, "Arg4");
            }

            Assert.AreSame(formatSource, target.String.Target);
            Assert.AreEqual("Output", target.String.MemberName);
            Assert.AreEqual(0, target.String.Key);
            Assert.IsTrue(target.String.IsSet);
            Assert.IsTrue(target.Arg1.IsSet);
            Assert.IsTrue(target.Arg2.IsSet);
            Assert.IsFalse(target.Arg3.IsSet);
            Assert.IsFalse(target.Arg4.IsSet);
            Assert.IsNull(target.Arg3.GetValue());
            Assert.IsNull(target.Arg4.GetValue());
            Assert.AreEqual(typeof(string), target.String.ValueType);
            Assert.AreEqual("Output", target.String.Member.Name);
            Assert.AreEqual("Rewards {0} / {1}", target.String.GetValue<string>());
            Assert.AreEqual("5", target.Arg1.GetValue());
            target.Refresh();
            Assert.AreEqual("Rewards 5 / 5", target.Output.Value);

            target.Arg2.SetTarget(secondSource, "Output", 0);
            Assert.AreSame(secondSource, target.Arg2.Target);
            Assert.AreEqual("Output", target.Arg2.MemberName);
            Assert.AreEqual(0, target.Arg2.Key);
            Assert.AreEqual("12", target.Arg2.GetValue());
            target.Refresh();
            Assert.AreEqual("Rewards 5 / 12", target.Output.Value);
            using (SerializedObject observed = new SerializedObject(target))
            {
                observed.Update();
                RequireReference(observed, "String", formatSource);
                RequireReference(observed, "Arg1", firstSource);
                RequireReference(observed, "Arg2", secondSource);
                RequireUnset(observed, "Arg3");
                RequireUnset(observed, "Arg4");
            }
            Assert.AreEqual("Rewards {0} / {1}", formatSource.Output.Value);
            Assert.AreEqual("5", firstSource.Output.Value);
            Assert.AreEqual("12", secondSource.Output.Value);
            foreach (GameObject value in owned)
            {
                Assert.IsFalse(value.activeSelf);
                Assert.IsFalse(value.activeInHierarchy);
            }
            Assert.IsFalse(Application.isPlaying);
        }
        catch (Exception error)
        {
            primary = error;
        }
        finally
        {
            for (int i = owned.Count - 1; i >= 0; --i)
            {
                try
                {
                    if (owned[i] != null) UnityEngine.Object.DestroyImmediate(owned[i]);
                }
                catch (Exception error)
                {
                    cleanup.Add(error);
                }
            }
        }
        if (primary != null && cleanup.Count != 0)
        {
            cleanup.Insert(0, primary);
            throw new AggregateException("Owned binding path and cleanup both failed", cleanup);
        }
        if (primary != null) ExceptionDispatchInfo.Capture(primary).Throw();
        if (cleanup.Count != 0) throw new AggregateException("Owned binding cleanup failed", cleanup);
        // Genuine lookup caches may retain type/member metadata. No cache reset,
        // foreign object cleanup, listener registration or lifecycle invocation.
    }

    private static StringFormatter CreateInactive(List<GameObject> owned, string name)
    {
        GameObject value = new GameObject(name);
        owned.Add(value);
        value.SetActive(false);
        StringFormatter formatter = value.AddComponent<StringFormatter>();
        Assert.IsNotNull(formatter);
        Assert.IsFalse(formatter.isActiveAndEnabled);
        Assert.IsFalse(value.activeInHierarchy);
        Assert.IsNotNull(formatter.Output);
        return formatter;
    }

    private static SerializedProperty Child(SerializedObject observed, string name,
        string childName, SerializedPropertyType expected)
    {
        SerializedProperty reference = observed.FindProperty(name);
        Assert.IsNotNull(reference);
        Assert.AreEqual(SerializedPropertyType.Generic, reference.propertyType);
        SerializedProperty child = reference.FindPropertyRelative(childName);
        Assert.IsNotNull(child);
        Assert.AreEqual(expected, child.propertyType);
        return child;
    }

    private static void SetReference(SerializedObject observed, string name, StringFormatter source)
    {
        Child(observed, name, "target", SerializedPropertyType.ObjectReference).objectReferenceValue = source;
        Child(observed, name, "memberName", SerializedPropertyType.String).stringValue = "Output";
        Child(observed, name, "key", SerializedPropertyType.Integer).intValue = 0;
    }

    private static void RequireReference(SerializedObject observed, string name, StringFormatter source)
    {
        Assert.AreSame(source, Child(observed, name, "target", SerializedPropertyType.ObjectReference).objectReferenceValue);
        Assert.AreEqual("Output", Child(observed, name, "memberName", SerializedPropertyType.String).stringValue);
        Assert.AreEqual(0, Child(observed, name, "key", SerializedPropertyType.Integer).intValue);
    }

    private static void RequireUnset(SerializedObject observed, string name)
    {
        SerializedProperty target = Child(observed, name, "target", SerializedPropertyType.ObjectReference);
        Assert.IsNull(target.objectReferenceValue);
        Assert.AreEqual(0, target.objectReferenceInstanceIDValue);
        Assert.AreEqual(string.Empty, Child(observed, name, "memberName", SerializedPropertyType.String).stringValue);
        Assert.AreEqual(0, Child(observed, name, "key", SerializedPropertyType.Integer).intValue);
    }
}
