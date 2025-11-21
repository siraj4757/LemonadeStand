using System;
using UnityEngine;
using LemonadeStand.Core.Domain;
using LemonadeStand.Presentation.UI;
using UnityEngine.UI;

namespace LemonadeStand.Core.Game
{
    public class GameFlowController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private LevelConfig[] practiceLevels;
        [SerializeField] private LevelConfig[] testLevels;

        [Header("Views")]
        [SerializeField] private SplashView splashView;
        [SerializeField] private NameEntryView nameEntryView;
        [SerializeField] private PracticeView practiceView;
        [SerializeField] private PracticeCompleteView practiceCompleteView;
        [SerializeField] private TestView testView;
        [SerializeField] private ScoreView scoreView;

        private GameState _state;
        private int _currentPracticeIndex;
        private int _currentTestIndex;
        private PlayerProfile _playerProfile;

        private int _practiceCorrectCount;
        private int _practiceQuestionCount;

        [Header("Score")]
        [SerializeField] private int scoreForCorrectAnswerPracticeMode = 10;
        [SerializeField] private int scoreForCorrectAnswerTestMode = 15;


        private void Start()
        {
            TransitionToState(GameState.Splash);
        }


        private void TransitionToState(GameState newState)
        {
            _state = newState;

            // Simple state machine – we can later refactor to State pattern if we need.
            switch (_state)
            {
                case GameState.Splash:
                    ShowSplash();
                    break;

                case GameState.NameEntry:
                    ShowNameEntry();
                    break;

                case GameState.Practice:
                    StartPracticeSequence();
                    break;

                case GameState.Test:
                    StartTestSequence();
                    break;

                case GameState.PracticeComplete:
                    ShowPracticeComplete();
                    break;

                case GameState.Score:
                    ShowScore();
                    break;
            }
        }


        private void ShowSplash()
        {
            splashView.Show(onContinue: () =>
            {
                TransitionToState(GameState.NameEntry);
            });
        }

        private void ShowNameEntry()
        {
            nameEntryView.Show(onNameConfirmed: studentName =>
            {
                _playerProfile = new PlayerProfile(studentName);
                TransitionToState(GameState.Practice);
            });
        }


        private void StartPracticeSequence()
        {
            _currentPracticeIndex = 0;
            _practiceCorrectCount = 0;
            _practiceQuestionCount = 0;
            ShowNextPracticeLevel();
        }


        private void ShowNextPracticeLevel()
        {
            if (_currentPracticeIndex >= practiceLevels.Length)
            {
                TransitionToState(GameState.PracticeComplete);
                return;
            }

            var levelConfig = practiceLevels[_currentPracticeIndex];
            var request = new CustomerRequest(levelConfig);

            practiceView.Show(request, onAnswerEvaluated: (isCorrect) =>
            {
                _practiceQuestionCount++;
                if (isCorrect)
                {
                    _playerProfile.AddScore(scoreForCorrectAnswerPracticeMode);
                    _practiceCorrectCount++;
                }

                _currentPracticeIndex++;
                ShowNextPracticeLevel();
            });
        }


        private void StartTestSequence()
        {
            _currentTestIndex = 0;
            ShowNextTestLevel();
        }


        private void ShowNextTestLevel()
        {
            if (_currentTestIndex >= testLevels.Length)
            {
                TransitionToState(GameState.Score);
                return;
            }

            var levelConfig = testLevels[_currentTestIndex];
            var request = new CustomerRequest(levelConfig);

            testView.Show(
                request,
                onAnswerEvaluated: (isCorrect) =>
                {
                    if (isCorrect)
                    {
                        _playerProfile.AddScore(scoreForCorrectAnswerTestMode);
                    }

                    _currentTestIndex++;
                    ShowNextTestLevel();
                });
        }


        private void ShowPracticeComplete()
        {

            practiceView.Hide();
            int totalQuestions = practiceLevels.Length;
            int correctAnswers = _practiceCorrectCount;


            practiceCompleteView.Show(
                studentName: _playerProfile.StudentName,
                correctAnswers: correctAnswers,
                totalQuestions: totalQuestions,
                onStartTest: () =>
                {
                    TransitionToState(GameState.Test);
                    practiceCompleteView.Hide();
                },
                onReplayPractice: () =>
                {
                    TransitionToState(GameState.Practice);
                    practiceCompleteView.Hide();
                });
        }


        private void ShowScore()
        {
            scoreView.Show(_playerProfile);
        }
    }
}
