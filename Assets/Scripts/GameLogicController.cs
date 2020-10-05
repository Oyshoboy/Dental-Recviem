using System.Collections;
using System.Collections.Generic;
using FIMSpace.Jiggling;
using UnityEngine;

public class GameLogicController : MonoBehaviour
{
    public GameObject upperJaw;
    public List<GameObject> upperTeeth;
    public List<int> upperShrinked;
    
    public GameObject lowerJaw;
    public List<GameObject> lowerTeeth;
    public List<int> lowerShrinked;

    public ToothPlateSpawner toothPlateSpawner;

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
            if (Random.Range(0, 5) > 1)
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
    void InitRandomTeethRemoved()
    {
        RandomShrinker(upperTeeth, true);
        RandomShrinker(lowerTeeth, false);
    }
    
    void InitEverything()
    {
        InitTeethFromJaws();
        InitRandomTeethRemoved();
        
        toothPlateSpawner.SpawnToothOnPlate(upperShrinked.Count + lowerShrinked.Count);   
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitEverything();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
