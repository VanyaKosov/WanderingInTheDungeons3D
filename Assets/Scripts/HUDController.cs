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
    public float worldRadarDistance = 48f;
    public GameObject indicatorPrefab;
    public RectTransform indicators;
    private Pool<MonsterIndicator> poolOfIndicators;
    private List<MonsterIndicator> pooledIndicators = new List<MonsterIndicator>();

    void Start()
    {
        poolOfIndicators = new Pool<MonsterIndicator>(() =>
        {
            MonsterIndicator mi = new MonsterIndicator();
            mi.gameObject = Instantiate(indicatorPrefab, indicators);
            mi.rectTransform = mi.gameObject.GetComponent<RectTransform>();
            return mi;
        });
    }

    void Update()
    {
        UpdateRadar();
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
            m => (playerController.gameObject.transform.position - m.transform.position).magnitude < worldRadarDistance
        );

        foreach (GameObject monster in closeMonsters)
        {
            MonsterIndicator indicator = poolOfIndicators.Take();
            pooledIndicators.Add(indicator);
            indicator.gameObject.SetActive(true);


        }
    }

    private class MonsterIndicator
    {
        public GameObject gameObject;
        public RectTransform rectTransform;
    }
}
