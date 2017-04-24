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

	public EgoComponent NewEntity(GameObject prefab = null) {
		if (prefab == null) {
			return Ego.AddGameObject(new GameObject());
		} else {
			return Ego.AddGameObject(Instantiate(prefab));
		}
	}

	public void Destory(GameObject go) { Destroy(go); }

	public Transform Find(string name) {
		return transform.Find(name);
	}
}
