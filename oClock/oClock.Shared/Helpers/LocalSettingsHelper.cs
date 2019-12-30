using Newtonsoft.Json;
using Windows.Storage;

namespace oClock.Shared.Helpers
{
    public static class LocalSettingsHelper
    {
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static void MarkContainer<T>(SettingContainer container, string containerValue, T value)
        {
            _ = LocalSettings.CreateContainer(container.ToString(), ApplicationDataCreateDisposition.Always);
            LocalSettings.Containers[container.ToString()].Values[containerValue] = value != null ? JsonConvert.SerializeObject(value) : null;
        }

        public static T GetContainerValue<T>(SettingContainer container, string containerValue)
        {
            _ = LocalSettings.CreateContainer(container.ToString(), ApplicationDataCreateDisposition.Always);
            if (!(LocalSettings.Containers[container.ToString()].Values[containerValue] is string currentValue))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(currentValue);
        }
    }

    public enum SettingContainer
    {
        CheckInTime

    }
}
