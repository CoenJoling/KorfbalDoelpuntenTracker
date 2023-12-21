using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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

            var adapter = new ScoreDataAdapter(this, scoreList);
            scoreListView.Adapter = adapter;

            deleteButton.Click += (sender, e) =>
            {
                if (scoreListView.CheckedItemPosition != AdapterView.InvalidPosition)
                {
                    var huidigeEntry = scoreList.ElementAt(scoreListView.CheckedItemPosition);

                    scoreList.RemoveAll(x => x.Score == huidigeEntry.Score);

                    adapter.NotifyDataSetChanged();

                    Toast.MakeText(this, "Item deleted", ToastLength.Short).Show();

                    var updatedListAsJson = JsonConvert.SerializeObject(scoreList);
                    Intent returnIntent = new Intent();
                    returnIntent.PutExtra("UpdatedScoreList", updatedListAsJson);
                    returnIntent.PutExtra("DeletedEntry", JsonConvert.SerializeObject(huidigeEntry));
                    SetResult(Result.Ok, returnIntent);
                    Finish(); 
                }
            };
        }
    }
}