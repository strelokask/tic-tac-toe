using Core.Models;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Streams
{
    public class GameMoveStream: IStreamRequest<string>
    {
        public int Id { get; }
        public GameModel Model { get; }

        public GameMoveStream(int id, GameModel model)
        {
            Id = id;
            Model = model;
        }
    }

    public class GameMoveStreamHandler : IStreamRequestHandler<GameMoveStream, string>
    {
        public async IAsyncEnumerable<string> Handle(GameMoveStream request, CancellationToken cancellationToken)
        {

            yield return await Task.Run(() =>
            {
                var game = request.Model;
                var id = request.Id;

                if (game.Id == id)
                {
                    var messageJson = JsonConvert.SerializeObject(game, new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    return messageJson;
                }

                return "";
            });
        }
    }
}
