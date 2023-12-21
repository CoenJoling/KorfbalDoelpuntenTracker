using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Score
{
    [Activity(Label = "Player Settings", MainLauncher = false)]
    public class PlayerSettingsActivity : Activity
    {

        private EditText playerNameEditText;
        private Button saveButton;
        private ListView playersListView;
        private List<string> existingPlayerNames;
        private Button deleteButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PlayerSettings);

            playerNameEditText = FindViewById<EditText>(Resource.Id.playerNameEditText);
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            playersListView = FindViewById<ListView>(Resource.Id.playersListView);
            deleteButton = FindViewById<Button>(Resource.Id.deleteButton); 

            this.existingPlayerNames = LoadPlayerNames();

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, this.existingPlayerNames);
            playersListView.Adapter = adapter;

            saveButton.Click += (sender, e) =>
            {
                var newPlayerName = playerNameEditText.Text.Trim();

                if (!string.IsNullOrEmpty(newPlayerName))
                {
                    this.existingPlayerNames.Add(newPlayerName);

                    SavePlayerNames(this.existingPlayerNames);

                    adapter.NotifyDataSetChanged();

                    Toast.MakeText(this, $"{newPlayerName} opgeslagen", ToastLength.Short).Show();
                    playerNameEditText.Text = string.Empty;

                    this.Recreate();
                }
            };

            deleteButton.Click += (sender, e) =>
            {
                if (playersListView.CheckedItemPosition != AdapterView.InvalidPosition)
                {
                    existingPlayerNames.RemoveAt(playersListView.CheckedItemPosition);

                    SavePlayerNames(existingPlayerNames);

                    adapter.NotifyDataSetChanged();

                    Toast.MakeText(this, "Speler verwijderd", ToastLength.Short).Show();
                    this.Recreate();
                }
                else
                {
                    Toast.MakeText(this, "Selecteer een speler om te verwijderen", ToastLength.Short).Show();
                }
            };
        }

        public List<string> LoadPlayerNames()
        {
            var sharedPreferences = GetSharedPreferences("PlayerSettings", FileCreationMode.Private);
            var jsonPlayerNames = sharedPreferences.GetString("PlayerNames", string.Empty);

            return JsonConvert.DeserializeObject<List<string>>(jsonPlayerNames) ?? new List<string>();
        }

        private void SavePlayerNames(List<string> playerNames)
        {
            var sharedPreferences = GetSharedPreferences("PlayerSettings", FileCreationMode.Private);
            var editor = sharedPreferences.Edit();

            var jsonPlayerNames = JsonConvert.SerializeObject(playerNames);

            editor.PutString("PlayerNames", jsonPlayerNames);
            editor.Apply();
        }
    }
}