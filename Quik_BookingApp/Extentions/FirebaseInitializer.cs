using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Quik_BookingApp.Extentions
{
    public class FirebaseInitializer
    {
        public static void InitializeFirebase()
        {

            // Ensure Firebase is only initialized once
            if (FirebaseApp.DefaultInstance == null)
            {
                string path = "Extentions/Config/firebaseConfig.json";
                var firebaseConfigJson = File.ReadAllText(path);
                var appOptions = new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(firebaseConfigJson)
                };

                FirebaseApp.Create(appOptions);
            }
        }
    }
}
