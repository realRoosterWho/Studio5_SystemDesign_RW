using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCardManager : MonosingletonTemp<DialogueCardManager>
{
    
    public int m_cardNumber = 1;
    private bool m_isSelection = false;
    public IEnumerator DisplayCard(int cardNumber)
    {
        CardList cardList = CardList.Instance;
        InputHandler inputHandler = InputHandler.Instance;
        UIManager uiManager = UIManager.Instance;

        // Display the question
        Sprite question = cardList.GetQuestion(cardNumber);
        uiManager.UpdateImage(question);
        inputHandler.m_Trigger = false;

        // Wait for trigger to display the selection
        yield return new WaitUntil(() => inputHandler.m_Trigger);
        inputHandler.m_Trigger = false;

        // Display the selection based on the knob_selection value
        m_isSelection = true;
        inputHandler.m_Trigger = false;
        while (m_isSelection)
        {
            Debug.Log("Selection");
            //检查一下knob_selection的值，如果不是1或者2就直接跳过
            
            var inputData = inputHandler.GetInputData();
            if (cardList == null)
{
    Debug.LogError("CardList is null");
    yield break;
}
            if (inputData == null)
            {
                Debug.LogError("InputData is null");
                yield return null;
                continue;
            }
            if (inputData.knob_selection == 1 || inputData.knob_selection == 2)
            { 
            Debug.Log(inputData.knob_selection + " " + cardNumber);
            Sprite selection = cardList.GetSelection(cardNumber, inputData.knob_selection);
            uiManager.UpdateImage(selection);
            }

            
            if (inputHandler.m_Trigger)
            {
                m_isSelection = false;
            }
            // 等待下一帧
            yield return null;
        }

        // Wait for trigger to display the answer
        // yield return new WaitUntil(() => inputHandler.m_Trigger);
        inputHandler.m_Trigger = false;

        // Display the answer based on the last displayed selection
        Sprite answer = cardList.GetAnswer(cardNumber, inputHandler.GetInputData().knob_selection);
        uiManager.UpdateImage(answer);

        // Wait for trigger to end the dialogue card
        yield return new WaitUntil(() => inputHandler.m_Trigger);
        inputHandler.m_Trigger = false;



        // End the dialogue card
        //等0.1秒
        yield return new WaitForSeconds(0.1f);
        ControlModeManager.Instance.m_controlMode = ControlMode.Free;
        m_cardNumber++;
        uiManager.UpdateImage(null);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}