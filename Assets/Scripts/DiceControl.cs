using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceControl : MonoBehaviour
{
    public bool rolling;//used to check if we are currently rolling
    public Image dice;
    public Sprite[] sides;
    public GameController dismount;

    private bool coroutineAllowed = true;

    void Start()
    {
        dice.sprite = sides[0];
    }

    public void Roll()
    {
        if (!GameController.gameOver &&  !GameController.inBattle && coroutineAllowed && !rolling)//if game is not over, allow dice roll
        {
            StartCoroutine("RollDice");
            dismount.dismountButton.interactable = false;
        }
    }

    private IEnumerator RollDice()
    {
        coroutineAllowed = false;
        int randomSide = 0;
        for(int i = 0; i <= 20; i++) //changes dice sprite during roll
        {
            randomSide = Random.Range(0, 6);
            dice.sprite = sides[randomSide];
            rolling = true;
            yield return new WaitForSeconds(0.05f);
        }

        if (GameController.onBoard) //if the roll occured on the board scene
        {
            GameController.diceSideThrown = randomSide;
            GameController.MovePlayer();
            rolling = false;
        }
        else//if roll occured in a battle scene
        {
            GameController.damage = randomSide + 1;
        }
        coroutineAllowed = true;
    }
}
