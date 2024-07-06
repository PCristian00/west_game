using UnityEngine;

public class SkillUI : MonoBehaviour
{
    void Start()
    {
        if (!SkillManager.instance)
        {
            Debug.Log("NO SKILLLL");
            gameObject.SetActive(false);
        }
    }
}
