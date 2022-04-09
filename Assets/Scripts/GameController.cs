using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script attached to the 'GameController' game object
public class GameController : MonoBehaviour
{
    private static GameObject player;

    public static int diceSideThrown;
    public static int startPoint;
    public static int damage;
    public static bool gameOver;
    public static bool onBoard;
    public static bool inBattle;

    public GameObject[] scenes;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject dragonTextPanel;
    public GameObject enemyHealthPanel;
    public GameObject totalDmgPanel;
    public GameObject dice;

    public Sprite[] weapons;
    public Image weaponIcon;

    public Button dismountButton;
    public Button attackButton;
    public Button searchButton;
    public Button xpButton;

    public Text buttonText;
    public Text enemyHPText;
    public Text damageText;
    public Text weaponName;
    public Text weaponDmgText;
    public Text xpText;
    public Text dragonText;
    public Text receivedWeaponText;

    public DiceControl rollButton;

    private List<int> areas = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2 };
    private List<int> weaponSlots = new List<int> { 0, 1, 2, 3, 4 };
    private System.Random rand = new System.Random();
    private int sceneIndex; //used to keep track of scene
    private int weaponDmg;//beginning damage for knife weapon
    private int enemyHealth;
    private int xp = 0;

    void Start()
    {
        player = GameObject.Find("PositionPointer");
        dismountButton.interactable = false;
        attackButton.gameObject.SetActive(false);
        enemyHealthPanel.SetActive(false);
        totalDmgPanel.SetActive(false);
        dragonTextPanel.SetActive(false);
        gameOver = false;
        onBoard = true;
        inBattle = false;
        startPoint = 0;
        damage = 0;
        sceneIndex = 0; 
        weaponDmg = 2;

        weaponDmgText.text = "Damage: +" + weaponDmg;
    }

    
    void Update()
    {
        if(player.GetComponent<MoveOnBoard>().waypointIndex > startPoint + diceSideThrown 
            && player.GetComponent<MoveOnBoard>().waypointIndex != player.GetComponent<MoveOnBoard>().waypoints.Length)//Checks if player has moved theproper amount of spaces
        {
            player.GetComponent<MoveOnBoard>().moveAllowed = false;//stops player movement
            startPoint = player.GetComponent<MoveOnBoard>().waypointIndex;//resets startpoint to the player's current position on the board

            choosingArea();//determines a random area
            dismountButton.interactable = true;
            rollButton.GetComponentInChildren<Button>().interactable = true;//reactivate the roll button once the rolling has stopped
        }

        if ((player.GetComponent<MoveOnBoard>().waypointIndex == player.GetComponent<MoveOnBoard>().waypoints.Length)//checks if player has reached the end of the board
            && startPoint < player.GetComponent<MoveOnBoard>().waypointIndex)//this secondary check is to ensure that this condition is only met once
        {
            startPoint = player.GetComponent<MoveOnBoard>().waypointIndex + 1;
            sceneIndex = 3;
            enemyHealth = 10;
            dismountButton.interactable = true;
        }

        enemyHPText.text = "Enemy HP: " + enemyHealth;//Update monsters health onto the screen.
        xpText.text = "XP: " + xp;//Update xp on the screen
    }

    void choosingArea()
    {
        int shuffleNum = rand.Next(0, areas.Count);//get random number from areas List to act as an index.

        switch (areas[shuffleNum])//check against the value picked from the areas List.
        {
            case 0://Monster Scene
                areas.Remove(areas[shuffleNum]);
                sceneIndex = 0;
                Debug.Log("Its 0");
                break;
            case 1://Weapon Scene
                areas.Remove(areas[shuffleNum]);
                sceneIndex = 1;
                Debug.Log("Its 1");
                break;
            case 2://Empty Scene
                areas.Remove(areas[shuffleNum]);
                sceneIndex = 2;
                Debug.Log("Its 2");
                break;
        }
    }

    void AdjustEnemyHP()
    {
        //if statements checks for the players current position to determine the amount of health the monster will have.
        if (startPoint < 4)
        {
            enemyHealth = 3;
        }
        else if (startPoint < 9)
        {
            enemyHealth = 4;
        }
        else if (startPoint < 14)
        {
            enemyHealth = 5;
        }
        else if (startPoint < 20)
        {
            enemyHealth = 6;
        }
        else if (startPoint < 27)
        {
            enemyHealth = 7;
        }
    }

    void ReturnToBoard()//set variables for board scene (used from other scenes)
    {
        scenes[sceneIndex].SetActive(false);//shut off current scene (go back to board)
        enemyHealthPanel.SetActive(false);
        totalDmgPanel.SetActive(false);
        attackButton.gameObject.SetActive(false);
        dice.SetActive(true);
        onBoard = true;
        inBattle = false;
        dismountButton.interactable = false;
        attackButton.interactable = true;  //reactivate the attack button for the next battle that may occur
        searchButton.interactable = true;
        xpButton.interactable = true;
        dice.GetComponent<DiceControl>().rolling = false;
        rollButton.GetComponentInChildren<Button>().interactable = true;
        buttonText.text = "Dismount";
        damageText.text = "Total Damage: " + damage; // reset damage text
        receivedWeaponText.text = "New Weapon?";
        damage = 0; //reset damage
    }

    public static void MovePlayer()
    {
        player.GetComponent<MoveOnBoard>().moveAllowed = true;
    }

    public void CheckWeapon()//Decides the weapon given (used by search button in weapon scene)
    {
        int weaponIndex = rand.Next(0, weaponSlots.Count);

        //switch statement checks for current weapon to determine the next weapon they'll get.
        switch (weaponSlots[weaponIndex])
        {
            case 0:
                if (weaponDmg < 3)
                {
                    weaponIcon.sprite = weapons[0];
                    weaponDmg = 3;
                    weaponName.text = "Crossbow";
                    weaponDmgText.text = "Damage: +" + weaponDmg;
                }
                else
                {
                    receivedWeaponText.text = "You have a better weapon.";
                    return;
                }
                break;
            case 1:
                if (weaponDmg < 4)
                {
                    weaponIcon.sprite = weapons[1];
                    weaponDmg = 4;
                    weaponName.text = "Flail";
                    weaponDmgText.text = "Damage: +" + weaponDmg;
                }
                else
                {
                    receivedWeaponText.text = "You have a better weapon.";
                    return;
                }
                break;
            case 2:
                if (weaponDmg < 5)
                {
                    weaponIcon.sprite = weapons[2];
                    weaponDmg = 5;
                    weaponName.text = "Broad Sword";
                    weaponDmgText.text = "Damage: +" + weaponDmg;
                }
                else
                {
                    receivedWeaponText.text = "You have a better weapon.";
                    return;
                }
                break;
            case 3:
                if (weaponDmg < 6)
                {
                    weaponIcon.sprite = weapons[3];
                    weaponDmg = 6;
                    weaponName.text = "Dragon Slayer";
                    weaponDmgText.text = "Damage: +" + weaponDmg;
                }
                else
                {
                    receivedWeaponText.text = "You have a better weapon.";
                    return;
                }
                break;
            case 4:
                if (weaponDmg < 7)
                {
                    weaponIcon.sprite = weapons[4];
                    weaponDmg = 7;
                    weaponName.text = "Spell of the Gods";
                    weaponDmgText.text = "Damage: +" + weaponDmg;
                }
                else
                {
                    receivedWeaponText.text = "You have a better weapon.";
                    return;
                }
                break;
        }

        searchButton.interactable = false;
        dismountButton.interactable = true;
        weaponSlots.Remove(weaponSlots[weaponIndex]);
        receivedWeaponText.text = "You received a better weapon!";
    }

    public void AddXP()
    {
        xp++;
        xpButton.interactable = false;
        dismountButton.interactable = true;
    }

    public void Dismount()
    {
        if (scenes[sceneIndex].activeSelf == false)//if there is no scene other than the board showing -> execute code
        {
            scenes[sceneIndex].SetActive(true);//activate new scene
            onBoard = false;//change flag to not on the board
            buttonText.text = "Ride";//change dismount button text
            dismountButton.interactable = false;//disable button
            dice.SetActive(false);//disable dice

            if(sceneIndex == 0 || (sceneIndex == 3))//if scene is battle scene -> keep dice on screen
            {
                AdjustEnemyHP();
                enemyHealthPanel.SetActive(true);
                totalDmgPanel.SetActive(true);
                attackButton.gameObject.SetActive(true);
                dice.SetActive(true);
                rollButton.GetComponentInChildren<Button>().interactable = true;//reactivate the roll button once the player is in a battle scene

                if (sceneIndex == 3 && xp < 8)//only execute if the player is on the dragon scene and their xp is lower than 8
                {
                    dice.SetActive(false);//deactivate dice
                    attackButton.GetComponentInChildren<Text>().text = "Continue";//change text on attack button
                    attackButton.interactable = true;//allow attack button to be pressed
                }
            }
        }
        else//if we are in a new scene -> execute this code
        {
            ReturnToBoard();
        }
    }

    public void Attack()
    {
        damage += weaponDmg; //add damage with weapon damage
        damageText.text = "Total Damage: " + damage;
        inBattle = true;

        if (damage >= enemyHealth)//checks damage and ensures that monster HP hits 0
        {
            enemyHealth = 0;

            if (sceneIndex == 3)//execute if on dragon scene
            {
                dragonText.text = "Due to your cunning and experience, you have defeated the deadly dragon. Your quest has ended good sir. " +
                "You’ve obtained the Chalice of knowledge and all of earth’s mysteries are revealed.";

                victoryPanel.SetActive(true);//activate victory panel
                dragonTextPanel.SetActive(true);//show text
                attackButton.interactable = false;

                return;
            }
            xp += 2;
            dismountButton.interactable = true;
        }
        else //else you lose
        {
            if (sceneIndex == 3)//execute if on dragon scene
            {
                dragonText.text = "Alas, the dragon’s eyes stare at you and places you under his spell. You try to move but fail to do so and find yourself torched by the dragon’s fire." +
            " If only you had more experience, you could have seen it coming.";

                dragonTextPanel.SetActive(true);//activate dragon text panel
                dice.SetActive(false);
            }

            enemyHealth -= damage;
            gameOverPanel.SetActive(true);
        }

        attackButton.interactable = false; //Make it so the player can only attack once
    }
}
