using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject StartScreen;
    [SerializeField] Text moneyText;
    [SerializeField] Text currentMoneyText;
    [SerializeField] Text tourText;
    [SerializeField] GameObject EndScreen;

    CarControls carControls;
    // Start is called before the first frame update
    void Start()
    {
        carControls = FindObjectOfType<CarControls>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = carControls.totalMoney.ToString();
        currentMoneyText.text = "Current Money: " + carControls.totalMoney.ToString();
        tourText.text = "Current Tour: " + carControls.CurrentTour.ToString();
    }

    public void GetInput1(string guess)
    {
        string basics = guess;
        string basicsCheck = basics.Replace(" ", "");
        int x;
        List<string> basicsList = basics.Split(' ').ToList();
        if (!int.TryParse(basicsCheck, out x) || basicsList.Count != 3)
        {
            Debug.Log("You should enter the nums with given rules");
            return;
        }

        carControls.l = int.Parse(basicsList[0]);
        carControls.c = int.Parse(basicsList[1]);
        carControls.n = int.Parse(basicsList[2]);

        if (carControls.l < 1 || carControls.l > Math.Pow(10, 7) || carControls.c < 1 || carControls.c > Math.Pow(10, 7) || carControls.n < 1 || carControls.n > 1000)
        {
            Console.WriteLine("You should enter the nums with given rules");
            return;
        }
    }
    public void GetInput2(string guess)
    {
        carControls.groups = new List<int>();
        string groups = guess;
        string groupsCheck = groups.Replace(" ", "");
        int x;
        List<string> groupsList = groups.Split(' ').ToList();
        if (!int.TryParse(groupsCheck, out x) || groupsList.Count != carControls.n)
        {
            Debug.Log("You should enter the nums with given rules");
            return;
        }

        for (int i = 0; i < carControls.n; i++)
        {
            carControls.groups.Add(int.Parse(groupsList[i]));
        }
    }

    public void StartGame()
    {
        StartScreen.SetActive(false);
        carControls.shouldStart = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); ;
    }

    public void ShowEndScreen()
    {
        Invoke("EndScreenSetActiveTrue", 1.5f);
    }

    public void EndScreenSetActiveTrue()
    {
        EndScreen.SetActive(true);
    }

}
