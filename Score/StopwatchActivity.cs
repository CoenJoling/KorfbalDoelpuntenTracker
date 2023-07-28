using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Score
{
    [Activity(Label = "StopwatchActivity")]
    public class StopwatchActivity : Activity
    {
        //Stopwatch
        private TextView timeTextView;
        private Button startButton;
        private Button pauseButton;
        private Button resetButton;
        private StopwatchManager stopwatchManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Stopwatch_layout);

            stopwatchManager = new StopwatchManager();

            ////Stopwatch dingen
            timeTextView = FindViewById<TextView>(Resource.Id.timeTextView);
            startButton = FindViewById<Button>(Resource.Id.startButton);
            pauseButton = FindViewById<Button>(Resource.Id.pauseButton);
            resetButton = FindViewById<Button>(Resource.Id.resetButton);

            // Initialize the StopwatchManager
            stopwatchManager = new StopwatchManager();

            // Attach click event handlers to buttons
            startButton.Click += (sender, e) =>
            {
                stopwatchManager.Start();
                UpdateTime();
            };

            pauseButton.Click += (sender, e) =>
            {
                stopwatchManager.Pause();
                UpdateTime();
            };

            resetButton.Click += (sender, e) =>
            {
                stopwatchManager.Reset();
                UpdateTime();
            };
        }

        private async void UpdateTime()
        {
            while (true)
            {
                // Update the UI with the current elapsed time
                RunOnUiThread(() =>
                {
                    timeTextView.Text = stopwatchManager.ElapsedTime.ToString(@"mm\:ss");
                });

                // Delay for 10 milliseconds before updating the time again
                await Task.Delay(10);
            }
        }
    }
}