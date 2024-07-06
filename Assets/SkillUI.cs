using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        if (!SkillManager.instance)
        {
            Debug.Log("NO SKILLLL");
            gameObject.SetActive(false);
        }
    }
}
