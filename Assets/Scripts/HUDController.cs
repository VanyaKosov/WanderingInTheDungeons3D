using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //public TextMeshProUGUI testText;
    public bool radarIsActive = false;
    public PlayerController playerController;
    public GameController gameController;
    public Canvas canvas;
    public Image radar;
    public float worldRadarRadius = 24f;
    public GameObject indicatorPrefab;
    public RectTransform monsterIndicators;
    public GameObject healthBar;
    //private int maxHealth;
    private Pool<MonsterIndicator> poolOfIndicators;
    private List<MonsterIndicator> pooledIndicators = new List<MonsterIndicator>();

    void Start()
    {
        poolOfIndicators = new Pool<MonsterIndicator>(() =>
        {
            MonsterIndicator mi = new MonsterIndicator();
            mi.gameObject = Instantiate(indicatorPrefab, monsterIndicators);
            mi.rectTransform = mi.gameObject.GetComponent<RectTransform>();
            return mi;
        });
    }

    void Update()
    {
        UpdateRadar();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        int health = playerController.Health;
        healthBar.transform.localScale = new Vector3((float)health / (float)playerController.MaxHealth, healthBar.transform.localScale.y);
    }

    private void UpdateRadar()
    {
        if (!radarIsActive)
        {
            radar.gameObject.SetActive(false);
            return;
        }
        radar.gameObject.SetActive(true);

        foreach (MonsterIndicator indicator in pooledIndicators)
        {
            indicator.gameObject.SetActive(false);
            poolOfIndicators.Return(indicator);
        }
        pooledIndicators.Clear();

        IEnumerable<GameObject> closeMonsters = gameController.monsters.Where(
            m => (playerController.gameObject.transform.position - m.transform.position)
            .magnitude < worldRadarRadius
        );

        var angle = Vector3.Angle(Vector3.forward, playerController.gameObject.transform.forward);
        var crossProduct = Vector3.Cross(Vector3.forward, playerController.gameObject.transform.forward);
        Vector3 radarPos = radar.rectTransform.position;
        radarPos.z = 0;
        foreach (GameObject monster in closeMonsters)
        {
            MonsterIndicator indicator = poolOfIndicators.Take();
            pooledIndicators.Add(indicator);
            indicator.gameObject.SetActive(true);

            Vector3 monsterOffset = monster.transform.position - playerController.transform.position;
            float tempZ = monsterOffset.z;
            monsterOffset.z = monsterOffset.y;
            monsterOffset.y = tempZ;
            monsterOffset = monsterOffset * radar.rectTransform.rect.width / worldRadarRadius * 0.85f;
            monsterOffset.z = indicator.rectTransform.position.z;

            indicator.rectTransform.position = monsterOffset + radarPos;
            indicator.gameObject.transform.RotateAround(radar.rectTransform.position, Vector3.forward, angle * Mathf.Sign(crossProduct.y));
            indicator.gameObject.transform.rotation = Quaternion.identity;
        }
    }

    private class MonsterIndicator
    {
        public GameObject gameObject;
        public RectTransform rectTransform;
    }
}
