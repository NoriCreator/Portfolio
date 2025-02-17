using UnityEngine;

public abstract class GravityObject : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    public int priority; // 重力空間重複時の優先度
    private Collider col;

    public abstract Vector3 GetGravityDirection(Vector3 playPosition);

    void Awake() 
    {
        col = this.GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            var characterGravity = other.GetComponent<CharacterGravity>();
            if (characterGravity != null)
            {
                characterGravity.AddGravity(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            var characterGravity = other.GetComponent<CharacterGravity>();
            if (characterGravity != null)
            {
                characterGravity.RemoveGravity(this);
            }
        }
    }
}
