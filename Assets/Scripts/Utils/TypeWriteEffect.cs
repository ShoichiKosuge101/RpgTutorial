using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Utils
{
    public static class TypeWriteEffect
    {
        /// <summary>
        /// 文字送り機能
        /// </summary>
        /// <param name="textMeshPro"></param>
        /// <param name="fullText"></param>
        /// <param name="token"></param>
        /// <param name="delay"></param>
        public static async UniTask ShowTypeWriteEffectAsync(
            this TMP_Text textMeshPro, 
            string fullText, 
            CancellationToken token,
            float delay = 0.02f
            )
        {
            // nullチェック
            if (textMeshPro == null)
            {
                Debug.LogError("textMeshPro is null");
                return;
            }
            
            // textを空にする
            textMeshPro.text = string.Empty;
            
            // fullTextが空の場合は何もしない
            if (string.IsNullOrEmpty(fullText))
            {
                return;
            }

            for (int i = 0; i < fullText.Length; ++i)
            {
                textMeshPro.text = fullText.Substring(0, i + 1);
                try
                {
                    // delay秒待つ
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
                }
                catch (OperationCanceledException)
                {
                    // キャンセルされたら処理を終了
                    break;
                }
            }
        }
    }
}