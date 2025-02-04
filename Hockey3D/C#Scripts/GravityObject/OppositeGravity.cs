using UnityEngine;

// 重心と逆方向の重力空間
public class OppositeGravity : GravityObject
{
    public override Vector3 GetGravityDirection(Vector3 characterPosition)
    {
        return (characterPosition - transform.position).normalized;
    }
}