using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skills;

    private int current;    

    private GameObject _currentSkill;

    public event Action<GameObject> OnSkillChanged;


    public GameObject CurrentSkill
    {
        get => _currentSkill;

        protected set
        {
            if (_currentSkill == value)
            {
                return;
            }

            _currentSkill = value;

            skill = CurrentSkill.GetComponent<IPowerup>();

            skillText.text = CurrentSkill.name;
            skillIcon.sprite = CurrentSkill.GetComponent<Image>().sprite;
            skillIcon.color = Color.white;

            OnSkillChanged?.Invoke(_currentSkill);
        }
    }


    [Header("Skills")]
    // public GameObject activeSkill;
    public IPowerup skill;
    public float skillCooldown = 10f;
    private bool skillLoaded = false;
    public bool skillReady = true;
    public Slider skillBar;
    public TextMeshProUGUI skillText;
    public Image skillIcon;
    private Color activeColor;

    public static SkillManager instance;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        for (int i = 0; i < ActiveSkillPopup.states.Length; i++)
        {
            if (ActiveSkillPopup.states[i] == 2)
            {
                current = i;
                skillLoaded = true;
                break;
            }
        }

        if (!skillLoaded) NoSkillEquipped();
        else
            CurrentSkill = skills[current];
    }

    public void NoSkillEquipped()
    {
        CurrentSkill = null;
        skillIcon.gameObject.SetActive(false);
        skillText.gameObject.SetActive(false);
        skillReady = false;
    }

    
    public IEnumerator Cooldown(float time)
    {
        // skillBar.gameObject.SetActive(true);

        float t = 0f;

        activeColor = skillBar.fillRect.GetComponent<Image>().color;


        //yield return new WaitForSeconds(time);       


        while (t < 1f)
        {
            yield return null;           

            if (t <= 0.5f) skillIcon.color = activeColor;
            else skillIcon.color = new Color(t, t, t, t);

            t += Time.deltaTime / time;

            if (t >= 0.5f) skillBar.gameObject.SetActive(true);

            skillBar.value = t;

            // Inserire qui ripristino icona, messaggio etc...
            // Provare barra di caricamento 

        }

        skillBar.gameObject.SetActive(false);

        // Debug.Log("Skill PRONTA");
        skillReady = true;
        skillIcon.color = Color.white;
    }
}