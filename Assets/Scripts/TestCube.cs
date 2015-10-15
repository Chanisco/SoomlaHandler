using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store.Example{

	public class TestCube : MonoBehaviour {

			void Start(){
				StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreIntitialized;
				SoomlaStore.Initialize (new SoomlaController()); 
			}

			public void onSoomlaStoreIntitialized(){
				
			}

			void Update(){  	
				
				if (Input.GetKeyUp (KeyCode.A)) {	
					try
					{
						SoomlaStore.RestoreTransactions();                                                                                                     
					}
					catch (Exception e)
					{
						Debug.Log ("SOOMLA/UNITY" + e.Message);
					}
				}
				if (Input.GetKeyUp (KeyCode.S)) {
					PlayerPrefs.DeleteAll();
					StoreInventory.TakeItem("no_ads",1);
				}
			Debug.Log (StoreInventory.GetItemBalance("no_ads"));


			}

			void OnMouseDown(){
			if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork){
				try {
					Debug.Log ("attempt to purchase");
				
					StoreInventory.BuyItem ("no_ads");                                                                          
				} catch (Exception e) {                                                                                                                                                                               // if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
					Debug.Log ("SOOMLA/UNITY" + e.Message);                                                
				}
			}else{
				Debug.Log("You got no internet ");
			}

		}
	}
}
