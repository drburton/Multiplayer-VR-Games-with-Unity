using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

using Colyseus;
using Colyseus.Schema;

using GameDevWare.Serialization;

[Serializable]
class MoveData
{
	public float xPos;
	public float yPos;
	public float zPos;
	public float xRot;
	public float yRot;
	public float zRot;
	public string name;
}

public class myColyseusClient : ColyseusManager<myColyseusClient> {

	public String PlayerName, SessionId;
	public String roomName;
	
	public GameObject Spawn;
    public GameObject myPlayer;
	public GameObject enemy;

    protected ColyseusClient ColyseusClient;
	protected ColyseusRoom<State> ColyseusRoom;
	protected IndexedDictionary<Entity, GameObject> entities = new IndexedDictionary<Entity, GameObject>();

    
    // Use this for initialization
	void Start () {
		/*
		 * Connect to Colyeus Server
		 */
 
       base.InitializeClient();	

	   JoinOrCreateRoom();
	}
    
    public async void JoinOrCreateRoom() {
		ColyseusRoom = await client.JoinOrCreate<State>(roomName);
		
//		Debug.Log(ColyseusRoom);
		RegisterRoomHandlers();
	}

    public void RegisterRoomHandlers() {
		SessionId = "sessionId: " + ColyseusRoom.SessionId;
	
		ColyseusRoom.State.entities.OnAdd += OnEntityAdd;
//		ColyseusRoom.State.entities.OnRemove += OnEntityRemove;
		ColyseusRoom.State.TriggerAll();

		PlayerPrefs.SetString("roomId", ColyseusRoom.Id);
		PlayerPrefs.SetString("sessionId", ColyseusRoom.SessionId);
		PlayerPrefs.Save();

		ColyseusRoom.OnLeave += (code) => Debug.Log("ROOM: ON LEAVE");
		ColyseusRoom.OnError += (code, message) => Debug.LogError("ERROR, code =>" + code + ", message => " + message);
		ColyseusRoom.OnStateChange += OnStateChangeHandler;
    }

	public void OnTankMove() {
 	  //	Debug.Log("Sent Move to server");
	  ColyseusRoom.Send("move", new MoveData() {
		xPos = myPlayer.transform.position.x, 
		yPos = 0.5f,
		zPos = myPlayer.transform.position.z,
		xRot = myPlayer.transform.rotation.x,
		yRot = myPlayer.transform.rotation.y,
		zRot = myPlayer.transform.rotation.z,
		name = PlayerName
	  });  
	}

    async void LeaveRoom() {
		await ColyseusRoom.Leave(false);

		// Destroy player entities
		foreach (KeyValuePair<Entity, GameObject> entry in entities)
		{
			Destroy(entry.Value);
		}

		entities.Clear();
	}

	void OnStateChangeHandler (State state, bool isFirstState )	{
	}   

    void OnEntityAdd(string key, Entity entity) {   
		Debug.Log("Entity Added ");
/*      var output = JsonUtility.ToJson(state, true);
   	    Debug.Log(output);  */

		var tempPos = Spawn.transform.position;
		var tempRot = Spawn.transform.rotation;
        if (entity.sessionId == ColyseusRoom.SessionId ) { // Set Player's spawnpoint		
            myPlayer.transform.position = tempPos;
            myPlayer.transform.rotation = tempRot;
            
            entities.Add(entity, myPlayer);
//            OnTankMove();
	    } else {  // Set enemy's spawnpoints

            GameObject myEnemy = Instantiate(enemy, tempPos, tempRot);

            Debug.Log("Enemy add! x => " + entity.xPos + ", z => " + entity.zPos);

            // Add "enemy" to map of players
            entities.Add(entity, myEnemy);
			myEnemy.GetComponentInChildren<TextMesh>().text = entity.ownerId;
            entity.OnChange += (List<Colyseus.Schema.DataChange> changes) =>
            {
                myEnemy.transform.position = new Vector3(entity.xPos, 0.5f, entity.zPos);
                myEnemy.transform.rotation = new Quaternion(entity.xRot, entity.yRot, entity.zRot,0f);			
                
                myEnemy.GetComponentInChildren<TextMesh>().transform.rotation = Camera.main.transform.rotation;
            };
	    }
	}
}
