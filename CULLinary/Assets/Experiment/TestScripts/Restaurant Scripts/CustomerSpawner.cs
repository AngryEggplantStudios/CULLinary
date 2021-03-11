using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Transform[] Seats; 
    public int maxCustNum = 5; // Can adjust later as y'all deem fit, MUST be <= Seats.Length
    public int Spawnfreq = 10; // Can adjust later as y'all deem fit

    public GameObject[] customerPrefabs;

    public int currCustNum;
    bool canSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        currCustNum = 0;
        StartCoroutine(SpawnCustomers()); // for first customer only
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("gonna spawn 1 customer");
            StartCoroutine(SpawnCustomers());

            if (canSpawn)
            {
                instantiateCustomer();
                canSpawn = false;
            }
        }

    }

    IEnumerator SpawnCustomers()
    {
        yield return new WaitForSeconds(Spawnfreq); // only can spawn every 5 seconds 

        canSpawn = true;
        Debug.Log("Spawn customers now");
    }

    private void instantiateCustomer()
    {
        if (currCustNum < maxCustNum)
        {
            int randomID = Random.Range(0, customerPrefabs.Length);

            // To find a random seat to instantiate the customer
            bool foundSeat = false; // guaranteed to find a seat as long maxCustNum <= Seats.Length

            while (foundSeat == false)
            {
                int randomSeatIdx = Random.Range(0, Seats.Length);
                bool canSit = GetRandomAvailableSeat(randomSeatIdx);

                if (canSit)  // Can add audio here when customer is instantiated?
                {
                    Transform seatLocation = Seats[randomSeatIdx];
                    GameObject customer = Instantiate(customerPrefabs[randomID], seatLocation.position, Quaternion.identity);
                    customer.transform.SetParent(seatLocation);
                    foundSeat = true;
                }
            }

            currCustNum++;
        }
    }

    public bool GetRandomAvailableSeat(int seatIdx)
    {
        if (Seats[seatIdx].childCount == 0)
        {
            return true;
        }

        return false;
    }
}
