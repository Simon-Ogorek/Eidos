using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMoveInfo : MonoBehaviour
{
    List<GameObject> listOfMoveUIs;
    Dictionary<GameObject, Move> mapUIToMove;
    [SerializeField]
    private GameObject template;

    [SerializeField]
    private Transform offScreenPrevious;
    [SerializeField]
    private Transform previous;
    [SerializeField]
    private Transform selected;
    [SerializeField]
    private Transform next;
    [SerializeField]
    private Transform offScreenNext;

    private int moveIndex = 0;

    Coroutine transformerPrevPrev;
    Coroutine transformerPrev;
    Coroutine transformerCurrent;
    Coroutine transformerNext;
    Coroutine transformerNextNext;
    int activeTransformers;
    float cooldownTimer = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        listOfMoveUIs = new List<GameObject>();
        mapUIToMove = new Dictionary<GameObject, Move>();
    }
    public void UpdateMoveSelection(Combatant combatant)
    {
        listOfMoveUIs.Clear();
        mapUIToMove.Clear();

        foreach (Move move in combatant.GetComponents<Move>())
        {
            GameObject tempUI = Instantiate(template);
            TMP_Text text = tempUI.GetComponentInChildren<TMP_Text>();
                text.text = move.GetData().moveName;

            mapUIToMove.Add(tempUI, move);
            listOfMoveUIs.Add(tempUI);
        }

        if (listOfMoveUIs.Count < 5)
        {
            throw new Exception("The UI currently dosent support less than 5 moves for the move cycler element");
        }

        foreach (GameObject ui in listOfMoveUIs)
        {
            ui.SetActive(false);
        }

        listOfMoveUIs[0].SetActive(true);
        listOfMoveUIs[1].SetActive(true);
        listOfMoveUIs[2].SetActive(true);
        listOfMoveUIs[3].SetActive(true);
        listOfMoveUIs[4].SetActive(true);
        
        listOfMoveUIs[0].transform.SetParent(offScreenPrevious);
        listOfMoveUIs[1].transform.SetParent(previous);
        listOfMoveUIs[2].transform.SetParent(selected);
        listOfMoveUIs[3].transform.SetParent(next);
        listOfMoveUIs[4].transform.SetParent(offScreenNext);

        listOfMoveUIs[0].transform.localPosition = Vector3.zero;
        listOfMoveUIs[1].transform.localPosition = Vector3.zero;
        listOfMoveUIs[2].transform.localPosition = Vector3.zero;
        listOfMoveUIs[3].transform.localPosition = Vector3.zero;
        listOfMoveUIs[4].transform.localPosition = Vector3.zero;

        listOfMoveUIs[0].transform.localScale = Vector3.one;
        listOfMoveUIs[1].transform.localScale = Vector3.one;
        listOfMoveUIs[2].transform.localScale = Vector3.one;
        listOfMoveUIs[3].transform.localScale = Vector3.one;
        listOfMoveUIs[4].transform.localScale = Vector3.one;

    } 

    IEnumerator Transformer(GameObject obj)
    {
        if (Vector3.Distance(obj.transform.localScale, Vector3.one) < 30f && 
            Vector3.Distance(obj.transform.localPosition, Vector3.zero) < 30f)
        {
            Debug.Log("broke out");
            activeTransformers--;
            yield break;
        }
            
        Debug.Log("iterated");
        obj.transform.localScale = Vector3.Slerp(obj.transform.localScale, Vector3.one, 0.06f);
        obj.transform.localPosition = Vector3.Slerp(obj.transform.localPosition, Vector3.zero, 0.06f);
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(Transformer(obj));
    }

    public void ChangeMove(bool decrement)
    {
        moveIndex += decrement ? -1 : 1;

        if (moveIndex <= 0)
            moveIndex = moveIndex % listOfMoveUIs.Count + int.MaxValue / 4;

        listOfMoveUIs[moveIndex%listOfMoveUIs.Count].transform.SetParent(offScreenPrevious);
        listOfMoveUIs[(moveIndex+1)%listOfMoveUIs.Count].transform.SetParent(previous);
        listOfMoveUIs[(moveIndex+2)%listOfMoveUIs.Count].transform.SetParent(selected);
        listOfMoveUIs[(moveIndex+3)%listOfMoveUIs.Count].transform.SetParent(next);
        listOfMoveUIs[(moveIndex+4)%listOfMoveUIs.Count].transform.SetParent(offScreenNext);

        activeTransformers = 4;

        if (decrement)
        {
            listOfMoveUIs[NegativeAccess(moveIndex)].transform.localScale = Vector3.one;
            listOfMoveUIs[NegativeAccess(moveIndex)].transform.localPosition = Vector3.zero;
            transformerPrevPrev = StartCoroutine(Transformer(listOfMoveUIs[(moveIndex+4)%listOfMoveUIs.Count]));
        }
        else
        {
            listOfMoveUIs[(moveIndex+4)%listOfMoveUIs.Count].transform.localScale = Vector3.one;
            listOfMoveUIs[(moveIndex+4)%listOfMoveUIs.Count].transform.localPosition = Vector3.zero;
            transformerNext = StartCoroutine(Transformer(listOfMoveUIs[moveIndex%listOfMoveUIs.Count]));
        }
        
        transformerPrevPrev = StartCoroutine(Transformer(listOfMoveUIs[(moveIndex+1)%listOfMoveUIs.Count]));
        transformerPrev = StartCoroutine(Transformer(listOfMoveUIs[(moveIndex+2)%listOfMoveUIs.Count]));
        transformerCurrent = StartCoroutine(Transformer(listOfMoveUIs[(moveIndex+3)%listOfMoveUIs.Count]));


        
        listOfMoveUIs[(moveIndex-1)%listOfMoveUIs.Count].SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            listOfMoveUIs[(moveIndex+i)%listOfMoveUIs.Count].SetActive(true);
        } 
    }
    int NegativeAccess(int value)
    {
        return ((value % listOfMoveUIs.Count) + listOfMoveUIs.Count) % listOfMoveUIs.Count;
    }

    public void DoSelectedMove()
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        mapUIToMove[listOfMoveUIs[(moveIndex+2)%listOfMoveUIs.Count]].DoMove();
    }

    public void startCooldownTimerMoves(float timer)
    {
        if (timer <= 0 )
        {
            Debug.LogWarning("Nonsense timer set: " + timer);
        }
        cooldownTimer = timer;

        foreach (GameObject moveUIs in listOfMoveUIs)
        {
            moveUIs.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(0.7f,0.7f,0.7f,1f);
        }
        StartCoroutine(endCooldown());
    }   

    IEnumerator endCooldown()
    {
        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        foreach (GameObject moveUIs in listOfMoveUIs)
        {
            moveUIs.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(1f,1f,1f,1f);
        }
    }
}
