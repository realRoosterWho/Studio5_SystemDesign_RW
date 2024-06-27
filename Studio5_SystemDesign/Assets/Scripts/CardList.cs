using UnityEngine;

public class CardList : MonosingletonTemp<CardList>
{
    [Header("Card 1")]
    [SerializeField] public Sprite Question1;
    [SerializeField] public Sprite Selection1_1;
    [SerializeField] public Sprite Selection1_2;
    [SerializeField] public Sprite Answer1_1;
    [SerializeField] public Sprite Answer1_2;
	[SerializeField] public Sprite Achievement1_1;
	[SerializeField] public Sprite Achievement1_2;
    [Space(10)]
    
    [Header("Card 2")]
    [SerializeField] public Sprite Question2;
    [SerializeField] public Sprite Selection2_1;
    [SerializeField] public Sprite Selection2_2;
    [SerializeField] public Sprite Answer2_1;
    [SerializeField] public Sprite Answer2_2;
	[SerializeField] public Sprite Achievement2_1;
	[SerializeField] public Sprite Achievement2_2;
    [Space(10)]
    
    [Header("Card 3")]
    [SerializeField] public Sprite Question3;
    [SerializeField] public Sprite Selection3_1;
    [SerializeField] public Sprite Selection3_2;
    [SerializeField] public Sprite Answer3_1;
    [SerializeField] public Sprite Answer3_2;
	[SerializeField] public Sprite Achievement3_1;
	[SerializeField] public Sprite Achievement3_2;

    public Sprite GetQuestion(int cardNumber)
    {
        switch (cardNumber)
        {
            case 1:
                return Question1;
            case 2:
                return Question2;
            case 3:
                return Question3;
            default:
                return null;
        }
    }

    public Sprite GetSelection(int cardNumber, int selectionNumber)
    {
        switch (cardNumber)
        {
            case 1:
                return selectionNumber == 1 ? Selection1_1 : Selection1_2;
            case 2:
                return selectionNumber == 1 ? Selection2_1 : Selection2_2;
            case 3:
                return selectionNumber == 1 ? Selection3_1 : Selection3_2;
            default:
                return null;
        }
    }

    public Sprite GetAnswer(int cardNumber, int answerNumber)
    {
        switch (cardNumber)
        {
            case 1:
                return answerNumber == 1 ? Answer1_1 : Answer1_2;
            case 2:
                return answerNumber == 1 ? Answer2_1 : Answer2_2;
            case 3:
                return answerNumber == 1 ? Answer3_1 : Answer3_2;
            default:
                return null;
        }
    }

	public Sprite GetAchievement(int cardNumber, int achievementNumber)
    {
        switch (cardNumber)
        {
            case 1:
                return achievementNumber == 1 ? Achievement1_1 : Achievement1_2;
            case 2:
                return achievementNumber == 1 ? Achievement2_1 : Achievement2_2;
            case 3:
                return achievementNumber == 1 ? Achievement3_1 : Achievement3_2;
            default:
                return null;
        }
    }
}