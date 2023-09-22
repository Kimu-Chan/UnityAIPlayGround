using UnityEngine;
using UnityEngine.UI;

namespace BadgerHTN.Examples.Zombies.Scripts.Ui
{
    [ExecuteInEditMode]
    public class Progress : MonoBehaviour
    {
        private float _elapsedFill;

        [SerializeField] private float delay = 0.5f;

        [SerializeField][Range(0f, 1f)] private float delayValue = 1f;

        [Header("Images")][SerializeField] private Image imgBackground;

        [SerializeField] private Image imgFill;

        [SerializeField] private Image imgPrediction;

        [Header("Behaviour")][SerializeField] private float speed = 1f;

        [Header("Values")]
        [SerializeField]
        [Range(0f, 1f)]
        private float value = 1f;

        public float Value => value;

        public bool IsAnimating()
        {
            return value < delayValue;
        }

        public void SetBackgroundColor(Color color)
        {
            if (imgBackground != null)
            {
                imgBackground.color = color;
            }
        }

        public void SetFill(float value)
        {
            value = Mathf.Clamp01(value);
            this.value = value;
            UpdateValueSprite();
        }

        public void SetFillColor(Color color)
        {
            if (imgFill != null)
            {
                imgFill.color = color;
            }
        }

        public void SetPredictionColor(Color color)
        {
            if (imgPrediction != null)
            {
                imgPrediction.color = color;
            }
        }

        public void Update()
        {
            if (delayValue <= value)
            {
                SetDelayValue(value);
                _elapsedFill = 0;
                return;
            }

            if (_elapsedFill < delay)
            {
                _elapsedFill += Time.deltaTime;
            }
            else
            {
                var p = delayValue - speed * Time.deltaTime;
                p = Mathf.Max(value, p);
                SetDelayValue(p);
            }
        }

        private void OnValidate()
        {
            UpdateValueSprite();
            UpdateDelaySprite();
        }

        private void SetDelayValue(float value)
        {
            value = Mathf.Clamp01(value);
            delayValue = value;
            UpdateDelaySprite();
        }

        private void UpdateDelaySprite()
        {
            if (imgPrediction != null)
            {
                imgPrediction.fillAmount = delayValue;
            }
        }

        private void UpdateValueSprite()
        {
            if (imgFill != null)
            {
                imgFill.fillAmount = value;
            }
        }
    }
}