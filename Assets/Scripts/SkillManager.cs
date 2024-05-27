using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    // VEDERE SE METTERE ANCHE COOLDOWN QUI
    // INSERIRE FORSE STATI PER LE VARIE SKILL; COPIA LOADOUT MANAGER

    public GameObject[] skills;

    private int current = 0;

    private GameObject _currentSkill;

    public event Action<GameObject> OnSkillChanged;


    public GameObject CurrentSkill
    {
        get => _currentSkill;

        protected set
        {
            if (_currentSkill == value)
            {
                // Debug.Log("Current skill: " + _currentSkill.name);
                return;
            }

            _currentSkill = value;

            skill = CurrentSkill.GetComponent<IPowerup>();

            skillText.text = CurrentSkill.name;
            skillIcon.sprite = CurrentSkill.GetComponent<Image>().sprite;
            skillIcon.color = Color.white;


            // Debug.Log("New current skill: " + _currentSkill.name);

            OnSkillChanged?.Invoke(_currentSkill);
        }
    }


    [Header("Skills")]
    // public GameObject activeSkill;
    public IPowerup skill;
    public float skillCooldown = 10f;
    public bool skillReady = true;
    public Slider skillBar;
    public TextMeshProUGUI skillText;
    public Image skillIcon;

    public static SkillManager instance;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // skill = activeSkill.GetComponent<IPowerup>();

        CurrentSkill = skills[current];

        // skill = CurrentSkill.GetComponent<IPowerup>();
    }


    // UPDATE UTILIZZATO SOLO PER TEST
    // LE SKILL NON POTRANNO ESSERE CAMBIATE DURANTE LA PARTITA COME LE ARMI
    void Update()
    {
        if (skillReady)
        {
            if (Input.GetButtonDown("NextWeapon") || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                ChangeSkill(true);
            }
            else if (Input.GetButtonDown("PrevWeapon") || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                ChangeSkill(false);
            }
        }


    }

    private void ChangeSkill(bool next)
    {

        if (next)
        {
            skills[current].SetActive(false);
            current++;
            // Debug.Log(current);
            if (current >= skills.Length)
            {
                current = 0;
            }
            // Debug.Log("AUM// SKILL " + current + ": " + skills[current].gameObject.name);
            skills[current].SetActive(true);
            CurrentSkill = skills[current];
        }

        else
        {
            skills[current].SetActive(false);
            current--;
            //  Debug.Log(current);
            if (current < 0)
            {
                //  Debug.Log(current);
                current = skills.Length - 1;
            }
            //  Debug.Log("DEC// SKILL " + current+": " + skills[current].gameObject.name);
            skills[current].SetActive(true);
            CurrentSkill = skills[current];
        }

        // Debug.Log("cambio skill completato");

    }


    public IEnumerator Cooldown(float time)
    {
        // skillBar.gameObject.SetActive(true);

        float t = 0f;


        //yield return new WaitForSeconds(time);       


        while (t < 1f)
        {
            yield return null;

            

            if (t <= 0.5f) skillIcon.color = Color.green;
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