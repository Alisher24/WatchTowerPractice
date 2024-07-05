namespace WatchTower.Server.BusinessLogic.Result
{
    public class CollectionResult<T>: BaseResult<IEnumerable<T>>
    {
        public int Count { get; set; }
    }
}
