using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PauseMenuManager : MonoBehaviour {

	private Sprite NORMAL_BUTTON;
	private Sprite ACTIVE_BUTTON;

	private List<Player> players;	

	private GameObject menuBody;
	private GameObject menuTabs;
	private GameObject menuLogo;

	// BODY
	private GameObject statusBody;
		private GameObject target;
		private GameObject additionalStats;
		private GameObject[] statButtons;
	private GameObject skillsBody;
		private GameObject skillsTarget;
		private GameObject skillTabs;
		private GameObject[] skillItems;
		private GameObject[] skillButtons;
	private GameObject inventoryBody;
		private GameObject inventoryDescription;
        private GameObject inventoryUseButton;
		private GameObject[] itemsGameObjects;
	private GameObject craftBody;
		private GameObject targetRecipe;
		private GameObject craftDescription;
		private GameObject[] recipesGameObjects;
		private GameObject[] materialsGameObjects;
		private GameObject createButton;
	private GameObject saveLoadBody;
		private GameObject savingGames;
		private GameObject[] savingGamesSavedGames;
		private GameObject loadingGames;
		private GameObject[] loadingGamesSavedGames;
		private GameObject saveGameButton;
		private GameObject loadGameButton;

	public GameObject currentButton;
	public GameObject currentTab;

	private GameObject nonTargetPlayers;

	// TABS
	private GameObject statusTab;
	private GameObject skillsTab;
	private GameObject inventoryTab;
	private GameObject craftTab;
	private GameObject saveLoadTab;

	private List<SaveData> savegames;

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

	private void initialize(){
		if(!initialized){
			NORMAL_BUTTON = Resources.Load<Sprite>("Buttons/inactive");
			ACTIVE_BUTTON = Resources.Load<Sprite>("Buttons/active");

			instance.menuBody = GameObject.FindGameObjectWithTag("PauseMenuBody");
			instance.menuTabs = GameObject.FindGameObjectWithTag("PauseMenuTabs");
			instance.menuLogo = GameObject.Find("Gamestate/PauseMenuCanvas/Body/Logo");

			//				BODY
			Transform bodyTransform = instance.menuBody.transform;

			instance.statusBody = bodyTransform.FindChild("StatusBody").gameObject;
				instance.target = bodyTransform.FindChild("StatusBody").FindChild("Target").gameObject;
				instance.additionalStats = bodyTransform.FindChild("StatusBody").FindChild("Target").FindChild("AdditionalStats").gameObject;
				instance.statButtons = GameObject.FindGameObjectsWithTag("StatButton");
			instance.skillsBody = bodyTransform.FindChild("SkillsBody").gameObject;
				instance.skillsTarget = bodyTransform.FindChild("SkillsBody").FindChild("Target").gameObject;
				instance.skillTabs = bodyTransform.FindChild("SkillsBody").FindChild("SkillsFrame").FindChild("Tabs").gameObject;
				instance.skillItems = GameObject.FindGameObjectsWithTag("Skill");
				instance.skillButtons = GameObject.FindGameObjectsWithTag("SkillButton");
			instance.inventoryBody = bodyTransform.FindChild("InventoryBody").gameObject;
				instance.inventoryDescription = bodyTransform.FindChild("InventoryBody").FindChild("Description").gameObject;
				instance.inventoryUseButton = instance.inventoryDescription.transform.FindChild("Button").gameObject;			
				instance.inventoryUseButton.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getWord(instance.inventoryUseButton.transform.FindChild("Text").GetComponent<Text>().text).getText();
			instance.craftBody = bodyTransform.FindChild("CraftBody").gameObject;
				instance.craftDescription = bodyTransform.FindChild("CraftBody").FindChild("Description").gameObject;
			instance.saveLoadBody = bodyTransform.FindChild("SaveLoadBody").gameObject;
				instance.savingGames = bodyTransform.FindChild("SaveLoadBody").FindChild("SavingGames").gameObject;
				instance.loadingGames = bodyTransform.FindChild("SaveLoadBody").FindChild("LoadingGames").gameObject;
				instance.savingGamesSavedGames = GameObject.FindGameObjectsWithTag("SavingGames");
				instance.loadingGamesSavedGames = GameObject.FindGameObjectsWithTag("LoadingGames");
				instance.saveGameButton = bodyTransform.FindChild("SaveLoadBody").FindChild("SaveButton").gameObject;
				instance.loadGameButton = bodyTransform.FindChild("SaveLoadBody").FindChild("LoadButton").gameObject;
			instance.nonTargetPlayers = bodyTransform.FindChild("NonTargetPlayers").gameObject;

			//				TABS
			Transform tabTransform = instance.menuTabs.transform;

			instance.statusTab = tabTransform.FindChild("StatusTab").gameObject;
			instance.skillsTab = tabTransform.FindChild("SkillsTab").gameObject;
			instance.inventoryTab = tabTransform.FindChild("InventoryTab").gameObject;
			instance.craftTab = tabTransform.FindChild("CraftTab").gameObject;
			instance.saveLoadTab = tabTransform.FindChild("SaveLoadTab").gameObject;

			//instance.players = new List<Player>();

			translateTabs();
			translateBody();

			//hideAll();			

			this.gameObject.SetActive(false);

			Debug.Log("PauseMenuManager initialized.");

			initialized = true;
		}		
	}

	private void setPlayers(){
		Player player;
		instance.players = new List<Player>();

		foreach(PlayerData data in Gamestate.instance.playersData){
			player = buildPlayer(data);
			player.populate(data);
			instance.players.Add(player);
		}
	}

	private Player buildPlayer(PlayerData data){
		return GameObject.FindGameObjectWithTag(data.characterName).GetComponent<Player>();
	}

	public Player getPlayer(string name){
		return instance.players.Where<Player>(x => x.characterName == name).FirstOrDefault();
	}

	private void hideAll(){
		this.gameObject.SetActive(true);
		hideBody(instance.statusBody);		
		hideBody(instance.skillsBody);
		hideBody(instance.inventoryBody);
		hideBody(instance.craftBody);
		hideBody(instance.saveLoadBody);
		hideBody(instance.nonTargetPlayers);

		if(currentButton != null)
			currentButton.GetComponent<Image>().sprite = NORMAL_BUTTON;

		//currentButton = null;
	}

	#region status

	public void showStatus(){
		hideAll();
		
		instance.statusBody.SetActive(true);
		instance.nonTargetPlayers.SetActive(true);

		Player targetPlayer = instance.players[0];

		if(instance.target.GetComponent<Player>() != null)
			instance.target.GetComponent<Player>().populate(targetPlayer.getData());
		else
			instance.target.AddComponent<Player>().populate(targetPlayer.getData());

		fillTarget(targetPlayer, "Status");
		fillNonTargets();

		if (targetPlayer.statPoints > 0)
			showStatButtons();
		else
			hideStatButtons();
	}

	private void showStatButtons() {
		foreach (GameObject button in instance.statButtons)
			button.SetActive(true);
	}

	private void hideStatButtons() {
		foreach (GameObject button in instance.statButtons)
			button.SetActive(false);
	}

	private void fillTarget(Player targetPlayer, string from){
		if(from.Equals("Status")) {		
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
			basicStatsTransform.FindChild("ITG").FindChild("Value").GetComponent<Text>().text = targetPlayer.itg.ToString();
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

			instance.additionalStats.transform.FindChild("RemainingStatPoints").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.additionalStats.transform.FindChild("RemainingStatPoints").FindChild("Text").GetComponent<Text>().text);
			instance.additionalStats.transform.FindChild("RemainingSkillPoints").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.additionalStats.transform.FindChild("RemainingSkillPoints").FindChild("Text").GetComponent<Text>().text);

			instance.additionalStats.transform.FindChild("RemainingStatPoints").FindChild("Value").GetComponent<Text>().text = targetPlayer.statPoints.ToString();
			instance.additionalStats.transform.FindChild("RemainingSkillPoints").FindChild("Value").GetComponent<Text>().text = targetPlayer.skillPoints.ToString();
		}
		else if(from.Equals("Skills")){
			Transform basicInfoTransform = instance.skillsTarget.transform;
			basicInfoTransform.FindChild("PlayerPortrait").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Portraits/" + targetPlayer.characterName + "Portrait");
			basicInfoTransform.FindChild("PlayerName").GetComponent<Text>().text = targetPlayer.characterName;
			basicInfoTransform.FindChild("PlayerLevel").GetComponent<Text>().text = "Lv " + targetPlayer.level.ToString();

			basicInfoTransform.FindChild("RemainingStatPoints").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(basicInfoTransform.FindChild("RemainingStatPoints").FindChild("Text").GetComponent<Text>().text);
			basicInfoTransform.FindChild("RemainingSkillPoints").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(basicInfoTransform.FindChild("RemainingSkillPoints").FindChild("Text").GetComponent<Text>().text);

			basicInfoTransform.FindChild("RemainingStatPoints").FindChild("Value").GetComponent<Text>().text = targetPlayer.statPoints.ToString();
			basicInfoTransform.FindChild("RemainingSkillPoints").FindChild("Value").GetComponent<Text>().text = targetPlayer.skillPoints.ToString();
		}
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

	#endregion status

	#region skills

	public void showSkills() {
		hideAll();

		instance.skillsBody.SetActive(true);
		instance.nonTargetPlayers.SetActive(true);

		Player targetPlayer = instance.players[0];
		instance.skillsTarget.GetComponent<Player>().populate(targetPlayer.getData());

		fillTarget(targetPlayer, "Skills");
		fillNonTargets();

		showSkillTabs();
		hideSkills();

		if (targetPlayer.skillPoints > 0)
			showSkillButtons();
		else
			hideSkillButtons();
		//showCurrentSkills("mage_branch_destruction");
	}

	private void showSkillButtons() {
		foreach (GameObject button in instance.skillButtons)
			button.SetActive(true);
	}

	private void hideSkillButtons() {
		foreach (GameObject button in instance.skillButtons)
			button.SetActive(false);
	}

	public void showSkillTabs() {
		string target = instance.skillsTarget.GetComponent<Player>().characterName.ToLower();

		List<string[]> branches = Singleton.Instance.getBranches(target);
		GameObject tab;
		for(int i = 1; i <= 3; i++) {
			tab = GameObject.Find("Gamestate/PauseMenuCanvas/Body/SkillsBody/SkillsFrame/Tabs/Tab" + i);
			if(tab == null)
				tab = GameObject.Find("Gamestate/PauseMenuCanvas/Body/SkillsBody/SkillsFrame/Tabs/" + branches[i - 1][0]);

			tab.name = branches[i - 1][0];
			tab.transform.FindChild("Text").GetComponent<Text>().text = branches[i - 1][1];
		}
		
	}

	private void hideSkills() {
		foreach (GameObject skill in instance.skillItems)
			skill.SetActive(false);
	}

	public void showCurrentSkills(string branch) {
		Player target = instance.skillsTarget.GetComponent<Player>();
		List<Skill> skillsInBranch = target.getSkillsByBranch(branch);

		for (int i = 0; i < instance.skillItems.Length; i++) {
			if(i < skillsInBranch.Count) {
				instance.skillItems[i].SetActive(true);
				instance.skillItems[i].name = skillsInBranch[i].id;
				instance.skillItems[i].transform.FindChild("Name").GetComponent<Text>().text = skillsInBranch[i].name;
				instance.skillItems[i].transform.FindChild("Level").GetComponent<Text>().text = "Lv. " + skillsInBranch[i].currLevel.ToString();

				// MIRAR BIEN, ESTÁ LA BASE PERO DEPENDE DE LA SKILL DEBERÁ MOSTRAR UNA INFO U OTRA
				instance.skillItems[i].transform.FindChild("NextLevelInfo").GetComponent<Text>().text = skillsInBranch[i].info.getReadableEvo(skillsInBranch[i].currLevel);

				if(!skillsInBranch[i].canLevelUp())
					instance.skillItems[i].transform.FindChild("Button").gameObject.SetActive(false);
				else
					instance.skillItems[i].transform.FindChild("Button").gameObject.SetActive(true);
			}
			else {
				instance.skillItems[i].SetActive(false);
			}
		}
	}

	#endregion skills

	#region inventory

	public void showInventory(){
		hideAll();

		instance.inventoryBody.SetActive(true);

		/*if (instance.inventoryUseButton.activeInHierarchy){
			instance.inventoryUseButton.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.inventoryUseButton.transform.FindChild("Text").GetComponent<Text>().text);
		}*/

		instance.inventoryDescription.SetActive(false);

		if(instance.itemsGameObjects == null){
			instance.itemsGameObjects = GameObject.FindGameObjectsWithTag("Item");
		}

		setItems();
	}

	private void setItems(){
		List<Item> items = Singleton.Instance.inventory.getItems();		

		int i = 0;
		foreach (GameObject item in instance.itemsGameObjects){
			if(i < items.Count){
				instance.itemsGameObjects[i].SetActive(true);	
				instance.setItemInfo(items[i], instance.itemsGameObjects[i]);
			}
			else{
				instance.itemsGameObjects[i].SetActive(false);	
			}
			i++;
		}
	}

	private void setItemInfo(Item item, GameObject itemGameObject){
		itemGameObject.name = item.id;

		itemGameObject.transform.FindChild("Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + item.id);
		itemGameObject.transform.FindChild("ItemName").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(item.id);
		itemGameObject.transform.FindChild("ItemQuantity").GetComponent<Text>().text = "x" + item.quantity.ToString();

		// CHANGE COLOR IF ITEM IS NOT USABLE
		if(!item.isUsable()){
			//Debug.Log(item.toString());
			itemGameObject.transform.FindChild("ItemName").GetComponent<Text>().color = Color.grey;
			itemGameObject.transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.grey;
		}
	}

	public void setItemDescription(GameObject itemGameObject){
		instance.inventoryDescription.SetActive(true);

		Item item = Singleton.Instance.inventory.getItem(itemGameObject.name);

        if(item.isUsable() && item.isHealType()){
			instance.inventoryUseButton.SetActive(true);
        }
        else{
			instance.inventoryUseButton.SetActive(false);
        }

		inventoryDescription.transform.FindChild("Item").FindChild("Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + item.id);
		inventoryDescription.transform.FindChild("Item").FindChild("Name").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(item.id);
		inventoryDescription.transform.FindChild("Item").FindChild("Type").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(item.type);
		inventoryDescription.transform.FindChild("Item").FindChild("Description").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(item.description);
	}

    public void clickUseItem(string itemId){
		Player target = Gamestate.instance.getPlayer("Mage");

		Singleton.Instance.inventory.useItem(itemId, target);

		showInventory();

		if(Singleton.Instance.inventory.isItemInInventory(itemId)){
			instance.inventoryDescription.SetActive(true);
		}
    }

	#endregion inventory

	#region craft

    public void showCraft(){
		instance.hideAll();

		List<Craft> recipes = CraftManager.Instance.getRecipes();

		instance.craftBody.SetActive(true);
		instance.craftDescription.SetActive(false);

		if(instance.createButton == null){
			instance.createButton = GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Create/Button");
			instance.createButton.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.createButton.transform.FindChild("Text").GetComponent<Text>().text);
		}
		if(instance.materialsGameObjects != null){
			foreach(GameObject material in instance.materialsGameObjects){
				material.SetActive(false);
			}
		}
		
		instance.createButton.SetActive(false);

		//instance.targetRecipe = null;
		//instance.recipesGameObjects = null;
		//instance.materialsGameObjects = null;

		instance.showRecipes(recipes);
	}

	private void showRecipes(List<Craft> recipes){	
		instance.recipesGameObjects = GameObject.FindGameObjectsWithTag("Recipe");

		int i = 0;
		foreach (GameObject recipe in instance.recipesGameObjects){
			if(i < recipes.Count){
//				Debug.Log(recipes[i].result);
				instance.setRecipeInfo(recipes[i], instance.recipesGameObjects[i]);
			}
			else{
				instance.recipesGameObjects[i].SetActive(false);	
			}
			i++;
		}

		if(instance.materialsGameObjects == null){
			instance.materialsGameObjects = GameObject.FindGameObjectsWithTag("Material");
		}

		foreach (GameObject material in instance.materialsGameObjects){
			material.SetActive(false);
		}

		instance.craftDescription.SetActive(false);
	}

	private void setRecipeInfo(Craft recipe, GameObject recipeGameObject){
		//Craft materials = CraftManager.Instance.getRecipe(recipe.id);
		
		recipeGameObject.name = recipe.result;

		recipeGameObject.transform.FindChild("Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + recipe.result);
		recipeGameObject.transform.FindChild("ItemName").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.result);
		recipeGameObject.transform.FindChild("ItemQuantity").GetComponent<Text>().text = "x" + recipe.resultQuantity.ToString();

		if (recipe.hasMaterials()) {
			recipeGameObject.transform.FindChild("ItemName").GetComponent<Text>().color = Color.white;
			recipeGameObject.transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.white;
		}
		else {
			recipeGameObject.transform.FindChild("ItemName").GetComponent<Text>().color = Color.red;
			recipeGameObject.transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.red;
		}
	}	

	private void setRecipeDescription(){
		setRecipeDescription(instance.targetRecipe);
	}

	public void setRecipeDescription(GameObject recipeGameObject){
		// SETTING DESCRIPTION
		instance.targetRecipe = recipeGameObject;

		instance.craftDescription.SetActive(true);

		Item recipe = Singleton.Instance.inventory.getItem(recipeGameObject.name);//recipeGameObject.GetComponent<Item>();

		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + recipe.id);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Name").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.id);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Type").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.type);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/Description/Item/Description").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(recipe.description);

		// SETTING MATERIALS
		Craft materials = CraftManager.Instance.getRecipe(recipe.id);

		instance.materialsGameObjects[0].SetActive(true);
		instance.materialsGameObjects[0].name = materials.item1;		
		instance.materialsGameObjects[0].transform.FindChild("Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + materials.item1);
		instance.materialsGameObjects[0].transform.FindChild("ItemName").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(materials.item1);
		instance.materialsGameObjects[0].transform.FindChild("ItemQuantity").GetComponent<Text>().text = "x" + materials.item1Quantity.ToString();

		instance.materialsGameObjects[1].SetActive(true);
		instance.materialsGameObjects[1].name = materials.item2;
		instance.materialsGameObjects[1].transform.FindChild("Icon").GetComponent<Image>().sprite = Resources.Load <Sprite> ("Sprites/Items/" + materials.item2);
		instance.materialsGameObjects[1].transform.FindChild("ItemName").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(materials.item2);
		instance.materialsGameObjects[1].transform.FindChild("ItemQuantity").GetComponent<Text>().text = "x" + materials.item2Quantity.ToString();

		if(Singleton.Instance.inventory.hasItem(materials.item1, materials.item1Quantity) && Singleton.Instance.inventory.hasItem(materials.item2, materials.item2Quantity)){
			if(materials.item1.Equals(materials.item2)){
				if(Singleton.Instance.inventory.hasItem(materials.item1, materials.item1Quantity + materials.item2Quantity)){
					instance.createButton.SetActive(true);
				}
				else{
					instance.createButton.SetActive(false);
				}
			}else{
				instance.createButton.SetActive(true);
			}			
		}
		else{
			instance.createButton.SetActive(false);
		}

		// CHANGE COLOR IF ITEM IS NOT IN INVENTORY
		if(!Singleton.Instance.inventory.hasItem(materials.item1, materials.item1Quantity)){
			instance.materialsGameObjects[0].transform.FindChild("ItemName").GetComponent<Text>().color = Color.red;
			instance.materialsGameObjects[0].transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.red;
		}
		else{
			instance.materialsGameObjects[0].transform.FindChild("ItemName").GetComponent<Text>().color = Color.white;
			instance.materialsGameObjects[0].transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.white;
		}
		if(materials.item1.Equals(materials.item2)){
			if(!Singleton.Instance.inventory.hasItem(materials.item2, materials.item1Quantity + materials.item2Quantity)){
				instance.materialsGameObjects[1].transform.FindChild("ItemName").GetComponent<Text>().color = Color.red;
				instance.materialsGameObjects[1].transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.red;
			}
			else{
				instance.materialsGameObjects[1].transform.FindChild("ItemName").GetComponent<Text>().color = Color.white;
				instance.materialsGameObjects[1].transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.white;
			}
		}
		else{
			if(!Singleton.Instance.inventory.hasItem(materials.item2, materials.item2Quantity)){
				instance.materialsGameObjects[1].transform.FindChild("ItemName").GetComponent<Text>().color = Color.red;
				instance.materialsGameObjects[1].transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.red;
			}
			else{
				instance.materialsGameObjects[1].transform.FindChild("ItemName").GetComponent<Text>().color = Color.white;
				instance.materialsGameObjects[1].transform.FindChild("ItemQuantity").GetComponent<Text>().color = Color.white;
			}
		}
		
	}

	public void craftItem(){
		CraftManager.Instance.craft(instance.materialsGameObjects[0].name, instance.materialsGameObjects[1].name);

		setRecipeDescription();
	}

	#endregion craft

	#region saveLoad

	public void showSaveLoad(){
		hideAll();

		instance.saveLoadBody.SetActive(true);
		instance.savingGames.SetActive(true);
		instance.loadingGames.SetActive(true);	

		instance.savegames = SaveManager.Instance.getFormattedSavegames();
		instance.savingGamesSavedGames[0].transform.FindChild("Id").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.savingGamesSavedGames[0].transform.FindChild("Id").GetComponent<Text>().text);

		for (int i = 1; i < instance.savingGamesSavedGames.Length; i++){			
			if(i <= instance.savegames.Count)
				formatSavegame(instance.savingGamesSavedGames[i], instance.savegames[i-1], i);			
			else
				instance.savingGamesSavedGames[i].SetActive(false);			
		}

		for (int i = 0; i < instance.loadingGamesSavedGames.Length; i++){
			if(i < instance.savegames.Count)
				formatSavegame(instance.loadingGamesSavedGames[i], instance.savegames[i], i+1);			
			else
				instance.loadingGamesSavedGames[i].SetActive(false);			
		}

		instance.savingGames.SetActive(false);
		instance.loadingGames.SetActive(false);		
	}

	private void formatSavegame(GameObject slot, SaveData savegame, int id){
		slot.SetActive(true);
		Sprite sprite = null;
		//Debug.Log(id + " - " + slot.transform.GetComponentInChildren<Button>().gameObject.name);

		if(savegame.map.mapName.Contains("Forest"))
			sprite = Resources.Load <Sprite> ("Backgrounds/Battle/Forest"); 		
		else if(savegame.map.mapName.Contains("Castle"))
			sprite = Resources.Load <Sprite> ("Backgrounds/Battle/Castle");
		
		slot.transform.FindChild("Image").GetComponent<Image>().sprite = sprite;
		slot.transform.FindChild("Id").GetComponent<Text>().text = id + ". ";
		slot.transform.FindChild("Date").GetComponent<Text>().text = savegame.date.ToString("dd/MM/yyyy HH:mm");
		slot.transform.FindChild("MapName").GetComponent<Text>().text = "Map: " + savegame.map.mapName;
		if(savegame != null)
			slot.transform.GetComponentInChildren<Button>().gameObject.name = savegame.getPath();
		//slot.SetActive(true);
	}

	public void clickSaveGameButton(){
		if (instance.savingGames.activeInHierarchy)
			hideSavingGames();
		else
			showSavingGames();		
	}

	public void clickLoadGameButton(){
		if(instance.loadingGames.activeInHierarchy)
			hideLoadingGames();		
		else
			showLoadingGames();		
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

	#endregion saveLoad

	private void hideBody(GameObject body){
		body.SetActive(false);
	}

	public void hideCanvas(){
		if(instance.currentTab != null)
			instance.currentTab.GetComponent<Image>().sprite = NORMAL_BUTTON;

		instance.menuBody.SetActive(false);
		instance.menuTabs.SetActive(false);
		instance.menuLogo.SetActive(false);
	}

	public void showCanvas(){
		setPlayers();
		instance.menuBody.SetActive(true);
		instance.menuTabs.SetActive(true);
		instance.menuLogo.SetActive(true);
		initialize();

		showStatus();
		//instance.statusTab.transform.FindChild("Button").gameObject.GetComponent<ClickButton>().click();
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
		instance.craftBody.transform.FindChild("FirstColumn").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(craftBody.transform.FindChild("FirstColumn").FindChild("Text").GetComponent<Text>().text);
		instance.craftBody.transform.FindChild("SecondColumn").FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(craftBody.transform.FindChild("SecondColumn").FindChild("Text").GetComponent<Text>().text);

		/*GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/FirstColumn/Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/FirstColumn/Text").GetComponent<Text>().text);
		GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/SecondColumn/Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(GameObject.Find("Gamestate/PauseMenuCanvas/Body/CraftBody/SecondColumn/Text").GetComponent<Text>().text);*/
		
		instance.savingGames.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.savingGames.transform.FindChild("Text").GetComponent<Text>().text);
		instance.loadingGames.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.loadingGames.transform.FindChild("Text").GetComponent<Text>().text);
		instance.saveGameButton.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.saveGameButton.transform.FindChild("Text").GetComponent<Text>().text);
		instance.loadGameButton.transform.FindChild("Text").GetComponent<Text>().text = LanguageManager.Instance.getMenuText(instance.loadGameButton.transform.FindChild("Text").GetComponent<Text>().text);
	}

	public void clickSomeButton(GameObject button){
		if(instance.currentButton != null)
			instance.currentButton.GetComponent<Image>().sprite = NORMAL_BUTTON;		

		instance.currentButton = button;
		instance.currentButton.GetComponent<Image>().sprite = ACTIVE_BUTTON;
	}

	public void clickSomeTab(GameObject tab){
		//menuTabs.GetComponent<AudioSource>().Play();

		if(instance.currentTab != null)
			instance.currentTab.GetComponent<Image>().sprite = NORMAL_BUTTON;

		instance.currentTab = tab;
		instance.currentTab.GetComponent<Image>().sprite = ACTIVE_BUTTON;
	}

	public void clickSave(GameObject button){
		if (instance.currentButton != null && instance.currentButton == button){
			if(!button.name.Equals("Button"))
				SaveManager.Instance.saveData(button.name);
			else {
				if (instance.savegames.Count == 0)
					SaveManager.Instance.saveData(Application.dataPath + SaveManager.SAVE_PATH + "/New Savegame.sav");
				else
					SaveManager.Instance.saveData(Application.dataPath + SaveManager.SAVE_PATH + "/New Savegame("+instance.savegames.Count+").sav"); //
			}

			showSaveLoad();
			showSavingGames();
		}
	}

	public void clickLoad(GameObject button){
		if (instance.currentButton != null && instance.currentButton == button) {
			SaveManager.Instance.loadData(button.name);
		}
	}
}