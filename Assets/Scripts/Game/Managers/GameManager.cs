using UnityEngine;
using System.Collections.Generic;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	const string SpawnPointName = "Spawn Point";

	public GameObject PlayerPrefab;
	public GameObject CameraPrefab;

	public Transform ActorContainer;
	public Transform PropsContainer;

	[SerializeField]
	private Dictionary<ControlType, GameObject> Players;
	private Transform spawnPoint;

	public Queue<GameObject> DestroyedObjects = new Queue<GameObject>();

	void Awake() {
		Players = new Dictionary<ControlType, GameObject>();
		spawnPoint = transform.Find(SpawnPointName);

		InitGame();
	}

	void InitGame() {
		if (Players.Count == 0) SpawnCamera(SpawnPlayer());
	}

	GameObject SpawnPlayer(ControlType controller = ControlType.Keyboard) {
		if (Players.ContainsKey(controller)) return Players[controller];

		GameObject player = Instantiate(PlayerPrefab, spawnPoint.position, Quaternion.identity);
		if (ActorContainer) player.transform.parent = ActorContainer;

		LocalPlayer marker = player.AddComponent<LocalPlayer>();
		marker.Controller = controller;

		Players.Add(controller, player);
		player.GetComponent<Prime31.CharacterController2D>().warpToGrounded();

		Ego.AddGameObject(player);
		return player;
	}

	Camera SpawnCamera(GameObject target) {
		Collider2D collider = target.GetComponent<Collider2D>();
		Camera camera = Camera.main == null
			? Instantiate(CameraPrefab).GetComponent<Camera>()
			: Camera.main;

		camera.GetComponent<FollowerCamera>().Target = collider;
		return camera;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		}

		if (Input.GetKeyDown(KeyCode.F5)) {
			foreach (ControlType c in System.Enum.GetValues(typeof (ControlType))) {
				if (!Players.ContainsKey(c)) {
					SpawnPlayer(c);
					break;
				}
			}
		}
	}

	public static bool IsPaused() {
		return Time.timeScale == 0;
	}

	public EgoComponent NewEntity(GameObject prefab = null) {
		if (prefab == null) {
			return Ego.AddGameObject(new GameObject());
		} else {
			return Ego.AddGameObject(Instantiate(prefab));
		}
	}

	public void Destory(GameObject go) { Destroy(go); }
}
