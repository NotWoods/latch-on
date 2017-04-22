public class EgoSystem
{
#if UNITY_EDITOR
    public bool enabled = true;
#endif

    protected BitMask _mask = new BitMask( ComponentIDs.GetCount() );

    public EgoSystem() { }

    protected bool CanUpdate(EgoComponent entity) {
        return Ego.CanUpdate( _mask, entity.mask );
    }

    public virtual void CreateBundles(EgoComponent[] egoComponents) { }

    public virtual void Start() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void LateUpdate() { }
}
