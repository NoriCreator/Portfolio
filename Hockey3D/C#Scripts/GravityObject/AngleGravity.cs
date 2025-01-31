using UnityEngine;

// エディタ上で重力方向を設定できる重力空間
public class AngleGravity : GravityObject
{
    [SerializeField] private Vector3 gravityDirection;

    public override Vector3 GetGravityDirection(Vector3 characterPosition)
    {
        return gravityDirection.normalized * gravityStrength;
    }

    private void OnDrowGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, gravityDirection.normalized * 2f);
    }
}
