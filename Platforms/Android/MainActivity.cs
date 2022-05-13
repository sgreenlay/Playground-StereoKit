﻿using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Content;
using StereoKit;
using Android.Graphics;
using Java.Lang;
using AndroidX.AppCompat.App;
using System;
using System.Threading.Tasks;

using Playground;

namespace PlaygroundAndroid;

[Activity(Theme = "@style/Theme.AppCompat.Light", MainLauncher = true, Exported = true)]
[IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { "com.oculus.intent.category.VR", Intent.CategoryLauncher })]
public class MainActivity : AppCompatActivity, ISurfaceHolderCallback2
{
    App app;
    Android.Views.View surface;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        JavaSystem.LoadLibrary("openxr_loader");
        JavaSystem.LoadLibrary("StereoKitC");

        // Set up a surface for StereoKit to draw on
        Window.TakeSurface(this);
        Window.SetFormat(Format.Unknown);
        surface = new Android.Views.View(this);
        SetContentView(surface);
        surface.RequestFocus();

        base.OnCreate(savedInstanceState);
        Microsoft.Maui.ApplicationModel.Platform.Init(this, savedInstanceState);

        Run(Handle);
    }
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
        Microsoft.Maui.ApplicationModel.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    static bool running = false;
    void Run(IntPtr activityHandle)
    {
        if (running)
            return;
        running = true;

        Task.Run(() =>
        {
            // If the app has a constructor that takes a string array, then
            // we'll use that, and pass the command line arguments into it on
            // creation
            Type appType = typeof(App);
            app = appType.GetConstructor(new Type[] { typeof(string[]) }) != null
                ? (App)Activator.CreateInstance(appType, new object[] { new string[0] { } })
                : (App)Activator.CreateInstance(appType);
            if (app == null)
                throw new System.Exception("StereoKit loader couldn't construct an instance of the App!");

            // Initialize StereoKit, and the app
            SKSettings settings = app.Settings;
            settings.androidActivity = activityHandle;
            if (!SK.Initialize(settings))
                return;
            app.Init();

            // Now loop until finished, and then shut down
            while (SK.Step(app.Step)) { }
            SK.Shutdown();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        });
    }

    // Events related to surface state changes
    public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height) => SK.SetWindow(holder.Surface.Handle);
    public void SurfaceCreated(ISurfaceHolder holder) => SK.SetWindow(holder.Surface.Handle);
    public void SurfaceDestroyed(ISurfaceHolder holder) => SK.SetWindow(IntPtr.Zero);
    public void SurfaceRedrawNeeded(ISurfaceHolder holder) { }
}
