using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Score
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnTouchListener
    {
        //Stopwatch
        private TextView timeTextView;
        private Button startButton;
        private Button pauseButton;
        private Button resetButton;
        private StopwatchManager stopwatchManager;

        //ScoreData
        private ScoreDataManager scoreDataManager;
        private List<ScoreDataManager> scoreList = new List<ScoreDataManager>();

        //Score
        private Button button;
        private TextView textViewScoreThuis;
        private TextView textViewScoreUit;
        private int scoreThuis;
        private int scoreUit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            RequestedOrientation = ScreenOrientation.Landscape;

            var touchView = FindViewById<View>(Resource.Id.touchView);
            touchView.SetOnTouchListener(this);

            scoreDataManager = new ScoreDataManager();


            //// Get references to UI elements
            //timeTextView = FindViewById<TextView>(Resource.Id.timeTextView);
            //startButton = FindViewById<Button>(Resource.Id.startButton);
            //pauseButton = FindViewById<Button>(Resource.Id.pauseButton);
            //resetButton = FindViewById<Button>(Resource.Id.resetButton);

            //// Initialize the StopwatchManager
            //stopwatchManager = new StopwatchManager();

            //// Attach click event handlers to buttons
            //startButton.Click += (sender, e) =>
            //{
            //    stopwatchManager.Start();
            //    UpdateTime();
            //};

            //pauseButton.Click += (sender, e) =>
            //{
            //    stopwatchManager.Pause();
            //    UpdateTime();
            //};

            //resetButton.Click += (sender, e) =>
            //{
            //    stopwatchManager.Reset();
            //    UpdateTime();
            //};

            //textViewScoreThuis = FindViewById<TextView>(Resource.Id.ScoreThuis);
            //textViewScoreUit = FindViewById<TextView>(Resource.Id.ScoreUit);

            //button = FindViewById<Button>(Resource.Id.ThuisPlus);
            //this.FindViewById<Button>(Resource.Id.ThuisPlus).Click += this.ThuisPlus;

            //button = FindViewById<Button>(Resource.Id.UitPlus);
            //this.FindViewById<Button>(Resource.Id.UitPlus).Click += this.UitPlus;

            //button = FindViewById<Button>(Resource.Id.ThuisMin);
            //this.FindViewById<Button>(Resource.Id.ThuisMin).Click += this.ThuisMin;

            //button = FindViewById<Button>(Resource.Id.UitMin);
            //this.FindViewById<Button>(Resource.Id.UitMin).Click += this.UitMin;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            float x = e.GetX();
            float y = e.GetY();

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

            // Calculate the midpoint of the screen
            int screenWidth = displayMetrics.WidthPixels;
            int screenHeight = displayMetrics.HeightPixels;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    // Touch started
                    // Handle the touch down event
                    ScoreDataManager scoreData = new ScoreDataManager();
                    scoreData.DoelpuntVoorTegen = scoreDataManager.CheckDoelpuntVoorOfTegen(x, screenWidth);
                    scoreData.PlaatsDoelpunt =
                        scoreData.DoelpuntVoorTegen == "Voor" ? scoreDataManager.CheckPlaatsDoelpunt(x, y, screenHeight, screenWidth) :
                        scoreDataManager.CheckPlaatsTegenDoelpunt(x, y, screenHeight, screenWidth);

                    ShowPopup(scoreData);

                    break;
                case MotionEventActions.Move:
                    // Touch moved  
                    // Handle the touch move event
                    break;
                case MotionEventActions.Up:
                    // Touch ended
                    // Handle the touch up event
                    break;
            }
            return true;
        }

        private void ShowPopup(ScoreDataManager scoreData)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Score Information");
            builder.SetMessage($"DoelpuntVoorTegen: {scoreData.DoelpuntVoorTegen}\nPlaatsDoelpunt: {scoreData.PlaatsDoelpunt}");

            // Create an array of the options
            string[] options = { "Doorloopbal", "Afstandschot", "Strafworp" };

            // Inflate a layout containing a Spinner
            View view = LayoutInflater.Inflate(Resource.Layout.popup_layout, null);
            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinner);

            // Create an ArrayAdapter using the options array and a default spinner layout
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, options);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            builder.SetView(view);

            builder.SetPositiveButton("OK", (s, e) =>
            {
                string selectedOption = spinner.SelectedItem.ToString();

                // TODO: Use the selectedOption as needed

                // Show a toast with the selected option
                Toast.MakeText(this, $"Selected option: {selectedOption}", ToastLength.Short).Show();
            });

            AlertDialog dialog = builder.Create();
            dialog.Show();
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

        private void ThuisPlus(object sender, EventArgs e)
        {
            scoreThuis++;
            textViewScoreThuis.Text = scoreThuis.ToString();
        }

        private void UitPlus(object sender, EventArgs e)
        {
            scoreUit++;
            textViewScoreUit.Text = scoreUit.ToString();
        }
        private void ThuisMin(object sender, EventArgs e)
        {
            if (scoreThuis <= 0) { scoreThuis = 0; } else { scoreThuis--; }
            textViewScoreThuis.Text = scoreThuis.ToString();
        }

        private void UitMin(object sender, EventArgs e)
        {
            if (scoreUit <= 0) { scoreUit = 0; } else { scoreUit--; }
            textViewScoreUit.Text = scoreUit.ToString();
        }
    }
}
