//ExampleWindowScript.cs
//Alexander Young
//February 5, 2015
//Description - Creates the functionality to allow for in-app purchasing, specifically with reguards the GUI and using purchases to make changes to the game

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store.Example
{                                                                                                                                               //Allows for access to Soomla API                                                                              
	public class SoomlaElement : MonoBehaviour
	{      
		public Transform cube;                                                                                                                                                  // Stores the scene cube as a variable
		//secTime/floatTime are used to check for IAP changes every 2 seconds (ideally, you should only check for updates when you absoultely need to. This just shows a way check for the IAP changes)
		public float secTime                    = 2.0f;
		public float totTime                    = 0.0f;
		public bool greenCubeIAPOwned  = false;
		
		//Load the Scene with the cube/ setup the soomla intergration
		void Start ()
		{
			Application.LoadLevel ("test");                                                                                                                         //Load actual scene
			DontDestroyOnLoad(transform.gameObject);                                                                                                        //Allows this gameObject to remain during level loads, solving restart crashes
			StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreIntitialized;                                                      //Handle the initialization of store events (calls function below - unneeded in this case)
			SoomlaStore.Initialize (new SoomlaController());                                                                                           //Intialize the store
		}
		
		//this is likely unnecessary, but may be required depending on how you plan on doing IAPS
		public void onSoomlaStoreIntitialized(){

		}
		
		//ASSIGN CUBE TO BE COLORED
		void OnLevelWasLoaded(int level)
		{                                                                                                                                                                                               //Assigns the cube if the level is loaded to correct level{                                                                                    
			if (level == 1) {                                                                                                                                                       //the second level in the build list (0 == first level, 1 == second level)
				cube = GameObject.Find ("testCube").transform;                                                                                  //Assign cube by finding it the the hierarchy in the scene ( via its name)
			}
		}
		
		//UPDATE CUBE COLOR
		//Assign cube color based on it (using playerprefs) (see CheckIAP_PurchaseStatus() function below to understand)
		void Update ()
		{
			if (Time.timeSinceLevelLoad > totTime)
			{
				CheckIAP_PurchaseStatus ();                                                                                                                             //Check status of in app purchase (true/false if player has purchased it)
				totTime = Time.timeSinceLevelLoad + secTime;
			}
			if(cube != null)
			{
				if(!greenCubeIAPOwned)
				{
					cube.transform.GetComponent<Renderer>().material.color = Color.red;                                                                     // if player has purchased item, turn the cube green
				}
				
				if(greenCubeIAPOwned)
				{
					cube.transform.GetComponent<Renderer>().material.color = Color.green;                                                           // if player has not purchased item (or hasnt restored previous purchases) turn the cube red
				}
			}
		}
		
		//CHECK IAP STATUS (0 = not owned, 1 = owned for GetItemBalance())
		//Check the Status of the In App Purchase (true/false if player has bought it)
		void CheckIAP_PurchaseStatus(){
			Debug.Log (StoreInventory.GetItemBalance("turn_green_item_id"));                                                        // Print the current status of the IAP
			if (StoreInventory.GetItemBalance("turn_green_item_id") >= 1)
			{
				greenCubeIAPOwned = true;               // check if the non-consumable in app purchase has been bought or not
			}
		}
		
		//GUI ELEMENTS
		void OnGUI() {
			//Button To PURCHASE ITEM
			if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 150,150),"Make green?"))
			{
				try {
					Debug.Log("attempt to purchase");
					
					StoreInventory.BuyItem ("turn_green_item_id");                                                                          // if the purchases can be completed sucessfully
				}
				catch (Exception e)
				{                                                                                                                                                                               // if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
					Debug.Log ("SOOMLA/UNITY" + e.Message);                                                
				}
			}
			//Button to RESTORE PURCHASES
			if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.8f, 150,150),"Restore\nPurchases")) {
				try
				{
					SoomlaStore.RestoreTransactions();                                                                                                      // restore purchases if possible
				}
				catch (Exception e)
				{
					Debug.Log ("SOOMLA/UNITY" + e.Message);                                                                                         // if restoring purchases fails (connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
				}
			}
			
			//Button to RESTART LEVEL (ensure it doesnt crash)
			if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.8f, 150,150),"Restart"))  
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}


	
	namespace Soomla.Store.Example                                                                                                                  //Allows for access to Soomla API
	{
		public class ExampleAssets : IStoreAssets                                                                                       //Extend from IStoreAssets (required to define assets)
		{
			public int GetVersion() {                                                                                                               // Get Current Version
				return 0;
			}
			
			public VirtualCurrency[] GetCurrencies() {                                                                              // Get/Setup Virtual Currencies
				return new VirtualCurrency[]{};
			}
			
			public VirtualGood[] GetGoods() {                                                                                               // Add "TURN_GREEN" IAP to GetGoods
				return new VirtualGood[]{TURN_GREEN};
			}
			
			public VirtualCurrencyPack[] GetCurrencyPacks() {                                                               // Get/Setup Currency Packs
				return new VirtualCurrencyPack[]{};
			}
			
			public VirtualCategory[] GetCategories() {                                                                              // Get/ Setup Categories (for In App Purchases)
				return new VirtualCategory[]{};
			}
			
			//****************************BOILERPLATE ABOVE(modify as you see fit/ if nessisary)***********************
			public const string TURN_GREEN_PRODUCT_ID = "com.alexanderyounggames.soomlaiaptutorial.turngreen";                              //create a string to store the "turn green" in app purchase
			
			
			/** Lifetime Virtual Goods (aka - lasts forever **/
			
			// Create the 'TURN_GREEN' LifetimeVG In-App Purchase
			public static VirtualGood TURN_GREEN = new LifetimeVG(         
			                                                      "turn_green",                                                                                                                               // Name of IAP
			                                                      "This will turn the cube green.",                                                                                   // Description of IAP
			                                                      "turn_green_item_id",                                                                                                               // Item ID (different from 'product id" used by itunes, this is used by soomla)
			                                                      
			                                                      // 1. assign the purchase type of the IAP (purchaseWithMarket == item cost real money),
			                                                      // 2. assign the IAP as a market item (using its ID)
			                                                      // 3. set the item to be a non-consumable purchase type
			                                                      
			                                                      //                  1.                                      2.                                              3.
			                                                      new PurchaseWithMarket(TURN_GREEN_PRODUCT_ID, 0.99)
			                                                      );
		}
	}

}