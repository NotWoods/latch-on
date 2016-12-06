﻿using UnityEngine;

public class EgoBundle<C1, C2> : EgoBundle
    where C1 : Component
    where C2 : Component
{
    public readonly EgoComponent egoComponent;
    public readonly Transform transform;
    public readonly C1 component1;
    public readonly C2 component2;

    public EgoBundle( EgoComponent egoComponent, C1 component1, C2 component2 )
    {
        this.egoComponent = egoComponent;
        this.component1 = component1;
        this.component2 = component2;
    }
}
