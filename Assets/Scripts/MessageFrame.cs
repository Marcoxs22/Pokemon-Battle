using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageFrame : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeBetweenLetters = 0.05f;
    [SerializeField] private float _timeToHide = 2f;
    [SerializeField] private string _showAnimationName = "ShowMessageFrame";
    [SerializeField] private string _hideAnimationName = "HideMessageFrame";

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _showSound;
    [SerializeField] private AudioClip _hideSound;

    private string _currentText;
    private Coroutine _typingCoroutine;

    public static MessageFrame Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMessage(string message)
    {
        StopCoroutine();
        _currentText = message;
        _text.text = "";
        _animator.Play(_showAnimationName, 0, 0f);

        // Sonido de aparición
        if (_audioSource != null && _showSound != null)
        {
            _audioSource.PlayOneShot(_showSound);
        }

        _typingCoroutine = StartCoroutine(TypeMessage());
    }

    private IEnumerator TypeMessage()
    {
        for (int i = 0; i < _currentText.Length; i++)
        {
            _text.text += _currentText[i];
            yield return new WaitForSeconds(_timeBetweenLetters);
        }

        yield return new WaitForSeconds(_timeToHide);
        _animator.Play(_hideAnimationName, 0, 0f);

        // Sonido de desaparición
        if (_audioSource != null && _hideSound != null)
        {
            _audioSource.PlayOneShot(_hideSound);
        }
    }

    private void StopCoroutine()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }
    }

    public void StopMessage()
    {
        StopCoroutine();
        _animator.Play(_hideAnimationName, 0, 0f);

        // Sonido de desaparición
        if (_audioSource != null && _hideSound != null)
        {
            _audioSource.PlayOneShot(_hideSound);
        }

        _text.text = "";
    }
}








