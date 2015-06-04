using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DisplaySkills : MonoBehaviour {
	public static int MAX_SKILLS_UI = 4;
	public static string DEFAULT_SKILL_TEXT = "Skill";
	public static int ALLOCATE_HEIGHT = 15;

	public List<GameObject> skillButtons;
	public bool finished = false;

	public BattleManager bm;

	public Player player;
	public List<Skill> skills;

	public Vector2 parentPosition;

	public int numInactives = 0;

	private LanguageManager languageManager;

	void Start () {
		bm = BattleManager.Instance;
		languageManager = LanguageManager.Instance;
		//this.gameObject.SetActive(false);

		skillButtons = new List<GameObject>();

		parentPosition = this.transform.position;
	}

	void Update () {
		if(bm != null && bm.isPlayerTurn()){
			player = bm.getCurrentPlayer();
			if(player != null){
				skills = player.getSkills();

				populateButtons();
				allocatePanel();
			}
		}
	}

	private void populateButtons(){
		if(skills != null){
			if(!finished){
				int i = 0;
				if(skills.Count > 0){
					string text;
					foreach(Skill skill in skills){
						if(i <= MAX_SKILLS_UI){
							GameObject instance = createInstance();
							text =  languageManager.getMenuText(skill.id) + " Lv. " + skill.currLevel;
							setTextToButton(instance, text);
							instance.name = skill.id;
							//instance.AddComponent<Skill>();
							//instance.GetComponent<Skill>().id = skill.id;
							
							if(sameAsLast(i) || text.Equals(DEFAULT_SKILL_TEXT)){
								skillButtons[i].SetActive(false);
								numInactives++;
							}
							i++;
						}
					}
				}
				else{
					GameObject instance = createInstance();
					setTextToButton(instance, LanguageManager.Instance.getMenuText(Skill.EMPTY));
					instance.GetComponent<getSkillButtonText>().enabled = false;
				}

				finished = true;
			}
		}
	}
	
	private GameObject createInstance(){
		GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/Buttons/SkillButton"));
		instance.transform.SetParent(this.transform);
		skillButtons.Add(instance);

		return instance;
	}
	
	private void setTextToButton(GameObject instance, string txt){
		Button button = instance.GetComponent<Button>();
		Text text = button.GetComponentInChildren<Text>();
		text.text = txt;
	}

	private bool sameAsLast(int position){
		if(position != 0){
			string last = skillButtons[position-1].GetComponentInChildren<Text>().text;
			string actual = skillButtons[position].GetComponentInChildren<Text>().text;

			return last.Equals(actual);
		}

		return false;
	}

	private void allocatePanel(){
		numInactives = MAX_SKILLS_UI - skillButtons.Count;

		this.transform.position = new Vector2(this.transform.position.x, parentPosition.y + (ALLOCATE_HEIGHT*numInactives));
		
		numInactives = 0;
	}
}
