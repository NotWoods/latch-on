﻿using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

public static class ComponentIDs
{
    static readonly Dictionary<Type, int> _types;

    static ComponentIDs()
    {
        // Get all Component Types
        var componentTypes = new List<Type>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach( var assembly in assemblies )
        {
            var types = assembly.GetTypes();
            foreach( var type in types )
            {
                if( type.IsSubclassOf( typeof( Component ) ) )
                {
                    componentTypes.Add( type );
                }
            }
        }

        _types = new Dictionary<Type, int>( componentTypes.Count );
        foreach( var componentType in componentTypes )
        {
            _types.Add( componentType, _types.Count );
        }
    }

    public static int GetCount()
    {
        return _types.Count;
    }

    public static int Get( Type componentType )
    {
        Assert.IsTrue( componentType.IsSubclassOf( typeof( Component ) ), "Only get IDs of Component Types!" );
        return _types[ componentType ];
    }
}
