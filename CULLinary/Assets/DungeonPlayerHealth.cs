using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPlayerHealth : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject hpBarUI;
    [SerializeField] private float invincibilityDurationSeconds;
    [SerializeField] private GameObject model;
    private GameObject hpBar;
    private Image hpBarFull;
    private Text hpText;
    private bool isInvincible;
    private float invincibilityDeltaTime = 0.025f;

    private Renderer rend;
    private Color[] originalColors;
    private Color onDamageColor = Color.white;
    DungeonPlayerLocomotion dpl;

    private float health;

    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
        health = maxHealth;
        hpBar = hpBarUI;
        hpBarFull = hpBar.transform.Find("HpBar").gameObject.GetComponent<Image>();
        hpText = hpBar.transform.Find("HpText").gameObject.GetComponent<Text>();
        hpBarFull.fillAmount = health / maxHealth;
        hpText.text = health + "/" + maxHealth;
        SetupFlash();
        dpl = this.gameObject.GetComponent<DungeonPlayerLocomotion>();
    }

    private void SetupFlash()
    {
        rend = model.GetComponentInChildren<Renderer>();
        originalColors = new Color[rend.materials.Length];
        for (var i = 0; i < rend.materials.Length; i++)
        {
            originalColors[i] = rend.materials[i].color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //bool value is for if successfully hit the player, so can knockback.
    public bool HandleHit(float damage)
    {
        if (isInvincible) return false;

        this.health -= damage;
        hpBarFull.fillAmount = health / maxHealth;
        hpText.text = health + "/" + maxHealth;

        if (this.health <= 0)
        {
           //Die();
        }
        StartCoroutine(BecomeTemporarilyInvincible());
        return true;
    }

    public void KnockbackPlayer(Vector3 positionOfEnemy)
    {
        //ToImplementKnockback
        //StartCoroutine(KnockCoroutine(positionOfEnemy));
        Vector3 forceDirection = transform.position - positionOfEnemy;
        Vector3 force = forceDirection.normalized;
        dpl.KnockBack(force, 50, 3, true);
    }

    private IEnumerator KnockCoroutine(Vector3 positionOfEnemy)
    {

        Vector3 forceDirection = transform.position - positionOfEnemy;
        Vector3 force = forceDirection.normalized;
        gameObject.GetComponent<Rigidbody>().velocity = force * 4;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        bool isFlashing = false;
        for (float i = 0; i < invincibilityDurationSeconds; i += invincibilityDeltaTime)
        {
            // Alternate between 0 and 1 scale to simulate flashing
            if (isFlashing)
            {
                for (var k = 0; k < rend.materials.Length; k++)
                {
                    rend.materials[k].color = onDamageColor;
                }
            }
            else
            {
                for (var k = 0; k < rend.materials.Length; k++)
                {
                    rend.materials[k].color = originalColors[k];
                }
            }
            isFlashing = !isFlashing;
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }
        isInvincible = false;
    }
}
