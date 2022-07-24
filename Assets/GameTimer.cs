using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataPipeline;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Spellbound.Managers
{
    public class GameTimer : MonoBehaviour, IGenerator<PlayerInteractionState>, IHandler<PlayerInteractionState>
    {
        [SerializeField] private int roundLength;
        [SerializeField] private int waveLength;
        [SerializeField] private int numRounds;

        [SerializeField] private double gameTimer;
        [SerializeField] private double roundTimer;
        [SerializeField] private double waveTimer;
        [SerializeField] private int roundsElapsed;
        [SerializeField] private int wavesElapsed;
        [SerializeField] private RoundPhase roundPhase;

        [SerializeField] private bool spawnBosses;

        private GameAudio gameAudio;

        bool refreshKilled;

        private void Start()
        {
            GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
            GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddGenerator(this);
            gameAudio = GameObject.Find("Game Audio").GetComponent<GameAudio>();
        }

        private void FixedUpdate()
        {
            gameTimer += Time.deltaTime;
            roundTimer += Time.deltaTime;
            waveTimer += Time.deltaTime;

            if(roundPhase == RoundPhase.Wave)
            {
                if (waveTimer > waveLength)
                {
                    waveTimer -= waveLength;
                    wavesElapsed++;
                }

                if (roundTimer > roundLength * 60)
                {
                    roundTimer -= roundLength;
                    waveTimer = roundTimer;
                    wavesElapsed++;
                    if (spawnBosses)
                    {
                        roundPhase = RoundPhase.Boss;
                        gameAudio.PlaySound("King Die", AudioTrackType.Soundtrack);
                    }
                    else
                    {
                        roundsElapsed++;
                    }
                }
            }
        }

        public void Write(ref PlayerInteractionState data)
        {
            data.GameState.roundLength = roundLength;
            data.GameState.waveLength = waveLength;
            data.GameState.numRounds = numRounds;
            data.GameState.gameTimer = gameTimer;
            data.GameState.roundTimer = roundTimer;
            data.GameState.waveTimer = waveTimer;
            data.GameState.roundLength = roundLength;
            data.GameState.roundsElapsed = roundsElapsed;
            data.GameState.wavesElapsed = wavesElapsed;
            data.GameState.roundPhase = roundPhase;
            data.GameState.spawnBosses = spawnBosses;

            if (refreshKilled)
            {
                data.GameState.bossKilled = false;
                refreshKilled = false;
            }
        }

        public void Handle(in PlayerInteractionState data)
        {
            if (data.GameState.bossKilled)
            {
                roundsElapsed++;
                roundPhase = RoundPhase.Wave;
                roundTimer = 0;
                waveTimer = 0;
                wavesElapsed = 0;
                refreshKilled = true;
                data.sharedData.GameAudio.PlaySound("Level Theme", AudioTrackType.Soundtrack);
            }
        }
    }

    public enum RoundPhase
    {
        Wave,
        Boss
    }

    #region Custom Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(GameTimer))]
    public class GameTimerEditor : Editor
    {
        private SerializedProperty roundLength;
        private SerializedProperty waveLength;
        private SerializedProperty numRounds;
        private SerializedProperty gameTimer;
        private SerializedProperty roundTimer;
        private SerializedProperty roundsElapsed;
        private SerializedProperty wavesElapsed;
        private SerializedProperty roundPhase;
        private SerializedProperty waveTimer;
        private SerializedProperty spawnBosses;

        void OnEnable()
        {
            roundLength = serializedObject.FindProperty("roundLength");
            waveLength = serializedObject.FindProperty("waveLength");
            numRounds = serializedObject.FindProperty("numRounds");
            gameTimer = serializedObject.FindProperty("gameTimer");
            roundTimer = serializedObject.FindProperty("roundTimer");
            roundsElapsed = serializedObject.FindProperty("roundsElapsed");
            roundPhase = serializedObject.FindProperty("roundPhase");
            waveTimer = serializedObject.FindProperty("waveTimer");
            spawnBosses = serializedObject.FindProperty("spawnBosses");
            wavesElapsed = serializedObject.FindProperty("wavesElapsed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUIStyle descriptionStyle = new GUIStyle();
            descriptionStyle.normal.textColor = Color.HSVToRGB(0, 0, 0.5f);
            descriptionStyle.wordWrap = true;

            GUIStyle headerStyle = new GUIStyle();
            headerStyle.normal.textColor = Color.HSVToRGB(0, 0, 0.8f);
            headerStyle.fontSize = 20;

            GUIStyle textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.HSVToRGB(0, 0, 0.6f);

            var myLayout = new GUILayoutOption[] {
                  GUILayout.Height(20)
            };

            EditorGUILayout.LabelField("The game timer keeps track of the current game state and the amount of time that has elapsed in that state", descriptionStyle);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Game", headerStyle, myLayout);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField($"Game Timer: {gameTimer.floatValue.ToString("0.00")}");
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Rounds", headerStyle, myLayout);
            EditorGUI.indentLevel++;
            EditorGUILayout.IntSlider(numRounds, 0, 8, "Number of Rounds");
            EditorGUILayout.IntSlider(roundLength, 0, 10, "Round Length (Min)");
            EditorGUILayout.PropertyField(spawnBosses);
            EditorGUILayout.LabelField($"Round number: {roundsElapsed.intValue + 1}");
            EditorGUILayout.LabelField($"Round phase: {roundPhase.enumNames[roundPhase.enumValueIndex]}");
            EditorGUILayout.LabelField($"Round timer: {roundTimer.floatValue.ToString("0.00")}");
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Waves", headerStyle, myLayout);
            EditorGUI.indentLevel++;
            EditorGUILayout.IntSlider(waveLength, 0, 120, "Wave Length (Sec)");
            EditorGUILayout.LabelField($"Wave number: {wavesElapsed.intValue + 1}");
            EditorGUILayout.LabelField($"Wave timer: {waveTimer.floatValue.ToString("0.00")}");
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}