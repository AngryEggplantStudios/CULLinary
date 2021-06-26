using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPlayerStamina : MonoBehaviour
{

    [SerializeField] private GameObject staminaBarUI;
    [SerializeField] private GameObject outLine;

    private float maxStamina = 100.0f;
    private float currStamina;
    private float staminaConsumed = 0.1f;
    private WaitForSeconds timeTakenRegen = new WaitForSeconds(0.05f);
    private Coroutine regen;
    private GameObject staminaBar;
    private Image stmBarFull;
    private float coroutineFlash = 0.1f;
    private GameObject flashyOutline;
    private Coroutine regenZero;

    // Start is called before the first frame update
    void Start()
    {
        staminaBar = staminaBarUI;
        currStamina = maxStamina;
        flashyOutline = outLine;
        stmBarFull = staminaBar.transform.Find("StaminaBar")?.gameObject.GetComponent<Image>();
        flashyOutline.gameObject.SetActive(false);
        stmBarFull.fillAmount = currStamina / maxStamina;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Query if enough stamina to perform sprint
    public bool hasStamina()
    {

        resetStaminaRegen();
        if (currStamina - staminaConsumed < 0.0f)
        {
            if (regenZero == null)
            {
                regenZero = StartCoroutine(flashBar());
            }
            return false;
        }
        else
        {
            StopCoroutine(flashBar());
            flashyOutline.gameObject.SetActive(false);
            regenZero = null;
            return true;
        }
    }

    //Will only be called when hasEnoughStamina
    public void useStamina()
    {
        currStamina = currStamina - staminaConsumed;
        stmBarFull.fillAmount = currStamina / maxStamina;
        resetStaminaRegen();
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

    private void resetStaminaRegen()
    {
        if (regen != null)
        {
            StopCoroutine(regen);
        }

        regen = StartCoroutine(RegenStamina());
    }

    private IEnumerator flashBar()
    {
        bool isFlashing = false;
        while (true)
        {
            if (currStamina - staminaConsumed >= 0)
            {
                StopCoroutine(flashBar());
                flashyOutline.gameObject.SetActive(false);
                yield break;
            }
            // Alternate between 0 and 1 scale to simulate flashing
            if (isFlashing)
            {
                flashyOutline.gameObject.SetActive(true);
            }
            else
            {
                flashyOutline.gameObject.SetActive(false);
            }
            isFlashing = !isFlashing;
            yield return new WaitForSeconds(coroutineFlash);
        }
    }
}
