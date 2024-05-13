using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseID", menuName = "Subscription/PurchaseID", order = 1)]
public class PurchaseIDHolder : ScriptableObject
{
#if UNITY_ANDROID

    [SerializeField]
    private string _subscriptionMonthProductID;
    [SerializeField]
    private string _subscriptionYearProductID;
    [SerializeField]
    private string _subscriptionForeverProductID;
    
    [SerializeField]
    private string _buy100Id;
    [SerializeField]
    private string _buy300Id;
    [SerializeField]
    private string _buy1000Id;
    [SerializeField]
    private string _buy3000Id;
        
#elif UNITY_IOS
    
    [Header("Production Purchase ID IOS")]
    [SerializeField]
    private string _subscriptionMonthProductID;
    [SerializeField]
    private string _subscriptionYearProductID;
    [SerializeField]
    private string _subscriptionForeverProductID;
    
    [SerializeField]
    private string _buy100Id;
    [SerializeField]
    private string _buy300Id;
    [SerializeField]
    private string _buy1000Id;
    [SerializeField]
    private string _buy3000Id;

#endif
        
    [Header("Test ID")]
    [SerializeField]
    private string _subscriptionMonthID_Test = "sub.gamedev.test.one.month";
    [SerializeField]
    private string _subscriptionYearID_Test  = "sub.gamedev.test.one.year";
    [SerializeField]
    private string _subscriptionForeverID_Test  = "sub.gamedev.deletead.forever";
    
    [SerializeField]
    private string _buy100Id_Test  = "test.gamedev.buy100";
    [SerializeField]
    private string _buy300Id_Test  = "test.gamedev.buy300";
    [SerializeField]
    private string _buy1000Id_Test  = "test.gamedev.buy1000";
    [SerializeField]
    private string _buy3000Id_Test  = "test.gamedev.buy3000";
        
    public string SubscriptionMonthID => _subscriptionMonthProductID;
    public string SubscriptionYearID => _subscriptionYearProductID;
    public string SubscriptionForeverID => _subscriptionForeverProductID;
    
    public string Buy100Id => _buy100Id;
    public string Buy300Id => _buy300Id;
    public string Buy1000Id => _buy1000Id;
    public string Buy3000Id => _buy3000Id;
        
    public string SubscriptionMonthID_Test => _subscriptionMonthID_Test;
    public string SubscriptionYearID_Test => _subscriptionYearID_Test;
    public string SubscriptionForeverID_Test => _subscriptionForeverID_Test;
    
    public string Buy100Id_Test => _buy100Id_Test;
    public string Buy300Id_Test => _buy300Id_Test;
    public string Buy1000Id_Test => _buy1000Id_Test;
    public string Buy3000Id_Test => _buy3000Id_Test;
}
