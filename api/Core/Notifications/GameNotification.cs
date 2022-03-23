using Core.Models;

namespace Core.Notifications
{
    public interface IGameNotification
    {
        void NotifyChanges(GameModel param);
        event EventHandler<GameModel> NotificationEvent;
    }
    public class GameGameNotification : IGameNotification
    {
        public event EventHandler<GameModel> NotificationEvent;

        public void NotifyChanges(GameModel game)
        {
            NotificationEvent?.Invoke(this, game);
        }
    }
}
