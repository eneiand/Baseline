﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Baseline.ObjectCreation
{
    //contains the information about how an ObjectInstance is created
    public class ObjectCreationData
    {
       
        private List<ObjectInstance> m_Arguments;

        public ObjectCreationData(ConstructorInfo constructorInfo, IEnumerable<ObjectInstance> arguments = null)
        {
            if (constructorInfo == null) throw new ArgumentNullException("constructorInfo");

            Constructor = constructorInfo;

            if (arguments != null){
                m_Arguments = new List<ObjectInstance>(arguments);
            }
            else{
                m_Arguments = new List<ObjectInstance>();
            }

            var parameters = constructorInfo.GetParameters();
            
            for (int i = 0; i < parameters.Length ; ++i)
            {
                var p = parameters[i];
                if(! (p.ParameterType.IsAssignableFrom(m_Arguments[i].Instance.GetType())))
                {
                    throw new ArgumentException("argument types ("+ p.ParameterType.ToString() +" " + m_Arguments[i].GetType().ToString()+ ") do not match the constructor parameter types");
                }
            }


        }

        public ConstructorInfo Constructor
        {
            get;
            private set;
        }

        public List<ObjectInstance> Arguments
        {
            get
            {
                return this.m_Arguments;
            }
        }

        public bool HasArguments
        {
            get{
                return Arguments.Count > 0;
            }
        }

        public override string ToString()
        {
            var s = new StringBuilder(Constructor.ToString() + " ");

            this.Arguments.ForEach(a => { s.Append(a); s.Append(" "); });

            return s.ToString();

        }
    }
}
