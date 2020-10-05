using System.Collections;
using System.Collections.Generic;
using FIMSpace.Jiggling;
using UnityEngine;
using UnityEngine.Events;

public class GameLogicController : MonoBehaviour
{
    public GameObject upperJaw;
    public List<GameObject> upperTeeth;
    public List<int> upperShrinked;

    public GameObject lowerJaw;
    public List<GameObject> lowerTeeth;
    public List<int> lowerShrinked;

    public UnityEvent toothGrown;
    public UnityEvent finishedSurgery;

    public ToothPlateSpawner toothPlateSpawner;

    public int teethFixed = 0;
    
    List<GameObject> ChildExtractor(GameObject objct, List<GameObject> tempList)
    {
        int children = objct.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            tempList.Add(objct.transform.GetChild(i).gameObject);
        }

        return tempList;
    }

    void InitTeethFromJaws()
    {
        upperTeeth = ChildExtractor(upperJaw, upperTeeth);
        lowerTeeth = ChildExtractor(lowerJaw, lowerTeeth);
    }

    void RandomShrinker(List<GameObject> list, bool upper)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (Random.Range(0, 4) > 1)
            {
                if (upper)
                {
                    upperShrinked.Add(i);
                }
                else
                {
                    lowerShrinked.Add(i);
                }

                list[i].GetComponent<FJiggling_Grow>().StartShrinking();
            }
        }
    }

    public void GrowOneTooth()
    {
        if (upperShrinked.Count > 0)
        {
            var indexPicked = Random.Range(0, upperShrinked.Count);
            upperTeeth[upperShrinked[indexPicked]].GetComponent<FJiggling_Grow>().StartGrowing();
            upperShrinked.RemoveAt(indexPicked);
        }
        else if (lowerShrinked.Count > 0)
        {
            var indexPicked = Random.Range(0, lowerShrinked.Count);
            lowerTeeth[lowerShrinked[indexPicked]].GetComponent<FJiggling_Grow>().StartGrowing();
            lowerShrinked.RemoveAt(indexPicked);
        }

        toothGrown.Invoke();

        teethFixed++;

        if (upperShrinked.Count == 0 && lowerShrinked.Count == 0)
        {
            Debug.Log("SURGERY COMPLETE!");
            finishedSurgery.Invoke();
        }
    }

    void InitRandomTeethRemoved()
    {
        RandomShrinker(upperTeeth, true);
        RandomShrinker(lowerTeeth, false);
    }

    public void InitToothAndStuff()
    {
        InitRandomTeethRemoved();

        toothPlateSpawner.SpawnToothOnPlate(upperShrinked.Count + lowerShrinked.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitTeethFromJaws();
    }
    
}