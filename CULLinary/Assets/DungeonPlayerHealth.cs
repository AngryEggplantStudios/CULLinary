using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject hpBarUI;
    [SerializeField] private float invincibilityDurationSeconds;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject damageCounter_prefab;
    [SerializeField] private AudioSource audioSource;

    private GameObject hpBar;
    private Image hpBarFull;
    private Text hpText;
    private bool isInvincible;
    private float invincibilityDeltaTime = 0.025f;

    private Renderer rend;
    private Color[] originalColors;
    private Color onDamageColor = Color.white;
    DungeonPlayerLocomotion dpl;
    private Camera cam;

    private float health;

    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
        //health = maxHealth;
        health = PlayerManager.instance.GetMaxHealth();
        maxHealth = health;
        //Debug.Log(PlayerManager.instance.GetMaxHealth());
        hpBar = hpBarUI;
        hpBarFull = hpBar.transform.Find("HpBar")?.gameObject.GetComponent<Image>();
        hpText = hpBar.transform.Find("HpText")?.gameObject.GetComponent<Text>();
        if (hpBarFull) {
            hpBarFull.fillAmount = health / maxHealth;
        }
        if (hpText) {
            hpText.text = health + "/" + maxHealth;
        }
        SetupFlash();
        dpl = this.gameObject.GetComponent<DungeonPlayerLocomotion>();
        cam = transform.GetComponentInChildren<Camera>();
    }

    private void SetupFlash()
    {
        rend = model.GetComponentInChildren<Renderer>();
        if (rend) {
            originalColors = new Color[rend.materials.Length];
            for (var i = 0; i < rend.materials.Length; i++)
            {
                originalColors[i] = rend.materials[i].color;
            }
        }
    }

    //bool value is for if successfully hit the player, so can knockback.
    public bool HandleHit(float damage)
    {
        if (isInvincible) return false;

        this.health -= damage;
        hpBarFull.fillAmount = health / maxHealth;
        hpText.text = health + "/" + maxHealth;
        SpawnDamageCounter(damage);
        audioSource.Play();

        if (this.health <= 0)
        {
            gameOverUI.GetComponent<GameManager>().GameOver();
           //Die();
        }
        StartCoroutine(BecomeTemporarilyInvincible());
        return true;
    }

    private void SpawnDamageCounter(float damage)
    {
        GameObject damageCounter = Instantiate(damageCounter_prefab);
        damageCounter.transform.GetComponentInChildren<Text>().text = damage.ToString();
        damageCounter.transform.SetParent(GameObject.FindObjectOfType<InventoryUI>().transform);
        damageCounter.transform.position = cam.WorldToScreenPoint(transform.position);
    }

    public void KnockbackPlayer(Vector3 positionOfEnemy)
    {
        //ToImplementKnockback
        //StartCoroutine(KnockCoroutine(positionOfEnemy));
        Vector3 forceDirection = transform.position - positionOfEnemy;
        forceDirection.y = 0;
        Vector3 force = forceDirection.normalized;
        //dpl.KnockBack(force, 50, 3, true);
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
