using UnityEngine;

public class SkillUI : MonoBehaviour
{
    private void Update()
    {
        if (!SkillManager.instance)
        {
            // Debug.Log("NO SKILL MANAGER");
            gameObject.SetActive(false);
        }

        else if (SkillManager.instance.CurrentSkill == null) gameObject.SetActive(false);

    }
}
