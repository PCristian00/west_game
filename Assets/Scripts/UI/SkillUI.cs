using UnityEngine;

public class SkillUI : MonoBehaviour
{
    void Start()
    {
        
    }

    private void Update()
    {
        if (!SkillManager.instance)
        {
            Debug.Log("NO SKILLLL");
            gameObject.SetActive(false);
        }

    }
}
