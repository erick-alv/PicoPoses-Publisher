using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseBallManager : MonoBehaviour
{

    public GameObject poseBallPrefab;
    public float extraDistance;
    public GameObject refObj;

    private GameObject[] poseBalls = new GameObject[3];

    // used to avoid that a continuous press activate
    private bool inToggleState = false;
    private float toggleDuration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        poseBalls[0] = Instantiate(poseBallPrefab);
        poseBalls[1] = Instantiate(poseBallPrefab);
        poseBalls[2] = Instantiate(poseBallPrefab);
    }

    public void MoveBalls(List<Vector3> positions, List<Quaternion> rotations)
    {
        Vector3 forwardExtra = refObj.transform.forward.normalized * extraDistance;
        for (int i=0; i<poseBalls.Length; i++)
        {
            if (poseBalls[i].activeSelf)
            {
                poseBalls[i].transform.position = positions[i] + forwardExtra;
                poseBalls[i].transform.rotation = rotations[i];
            }
            
        }
    }

    public void toggleVisibility()
    {
       if(!inToggleState)
        {
            StartCoroutine(ToggleCoroutine());
        } 
    }

    IEnumerator ToggleCoroutine()
    {
        inToggleState = true;
        for (int i = 0; i < poseBalls.Length; i++)
        {
            poseBalls[i].SetActive(!poseBalls[i].activeSelf);
        }
        yield return new WaitForSeconds(toggleDuration);
        inToggleState = false;
    }




}
