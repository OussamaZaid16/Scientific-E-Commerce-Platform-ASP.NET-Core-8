using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace GestionUsersMVC.Extensions
{
    public static class SessionExtensions
    {
        // Pour sauvegarder la liste (Set)
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Pour récupérer la liste (Get)
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

    }
}