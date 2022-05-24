using System.Diagnostics.CodeAnalysis;
using CustomSelectables;
using DG.Tweening;
using Runtime.Data;
using Runtime.Data.PlayerData.Levels;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.Ui.Windows.GameOver 
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GameOverView : UiView 
    {
        [Inject] private readonly ILevelsData _levelsData;
        
        [SerializeField] private TMP_Text _levelN;
        // [SerializeField] private TMP_Text _score;
        // [SerializeField] private TMP_Text _highScore;
        [SerializeField] public CustomButton Restart;

        [SerializeField] public Image Back;
        [SerializeField] public RectTransform Top;
        [SerializeField] public RectTransform Bottom;
        
        private int _scoreValue;
        public void Show(CommonPlayerData playerData)
        {
            // _levelN.text = Enum.GetName(typeof(EScene), playerData.Level)?.Replace("_", " ");
            // UpdateHighScore(playerData.HighScore);
            
            _levelN.text = _levelsData.Get().Get(playerData.CurrentLevel).GetName();
            
            var color = Back.color;
            Back.color = new Color(0, 0, 0, 0);
            var oldPosTop = Top.anchoredPosition;
            Top.anchoredPosition = new Vector2(0, 650f);
            var oldPosBottom = Bottom.anchoredPosition;
            Bottom.anchoredPosition = new Vector2(0, -800f);
            Back.DOColor(color, 1.2f).OnComplete(() =>
            {
                Top.DOAnchorPos(oldPosTop, 0.7f);
                Bottom.DOAnchorPos(oldPosBottom, 0.7f);
            });
        }

        // public void UpdateScore(ref SignalScoreUpdate signal)
        // {
        //     _scoreValue = signal.Value;
        //     _score.text = new StringBuilder("Score: ").Append(signal.Value).ToString();
        // }
        //
        // public void UpdateHighScore(int highScore)
        // {
        //     if (highScore < _scoreValue)
        //         _highScore.text = "You have a new record!";
        //     else
        //         _highScore.text = new StringBuilder("Highs Score: ").Append(highScore).ToString();
        // }

        // public ref int GetScore() => ref _scoreValue;
    }
}