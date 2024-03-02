using System;
using UnityEngine;
namespace InversionOfControl
{
    public class SampleResolve: MonoBehaviour
    {
        private void Start()
        {
          SampleRegister sampleRegister =  IOC.Resolve<SampleRegister>();
          sampleRegister.Log();
        }
    }
}