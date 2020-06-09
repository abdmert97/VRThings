using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace ComplexGame
{
    public class AnimatedCanvas : MonoBehaviour
    {
        public static AnimatedCanvas Instance { get; private set; }
        [SerializeField] private TextMeshProUGUI _tmproText;
        [SerializeField] private Animator _animator;
       
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void ShowMessage(string text)
        {
            _tmproText.text = text;
            
          _animator.Play("AnimatedMessage",-1,0);
        }
        public void ShowError(string text)
        {
            _tmproText.text = text;
            
            _animator.Play("RedMessage",-1,0);
        }

        public void SetColor(Color color)
        {
            _tmproText.color = color;
        }



    }
    

}
