using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBurger : MonoBehaviour
{
    [SerializeField] public GameObject burgerToSpawn;

    // Start is called before the first frame update
    public void callRainBurger()
    {
        StartCoroutine("rainBurgers");
    }

    // Update is called once per frame

    private IEnumerator rainBurgers()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 positionToSpawn = Random.insideUnitCircle * 15;
            Vector3 locationToSpawn = new Vector3(positionToSpawn.x, transform.position.y, positionToSpawn.y);
            Instantiate(burgerToSpawn, locationToSpawn, Quaternion.Euler(0, 90, 0));
        }

    }
}
