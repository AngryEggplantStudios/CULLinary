using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 projDir;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float heightFromGround;

    private float spinSpeed = -1500;
    private int damage;
    public void Setup(Vector3 sourcePosition, Vector3 targetPosition, int damage=20)
    {
        this.damage = damage;
        if (Random.Range(0, 100) < PlayerManager.playerData.critRate)
        {
            this.damage *= 2;
        }
        this.projDir = (targetPosition - sourcePosition).normalized;
        transform.position += new Vector3(0, heightFromGround, 0);
        transform.eulerAngles = new Vector3(0, CalculateAngle(projDir), 90);
        Destroy(gameObject, 5);
    }

    private float CalculateAngle(Vector3 v)
    {
        v = v.normalized;
        float n = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    private float CalculateDuration(Vector3 sourcePosition, Vector3 targetPosition)
    {
        return Vector3.Distance(sourcePosition, targetPosition) / moveSpeed;
    }

    private void Update()
    {
        transform.position += projDir * moveSpeed * Time.deltaTime;
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Enemy target = collider.GetComponent<Enemy>();
        if (target != null)
        {
            target.HandleHit(damage);
            Destroy(gameObject);
            Debug.Log("Hit!");
        }
    }


}
