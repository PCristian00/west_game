using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    // VEDERE SE METTERE ANCHE COOLDOWN QUI
    // INSERIRE FORSE STATI PER LE VARIE SKILL; COPIA LOADOUT MANAGER

    [Header("Skills")]
    public GameObject activeSkill;
    public IPowerup skill;
    public float skillCooldown = 10f;
    public bool skillReady = true;
    public Slider skillBar;

    public static SkillManager instance;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        skill = activeSkill.GetComponent<IPowerup>();
    }

    public IEnumerator Cooldown(float time)
    {
       // skillBar.gameObject.SetActive(true);

        float t = 0f;


        //yield return new WaitForSeconds(time);       


        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime / time;

            if (t >= 0.5f) skillBar.gameObject.SetActive(true);
            
            skillBar.value = t;

            // Inserire qui ripristino icona, messaggio etc...
            // Provare barra di caricamento 

        }

        skillBar.gameObject.SetActive(false);

       // Debug.Log("Skill PRONTA");
        skillReady = true;
    }
}