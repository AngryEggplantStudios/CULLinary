using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPlayerStamina : MonoBehaviour
{

    [SerializeField] private GameObject staminaBarUI;

    private float maxStamina = 100.0f;
    private float currStamina;
    private float staminaConsumed = 0.1f;
    private WaitForSeconds timeTakenRegen = new WaitForSeconds(0.05f);
    private Coroutine regen;
    private GameObject staminaBar;
    private Image stmBarFull;

    // Start is called before the first frame update
    void Start()
    {
        staminaBar = staminaBarUI;
        currStamina = maxStamina;
        stmBarFull = staminaBar.transform.Find("StaminaBar")?.gameObject.GetComponent<Image>();
        stmBarFull.fillAmount = currStamina / maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Query if enough stamina to perform sprint
    public bool hasStamina()
    {
        if (currStamina - staminaConsumed < 0.0f)
        {
            return false;
        } else
        {
            return true;
        }
    }

    //Will only be called when hasEnoughStamina
    public void useStamina()
    {
        currStamina = currStamina - staminaConsumed;
        stmBarFull.fillAmount = currStamina / maxStamina;
        if (regen != null)
        {
            StopCoroutine(regen);
        }

        regen = StartCoroutine(RegenStamina());
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1);

        while (currStamina < maxStamina)
        {
            currStamina = currStamina + maxStamina / 100;
            stmBarFull.fillAmount = currStamina / maxStamina;
            yield return timeTakenRegen;
        }
    }
}
