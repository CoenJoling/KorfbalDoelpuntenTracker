using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Score;

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
        var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.score_item_layout, null);

        var scoreData = scoreDataList[position];

        var textViewScore = view.FindViewById<TextView>(Resource.Id.textViewScore);
        textViewScore.Text = $"Score: {scoreData.Score}";

        var textViewDoelpuntVoorTegen = view.FindViewById<TextView>(Resource.Id.textViewDoelpuntVoorTegen);
        textViewDoelpuntVoorTegen.Text = $"DoelpuntVoorTegen: {scoreData.DoelpuntVoorTegen}";

        var textViewWie = view.FindViewById<TextView>(Resource.Id.textViewWie);
        textViewWie.Text = $"Wie: {scoreData.Wie}";

        return view;
    }
}
