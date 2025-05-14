using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
    public class ObservableProperty<T>
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value)) return;
                _value = value;
                Notify();
            }
        }
        private UnityEvent<T> _onValueChanged = new();

        public ObservableProperty(T value = default)
        {
            _value = value;
        }

        public void Subscribe(UnityAction<T> action)
        {
            _onValueChanged.AddListener(action);
        }

        public void UnSubscribe(UnityAction<T> action)
        {
            _onValueChanged.RemoveListener(action);
        }

        public void UnSubscribeAll()
        {
            _onValueChanged.RemoveAllListeners();
        }

        public void Notify()
        {
            _onValueChanged?.Invoke(Value);
        }
    }
}