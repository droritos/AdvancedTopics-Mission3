using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [Header("Shop Data")]
    public GameObject ShopMenu;
    private bool _isShopActive;

    [Header("Animators")]
    [SerializeField] private Animator _eraserANIM;

    [Header("Player's Upgrade & Data")]
    [SerializeField] GameObject[] ArrayBulletPrefab;
    private BulletFire bulletFire;
    private PlayerHealth _currentHealth;
    public bool isDoubleShot = false;

    [Header("Upgrade Cards")]
    [SerializeField] List<GameObject> LeftUpgradeCard;
    [SerializeField] List<GameObject> RightUpgradeCard;
    [SerializeField] List<GameObject> MiddleUpgradeCard;


    public static UpgradeMenu Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        bulletFire = GetComponent<BulletFire>();
        _currentHealth = FindObjectOfType<PlayerHealth>();
        
        SetFalseCardsUpgrade();
    }
    public void PopUpShow()
    {
        if (!_isShopActive)
        {
            _isShopActive = true;
            PickRandomUpgradeCards();
            ShopMenu.GetComponent<Animator>().SetTrigger("Show");
        }
    }
    public void PopUpHide()
    {
        if (_isShopActive)
        {
            _isShopActive = false;
            SetFalseCardsUpgrade();
            ShopMenu.GetComponent<Animator>().SetTrigger("Hide");
            Time.timeScale = 1f;
        }
    }

    private IEnumerator StopTimeWhenUpgrade(float sec)
    {
        yield return new WaitForSeconds(sec);
        Time.timeScale = 0f;
    }

    private void PickRandomUpgradeCards()
    {
        SetFalseCardsUpgrade();
        EnableCardsFromList(LeftUpgradeCard);
        DifferentCards(RightUpgradeCard, MiddleUpgradeCard);
    }

    private void SetFalseCardsUpgrade()
    {
        if (!_isShopActive)
        {
            foreach (var upgradeCard in LeftUpgradeCard)
            {
                upgradeCard.SetActive(false);
            }
            foreach (var upgradeCard in MiddleUpgradeCard)
            {
                upgradeCard.SetActive(false);
            }
            foreach (var upgradeCard in RightUpgradeCard)
            {
                upgradeCard.SetActive(false);
            }
        }
    }
    private void DifferentCards(List<GameObject> rightList , List<GameObject> middleList)
    {
        int indexRight = EnableCardsFromList(rightList);
        int indexMiddle = EnableCardsFromList(middleList);
        while (rightList[indexRight].name == middleList[indexMiddle].name)
        {
            indexMiddle = EnableCardsFromList(middleList);
        }
    }
    private int EnableCardsFromList(List<GameObject> enableRandomUpgrade)
    {
        if (enableRandomUpgrade.Count <= 0)
        {
            Debug.LogError("No More Upgrades To Handle");
            return -1;
        }

        int randomCard = Random.Range(0, enableRandomUpgrade.Count);
        enableRandomUpgrade[randomCard].SetActive(true);
        return randomCard;
    }

    public void FirstButton() // HomingShots
    {
        if (bulletFire != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            bulletFire.ChangeBullet(ArrayBulletPrefab[0]);
            Debug.Log("You Bought HomingShots");
        }
        PopUpHide();
    }

    public void SecondButton() // Piercing Shots
    {
        if (bulletFire != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            bulletFire.ChangeBullet(ArrayBulletPrefab[1]);
        }
        PopUpHide();
    }

    public void ThirdButton() //Double Shots
    {
        if (bulletFire != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = true;
            bulletFire.ChangeBullet(ArrayBulletPrefab[2]);
        }
        PopUpHide();
    }
    public void FourthButton() //Increase Damage
    {
        if (bulletFire != null)
            bulletFire.bulletDamage++;
        Debug.Log($"Bullet Damage : {bulletFire.bulletDamage}");
        PopUpHide();

    }
    public void FifthButton() // Increase shots amount
    {
        if (bulletFire != null)
            bulletFire._totalShots++;
        Debug.Log($"Bullet Shots Amount : {bulletFire._totalShots}");
        PopUpHide();
    }
    public void SixthButton() //Explosive Shots
    {
        if (bulletFire != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            bulletFire.ChangeBullet(ArrayBulletPrefab[3]);
        }
        PopUpHide();
    }

    public void SeventhButton() //Nuke them all
    {
        Debug.Log("Erase all enemies");
        if (_eraserANIM != null)
            _eraserANIM.SetTrigger("IsErasering");
        else
            Debug.LogError("Animator not found on the GameObject.");

        GameObject enemies = GameObject.Find("EnemiesContainer");
        for (int i = enemies.transform.childCount - 1; i >= 0; i--)
            Destroy(enemies.transform.GetChild(i).gameObject);
        PopUpHide();
    }
    public void EighthButton() // HealthRestore
    {
        _currentHealth.ResetPlayerHealthOnDemand();
        PopUpHide();
    }



}
