using System;
using Scripts.Gameplay.Managers;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Zenject;

namespace Integration
{
    public class IAPService : MonoBehaviour, IStoreListener 
    {
        private static IStoreController _storeController;
        private static IExtensionProvider _extensionsProvider;
        [SerializeField]
        private PurchaseIDHolder _purchaseIDHolder;
        [SerializeField]
        public Toggle _toggleMonth;
        [SerializeField]
        public Toggle _toggleYear;
        [SerializeField]
        public Toggle _toggleForever;
        [SerializeField]
        public Button _buySubscriptionButton;
        [SerializeField]
        public Button _closeSubpanel;
        [SerializeField]
        private GameObject _subscriptionCanvas;
        
        private string _subscriptionMonthProductID;
        private string _subscriptionYearProductID;
        private string _subscriptionForeverProductID;
        
        private string _buy100Id;
        private string _buy300Id;
        private string _buy1000Id;
        private string _buy3000Id;
        
        private AdMobController _adMobController;

        [Inject]
        private void Construct (AdMobController adMobController)
        {
            _adMobController = adMobController;
        }

        private void Awake()
        {
            LoadID();
            if (_storeController == null)
            {
                InitializePurchasing();
            }
            else
            {
                string nameOfError = "error _storeController = null";
                Debug.Log(nameOfError);
            }
            DontDestroyOnLoad(gameObject);
        }
        
        private void LoadID()
        {
            _subscriptionMonthProductID = _adMobController.IsProdaction ? _purchaseIDHolder.SubscriptionMonthID : _purchaseIDHolder.SubscriptionMonthID_Test;
            _subscriptionYearProductID = _adMobController.IsProdaction ? _purchaseIDHolder.SubscriptionYearID : _purchaseIDHolder.SubscriptionYearID_Test;
            _subscriptionForeverProductID = _adMobController.IsProdaction ? _purchaseIDHolder.SubscriptionForeverID : _purchaseIDHolder.SubscriptionForeverID_Test;
            
            _buy100Id = _adMobController.IsProdaction ? _purchaseIDHolder.Buy100Id : _purchaseIDHolder.Buy100Id_Test;
            _buy300Id = _adMobController.IsProdaction ? _purchaseIDHolder.Buy300Id : _purchaseIDHolder.Buy300Id_Test;
            _buy1000Id = _adMobController.IsProdaction ? _purchaseIDHolder.Buy1000Id : _purchaseIDHolder.Buy1000Id_Test;
            _buy3000Id = _adMobController.IsProdaction ? _purchaseIDHolder.Buy3000Id : _purchaseIDHolder.Buy3000Id_Test;
        }

        private void OnEnable()
        {
            _buySubscriptionButton.onClick.AddListener(BuySubscription);
            _closeSubpanel.onClick.AddListener(HideSubscriptionPanel);
        }

        private void OnDisable()
        {
            _buySubscriptionButton.onClick.RemoveListener(BuySubscription);
            _closeSubpanel.onClick.RemoveListener(HideSubscriptionPanel);
        }

        public void ShowSubscriptionPanel()
        {
            if (_adMobController.IsPurchased)
            {
                return;
            }
            _subscriptionCanvas.SetActive(true);
            _adMobController.ShowBanner(false);
        }
        
        public void HideSubscriptionPanel()
        {
            _subscriptionCanvas.SetActive(false);
            _adMobController.ShowBanner(true);
        }

        private void CheckSubscriptionStatus()
        {
            if (IsInitialized())
            {
                string[] productIds = { _subscriptionMonthProductID, _subscriptionYearProductID, _subscriptionForeverProductID };

                bool subscriptionActive = false;
                
                foreach (string productId in productIds)
                {
                    Product product = GetProduct(productId);
                    if (product != null && product.hasReceipt)
                    {
                        subscriptionActive = true;
                        break;
                    }
                }
                PlayerPrefs.SetInt(_adMobController.noAdsKey, subscriptionActive ? 1 : 0);
                PlayerPrefs.Save();
                
                if (subscriptionActive)
                {
                    HideSubscriptionPanel();
                }
                else
                {
                    ShowSubscriptionPanel();
                }
            }
        }

        public bool IsInitialized()
        {
            return _storeController != null && _extensionsProvider != null;
        }

