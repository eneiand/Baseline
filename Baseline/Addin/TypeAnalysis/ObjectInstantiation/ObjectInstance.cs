﻿using System;

namespace Baseline.TypeAnalysis.ObjectInstantiation
{
    public interface IObjectInstance
    {
        bool InstanceNeedsConstructor { get; }
        Object Instance { get; }
        ObjectCreationData CreationData { get; }
        string ToString();
    }

    public class ObjectInstance : IObjectInstance
    {
        public ObjectInstance(Object instance, ObjectCreationData objectCreationData = null)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (!NeedsConstructor(instance.GetType()) && objectCreationData != null) 
                throw new ArgumentException("primitive object does not need creationData", "instance");
            if (NeedsConstructor(instance.GetType()) && objectCreationData == null)
                throw new ArgumentException("instance requires objectCreationData", "instance");

            Instance = instance;
            CreationData = objectCreationData;

            if (CreationData != null)
                if (CreationData.Constructor.ReflectedType != Instance.GetType())
                    throw new ArgumentException("objCreationData.Constructor does not match instance");
        }

        public static bool NeedsConstructor(Type type)
        {
            return !(type.IsPrimitive || type == typeof(Decimal) 
                || type ==typeof(String) || type.IsEnum);
        }

        public bool InstanceNeedsConstructor
        {
            get { return NeedsConstructor(Instance.GetType()); }
        }

        public Object Instance
        {
            get;
            private set;
        }
        public ObjectCreationData CreationData
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Instance.ToString();
        }
    }

    public class NullObjectInstance : IObjectInstance
    {
        public bool InstanceNeedsConstructor
        {
            get { return false; }
        }

        public object Instance
        {
            get { return null; }
        }

        public ObjectCreationData CreationData
        {
            get { return null; }
        }
    }
}
