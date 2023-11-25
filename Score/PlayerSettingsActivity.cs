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


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PlayerSettings);

            // Get references to UI elements
            playerNameEditText = FindViewById<EditText>(Resource.Id.playerNameEditText);
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            playersListView = FindViewById<ListView>(Resource.Id.playersListView);

            // Load existing player names from SharedPreferences
            var existingPlayerNames = LoadPlayerNames();

            // Set up the adapter for the ListView
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, existingPlayerNames);
            playersListView.Adapter = adapter;

            // Handle save button click
            saveButton.Click += (sender, e) =>
            {
                // Get the new player name from the EditText
                var newPlayerName = playerNameEditText.Text.Trim();

                if (!string.IsNullOrEmpty(newPlayerName))
                {
                    // Add the new player name to the existing list
                    existingPlayerNames.Add(newPlayerName);

                    // Save the updated player names to SharedPreferences
                    SavePlayerNames(existingPlayerNames);

                    adapter.NotifyDataSetChanged();

                    Toast.MakeText(this, $"{newPlayerName} opgeslagen", ToastLength.Short).Show();
                    playerNameEditText.Text = string.Empty;
                }
            };
        }

        private List<string> LoadPlayerNames()
        {
            // Load player names from SharedPreferences
            var sharedPreferences = GetSharedPreferences("PlayerSettings", FileCreationMode.Private);
            var jsonPlayerNames = sharedPreferences.GetString("PlayerNames", string.Empty);

            // Deserialize the JSON string to a list of player names
            return JsonConvert.DeserializeObject<List<string>>(jsonPlayerNames) ?? new List<string>();
        }

        private void SavePlayerNames(List<string> playerNames)
        {
            // Save player names to SharedPreferences
            var sharedPreferences = GetSharedPreferences("PlayerSettings", FileCreationMode.Private);
            var editor = sharedPreferences.Edit();

            // Serialize the list of player names to a JSON string
            var jsonPlayerNames = JsonConvert.SerializeObject(playerNames);

            editor.PutString("PlayerNames", jsonPlayerNames);
            editor.Apply();
            
        }
    }
}