        private void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(_subscriptionMonthProductID, ProductType.Subscription);
            builder.AddProduct(_subscriptionYearProductID, ProductType.Subscription);
            builder.AddProduct(_subscriptionForeverProductID, ProductType.Subscription);
            
            builder.AddProduct(_buy100Id, ProductType.Consumable);
            builder.AddProduct(_buy300Id, ProductType.Consumable);
            builder.AddProduct(_buy1000Id, ProductType.Consumable);
            builder.AddProduct(_buy3000Id, ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }
        
        public Product GetProduct(string productID)
        {
            if (IsInitialized())
            {
                return _storeController.products.WithID(productID);
            }
            return null;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: SUCSESS");
            _storeController = controller;
            _extensionsProvider = extensions;
            
            CheckSubscriptionStatus();
        }

        private void BuySubscription()
        {
            if (_toggleMonth.isOn)
            {
                BuyProductID(_subscriptionMonthProductID);
            }
            else if (_toggleYear.isOn)
            {
                BuyProductID(_subscriptionYearProductID);
            }
            else if (_toggleForever.isOn)
            {
                BuyProductID(_subscriptionForeverProductID);
            }
            else
            {
                Debug.LogError("No subscription type selected.");
            }
        }
        
        public void BuyPack1()
        {
            BuyProductID(_buy100Id);
        }

        public void BuyPack2()
        {
            BuyProductID(_buy300Id);
        }

        public void BuyPack3()
        {
            BuyProductID(_buy1000Id);
        }
        
        public void BuyPack4()
        {
            BuyProductID(_buy3000Id);
        }

        
        public void BuyProductID(string productId)
        {
            if (IsInitialized())
            {
                _storeController.InitiatePurchase(productId);
                Product product = _storeController.products.WithID(productId);

                if (product is {availableToPurchase: true})
                {
                    Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
                }
                else
                {
                    Debug.Log("Failed to purchase subscription. Product is not available.");
                }
            }
            else
            {
                Debug.Log("[STORE NOT INITIALIZED]");
            }
        }
        

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if (String.Equals(args.purchasedProduct.definition.id, _subscriptionMonthProductID, StringComparison.Ordinal))
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
                _adMobController.RemoveAds();
                HideSubscriptionPanel();
            }
            else if (String.Equals(args.purchasedProduct.definition.id, _subscriptionYearProductID, StringComparison.Ordinal))
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
                _adMobController.RemoveAds();
                HideSubscriptionPanel();
            }
            else if (String.Equals(args.purchasedProduct.definition.id, _subscriptionForeverProductID, StringComparison.Ordinal))
            {
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
                _adMobController.RemoveAds();
                HideSubscriptionPanel();
            }
            else if (String.Equals(args.purchasedProduct.definition.id, _buy100Id, StringComparison.Ordinal))
            {
                PlayerPrefsManager.AddPaidCurrency(100);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, _buy300Id, StringComparison.Ordinal))
            {
                PlayerPrefsManager.AddPaidCurrency(300);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, _buy1000Id, StringComparison.Ordinal))
            {
                PlayerPrefsManager.AddPaidCurrency(1000);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, _buy3000Id, StringComparison.Ordinal))
            {
                PlayerPrefsManager.AddPaidCurrency(3000);
                Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            }
            else
            {
                Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
            }
        
            return PurchaseProcessingResult.Complete;
        }
        
        public void RestorePurchases()
        {
            if (IsInitialized())
            {
                Debug.Log("Restoring purchases...");
                _extensionsProvider.GetExtension<IAppleExtensions>()?.RestoreTransactions(OnRestore);
            }
            else
            {
                Debug.Log("[STORE NOT INITIALIZED]");
            }
        }
        
        private void OnRestore(bool success, string error)
        {
            var restoreMessage = "";
            if (success)
            {
                restoreMessage = "Restore Successful";
            }
            else
            {
                restoreMessage = $"Restore Failed with error: {error}";
            }
            Debug.Log(restoreMessage);
        }

        private void OnRestoreComplete(bool success)
        {
            if (success)
            {
                Debug.Log("Purchases successfully restored.");
            }
            else
            {
                Debug.Log("Failed to restore purchases.");
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"OnPurchaseFailed: FAIL. Products: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string? message)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }                
    }
}
