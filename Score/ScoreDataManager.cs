using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;

namespace Score
{
    public class ScoreDataManager
    {
        public string Tijd { set; get; }
        public string Score { set; get; }
        public string DoelpuntVoorTegen { set; get; }
        public string ScoreMethode { set; get; }
        public string Wie {  set; get; }
        public string PlaatsDoelpunt { set; get; }
        public int Kansen {  set; get; }

        public ScoreDataManager()
        {
        }

        public string CheckDoelpuntVoorOfTegen(float x, int screenWidth)
        {
            return x < screenWidth / 2 ? "Voor" : "Tegen";
        }

        public string CheckPlaatsDoelpunt(float x, float y, int screenHeight, int screenWidth)
        {
            //Achterkant
            if (x < screenWidth * 0.1056 && y > screenHeight * 0.1794 && y < screenHeight * 0.8061)
            {
                return "Achterkant";
            }
            //Zijkant1
            else if (x < screenWidth * 0.2500 && y < screenHeight * 0.1794)
            {
                return "Zijkant";
            }
            //Zijkant 2
            else if (x > screenWidth * 0.1056 && x < screenWidth * 0.2500 && y > screenHeight * 0.1794 && y < screenHeight * 0.3166)
            {
                return "Zijkant";
            }
            //Zijkant kort 1
            else if (x > screenWidth * 0.1056 && x < screenWidth * 0.2535 && y > screenHeight * 0.3166 && y < screenHeight * 0.3945)
            {
                return "Zijkant kort";
            }
            //Achterkant kort
            else if (x > screenWidth * 0.1056 && x < screenWidth * 0.1690 && y > screenHeight * 0.3945 && y < screenHeight * 0.5610)
            {
                return "Achterkant kort";
            }
            //Zijkant kort 2
            else if (x > screenWidth * 0.1056 && x < screenWidth * 0.2535 && y > screenHeight * 0.5610 && y < screenHeight * 0.6689)
            {
                return "Zijkant kort";
            }
            //Zijkant 3
            else if (x > screenWidth * 0.1056 && x < screenWidth * 0.2500 && y > screenHeight * 0.6689 && y < screenHeight * 0.8061)
            {
                return "Zijkant";
            }
            //Zijkant 4
            else if (x < screenWidth * 0.2500 && y > screenHeight * 0.8061 && y < screenHeight * 0.9800)
            {
                return "Zijkant";
            }
            //Voorkant kort
            else if (x > screenWidth * 0.1690 && x < screenWidth * 0.2073 && y > screenHeight * 0.3945 && y < screenHeight * 0.5610)
            {
                return "Voorkant kort";
            }
            //2.5m
            else if (x > screenWidth * 0.2073 && x < screenWidth * 0.2535 && y > screenHeight * 0.3945 && y < screenHeight * 0.5610)
            {
                return "2.5m";
            }
            //Voorkant mid
            else if (x > screenWidth * 0.2535 && x < screenWidth * 0.3200 && y > screenHeight * 0.3166 && y < screenHeight * 0.6689)
            {
                return "Voorkant mid";
            }
            else
            {
                return "Voorkant afstand";
            }
        }

        public string CheckPlaatsTegenDoelpunt(float x, float y, int screenHeight, int screenWidth)
        {
            //Achterkant
            if (x > screenWidth * 0.8937 && x < screenWidth * 0.9993 && y > screenHeight * 0.1794 && y < screenHeight * 0.8061)
            {
                return "Achterkant";
            }
            //Zijkant1
            else if (x > screenWidth * 0.7987 && x < screenWidth * 0.9993 && y > screenHeight * 0 && y < screenHeight * 0.1794)
            {
                return "Zijkant";
            }
            //Zijkant 2
            else if (x > screenWidth * 0.7987 && x < screenWidth * 0.8937 && y > screenHeight * 0.1794 && y < screenHeight * 0.3166)
            {
                return "Zijkant";
            }
            //Zijkant kort 1
            else if (x > screenWidth * 0.7459 && x < screenWidth * 0.8937 && y > screenHeight * 0.3166 && y < screenHeight * 0.3945)
            {
                return "Zijkant kort";
            }
            //Achterkant kort
            else if (x > screenWidth * 0.8304 && x < screenWidth * 0.8937 && y > screenHeight * 0.3945 && y < screenHeight * 0.5610)
            {
                return "Achterkant kort";
            }
            //Zijkant kort 2
            else if (x > screenWidth * 0.7459 && x < screenWidth * 0.8937 && y > screenHeight * 0.5610 && y < screenHeight * 0.6388)
            {
                return "Zijkant kort";
            }
            //Zijkant 3
            else if (x > screenWidth * 0.7987 && x < screenWidth * 0.8937 && y > screenHeight * 0.6689 && y < screenHeight * 0.8061)
            {
                return "Zijkant";
            }
            //Zijkant 4
            else if (x > screenWidth * 0.7987 && x < screenWidth * 0.9993 && y > screenHeight * 0.8061 && y < screenHeight * 0.9800)
            {
                return "Zijkant";
            }
            //Voorkant kort
            else if (x > screenWidth * 0.7921 && x < screenWidth * 0.8304 && y > screenHeight * 0.3945 && y < screenHeight * 0.5610)
            {
                return "Voorkant kort";
            }
            //2.5m
            else if (x > screenWidth * 0.7459 && x < screenWidth * 0.7921 && y > screenHeight * 0.3945 && y < screenHeight * 0.5610)
            {
                return "2.5m";
            }
            //Voorkant mid
            else if (x > screenWidth * 0.6977 && x < screenWidth * 0.7459 && y > screenHeight * 0.3166 && y < screenHeight * 0.6689)
            {
                return "Voorkant mid";
            }
            else
            {
                return "Voorkant afstand";
            }
        }

