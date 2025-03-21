﻿using System;

namespace RomeEngine
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class SerializeFieldAttribute : Attribute
	{
        public bool HideInInspector { get; set; } = false;
    }
}
