using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private GameManeger gameManeger;
    [SerializeField] private CatController catController;

    private PlayableDirector playableDirector;
    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }
    private void OnEnable()
    {
        playableDirector.Play();
        playableDirector.stopped += OnTimelineFinished;
    }
    private void OnDisable()
    {
        playableDirector.stopped -= OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        
        gameManeger.ChangeGameState(GameState.Play);
    }
}
