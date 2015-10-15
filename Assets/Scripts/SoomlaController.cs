using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

namespace Soomla.Store.Example{

	public class SoomlaController : IStoreAssets {
	
	public const string NO_ADS_LIFETIME_PRODUCT_ID = "no_ads";

	public int GetVersion() {
		return 0;
	}

	public VirtualCurrency[] GetCurrencies() {
			return new VirtualCurrency[] {};
	}

	public VirtualGood[] GetGoods() {
			return new VirtualGood[] {NO_ADS_LTVG};
	}

	public VirtualCurrencyPack[] GetCurrencyPacks() {
			return new VirtualCurrencyPack[] {};
	}

	public VirtualCategory[] GetCategories() {
			return new VirtualCategory[] {};
	}

	public static VirtualGood NO_ADS_LTVG = new LifetimeVG(
		"No Ads", 														// name
		"No More Ads!",				 									// description
		"no_ads",														// item id
		new PurchaseWithMarket(NO_ADS_LIFETIME_PRODUCT_ID, 0.99));	
	}

}

