using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Score
{
    [Activity(Label = "Edit score", MainLauncher = false)]
    public class EditScoreActivity : Activity
    {
        private ListView scoreListView;
        private Button deleteButton;
        private List<ScoreDataManager> scoreList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.score_overview_activity);

            deleteButton = FindViewById<Button>(Resource.Id.deleteButton);

            scoreListView = FindViewById<ListView>(Resource.Id.scoreListView);

            scoreList = JsonConvert.DeserializeObject<List<ScoreDataManager>>(
                Intent.GetStringExtra("SenderScoreList"));

            // Set up the adapter for the ListView
            var adapter = new ScoreDataAdapter(this, scoreList);
            scoreListView.Adapter = adapter;

            deleteButton.Click += (sender, e) =>
            {
                // Check if an item is selected
                if (scoreListView.CheckedItemPosition != AdapterView.InvalidPosition)
                {
                    Toast.MakeText(this, "Score verwjderd", ToastLength.Short).Show();
                }
            };
        }
    }
}