using System.Collections;
using UnityEngine;

public static class CoroutineRunner
{
    private class CoroutineHolder : MonoBehaviour { }

    private static CoroutineHolder _runner;

    public static void StartCoroutine(IEnumerator coroutine)
    {
        if (_runner == null)
        {
            var obj = new GameObject("CoroutineRunner");
            _runner = obj.AddComponent<CoroutineHolder>();
            Object.DontDestroyOnLoad(obj);
        }
        _runner.StartCoroutine(coroutine);
    }
}
