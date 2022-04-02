using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceControl : MonoBehaviour
{
    public Image dice;
    public Sprite[] sides;

    private bool coroutineAllowed = true;

    void Start()
    {
        dice.sprite = sides[0];
    }

    public void Roll()
    {
        if (!GameController.gameOver && coroutineAllowed)
            StartCoroutine("RollDice");
    }

    private IEnumerator RollDice()
    {
        coroutineAllowed = false;
        int randomSide = 0;
        for(int i = 0; i <= 20; i++)
        {
            randomSide = Random.Range(0, 6);
            dice.sprite = sides[randomSide];
            yield return new WaitForSeconds(0.05f);
        }

        if (GameController.onBoard)
        {
            GameController.diceSideThrown = randomSide;
            GameController.MovePlayer();
        }
        else
        {
            GameController.damage = randomSide + 1;
        }
        coroutineAllowed = true;
    }
}
