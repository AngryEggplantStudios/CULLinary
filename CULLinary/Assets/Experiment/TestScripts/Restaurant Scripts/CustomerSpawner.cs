using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Transform[] Seats; 
    public int maxCustNum = 5; // Can adjust later as y'all deem fit, MUST be <= Seats.Length 

    public GameObject[] customerPrefabs;

    public int currCustNum;
    bool alrSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        currCustNum = 1;
        StartCoroutine(SpawnCustomers()); // for first customer only
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && (alrSpawn == false))
        {           
            currCustNum++;

            if (currCustNum <= maxCustNum)
            {
                Debug.Log("gonna spawn 1 customer, total now is " + currCustNum + " customers");
                StartCoroutine(SpawnCustomers());
                alrSpawn = true;
            } else
            {
                Debug.Log("Reached max customers!");
            }
                
        }
    }

    IEnumerator SpawnCustomers()
    {       
        int Spawnfreq = Random.Range(5, 15); // Can adjust later as y'all deem fit
        yield return new WaitForSeconds(Spawnfreq); // only can spawn every 5-15 seconds 

        Debug.Log("Can spawn customers now");
        StartCoroutine(InstantiateCustomer());              
    }

    IEnumerator InstantiateCustomer()
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

        yield return new WaitForSeconds(1);

        alrSpawn = false; //rest bool value so can spawn the next customer
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
