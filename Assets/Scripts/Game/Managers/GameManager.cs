using UnityEngine;
using System;
using System.Collections.Generic;
using LatchOn.ECS.Components.Input;
using Entity = UnityEngine.GameObject;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	const string SpawnPointName = "Spawn Point";

	[Serializable]
	public struct Prefabs {
		public GameObject Player;
		public GameObject Camera;
		public GameObject Hook;
	}
	public Prefabs prefabs;

	public Transform ActorContainer;
	public Transform PropsContainer;

	[SerializeField]
	private List<Entity> players;

	void Awake() {
		players = new List<Entity>();

		if (ActorContainer == null)
			ActorContainer = GameObject.Find("Actor").transform;
		if (PropsContainer == null)
			PropsContainer = GameObject.Find("Props").transform;

		InitGame();
	}

	void InitGame() {
		if (players.Count == 0) SpawnCamera(SpawnPlayer());
	}

	GameObject SpawnPlayer() {
		GameObject player = Instantiate(prefabs.Player, Vector2.up * 2, Quaternion.identity);
		if (ActorContainer) player.transform.parent = ActorContainer;

		player.AddComponent<LocalPlayer>();

		players.Add(player);
		player.GetComponent<Prime31.CharacterController2D>().warpToGrounded();

		Ego.AddGameObject(player);
		return player;
	}

	Camera SpawnCamera(GameObject target) {
		Collider2D collider = target.GetComponent<Collider2D>();
		Camera camera = Camera.main == null
			? Instantiate(prefabs.Camera).GetComponent<Camera>()
			: Camera.main;

		camera.GetComponent<FollowerCamera>().Target = collider;
		return camera;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.F5)) SpawnPlayer();
	}

	public EgoComponent NewEntity(GameObject prefab = null, Transform parent = null) {
		EgoComponent entity;
		if (prefab == null) {
			entity = Ego.AddGameObject(new GameObject());
		} else {
			entity = Ego.AddGameObject(Instantiate(prefab));
		}

		if (parent != null) entity.transform.parent = parent;
		return entity;
	}

	public void Destory(GameObject go) { Destroy(go); }

	public Transform Find(string name) {
		return transform.Find(name);
	}
}
