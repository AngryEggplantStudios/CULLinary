using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Ending sequence of boss scene
public class SpawnBurger : MonoBehaviour
{
    [SerializeField] public GameObject burgerToSpawn;
    [SerializeField] public GameObject fadingScreen;

    // Start is called before the first frame update
    public void callRainBurger()
    {
        if (PlayerManager.instance != null)
        {
            PlayerManager.SaveDataBoss();
        }
        StartCoroutine("rainBurgers");
    }

    // Update is called once per frame

    private IEnumerator rainBurgers()
    {
        float totalTime = 0;
        while (totalTime < 2.0f)
        {
            yield return new WaitForSeconds(0.01f);
            Vector2 positionToSpawn = Random.insideUnitCircle * 15;
            Vector3 locationToSpawn = new Vector3(positionToSpawn.x, transform.position.y, positionToSpawn.y);
            Instantiate(burgerToSpawn, locationToSpawn, Quaternion.Euler(0, 90, 0));
            totalTime += Time.deltaTime;
        }
        //Load Louise's Screen
        fadingScreen.SetActive(true);
        fadingScreen.GetComponent<Animator>().SetTrigger("isFadingOut");
        yield return new WaitForSeconds(1.0f);
        //PlayerManager.playerData.SetCurrentIndex((int)SceneIndexes.BOSS);
        //PlayerManager.SaveData();
        SceneManager.LoadScene((int)SceneIndexes.FINALE);
    }
}
