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
    public static bool inBattle = false;

    public GameObject[] scenes;
    public GameObject gameOverPanel;
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

    private List<int> areas = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2 };
    private System.Random rand = new System.Random();

    int sceneIndex = 0; //used to keep track of scene
    int weaponIndex = 0;
    int weaponDmg = 0;
    int enemyHealth = 0;
    int xp = 0;

    void Start()
    {
        player = GameObject.Find("PositionPointer");
        player.GetComponent<MoveOnBoard>().moveAllowed = false;
        dismountButton.interactable = false;
        attackButton.gameObject.SetActive(false);
        enemyHealthPanel.SetActive(false);
        totalDmgPanel.SetActive(false);
    }

    
    void Update()
    {
        if(player.GetComponent<MoveOnBoard>().waypointIndex > startPoint + diceSideThrown)//Checks if player has moved theproper amount of spaces
        {
            player.GetComponent<MoveOnBoard>().moveAllowed = false;//stops player movement
            startPoint = player.GetComponent<MoveOnBoard>().waypointIndex;//resets startpoint to the player's current position on the board

            choosingArea();//determines a random area
            dismountButton.interactable = true;
        }

        if (player.GetComponent<MoveOnBoard>().waypointIndex == player.GetComponent<MoveOnBoard>().waypoints.Length)//checks if player has reached the end of the board
        {
            startPoint = player.GetComponent<MoveOnBoard>().waypointIndex;
            Debug.Log(startPoint);
            sceneIndex = 3;
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
        else if(startPoint == 27)
        {
            enemyHealth = 10;
        }
    }

    void ReturnToBoard()//set variables for board scene (used from other scenes)
    {
        scenes[sceneIndex].SetActive(false);//shut off current scene (go back to board)
        enemyHealthPanel.SetActive(false);
        totalDmgPanel.SetActive(false);
        onBoard = true;
        inBattle = false;
        buttonText.text = "Dismount";
        dismountButton.interactable = false;
        attackButton.interactable = true;  //reactivate the attack button for the next battle that may occur
        searchButton.interactable = true;
        xpButton.interactable = true;
        damage = 0; //reset damage
        damageText.text = "Total Damage: " + damage; // reset damage text
        attackButton.gameObject.SetActive(false);
        dice.SetActive(true);
        dice.GetComponent<DiceControl>().rolling = false;
    }

    public static void MovePlayer()
    {
        player.GetComponent<MoveOnBoard>().moveAllowed = true;
    }

    public void CheckWeapon()//Decides the weapon given (used by search button in weapon scene)
    {
        //if statements checks for the players current position to determine the level of weapon they'll get.
        switch (weaponIndex)
        {
            case 0:
                weaponIcon.sprite = weapons[0];
                weaponDmg = 3;
                weaponName.text = "Crossbow";
                weaponDmgText.text = "Damage: +" + weaponDmg;
                break;
            case 1:
                weaponIcon.sprite = weapons[1];
                weaponDmg = 4;
                weaponName.text = "Flail";
                weaponDmgText.text = "Damage: +" + weaponDmg;
                break;
            case 2:
                weaponIcon.sprite = weapons[2];
                weaponDmg = 5;
                weaponName.text = "Broad Sword";
                weaponDmgText.text = "Damage: +" + weaponDmg;
                break;
            case 3:
                weaponIcon.sprite = weapons[3];
                weaponDmg = 6;
                weaponName.text = "Dragon Slayer";
                weaponDmgText.text = "Damage: +" + weaponDmg;
                break;
            case 4:
                weaponIcon.sprite = weapons[4];
                weaponDmg = 7;
                weaponName.text = "Spell of the Gods";
                weaponDmgText.text = "Damage: +" + weaponDmg;
                break;
        }

        searchButton.interactable = false;
        dismountButton.interactable = true;
        weaponIndex++;
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
            onBoard = false;
            buttonText.text = "Ride";
            dismountButton.interactable = false;
            dice.SetActive(false);

            if(sceneIndex == 0 || (sceneIndex == 3))//if scene is battle scene -> keep dice on screen
            {
                AdjustEnemyHP();
                enemyHealthPanel.SetActive(true);
                totalDmgPanel.SetActive(true);
                attackButton.gameObject.SetActive(true);
                dice.SetActive(true);

                if(sceneIndex == 3 && xp < 8)
                {
                    dragonText.text = "Alas, the dragon’s eyes stare at you and places you under his spell. You try to move but fail to do so and find yourself torched by the dragon’s fire." +
                " If only you had more experience, you could have seen it coming.";

                    dice.SetActive(false);
                    attackButton.GetComponentInChildren<Text>().text = "Continue";
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
            if(sceneIndex == 3)
            {
                dragonText.text = "Due to your cunning and experience, you have defeated the deadly dragon. Your quest has ended good sir. " +
                "You’ve obtained the Chalice of knowledge and all of earth’s mysteries are revealed.";

                gameOver = true;
            }
            enemyHealth = 0;
            xp += 2;
            dismountButton.interactable = true;
        }
        else //else you lose
        {
            enemyHealth -= damage;
            gameOverPanel.SetActive(true);
            onBoard = true;
            inBattle = false;
            startPoint = 0;
            player.GetComponent<MoveOnBoard>().waypointIndex = 0;
            dismountButton.interactable = false;
        }

        attackButton.interactable = false; //Make it so the player can only attack once
    }
}
