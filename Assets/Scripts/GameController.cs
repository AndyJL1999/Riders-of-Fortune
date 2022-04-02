using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameObject player;

    public static int diceSideThrown = 0;
    public static int startPoint = 0;
    public static int damage = 0;
    public static bool gameOver = false;
    public static bool onBoard = true;

    private List<int> areas = new List<int> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2 };
    private System.Random rand = new System.Random();

    public GameObject[] scenes;
    public Sprite[] weapons;
    public Image weaponIcon;
    public Button dismountButton;
    public Button attackButton;
    public Text buttonText;
    public Text monsterHP;
    public Text damageText;
    public Text weaponName;
    public Text weaponDmgText;

    int currentIndex = 0; //used to keep track of scene
    int weaponDmg = 0;
    int monsterHealth = 0;

    void Start()
    {
        player = GameObject.Find("PositionPointer");
        player.GetComponent<MoveOnBoard>().moveAllowed = false;
        dismountButton.interactable = false;
    }

    
    void Update()
    {
        if(player.GetComponent<MoveOnBoard>().waypointIndex > startPoint + diceSideThrown)
        {
            player.GetComponent<MoveOnBoard>().moveAllowed = false;
            startPoint = player.GetComponent<MoveOnBoard>().waypointIndex;

            choosingArea();
            dismountButton.interactable = true;
        }

        if (player.GetComponent<MoveOnBoard>().waypointIndex == player.GetComponent<MoveOnBoard>().waypoints.Length)
        {
            currentIndex = 3;
            dismountButton.interactable = true;
            gameOver = true;
        }

        monsterHP.text = "Monster HP: " + monsterHealth;
    }

    public static void MovePlayer()
    {
        player.GetComponent<MoveOnBoard>().moveAllowed = true;
    }

    void choosingArea()
    {
        int shuffleNum = rand.Next(0, areas.Count);

        switch (areas[shuffleNum])
        {
            case 0://Monster Scene
                areas.Remove(areas[shuffleNum]);
                currentIndex = 0;
                Debug.Log("Its 0");
                break;
            case 1://Weapon Scene
                areas.Remove(areas[shuffleNum]);
                currentIndex = 1;
                Debug.Log("Its 1");
                break;
            case 2://Empty Scene
                areas.Remove(areas[shuffleNum]);
                currentIndex = 2;
                Debug.Log("Its 2");
                break;
        }
    }

    void AdjustMonsterHP()
    {
        if (startPoint < 4)
        {
            monsterHealth = 3;
        }
        else if (startPoint < 9)
        {
            monsterHealth = 4;
        }
        else if (startPoint < 14)
        {
            monsterHealth = 5;
        }
        else if (startPoint < 19)
        {
            monsterHealth = 6;
        }
        else if (startPoint < 24)
        {
            monsterHealth = 7;
        }
    }

    public void CheckWeapon()//Decides the weapon given
    {
        if (startPoint < 4)
        {
            weaponIcon.sprite = weapons[0];
            weaponDmg = 3;
            weaponName.text = "Crossbow";
            weaponDmgText.text = "Damage: +" + weaponDmg;
        }
        else if(startPoint < 9)
        {
            weaponIcon.sprite = weapons[1];
            weaponDmg = 4;
            weaponName.text = "Flail";
            weaponDmgText.text = "Damage: +" + weaponDmg;
        }
        else if(startPoint < 14)
        {
            weaponIcon.sprite = weapons[2];
            weaponDmg = 5;
            weaponName.text = "Broad Sword";
            weaponDmgText.text = "Damage: +" + weaponDmg;
        }
        else if(startPoint < 19)
        {
            weaponIcon.sprite = weapons[3];
            weaponDmg = 6;
            weaponName.text = "Dragon Slayer";
            weaponDmgText.text = "Damage: +" + weaponDmg;
        }
        else if(startPoint < 24)
        {
            weaponIcon.sprite = weapons[4];
            weaponDmg = 7;
            weaponName.text = "Spell of the Gods";
            weaponDmgText.text = "Damage: +" + weaponDmg;
        }

    }

    public void Dismount()
    {
        if (scenes[currentIndex].activeSelf == false)
        {
            scenes[currentIndex].SetActive(true);
            AdjustMonsterHP();
            onBoard = false;
            buttonText.text = "Ride";
        }
        else
        {
            scenes[currentIndex].SetActive(false);
            onBoard = true;
            buttonText.text = "Dismount";
            dismountButton.interactable = false;
            attackButton.interactable = true;  //reactivate the attack button for the next battle that may occur
        }
    }

    public void Attack()
    {
        damage += weaponDmg;
        damageText.text = "Total Damage: " + damage;

        if (damage > monsterHealth)
            monsterHealth = 0;
        else
            monsterHealth -= damage;

        attackButton.interactable = false; //Make it so the player can only attack once
    }
}