        public static Dictionary<string, int> CalculateGoals(List<ScoreDataManager> scoreList)
        {
            // Group the scoreList by Wie and count the goals for each player
            var goalCounts = scoreList.Where(s => s.DoelpuntVoorTegen == "Voor")
                                      .GroupBy(s => s.Wie)
                                      .ToDictionary(g => g.Key, g => g.Count());

            return goalCounts;
        }
    }

    public class Kansen
    {
        public int Kans { get; set; }
        public string Tijd { get; set; }
        public string Wie { get; set; }
        public bool Doelpunt { get; set; }
    }

    public class KansenDataAdapter : BaseAdapter<IGrouping<string, Kansen>>
    {
        private readonly Activity context;
        private readonly List<IGrouping<string, Kansen>> groupedKansenData;
        private readonly List<ScoreDataManager> scoreList;

        public KansenDataAdapter(Activity context, List<IGrouping<string, Kansen>> groupedKansenData, List<ScoreDataManager> scoreList)
        {
            this.context = context;
            this.groupedKansenData = groupedKansenData;
            this.scoreList = scoreList;
        }

        public override int Count => groupedKansenData.Count;

        public override long GetItemId(int position) => position;

        public override IGrouping<string, Kansen> this[int position] => groupedKansenData[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.KansenDataItem, null);

            // Customize the layout for each grouped item in the list
            var groupedItem = groupedKansenData[position];

            if (groupedItem.Key == null)
            {
                return view;
            }

            // Find and set values to your TextViews in the grouped item layout
            var wieTextView = view.FindViewById<TextView>(Resource.Id.wieTextView);
            if (groupedItem.Count() ==1)
            {
                wieTextView.Text = $"{groupedItem.Key} {groupedItem.Count()} kans";
            }
            else
            {
                wieTextView.Text = $"{groupedItem.Key} {groupedItem.Count()} kansen";
            }

            int totalCount = groupedItem.Sum(k => k.Kans);

            // Calculate and display the goal count for the player
            var goalCounts = ScoreDataManager.CalculateGoals(scoreList);
            if (goalCounts.TryGetValue(groupedItem.Key, out int goalCount))
            {
                var goalCountTextView = view.FindViewById<TextView>(Resource.Id.goalCountTextView);
                var ratio = (double)goalCount / groupedItem.Count();
                goalCountTextView.Text = $"Goals: {goalCount} -> {ratio:P}";
            }

            return view;
        }
    }



    public class ScoreDataAdapter : BaseAdapter<ScoreDataManager>
    {
        private readonly Activity context;
        private readonly List<ScoreDataManager> scoreDataList;

        public ScoreDataAdapter(Activity context, List<ScoreDataManager> scoreDataList)
        {
            this.context = context;
            this.scoreDataList = scoreDataList;
        }

        public override int Count => scoreDataList.Count;

        public override long GetItemId(int position) => position;

        public override ScoreDataManager this[int position] => scoreDataList[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ScoreDataItem, null);

            // Customize the layout for each item in the list
            var scoreData = scoreDataList[position];

            // Find and set values to your TextViews in the item layout
            var scoreTextView = view.FindViewById<TextView>(Resource.Id.scoreTextView);
            scoreTextView.Text = $"Score: {scoreData.Score}";

            var timeTextView = view.FindViewById<TextView>(Resource.Id.wieWatView);
            timeTextView.Text = $"{scoreData.ScoreMethode} {scoreData.DoelpuntVoorTegen.ToLower()} {scoreData.Wie}";

            // Add more TextViews and customize as needed

            //var deleteButton = view.FindViewById<Button>(Resource.Id.deleteButton);

            //deleteButton.Click += (sender, e) =>
            //{
            //    // Show a confirmation dialog
            //    var confirmDialog = new AlertDialog.Builder(context)
            //        .SetTitle("Confirm Delete")
            //        .SetMessage("Are you sure you want to delete this entry?")
            //        .SetPositiveButton("Yes", (s, a) =>
            //        {
            //            //if (scoreData.DoelpuntVoorTegen == "Voor") kansen.Kans--;
            //            if (scoreData.DoelpuntVoorTegen == "Voor")  
            //            if (scoreData.DoelpuntVoorTegen == "Voor")
            //                    // Remove the corresponding entry from the list
            //                    scoreDataList.RemoveAt(position);S

            //            // Notify the adapter that the data set has changed
            //            NotifyDataSetChanged();
            //        })
            //        .SetNegativeButton("No", (s, a) => { })
            //        .Create();

            //    confirmDialog.Show();
            //};

            return view;
        }
    }
}