using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuManager : MonoBehaviour {

	private List<Player> players;	

	private GameObject menuBody;
	private GameObject menuTabs;

	// BODY
	private GameObject statusBody;
		private GameObject target;
	private GameObject skillsBody;
	private GameObject inventoryBody;
		private GameObject inventoryDescription;
	private GameObject craftBody;
		private GameObject craftDescription;
	private GameObject saveLoadBody;
		private GameObject savingGames;
		private GameObject loadingGames;
	private GameObject nonTargetPlayers;

	// TABS
	private GameObject statusTab;
	private GameObject skillsTab;
	private GameObject inventoryTab;
	private GameObject craftTab;
	private GameObject saveLoadTab;

	private bool initialized = false;

	private static PauseMenuManager instance = null;
	public static PauseMenuManager Instance{
		get{
			if (instance == null){
				instance = new GameObject("PauseMenuManager").AddComponent<PauseMenuManager>();
				instance.initialize();
				DontDestroyOnLoad(instance.gameObject);
			}
				
			return instance;
		}
	}

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetButtonDown("Quit Menu")){
			//Application.LoadLevel("Forest");
		}
	}

	private void initialize(){
		if(!initialized){
			instance.menuBody = GameObject.FindGameObjectWithTag("PauseMenuBody");
			instance.menuTabs = GameObject.FindGameObjectWithTag("PauseMenuTabs");

			//				BODY
			Transform bodyTransform = instance.menuBody.transform;

			instance.statusBody = bodyTransform.FindChild("StatusBody").gameObject;
				instance.target = bodyTransform.FindChild("StatusBody").FindChild("Target").gameObject;
			instance.skillsBody = bodyTransform.FindChild("SkillsBody").gameObject;
			instance.inventoryBody = bodyTransform.FindChild("InventoryBody").gameObject;
				instance.inventoryDescription = bodyTransform.FindChild("InventoryBody").FindChild("Description").gameObject;
			instance.craftBody = bodyTransform.FindChild("CraftBody").gameObject;
				instance.craftDescription = bodyTransform.FindChild("CraftBody").FindChild("Description").gameObject;
			instance.saveLoadBody = bodyTransform.FindChild("SaveLoadBody").gameObject;
				instance.savingGames = bodyTransform.FindChild("SaveLoadBody").FindChild("SavingGames").gameObject;
				instance.loadingGames = bodyTransform.FindChild("SaveLoadBody").FindChild("LoadingGames").gameObject;
			instance.nonTargetPlayers = bodyTransform.FindChild("NonTargetPlayers").gameObject;

			//				TABS
			Transform tabTransform = instance.menuTabs.transform;

			instance.statusTab = tabTransform.FindChild("StatusTab").gameObject;
			instance.skillsTab = tabTransform.FindChild("SkillsTab").gameObject;
			instance.inventoryTab = tabTransform.FindChild("InventoryTab").gameObject;
			instance.craftTab = tabTransform.FindChild("CraftTab").gameObject;
			instance.saveLoadTab = tabTransform.FindChild("SaveLoadTab").gameObject;

			translateTabs();
			translateBody();

			hideAll();			

			this.gameObject.SetActive(false);

			Debug.Log("PauseMenuManager initialized.");

			initialized = true;
		}		
	}

	private void setPlayers(){
		Player player;
		players = new List<Player>();

		foreach(PlayerData data in Gamestate.instance.playersData){
			player = buildPlayer(data);
			player.populate(data);
			instance.players.Add(player);
		}
	}

	private Player buildPlayer(PlayerData data){
		return GameObject.FindGameObjectWithTag(data.characterName).GetComponent<Player>();
	}

	private void hideAll(){
		this.gameObject.SetActive(true);
		hideBody(instance.statusBody);		
		hideBody(instance.skillsBody);
		hideBody(instance.inventoryBody);
		hideBody(instance.craftBody);
		hideBody(instance.saveLoadBody);
		hideBody(instance.nonTargetPlayers);
	}

	public void showStatus(){
		hideAll();

		Player target = instance.players[0];

		fillTarget(target);
		fillNonTargets();

		instance.statusBody.SetActive(true);
		instance.nonTargetPlayers.SetActive(true);
	}

	private void fillTarget(Player targetPlayer){
		Transform basicInfoTransform = instance.target.transform.FindChild("BasicInfo");
		basicInfoTransform.FindChild("PlayerPortrait").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Portraits/" + targetPlayer.name + "Portrait");
		basicInfoTransform.FindChild("PlayerName").GetComponent<Text>().text = targetPlayer.characterName;
		basicInfoTransform.FindChild("PlayerLevel").GetComponent<Text>().text = "Lv " + targetPlayer.level.ToString();
		basicInfoTransform.FindChild("Health").FindChild("HealthBar").GetComponent<Image>().fillAmount = (targetPlayer.currHP / targetPlayer.maxHP);
		basicInfoTransform.FindChild("Mana").FindChild("ManaBar").GetComponent<Image>().fillAmount = (targetPlayer.currMP / targetPlayer.maxMP);
		basicInfoTransform.FindChild("Exp").FindChild("ExpBar").GetComponent<Image>().fillAmount = (targetPlayer.exp / targetPlayer.expForNextLevel);
		basicInfoTransform.FindChild("Exp").FindChild("CurrentExp").GetComponent<Text>().text = "(" + targetPlayer.exp +  "/" + targetPlayer.expForNextLevel + ")";

		Transform basicStatsTransform = instance.target.transform.FindChild("BasicStats");
		basicStatsTransform.FindChild("STR").FindChild("Value").GetComponent<Text>().text = targetPlayer.str.ToString();
		basicStatsTransform.FindChild("VIT").FindChild("Value").GetComponent<Text>().text = targetPlayer.vit.ToString();
		basicStatsTransform.FindChild("AGI").FindChild("Value").GetComponent<Text>().text = targetPlayer.agi.ToString();
		basicStatsTransform.FindChild("INT").FindChild("Value").GetComponent<Text>().text = targetPlayer.itg.ToString();
		basicStatsTransform.FindChild("DEX").FindChild("Value").GetComponent<Text>().text = targetPlayer.dex.ToString();
		basicStatsTransform.FindChild("LUK").FindChild("Value").GetComponent<Text>().text = targetPlayer.luk.ToString();

		Transform secondStatsTransform = instance.target.transform.FindChild("SecondStats");
		secondStatsTransform.FindChild("ATK").FindChild("Value").GetComponent<Text>().text = targetPlayer.atk.ToString();
		secondStatsTransform.FindChild("MATK").FindChild("Value").GetComponent<Text>().text = targetPlayer.matk.ToString();
		secondStatsTransform.FindChild("DEF").FindChild("Value").GetComponent<Text>().text = targetPlayer.def.ToString();
		secondStatsTransform.FindChild("MDEF").FindChild("Value").GetComponent<Text>().text = targetPlayer.mdef.ToString();
		secondStatsTransform.FindChild("HIT").FindChild("Value").GetComponent<Text>().text = targetPlayer.hit.ToString();
		secondStatsTransform.FindChild("FLEE").FindChild("Value").GetComponent<Text>().text = targetPlayer.flee.ToString();
		secondStatsTransform.FindChild("CRITCH").FindChild("Value").GetComponent<Text>().text = targetPlayer.critChance.ToString();
		secondStatsTransform.FindChild("CRITDMG").FindChild("Value").GetComponent<Text>().text = targetPlayer.critDmg.ToString();
		secondStatsTransform.FindChild("HP REG").FindChild("Value").GetComponent<Text>().text = targetPlayer.hpRegen.ToString();
		secondStatsTransform.FindChild("MP REG").FindChild("Value").GetComponent<Text>().text = targetPlayer.mpRegen.ToString();

		/*GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicInfo/PlayerPortrait").transform.GetComponent<Image>().sprite = Resources.Load <Sprite> ("Portraits/" + targetPlayer.name + "Portrait");
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicInfo/PlayerName").transform.GetComponent<Text>().text = targetPlayer.characterName;
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicInfo/PlayerLevel").transform.GetComponent<Text>().text = "Lv " + targetPlayer.level.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicInfo/Health/HealthBar").transform.GetComponent<Image>().fillAmount = (targetPlayer.currHP / targetPlayer.maxHP);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicInfo/Mana/ManaBar").transform.GetComponent<Image>().fillAmount = (targetPlayer.currMP / targetPlayer.maxMP);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicInfo/Exp/ExpBar").transform.GetComponent<Image>().fillAmount = (targetPlayer.exp / targetPlayer.expForNextLevel);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicInfo/Exp/CurrentExp").transform.GetComponent<Text>().text = "(" + targetPlayer.exp +  "/" + targetPlayer.expForNextLevel + ")";

		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicStats/STR/Value").transform.GetComponent<Text>().text = targetPlayer.str.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicStats/VIT/Value").transform.GetComponent<Text>().text = targetPlayer.vit.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicStats/AGI/Value").transform.GetComponent<Text>().text = targetPlayer.agi.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicStats/INT/Value").transform.GetComponent<Text>().text = targetPlayer.itg.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicStats/DEX/Value").transform.GetComponent<Text>().text = targetPlayer.dex.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/BasicStats/LUK/Value").transform.GetComponent<Text>().text = targetPlayer.luk.ToString();

		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/ATK/Value").transform.GetComponent<Text>().text = targetPlayer.atk.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/MATK/Value").transform.GetComponent<Text>().text = targetPlayer.matk.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/DEF/Value").transform.GetComponent<Text>().text = targetPlayer.def.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/MDEF/Value").transform.GetComponent<Text>().text = targetPlayer.mdef.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/HIT/Value").transform.GetComponent<Text>().text = targetPlayer.hit.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/FLEE/Value").transform.GetComponent<Text>().text = targetPlayer.flee.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/CRITCH/Value").transform.GetComponent<Text>().text = targetPlayer.critChance.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/CRITDMG/Value").transform.GetComponent<Text>().text = targetPlayer.critDmg.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/HP REG/Value").transform.GetComponent<Text>().text = targetPlayer.hpRegen.ToString();
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/StatusBody/Target/SecondStats/MP REG/Value").transform.GetComponent<Text>().text = targetPlayer.mpRegen.ToString();*/
	}

	private void fillNonTargets(){
		if(instance.players.Count > 1){
			Player nonTarget;
			for(int i = 1; i < instance.players.Count; i++){
				nonTarget = instance.players[i];
				instance.nonTargetPlayers.transform.FindChild("Player"+i).FindChild("PlayerPortrait").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Portraits/" + nonTarget.name + "Portrait");
				instance.nonTargetPlayers.transform.FindChild("Player"+i).FindChild("PlayerName").GetComponent<Text>().text = nonTarget.characterName;
				instance.nonTargetPlayers.transform.FindChild("Player"+i).FindChild("PlayerLevel").GetComponent<Text>().text = "Lv " + nonTarget.level.ToString();
			}

			if(instance.players.Count == 2){
				instance.nonTargetPlayers.transform.FindChild("Player2").gameObject.SetActive(false);
			}
		}
		else{
			instance.nonTargetPlayers.transform.FindChild("Player1").gameObject.SetActive(false);
			instance.nonTargetPlayers.transform.FindChild("Player2").gameObject.SetActive(false);
		}
	}

	public void showSkills(){
		hideAll();

		instance.skillsBody.SetActive(true);
		instance.nonTargetPlayers.SetActive(true);
	}

	public void showInventory(){
		hideAll();

		instance.inventoryBody.SetActive(true);
		instance.inventoryDescription.SetActive(false);

		setItems();
	}

	private void setItems(){
		List<Item> items = Singleton.Instance.inventory.getItems();

		GameObject[] itemsGameObjects = GameObject.FindGameObjectsWithTag("Item");

		int i = 0;
		foreach (GameObject item in itemsGameObjects){
			if(i < items.Count){
				setItemInfo(items[i], itemsGameObjects[i]);
			}
			else{
				itemsGameObjects[i].SetActive(false);	
			}
			i++;
		}
	}

	private void setItemInfo(Item item, GameObject itemGameObject){
		itemGameObject.GetComponent<Item>().populate(item.id);

		itemGameObject.transform.FindChild("Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + item.id);
		itemGameObject.transform.FindChild("ItemName").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(item.id);
		itemGameObject.transform.FindChild("ItemQuantity").GetComponent<Text>().text = "x" + item.quantity.ToString();
	}

	//				NO HACER CON GAMEOBJECT.FIND

	public void setItemDescription(GameObject itemGameObject){
		instance.inventoryDescription.SetActive(true);

		Item item = itemGameObject.GetComponent<Item>();

		GameObject.Find("Gamestate/PauseMenuCanvas/Body/InventoryBody/Description/Item/Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + item.id);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/InventoryBody/Description/Item/Name").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(item.id);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/InventoryBody/Description/Item/Type").GetComponent<Text>().text = item.type;
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/InventoryBody/Description/Item/Description").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(item.description);
	}

	public void showCraft(){
		hideAll();

		List<Craft> recipes = CraftManager.Instance.getRecipes();

		instance.craftBody.SetActive(true);
		instance.craftDescription.SetActive(false);

		showRecipes(recipes);
	}

	private void showRecipes(List<Craft> recipes){
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/FirstColumn/Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/FirstColumn/Text").GetComponent<Text>().text);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/SecondColumn/Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/SecondColumn/Text").GetComponent<Text>().text);

		GameObject[] recipesGameObjects = GameObject.FindGameObjectsWithTag("Recipe");

		int i = 0;
		foreach (GameObject recipe in recipesGameObjects){
			if(i < recipes.Count){
				Debug.Log(recipes[i].result);
				setRecipeInfo(recipes[i], recipesGameObjects[i]);
			}
			else{
				recipesGameObjects[i].SetActive(false);	
			}
			i++;
		}

		GameObject[] materialsGameObjects = GameObject.FindGameObjectsWithTag("Material");

		foreach (GameObject material in materialsGameObjects){
			material.SetActive(false);
		}

		instance.craftDescription.SetActive(false);
	}

	private void setRecipeInfo(Craft recipe, GameObject recipeGameObject){
		instance.craftDescription.SetActive(true);

		recipeGameObject.GetComponent<Item>().populate(recipe.result);

		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/FirstColumn/Recipe/Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + recipe.result);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/FirstColumn/Recipe/ItemName").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.result);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/FirstColumn/Recipe/ItemQuantity").GetComponent<Text>().text = recipe.resultQuantity.ToString();
	}	

	public void setRecipeDescription(GameObject recipeGameObject){
		//Item item = Singleton.Instance.inventory.getItem(recipe.result);
		instance.craftDescription.SetActive(true);

		Item recipe = recipeGameObject.GetComponent<Item>();

		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + recipe.id);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Name").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.id);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Type").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.type);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Description").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.description);
	}	

	public void showSaveLoad(){
		hideAll();		

		instance.savingGames.SetActive(false);
		instance.loadingGames.SetActive(false);
		instance.saveLoadBody.SetActive(true);
	}

	public void clickSaveGameButton(){
		if(instance.savingGames.activeInHierarchy){
			hideSavingGames();
		}
		else{
			showSavingGames();
		}
	}

	public void clickLoadGameButton(){
		if(instance.loadingGames.activeInHierarchy){
			hideLoadingGames();
		}
		else{
			showLoadingGames();
		}
	}

	private void showSavingGames(){
		instance.savingGames.SetActive(true);
	}

	private void showLoadingGames(){
		instance.loadingGames.SetActive(true);
	}

	private void hideSavingGames(){
		instance.savingGames.SetActive(false);
	}

	private void hideLoadingGames(){
		instance.loadingGames.SetActive(false);
	}

	private void hideBody(GameObject body){
		body.SetActive(false);
	}

	public void hideCanvas(){
		menuBody.SetActive(false);
		menuTabs.SetActive(false);
	}

	public void showCanvas(){
		setPlayers();
		menuBody.SetActive(true);
		menuTabs.SetActive(true);
		showStatus();
	}

	private void translateTabs(){
		Text statusText = GameObject.Find("Gamestate/PauseMenuCanvas/Tabs/StatusTab/Text").GetComponent<Text>();
		statusText.text = LanguageManager.Instance.getMenuText(statusText.text);
		//instance.statusTab.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.statusTab.transform.FindChild("Text").GetComponent<Text>().text);
		instance.skillsTab.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.skillsTab.transform.FindChild("Text").GetComponent<Text>().text);
		instance.inventoryTab.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.inventoryTab.transform.FindChild("Text").GetComponent<Text>().text);
		instance.craftTab.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.craftTab.transform.FindChild("Text").GetComponent<Text>().text);
		instance.saveLoadTab.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.saveLoadTab.transform.FindChild("Text").GetComponent<Text>().text);
	}

	private void translateBody(){
		statusBody.transform.FindChild("Target").FindChild("AdditionalStats").FindChild("RemainingStatPoints").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(statusBody.transform.FindChild("Target").FindChild("AdditionalStats").FindChild("RemainingStatPoints").FindChild("Text").GetComponent<Text>().text);
		statusBody.transform.FindChild("Target").FindChild("AdditionalStats").FindChild("RemainingSkillPoints").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(statusBody.transform.FindChild("Target").FindChild("AdditionalStats").FindChild("RemainingSkillPoints").FindChild("Text").GetComponent<Text>().text);
	}
}