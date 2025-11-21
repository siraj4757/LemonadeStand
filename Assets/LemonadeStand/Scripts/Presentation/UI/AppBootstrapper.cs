using System.Collections;
using UnityEngine;
using LemonadeStand.Presentation.UI;

namespace LemonadeStand.Presentation
{
    public class AppBootstrapper : MonoBehaviour
    {
        [Header("Loading Screen")]
        [SerializeField] private LoadingView loadingView;

        [Header("Timings")]
        [SerializeField] private float fakeLoadingDuration = 1.0f;


        private IEnumerator Start()
        {
            if (loadingView == null)
                yield break;

            loadingView.Show();

            yield return loadingView.PlayProgress(fakeLoadingDuration);

            loadingView.Hide();
        }
    }
}
