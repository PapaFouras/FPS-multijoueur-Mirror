
using UnityEngine;
using System.Collections;
using Mirror;
[RequireComponent(typeof(PlayerSetUp))]
public class Player : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    [SyncVar]
    private bool _isDead = false;


    [SyncVar]
    private float currentHealth;

    public int kills;
    public int deaths;

    [SerializeField]
    private Behaviour[] disableOnDeath;

        [SerializeField]
    private GameObject[] disableGameObjectOnDeath;
    private bool[] wasEnabledOnStart;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public bool isDead{
        get{return _isDead;}
        protected set { _isDead = value;}
    }


    private void Update() {
        if(!isLocalPlayer){
            return;
        }

        if(Input.GetKeyDown(KeyCode.H)){
            RpcTakeDamage(59,"Joueur");
        }
    }
    public void SetUp() 
    {
        if(isLocalPlayer){
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetUp>().playerUIInstance.SetActive(true);
            CmdBroadcastNewPlayerSetUp();

        }
        
        
    }

    public float GetHealthPct(){
        return (float)currentHealth/maxHealth;
    }

    [SyncVar]
    public string username = "Player";

    [Command(requiresAuthority = true)]
    private void CmdBroadcastNewPlayerSetUp(){

        RpcSetUpPlayerOnAllClients();

    }

    [ClientRpc]
    private void RpcSetUpPlayerOnAllClients(){

        if(firstSetup){
                     wasEnabledOnStart = new bool[disableOnDeath.Length]; 
        for (var i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }
        
        firstSetup = false;

        }
SetDefaults();

    }

    private void SetDefaults(){
        isDead = false;
        currentHealth = maxHealth;
        //reactive les components
         for (var i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }
        //reactive les gameobjects
        for (var i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if(col != null){
            col.enabled = true;
        }

      
        //Apparition des particules lors de la mort
        GameObject _gpxIns =Instantiate(spawnEffect,transform.position,Quaternion.identity);
        Destroy(_gpxIns,4f);
    }
    [ClientRpc]
    public void RpcTakeDamage(float amount, string sourceID){
        if(isDead){
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name + " a maintenant : "+ currentHealth+ " points de vie");

        if(currentHealth <= 0){

            Die(sourceID);
        }
    }

    private IEnumerator Respawn(){
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;


        yield return new WaitForSeconds(0.1f);

        SetUp();


    }
        

    private void Die(string sourceID){
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(sourceID);
        if(sourcePlayer != null){
            sourcePlayer.kills +=1;
           GameManager.instance.onPlayerKilledCallback.Invoke(username,sourcePlayer.username);

        }


        deaths++;
        

        //desactive les components du player pour qu'il ne puisse plus jouer
        for (var i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        //desactive les Gameobjects du player pour qu'il ne puisse plus jouer

        for (var i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
        }
        //enlever le collider du joueur lors de sa mort
        Collider col = GetComponent<Collider>();
        if(col != null){
            col.enabled = false;
        }
        //Apparition des particules lors de la mort
        GameObject _gpxIns =Instantiate(deathEffect,transform.position,Quaternion.identity);
        Destroy(_gpxIns,4f);


        if(isLocalPlayer){
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetUp>().playerUIInstance.SetActive(false);
        }

        //Faire le respawn du joueur
        Debug.Log(transform.name + " a été éliminé");
        StartCoroutine(Respawn());
    }
    
}
