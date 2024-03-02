using UnityEngine;

namespace InversionOfControl
{
    public class SampleRegister : MonoBehaviour
    {
        void Start()
        {
            IOC.Register(this);
        }

        public void Log()
        {
            Debug.Log("Logging message");
        }
    }
    
}
