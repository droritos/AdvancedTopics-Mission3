using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
   

    [Header("Shop Data")]
    //public GameObject ShopMenu;
    private bool _isShopActive;
    [SerializeField] private Animator _shopMenuAnimator;

    [Header("Animators")]
    [SerializeField] private Animator _eraserANIM;

    [Header("Player's Upgrade & Data")]
    [SerializeField] GameObject[] ArrayBulletPrefab;
    [SerializeField] private PlayerWeapon playerWeapon;
    private PlayerHealth _currentHealth;
    public bool isDoubleShot = false;

    [Header("Upgrade Cards")]
    [SerializeField] List<GameObject> LeftUpgradeCard;
    [SerializeField] List<GameObject> RightUpgradeCard;
    [SerializeField] List<GameObject> MiddleUpgradeCard;
    
    
    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Hide = Animator.StringToHash("Hide");

    private void Awake()
    {
        _currentHealth = FindAnyObjectByType<PlayerHealth>();
        SetFalseCardsUpgrade();
    }

    private void Start()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnShowUpgradeMenu += PopUpShow;
        }

        AssignButtonListeners(LeftUpgradeCard);
        AssignButtonListeners(MiddleUpgradeCard);
        AssignButtonListeners(RightUpgradeCard);
    }

    private void AssignButtonListeners(List<GameObject> cards)
    {
        foreach (var card in cards)
        {
            if (card == null) continue;
            Button btn = card.GetComponent<Button>();
            if (btn == null) continue;

            btn.onClick.RemoveAllListeners(); // Clear old broken listeners

            if (card.name.Contains("Homing")) btn.onClick.AddListener(FirstButton);
            else if (card.name.Contains("Piercing")) btn.onClick.AddListener(SecondButton);
            else if (card.name.Contains("Double")) btn.onClick.AddListener(ThirdButton);
            else if (card.name.Contains("Damage")) btn.onClick.AddListener(FourthButton);
            else if (card.name.Contains("Shot++")) btn.onClick.AddListener(FifthButton);
            else if (card.name.Contains("Explosive")) btn.onClick.AddListener(SixthButton);
            else if (card.name.Contains("Nuke")) btn.onClick.AddListener(SeventhButton);
            else if (card.name.Contains("Heal")) btn.onClick.AddListener(EighthButton);
        }
    }

    private void OnDestroy()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnShowUpgradeMenu -= PopUpShow;
        }
    }

    private void PopUpShow()
    {
        Debug.Log("PopUpShow Triggered!");
        if (!_isShopActive)
        {
            _isShopActive = true;
            PickRandomUpgradeCards();
            
            if (_shopMenuAnimator != null) _shopMenuAnimator.SetTrigger(Show);
            

            if (GameEventManager.Instance != null)
                GameEventManager.Instance.TriggerGamePaused(true);
        }
    }

    private void PopUpHide()
    {
        Debug.Log("PopUpHide Triggered!");
        if (_isShopActive)
        {
            _isShopActive = false;
            SetFalseCardsUpgrade();
            
            if (_shopMenuAnimator != null) _shopMenuAnimator.SetTrigger(Hide);
            

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
        if (playerWeapon != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            playerWeapon.ChangeBullet(ArrayBulletPrefab[0], isDoubleShot);
            Debug.Log("You Bought HomingShots");
        }
        PopUpHide();
    }

    public void SecondButton() // Piercing Shots
    {
        if (playerWeapon != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            playerWeapon.ChangeBullet(ArrayBulletPrefab[1], isDoubleShot);
        }
        PopUpHide();
    }

    public void ThirdButton() //Double Shots
    {
        if (playerWeapon != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = true;
            playerWeapon.ChangeBullet(ArrayBulletPrefab[2], isDoubleShot);
        }
        PopUpHide();
    }
    public void FourthButton() //Increase Damage
    {
        if (playerWeapon != null)
        {
            playerWeapon.BulletDamage++;
            Debug.Log($"Bullet Damage : {playerWeapon.BulletDamage}");
        }
        PopUpHide();
    }
    public void FifthButton() // Increase shots amount
    {
        if (playerWeapon != null)
        {
            playerWeapon.TotalShots++;
            Debug.Log($"Bullet Shots Amount : {playerWeapon.TotalShots}");
        }
        PopUpHide();
    }
    public void SixthButton() //Explosive Shots
    {
        if (playerWeapon != null && ArrayBulletPrefab != null)
        {
            isDoubleShot = false;
            playerWeapon.ChangeBullet(ArrayBulletPrefab[3], isDoubleShot);
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

        var allEnemies = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude);
        foreach (var enemy in allEnemies)
        {
            if (enemy is IDamageable damageable && !enemy.gameObject.CompareTag("Player"))
            {
                damageable.TakeDamage(9999, null);
            }
        }
        PopUpHide();
    }
    public void EighthButton() // HealthRestore
    {
        _currentHealth.ResetPlayerHealthOnDemand();
        PopUpHide();
    }



}
