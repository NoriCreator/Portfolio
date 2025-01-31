using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterGravity : MonoBehaviour
{
    private List<GravityObject> activeGravities = new List<GravityObject>();
    private Vector3 currentGravityDirection;
    private float originalGravity;

    private void Start()
    {
        originalGravity = Physics.gravity.y;
        currentGravityDirection = Vector3.up * originalGravity;
    }

    private void Update()
    {
        if(activeGravities.Count > 0)
        {
            var highestPriorityGravity = activeGravities.OrderByDescending(g => g.priority).First();
            currentGravityDirection = highestPriorityGravity.GetGravityDirection(transform.position);
            Physics.gravity = currentGravityDirection;
        }
        else
        {
            currentGravityDirection = Vector3.up * originalGravity;
            Physics.gravity = currentGravityDirection;
        }
    }

    // AddGravity メソッドの修正
    public void AddGravity(GravityObject gravityObject)  // メソッド名を修正
    {
        activeGravities.Add(gravityObject);
    }

    // RemoveGravity メソッド
    public void RemoveGravity(GravityObject gravityObject)  // 必要なメソッドを追加
    {
        activeGravities.Remove(gravityObject);
    }
}
