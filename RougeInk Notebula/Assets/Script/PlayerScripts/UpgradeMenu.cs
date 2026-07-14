using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [Header("Shop Data")]
    public GameObject ShopMenu;
    private bool _isShopActive;
    [SerializeField] private Animator _shopMenuAnimator;
    private CanvasGroup _canvasGroup;

    [Header("Animators")]
    [SerializeField] private Animator _eraserANIM;

    [Header("Player's Upgrade & Data")]
    [SerializeField] GameObject[] ArrayBulletPrefab;
    [SerializeField] private BulletFire bulletFire;
    private PlayerHealth _currentHealth;
    public bool isDoubleShot = false;

    [Header("Upgrade Cards")]
    [SerializeField] List<GameObject> LeftUpgradeCard;
    [SerializeField] List<GameObject> RightUpgradeCard;
    [SerializeField] List<GameObject> MiddleUpgradeCard;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _currentHealth = FindAnyObjectByType<PlayerHealth>();
        SetFalseCardsUpgrade();
    }

    private void Start()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnShowUpgradeMenu += PopUpShow;
        }
    }

    private void OnDestroy()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnShowUpgradeMenu -= PopUpShow;
        }
    }
    public void PopUpShow()
    {
        Debug.Log("PopUpShow Triggered!");
        if (!_isShopActive)
        {
            _isShopActive = true;
            PickRandomUpgradeCards();
            
            if (_shopMenuAnimator != null) _shopMenuAnimator.SetTrigger("Show");
            
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }

            if (GameEventManager.Instance != null)
                GameEventManager.Instance.TriggerGamePaused(true);
        }
    }
    public void PopUpHide()
    {
        Debug.Log("PopUpHide Triggered!");
        if (_isShopActive)
        {
            _isShopActive = false;
            SetFalseCardsUpgrade();
            
            if (_shopMenuAnimator != null) _shopMenuAnimator.SetTrigger("Hide");
            
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }

            if (GameEventManager.Instance != null)
                GameEventManager.Instance.TriggerGamePaused(false);
        }
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
            bulletFire.ChangeBullet(ArrayBulletPrefab[0], isDoubleShot);
            Debug.Log("You Bought HomingShots");
        }
        PopUpHide();
    }

    public void SecondButton() // Piercing Shots
    {
        if (bulletFire != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            bulletFire.ChangeBullet(ArrayBulletPrefab[1], isDoubleShot);
        }
        PopUpHide();
    }

    public void ThirdButton() //Double Shots
    {
        if (bulletFire != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = true;
            bulletFire.ChangeBullet(ArrayBulletPrefab[2], isDoubleShot);
        }
        PopUpHide();
    }
    public void FourthButton() //Increase Damage
    {
        if (bulletFire != null)
            bulletFire.BulletDamage++;
        Debug.Log($"Bullet Damage : {bulletFire.BulletDamage}");
        PopUpHide();

    }
    public void FifthButton() // Increase shots amount
    {
        if (bulletFire != null)
            bulletFire.TotalShots++;
        Debug.Log($"Bullet Shots Amount : {bulletFire.TotalShots}");
        PopUpHide();
    }
    public void SixthButton() //Explosive Shots
    {
        if (bulletFire != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            bulletFire.ChangeBullet(ArrayBulletPrefab[3], isDoubleShot);
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
