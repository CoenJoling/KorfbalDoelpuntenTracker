using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Score
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnTouchListener
    {
        //ScoreData
        private ScoreDataManager scoreDataManager;
        private List<ScoreDataManager> scoreList = new List<ScoreDataManager>();

        //Speel klaar button
        private Button eindeSpel;

        //Stopwatch
        private TextView timeTextView;
        private Button startButton;
        private Button pauseButton;
        private Button resetButton;
        private StopwatchManager stopwatchManager;

        //Score
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
            ToggleScreenLock();

            var touchView = FindViewById<View>(Resource.Id.touchView);
            touchView.SetOnTouchListener(this);

            scoreDataManager = new ScoreDataManager();
            //Score manier
            //doorloopbalButton = FindViewById<Button>(Resource.Id.doorloopbalButton);
            //schotButton = FindViewById<Button>(Resource.Id.schotButton);
            //strafworpButton = FindViewById<Button>(Resource.Id.strafworpButton);
            //vrijeBalButton = FindViewById<Button>(Resource.Id.vrijeBalButton);
            //doorloopbalButton.Click += (sender, e) =>
            //{
            //    scoreData.ScoreMethode = "Doorloopbal";
            //};

            //schotButton.Click += (sender, e) =>
            //{
            //    scoreData.ScoreMethode = "Schot";
            //};

            //strafworpButton.Click += (sender, e) =>
            //{
            //    scoreData.ScoreMethode = "Strafworp";
            //};

            //vrijeBalButton.Click += (sender, e) =>
            //{
            //    scoreData.ScoreMethode = "Vrije bal";
            //};

            eindeSpel = FindViewById<Button>(Resource.Id.eindeSpel);
            this.FindViewById<Button>(Resource.Id.eindeSpel).Click += this.EindeWedstrijd;

            textViewScoreThuis = FindViewById<TextView>(Resource.Id.textViewScoreThuis);
            textViewScoreUit = FindViewById<TextView>(Resource.Id.textViewScoreUit);

            #region Stopwatch methodes
            timeTextView = FindViewById<TextView>(Resource.Id.timeTextView);
            startButton = FindViewById<Button>(Resource.Id.startButton);
            pauseButton = FindViewById<Button>(Resource.Id.pauseButton);
            resetButton = FindViewById<Button>(Resource.Id.resetButton);

            // Initialize the StopwatchManager
            stopwatchManager = new StopwatchManager();

            // Stopwatch click event handlers
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
            #endregion
        }

        public void ToggleScreenLock()
        {
            DeviceDisplay.KeepScreenOn = true;
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

        private void EindeWedstrijd(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now.ToString("ddMMyyyy");
            var guid = Guid.NewGuid().ToString().Substring(0,8);

            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Download");
            string fileName = $"{dateTimeNow}-{guid}.csv";
            string filePath = System.IO.Path.Combine(path, fileName);

            Intent intent = new Intent(Intent.ActionCreateDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("text/csv");
            intent.PutExtra(Intent.ExtraTitle, fileName);

            StartActivityForResult(intent, 1);

            
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                Android.Net.Uri uri = data.Data;

                using (Stream stream = ContentResolver.OpenOutputStream(uri))
                using (StreamWriter writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<ScoreDataManager>();
                    csv.NextRecord();
                    foreach (var score in scoreList)
                    {
                        csv.WriteRecord(score);
                        csv.NextRecord();
                    }
                }

                Toast.MakeText(this, "Bestand opgeslagen!", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Bestandsselectie geannuleerd.", ToastLength.Short).Show();
            }
            scoreList.Clear();
            scoreThuis = 0;
            textViewScoreThuis.Text = scoreThuis.ToString();
            scoreUit = 0;
            textViewScoreUit.Text = scoreUit.ToString();
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
                    scoreData.Tijd = timeTextView.Text;

                    ShowPopup(scoreData);

                    break;
            }
            return true;
        }

        private void ShowPopup(ScoreDataManager scoreData)
        {
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(this);
            dialogBuilder.SetTitle("Score Informatie");
            dialogBuilder.SetMessage($"Doelpunt" +
                $": {scoreData.DoelpuntVoorTegen}\nTijd doelpunt: {scoreData.Tijd}\nPlaats doelpunt: {scoreData.PlaatsDoelpunt}");

            View dialogView = LayoutInflater.Inflate(Resource.Layout.spinner_layout, null);
            dialogBuilder.SetView(dialogView);

            Spinner spinner = dialogView.FindViewById<Spinner>(Resource.Id.spinner1);
            Spinner spinner2 = dialogView.FindViewById<Spinner>(Resource.Id.spinner2);

            var spinnerData = new List<string> { "Jonne", "Niek", "Bas", "Lucas", "Lisa", "Linde", "Sanne", "Mette", "Kirsten", "Britt" };
            var spinnerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spinnerData);
            spinner.Adapter = spinnerAdapter;
            var spinnerData2 = new List<string> { "Schot", "Doorloopbal", "Vrije bal", "Strafworp"};
            var spinnerAdapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spinnerData2);
            spinner2.Adapter = spinnerAdapter2;

            string selectedSpinnerItem = null; // Define a variable to store the selection
            string selectedSpinnerItem2 = null; // Define a variable to store the selection

            spinner.ItemSelected += (sender, e) =>
            {
                selectedSpinnerItem = spinnerData[e.Position]; // Store the selected item
            };
            spinner2.ItemSelected += (sender, e) =>
            {
                selectedSpinnerItem2 = spinnerData2[e.Position]; // Store the selected item
            };

            // Add a button to dismiss the dialog
            dialogBuilder.SetPositiveButton("OK", (sender, args) =>
            {
                if (scoreData.DoelpuntVoorTegen == "Voor") ThuisPlus();
                if (scoreData.DoelpuntVoorTegen == "Tegen") UitPlus();

                scoreData.Wie = selectedSpinnerItem;

                scoreData.ScoreMethode = selectedSpinnerItem2;

                scoreData.Score = $"{scoreThuis.ToString()} - {scoreUit.ToString()}";

                scoreList.Add(scoreData);
            });
            // Add a button to dismiss the dialog
            dialogBuilder.SetNegativeButton("Cancel", (sender, args) =>
            {
                Toast.MakeText(this, "Score geannuleerd!", ToastLength.Short).Show();
            });

            // Create and show the dialog
            AlertDialog dialog = dialogBuilder.Create();
            dialog.Show();
        }

        private void ThuisPlus()
        {
            scoreThuis++;
            textViewScoreThuis.Text = scoreThuis.ToString();
        }

        private void UitPlus()
        {
            scoreUit++;
            textViewScoreUit.Text = scoreUit.ToString();
        }
    }
}
