using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    public GameObject foodPrefab;
    public List<Ant> ant0List;
    public List<Ant> ant1List;
    public GameObject[] pos;

    public Queen[] queens;

    public int ant0Order;
    public int ant1Order;
  
    void Start()
    {
        //GameObject[] antAll = GameObject.FindGameObjectsWithTag("Ant");
        //foreach (GameObject ant in antAll)
        //{
        //    Ant antS = ant.GetComponent<Ant>();
        //    if (antS.teamNum == 0) ant0List.Add(antS);
        //    if (antS.teamNum == 1) ant1List.Add(antS);
        //} 
    }

    public void antGetIntoList(int team, Ant ant)
    {
        if (team == 0)
        {
            ant0List.Add(ant);
            ant0Order++;
            ant.antOrder = ant0Order;
        }
        if (team == 1)
        {
            ant1List.Add(ant);
            ant1Order++;
            ant.antOrder = ant1Order;
        }
    }   

    public void antDelete(int team, Ant ant)
    {
        if (team == 0) ant0List.Remove(ant);
        if (team == 1) ant1List.Remove(ant);
    }

    public InputField InputText;
    public string[] InputWords;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(InputText.text);
            ImportString(InputText.text);
            InputText.text = "";
            InputText.ActivateInputField();
        }
    }

    public void ImportString(string InputWord)
    {
        InputWord = InputWord.ToLower();
        InputWords= InputWord.Split(' ');

        int Length = 0;
        foreach (string token in InputWords)
        {
            Length++;
        }
        //Debug.Log(Length);
        if (Length == 1) OrderT1(InputWords);
        if (Length == 2) OrderT0(InputWords);
        if (Length > 2)
        {
            Ant thisAnt = findASpeicalAnt(InputWords, 0, 1);
            if (thisAnt != null)
                specialOrderToOneAnt(findASpeicalAnt(InputWords, 0, 1), InputWords);
        }
    }
    public void OrderT2(string[] InputWords)
    {
        if (InputWords[0] == "blue")
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                ant0List[Random.Range(0, ant0List.Count)].changeAIState(InputWords[1]);
            }
        }

        if (InputWords[0] == "red")
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                ant1List[Random.Range(0, ant1List.Count)].changeAIState(InputWords[1]);
            }
        }
    }

    private Ant findASpeicalAnt(string[] InputWords, int N1, int N2)
    {
        Ant antThis = null;
        if (InputWords[N1] == "a")
        {
            //Debug.Log("teamA");
            for (int i5 = 0; i5 < ant0List.Count; i5++)
            {
                if (ant0List[i5].antOrder.ToString() == InputWords[N2])
                {
                    return ant0List[i5];
                    break;
                }
            }
            return antThis;
        }

        if (InputWords[N1] == "b")
        {
            for (int i6 = 0; i6 < ant1List.Count; i6++)
            {
                if (ant1List[i6].antOrder.ToString() == InputWords[N2])
                {
                    return ant1List[i6];
                    break;
                }
            }
            return antThis;
        }

        return antThis;
    }
    //public void OrderT3(string[] InputWords)
    //{
    //    Ant antThis = null;
    //    if (InputWords[0] == "a")
    //    {
    //        Debug.Log("teamA");
    //        for (int i5 = 0; i5 < ant0List.Count; i5++)
    //        {
    //            if (ant0List[i5].antOrder.ToString() == InputWords[1])
    //            {
    //                specialOrderToOneAnt(ant0List[i5], InputWords);
    //                break;
    //            }
    //        }
    //        return;
    //    }

    //    if (InputWords[0] == "b")
    //    {
    //        for (int i6 = 0; i6 < ant1List.Count; i6++)
    //        {
    //            if (ant0List[i6].antOrder.ToString() == InputWords[1])
    //            {
    //                specialOrderToOneAnt(ant0List[i6], InputWords);
    //                break;
    //            }
    //        }
    //        return;
    //    }
    //}
    public void OrderT0(string[] InputWords)
    {
        if (InputWords[1] == "birthbonus")
        {
            int queenNum = 0;
            if (InputWords[0] == "blue") queenNum = 0;
            else queenNum = 1;
            queens[queenNum].birthCD += 7.5f;
            return;
        }

        if (InputWords[0] == "blue")
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                ant0List[Random.Range(0, ant0List.Count)].changeAIState(InputWords[1]);
            }
            return;
        }

        if (InputWords[0] == "red")
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                ant1List[Random.Range(0, ant1List.Count)].changeAIState(InputWords[1]);
            }
            return;
        }
    }

    public void specialOrderToOneAnt(Ant ant, string[] InputWords)
    {

        if (InputWords[2] == "name")
        {
            ant.antSpecialName = "бо" + InputWords[3] + "'";
            return;
        }


        if (InputWords[2] == "health")
        {
            ant.health += 20;
            return;
        }

        int Length = 0;
        foreach (string token in InputWords)
        {
            Length++;
        }

        if (InputWords[2] == "attack" && Length > 3)
        {
            Ant thisAnt = findASpeicalAnt(InputWords, 3, 4);
            if (thisAnt != null)
            {
                ant.enemy = findASpeicalAnt(InputWords, 3, 4).gameObject;
                ant.changeAIState(InputWords[2]);
                return;
            }
        }

        ant.changeAIState(InputWords[2]);

    }
    public void OrderT1(string[] InputWords)
    {
        if (InputWords[0] == "food")
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                Instantiate(foodPrefab, pos[Random.Range(0, pos.Length)].transform.position, Quaternion.identity);
            }
        }

        if (InputWords[0] == "kill")
        {
            for (int i2 = 0; i2 < Random.Range(0, 2); i2++)
            {
                ant0List[Random.Range(0, ant0List.Count)].health -= 1000;
            }

            for (int i3 = 0; i3 < Random.Range(0, 2); i3++)
            {
                ant0List[Random.Range(0, ant0List.Count)].health -= 1000;
            }
        }
    }
}
