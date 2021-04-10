using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CongratsStats : MonoBehaviour
{

    [SerializeField] private Text totalGameTime;
    [SerializeField] private Text bossTime;
    [SerializeField] private Text maxHealth;
    [SerializeField] private Text amountEarned;
    [SerializeField] private Text meleeDmg;
    [SerializeField] private Text rangeDmg;
    [SerializeField] private Text wrongCust;
    [SerializeField] private Text rightCust;
    [SerializeField] private Text mobCulled;
    [SerializeField] private Text grade;

    private void OnEnable()
    {
        if (PlayerManager.instance != null)
        {
            PlayerData pd = SaveSystem.LoadData();
            totalGameTime.text = ProcessTime(pd.GetGameTime());
            bossTime.text = ProcessTime(pd.GetBossTime());
            maxHealth.text = pd.GetMaxHealth() + " HP";
            amountEarned.text = "$" + pd.GetMoney();
            rangeDmg.text = pd.GetRangeDamage() + " dmg";
            meleeDmg.text = pd.GetMeleeDamage() + " dmg";
            wrongCust.text = pd.GetWrongCustomersServed().ToString();
            rightCust.text = pd.GetRightCustomersServed().ToString();
            mobCulled.text = pd.GetNoOfMobsCulled().ToString();
            grade.text = ProcessGrade(pd);
        }
    }

    private string ProcessTime(float time)
    {
        int hrs;
        int mins;
        int secs;
        hrs = (int)Mathf.Floor(time / (60*60));
        time -= hrs * (60*60);
        mins = (int)Mathf.Floor(time / 60);
        time -= mins * 60;
        secs = (int)Mathf.Floor(time);
        return hrs + "h " + mins + "m " + secs + "s";
    }

    private string ProcessGrade(PlayerData pd)
    {
        float bossTime = pd.GetBossTime();
        float numServedCorrectly;
        int total = pd.GetRightCustomersServed() + pd.GetWrongCustomersServed();
        if (total == 0)
        {
            numServedCorrectly = 1f;
        }
        else
        {
            numServedCorrectly = pd.GetRightCustomersServed() / total;
        }
        if (bossTime < 30f && numServedCorrectly > 0.9f)
        {
            return "A+";
        }
        else if (bossTime < 60f && numServedCorrectly > 0.7f)
        {
            return "A";
        }
        else if (bossTime < 120f || numServedCorrectly > 0.5f)
        {
            return "B (Maybe you should S/U)";
        }
        return "C (It's time to S/U!)";
    }

}
