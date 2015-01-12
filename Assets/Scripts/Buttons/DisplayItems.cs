using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DisplayItems : MonoBehaviour {
	public static int MAX_ITEMS_UI = 4;
	public static string DEFAULT_ITEM_TEXT = "Item";
	public static int ALLOCATE_HEIGHT = 15;
	
	public static List<GameObject> itemButtons;
	public static bool finished = false;
	
	BattleManager bm;

	List<Item> items;
	
	Vector2 parentPosition;
	
	int numInactives = 0;
	
	void Start () {
		bm = GameObject.Find("BattleCanvas").GetComponent<BattleManager>();
		this.gameObject.SetActive(false);

		itemButtons = new List<GameObject>();

		parentPosition = this.transform.position;
	}
	
	void Update () {
		if(bm != null && bm.isPlayerTurn()){
			items = Singleton.inventory.getUsableItems();			
			
			populateButtons();
			allocatePanel();
		}
	}
	
	private void populateButtons(){

		if(items != null){
			if(!finished){
				int i = 0;
				clean();
				if(items.Count > 0){
					foreach(Item item in items){
						if(i <= MAX_ITEMS_UI){
							GameObject instance = createInstance();
							string text = item.name + " x" + item.quantity;
							setTextToButton(instance, text);
							
							if(sameAsLast(i) || text.Equals(DEFAULT_ITEM_TEXT)){
								itemButtons[i].SetActive(false);
								numInactives++;
							}
							i++;
						}					
					}
				}
				else{
					GameObject instance = createInstance();
					setTextToButton(instance, Inventory.EMPTY);
					instance.GetComponent<getItemButtonText>().enabled = false;
				}

				finished = true;
			}
		}
	}

	private GameObject createInstance(){
		GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/Buttons/ItemButton"));
		instance.transform.SetParent(this.transform);
		itemButtons.Add(instance);

		return instance;
	}

	private void setTextToButton(GameObject instance, string txt){
		Button button = instance.GetComponent<Button>();
		Text text = button.GetComponentInChildren<Text>();
		text.text = txt;
	}

	private void clean(){
		Destroy (GameObject.Find("ItemButton(Clone)"));
		itemButtons.Clear();
	}
	
	private bool sameAsLast(int position){
		if(position > 0){
			string last = getItemName(itemButtons[position-1].GetComponentInChildren<Text>().text);
			string actual = getItemName(itemButtons[position].GetComponentInChildren<Text>().text);

			Debug.Log ("LAST: " + last + ", ACTUAL: " + actual);

			if(last.Equals(actual)){
				Debug.Log ("TRUE");
				return true;
			}
		}
		
		return false;
	}
	
	private void allocatePanel(){
		numInactives = MAX_ITEMS_UI - itemButtons.Count;

		this.transform.position = new Vector2(this.transform.position.x, parentPosition.y + (ALLOCATE_HEIGHT*numInactives));

		numInactives = 0;
	}

	public static void repopulate(){
		finished = false;
		itemButtons.Clear();
	}

	private string getItemName(string text){
		string[] aux = text.Split(' ');

		return aux[0];
	}
}
