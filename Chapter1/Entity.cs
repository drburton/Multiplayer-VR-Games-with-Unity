// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.16
// 

using Colyseus.Schema;

public partial class Entity : Schema {
	[Type(0, "string")]
	public string id = default(string);

	[Type(1, "string")]
	public string ownerId = default(string);

	[Type(2, "number")]
	public float xPos = default(float);

	[Type(3, "number")]
	public float yPos = default(float);

	[Type(4, "number")]
	public float zPos = default(float);

	[Type(5, "number")]
	public float xRot = default(float);

	[Type(6, "number")]
	public float yRot = default(float);

	[Type(7, "number")]
	public float zRot = default(float);

	[Type(8, "string")]
	public string sessionId = default(string);

	[Type(9, "boolean")]
	public bool connected = default(bool);	
}

