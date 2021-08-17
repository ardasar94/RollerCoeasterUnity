using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarControls : MonoBehaviour
{
    [SerializeField] GameObject Person;
    [SerializeField] Animator animator;

    public int l = 3, c = 3, n = 4;
    public List<int> groups = new List<int>() { 3, 1, 1, 2 };
    public int totalMoney = 0;
    public bool shouldStart = false;
    public int CurrentTour = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldStart)
        {
            StartFun();
            shouldStart = false;
        }
        if (c == CurrentTour)
        {
            StopAllCoroutines();
            animator.SetBool("isRun", false);
            FindObjectOfType<GameController>().ShowEndScreen();
        }
    }

    private void StartFun()
    {
        InstantiatePeopleAtStart();
        StartCoroutine("StartCar");
    }

    private void InstantiatePeopleAtStart()
    {
        int totalPeople = groups.Sum();
        GameObject line = GameObject.Find("Line");

        for (int i = 0; i < totalPeople; i++)
        {
            if (i == 0)
            {
                GameObject newPerson = Instantiate(Person, line.transform.position, Quaternion.identity);
                newPerson.transform.parent = line.transform;
            }
            else
            {
                GameObject newPerson = Instantiate(Person, line.transform.position + (line.transform.GetChild(line.transform.childCount - 1)).transform.localPosition + new Vector3(0.6f, 0, 0), Quaternion.identity);
                newPerson.transform.parent = line.transform;
            }
        }
    }

    IEnumerator StartCar()
    {
        while (CurrentTour < c)
        {
            animator.SetBool("isRun", false);
            int oneRideSize = CheckOneRide(groups, l);
            SwipeTheLine(oneRideSize);
            totalMoney += oneRideSize;
            CurrentTour++;
            Debug.Log(CurrentTour);
            yield return new WaitForSeconds(2f);
        }
    }

    int x = 0;
    private void SwipeTheLine(int oneRideSize)
    {
        x = oneRideSize;
        GameObject line = GameObject.Find("Line");
        for (int i = 0; i < oneRideSize; i++)
        {
            Destroy(line.transform.GetChild(i).gameObject);
        }
        StartCoroutine(FallowPath(oneRideSize));
        animator.SetBool("isRun", true);

        Invoke("InstantiatePeopleAtRunTime", 1);
    }

    IEnumerator FallowPath(int oneRideSize)
    {
        GameObject line = GameObject.Find("Line");
        Vector3 startPosition = line.transform.position;
        Vector3 endPosition = new Vector3(line.transform.transform.position.x - 0.6f * oneRideSize, line.transform.transform.position.y, line.transform.transform.position.z); ;
        float travelPercent = 0f;

        transform.LookAt(endPosition);

        while (travelPercent < 1f)
        {
            travelPercent += Time.deltaTime * 1f;
            line.transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
            yield return new WaitForEndOfFrame();
        }
    }

    private void InstantiatePeopleAtRunTime()
    {
        GameObject line = GameObject.Find("Line");

        for (int i = 0; i < x; i++)
        {
            if (line.transform.childCount == 0)
            {
                GameObject newPerson = Instantiate(Person, line.transform.position, Quaternion.identity);
                newPerson.transform.parent = line.transform;
            }
            else
            {
                GameObject newPerson = Instantiate(Person, line.transform.position + (line.transform.GetChild(line.transform.childCount - 1)).transform.localPosition + new Vector3(0.6f, 0, 0), Quaternion.identity);
                newPerson.transform.parent = line.transform;
            }
        }
    }

    private int CheckOneRide(List<int> groups, int L)
    {
        int multiply = 0;
        int oneGroupSize = 0;
        foreach (var item in groups)
        {
            if (oneGroupSize + item > L)
            {
                for (int i = 0; i < multiply; i++)
                {
                    int x = groups[0];
                    for (int j = 1; j < groups.Count; j++)
                    {
                        groups[j - 1] = groups[j];
                    }
                    groups[groups.Count - 1] = x;
                }

                return oneGroupSize;
            }
            oneGroupSize += item;
            multiply++;
        }
        return oneGroupSize;
    }
}
