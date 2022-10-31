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
    public InputField InputText;
    public string[] InputWords;
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
        if (team == 0) ant0List.Add(ant);
        if (team == 1) ant1List.Add(ant);
    }   

    public void antDelete(int team, Ant ant)
    {
        if (team == 0) ant0List.Remove(ant);
        if (team == 1) ant1List.Remove(ant);
    }
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
        Debug.Log(Length);
        if (Length == 1) OrderT1(InputWords);
        if (Length == 2) OrderT0(InputWords);
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
    public void OrderT0(string[] InputWords)
    {
        if (InputWords[1] == "birth")
        {
            int queenNum = 0;
            if (InputWords[0] == "blue") queenNum = 0;
            else queenNum = 1;
            queens[queenNum].birth();
        }

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
