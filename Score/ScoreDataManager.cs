namespace Score
{
    public class ScoreDataManager
    {
        public string DoelpuntVoorTegen { set; get; }
        public string Tijd { set; get; }
        public string PlaatsDoelpunt { set; get; }
        public string scoreMethode { set; get; }

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
}