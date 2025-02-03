using UnityEngine;

// 重心に向かう重力空間
public class CenterGravity : GravityObject
{
    public override Vector3 GetGravityDirection(Vector3 characterPosition)
    {
        return (transform.position - characterPosition).normalized;
    }
}