using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

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

            return view;
        }
    }
}