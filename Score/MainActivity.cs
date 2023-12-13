using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private Kansen kansen;
        private List<Kansen> kansenList = new List<Kansen>();

        //Setting button spelers
        private List<string> playerNames;

        //Kans button
        private Button kans;

        private Button showPopupMenu;

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
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            RequestedOrientation = ScreenOrientation.Landscape;
            DeviceDisplay.KeepScreenOn = true;

            var touchView = FindViewById<View>(Resource.Id.touchView);
            touchView.SetOnTouchListener(this);

            scoreDataManager = new ScoreDataManager();
            kansen = new Kansen();

            showPopupMenu = FindViewById<Button>(Resource.Id.showMenuButton);

            showPopupMenu.Click += (s, arg) => {
                PopupMenu menu = new PopupMenu(this, showPopupMenu);
                menu.Inflate(Resource.Menu.popup_menu);

                menu.MenuItemClick += (s1, arg1) => {
                    switch(arg1.Item.ItemId)
                    {
                        case Resource.Id.menu_item_1:
                            this.ExportKansen(s1, arg1);
                            break;
                        case Resource.Id.menu_item_2:
                            this.EindeWedstrijd(s1, arg1);
                            break;
                        case Resource.Id.menu_item_3:
                            StartActivity(typeof(PlayerSettingsActivity));
                            break;
                        case Resource.Id.menu_item_4:
                            var listAsJson = JsonConvert.SerializeObject(scoreList);
                            Intent senderIntent = new Intent(this, typeof(EditScoreActivity));
                            senderIntent.PutExtra("SenderScoreList", listAsJson);
                            StartActivity(senderIntent);
                            break;
                    }
                };

                menu.DismissEvent += (s2, arg2) => {
                    Console.WriteLine("menu dismissed");
                };
                menu.Show();
            };

            kans = FindViewById<Button>(Resource.Id.kans);
            this.FindViewById<Button>(Resource.Id.kans).Click += this.KansGenomen;

            var lijstScores = FindViewById<Button>(Resource.Id.scoreListButton);
            lijstScores.Click += (sender, e) =>
            {
                ShowScores(lijstScores);
            };

            var lijstKansen = FindViewById<Button>(Resource.Id.kansenListButton);
            lijstKansen.Click += (sender, e) =>
            {
                ShowKansen(lijstKansen);
            };

            // Load player names from SharedPreferences
            var sharedPreferences = GetSharedPreferences("PlayerSettings", FileCreationMode.Private);
            var jsonPlayerNames = sharedPreferences.GetString("PlayerNames", string.Empty);
            playerNames = JsonConvert.DeserializeObject<List<string>>(jsonPlayerNames) ?? new List<string>();


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

        private void ShowKansen(View anchorView)
        {
            // Group the kansen data by Wie
            var groupedKansenData = kansenList.GroupBy(k => k.Wie).ToList();

            // Inflate the layout for the popup
            var inflater = (LayoutInflater)GetSystemService(LayoutInflaterService);
            var popupView = inflater.Inflate(Resource.Layout.PopupLayout, null);

            // Find the ListView in the popup layout
            var listView = popupView.FindViewById<ListView>(Resource.Id.listView);

            // Create an adapter for your grouped score data
            groupedKansenData = groupedKansenData.Where(item => item.Key != null).ToList();
            var adapter = new KansenDataAdapter(this, groupedKansenData, scoreList);

            // Set the adapter for the ListView
            listView.Adapter = adapter;

            // Create the popup window
            var popup = new PopupWindow(popupView, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, true);

            // Set the location of the popup relative to the anchor view
            popup.ShowAsDropDown(anchorView);

            // Set a dismiss event handler for the popup
            popup.DismissEvent += (sender, e) =>
            {
                // Handle dismiss event if needed
            };
        }

        private void ShowScores(View anchorView)
        {
            // Inflate the layout for the popup
            var inflater = (LayoutInflater)GetSystemService(LayoutInflaterService);
            var popupView = inflater.Inflate(Resource.Layout.PopupLayout, null);

            // Find the ListView in the popup layout
            var listView = popupView.FindViewById<ListView>(Resource.Id.listView);

            // Create an adapter for your score data
            var adapter = new ScoreDataAdapter(this, scoreList);

            // Set the adapter for the ListView
            listView.Adapter = adapter;

            // Create the popup window
            var popup = new PopupWindow(popupView, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, true);

            // Set the location of the popup relative to the anchor view
            popup.ShowAsDropDown(anchorView);
            
            // Set a dismiss event handler for the popup
            popup.DismissEvent += (sender, e) =>
            {
                // Handle dismiss event if needed
            };
        }

        private void KansGenomen(object send, EventArgs e)
        {
            var kans = new Kansen();
            ShowPopupKans(kans);
        }

        private void ShowPopupKans(Kansen kans)
        {
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(this);
            dialogBuilder.SetTitle("Kans genomen");
            dialogBuilder.SetMessage($"Kans genomen door:");

            View dialogView = LayoutInflater.Inflate(Resource.Layout.spinner_kans_layout, null);
            dialogBuilder.SetView(dialogView);

            Spinner spinner3 = dialogView.FindViewById<Spinner>(Resource.Id.spinner3);

            var spinnerData3 = playerNames;
            var spinnerAdapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spinnerData3);
            spinner3.Adapter = spinnerAdapter3;

            string selectedSpinnerItem3 = null; // Define a variable to store the selection

            spinner3.ItemSelected += (sender, e) =>
            {
                selectedSpinnerItem3 = spinnerData3[e.Position]; // Store the selected item
            };

            // Add a button to dismiss the dialog
            dialogBuilder.SetPositiveButton("OK", (sender, args) =>
            {
                kansen.Kans++;
                kans.Kans = kansen.Kans;
                kansen.Wie = selectedSpinnerItem3;
                kans.Wie = kansen.Wie;
                kansen.Tijd = timeTextView.Text;
                kans.Tijd = kansen.Tijd;
                kansen.Doelpunt = false;
                kans.Doelpunt = false;
                kansenList.Add(kans);
            });
            // Add a button to dismiss the dialog
            dialogBuilder.SetNegativeButton("Cancel", (sender, args) =>
            {
                Toast.MakeText(this, "Kans geannuleerd!", ToastLength.Short).Show();
            });

            // Create and show the dialog
            AlertDialog dialog = dialogBuilder.Create();
            dialog.Show();
        }

        private void ExportKansen(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now.ToString("ddMMyyyy");
            var guid = Guid.NewGuid().ToString().Substring(0, 8);
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Download");

            string kansenFileName = $"{dateTimeNow}-{guid}-kansen.csv";
            
            Intent kansenIntent = new Intent(Intent.ActionCreateDocument);
            kansenIntent.AddCategory(Intent.CategoryOpenable);
            kansenIntent.SetType("text/csv");
            kansenIntent.PutExtra(Intent.ExtraTitle, kansenFileName);

            // Start activity for Kansen file
            StartActivityForResult(kansenIntent, 2);
        }

        private void EindeWedstrijd(object sender, EventArgs e)
        {
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(this);
            dialogBuilder.SetTitle("Einde Wedstrijd!");
            dialogBuilder.SetMessage($"Alles zal resetten. Echt afsluiten?");

            dialogBuilder.SetPositiveButton("OK", (sender, args) =>
            {
                scoreList.Last().Kansen = kansenList.Last().Kans;

                var dateTimeNow = DateTime.Now.ToString("ddMMyyyy");
                var guid = Guid.NewGuid().ToString().Substring(0, 8);

                string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Download");

                // File paths for ScoreDataManager and Kansen
                string scoreFileName = $"{dateTimeNow}-{guid}-score.csv";
                string scoreFilePath = Path.Combine(path, scoreFileName);

                // Create an intent for ScoreDataManager file
                Intent scoreIntent = new Intent(Intent.ActionCreateDocument);
                scoreIntent.AddCategory(Intent.CategoryOpenable);
                scoreIntent.SetType("text/csv");
                scoreIntent.PutExtra(Intent.ExtraTitle, scoreFileName);

                // Start activity for ScoreDataManager file
                StartActivityForResult(scoreIntent, 1);
            });

            dialogBuilder.SetNegativeButton("Cancel", (sender, args) =>
            {
                Toast.MakeText(this, "Geannuleerd!", ToastLength.Short).Show();
            });

            AlertDialog dialog = dialogBuilder.Create();
            dialog.Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                Android.Net.Uri uri = data.Data;

                switch (requestCode)
                {
                    case 1: // ScoreDataManager file
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
                        scoreList.Clear();
                        kansenList.Clear();
                        scoreThuis = 0;
                        textViewScoreThuis.Text = scoreThuis.ToString();
                        scoreUit = 0;
                        textViewScoreUit.Text = scoreUit.ToString();

                        break;

                    case 2: // Kansen file
                        using (Stream stream = ContentResolver.OpenOutputStream(uri))
                        using (StreamWriter writer = new StreamWriter(stream))
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteHeader<Kansen>();
                            csv.NextRecord();
                            foreach (var kansen in kansenList)
                            {
                                csv.WriteRecord(kansen);
                                csv.NextRecord();
                            }
                        }
                        break;
                }
            }
            else
            {
                Toast.MakeText(this, "Bestandsselectie geannuleerd.", ToastLength.Short).Show();
            }
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

                    var sharedPreferences = GetSharedPreferences("PlayerSettings", FileCreationMode.Private);
                    var jsonPlayerNames = sharedPreferences.GetString("PlayerNames", string.Empty);
                    playerNames = JsonConvert.DeserializeObject<List<string>>(jsonPlayerNames) ?? new List<string>();

                    ShowPopupScore(scoreData, playerNames);

                    break;
            }
            return true;
        }

        private void ShowPopupScore(ScoreDataManager scoreData, List<string> spelers)
        {
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(this);
            dialogBuilder.SetTitle("Score Informatie");
            dialogBuilder.SetMessage($"Doelpunt" +
                $": {scoreData.DoelpuntVoorTegen}\nTijd doelpunt: {scoreData.Tijd}\nPlaats doelpunt: {scoreData.PlaatsDoelpunt}");

            View dialogView = LayoutInflater.Inflate(Resource.Layout.spinner_layout, null);
            dialogBuilder.SetView(dialogView);

            Spinner spinner = dialogView.FindViewById<Spinner>(Resource.Id.spinner1);
            Spinner spinner2 = dialogView.FindViewById<Spinner>(Resource.Id.spinner2);

            var spinnerData = playerNames;
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

            var kansUitScore = new Kansen();
            // Add a button to dismiss the dialog
            dialogBuilder.SetPositiveButton("OK", (sender, args) =>
            {
                if (scoreData.DoelpuntVoorTegen == "Voor") ThuisPlus("plus");
                if (scoreData.DoelpuntVoorTegen == "Tegen") UitPlus("plus");

                scoreData.Wie = selectedSpinnerItem;

                scoreData.ScoreMethode = selectedSpinnerItem2;

                scoreData.Score = $"{scoreThuis.ToString()} - {scoreUit.ToString()}";

                if (scoreData.DoelpuntVoorTegen == "Voor")
                {
                    kansen.Kans++;
                    scoreData.Kansen = kansen.Kans;
                    kansUitScore.Kans = kansen.Kans;
                    kansen.Wie = scoreData.Wie;
                    kansUitScore.Wie = scoreData.Wie;
                    kansen.Tijd = scoreData.Tijd;
                    kansen.Doelpunt = true;
                    kansUitScore.Doelpunt = true;
                    kansUitScore.Tijd = scoreData.Tijd;
                }

                scoreList.Add(scoreData);
                kansenList.Add(kansUitScore);
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

        public void ThuisPlus(string plusMin)
        {
            if (plusMin == "plus")
            {
                scoreThuis++;
            }
            else { scoreThuis--;}
            textViewScoreThuis.Text = scoreThuis.ToString();
        }

        public void UitPlus(string plusMin)
        {
            if (plusMin == "plus")
            {
                scoreUit++;
            }
            else { scoreUit--; }
            textViewScoreUit.Text = scoreUit.ToString();
        }
    }
}
