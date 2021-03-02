using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Vector3 projDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    public void Setup(Vector3 projDir, Vector3 lookVector)
    {
        this.projDir = projDir;
        transform.eulerAngles = new Vector3(0, 0, CalculateAngle(lookVector));
        Destroy(gameObject, 5.0f);
    }

    private float CalculateAngle(Vector3 v)
    {
        v = v.normalized;
        float n = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    private void Update()
    {
        transform.position += projDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        EnemyScript target = collider.GetComponent<EnemyScript>();
        if (target != null)
        {
            target.HandleHit(damage);
            Destroy(gameObject);
            Debug.Log("Hit!");
        }
    }


}
