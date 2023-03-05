using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TesParallel
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestParallel()
    {
        var folders = new string[] { "FrameWork", "Plugins" };
        Parallel.For(0, folders.Length, (i) =>
        {
            AsyncTask(folders[i]);
        });
        // for (int i = 0; i < folders.Length; i++)
        // {
        //     AsyncTask(folders[i]);
        // }
    }

    [Test]
    public void TestConcurrent()
    {
        var folders = new string[] { "FrameWork", "Plugins" };

        for (int i = 0; i < folders.Length; i++)
        {
            AsyncTask(folders[i]);
        }
    }

    // 异步方法
    private async void AsyncTask(string folderName)
    {
        var txtFiles = Directory.EnumerateFiles(@"C:\Users\liux4\Documents\UnityProjects\UIFrameWork\Assets\" +
            folderName, "*.cs", SearchOption.AllDirectories);
        foreach (string currentFile in txtFiles)
        {
            Task<string> task = File.ReadAllTextAsync(currentFile);
            var result = task.Result;
            await task;

            // var result = await Task.Run(() =>
            // {
            //     // 异步工作代码
            //     var astr = File.ReadAllText(currentFile);
            //     return $"{Thread.CurrentThread.Name}:{Thread.CurrentThread.IsBackground}";
            // });

        
            Debug.Log(result);
        }
    }
}
